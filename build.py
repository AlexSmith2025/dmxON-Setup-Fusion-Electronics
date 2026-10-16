# Build dmxON-Setup.exe — Windows, .NET Framework 4.8 C# compiler (csc.exe ships with Windows), Python 3 + Pillow.
#   python build.py            -> dist\dmxON-Setup.exe
# The latest releases of dmxON FabKit and dmxON JLC Rules are downloaded from GitHub and embedded
# as the offline fallback; at install time the installer always fetches the latest release first.
import hashlib, os, subprocess, sys, urllib.request, zipfile

HERE = os.path.dirname(os.path.abspath(__file__))
SRC = os.path.join(HERE, 'src')
EMB = os.path.join(HERE, 'embed')
DIST = os.path.join(HERE, 'dist')
CSC = os.path.join(os.environ.get('WINDIR', r'C:\Windows'), r'Microsoft.NET\Framework64\v4.0.30319\csc.exe')
ASSETS = [('AlexSmith2025/dmxON-FabKit-Fusion-Electronics', 'dmxON-FabKit.zip', 'dmxON-FabKit'),
          ('AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics', 'dmxON-JLC-Rules.zip', 'dmxON-JLC-Rules')]

os.makedirs(EMB, exist_ok=True)
os.makedirs(DIST, exist_ok=True)
for repo, asset, top in ASSETS:
    url = 'https://github.com/%s/releases/latest/download/%s' % (repo, asset)
    req = urllib.request.Request(url, headers={'User-Agent': 'dmxON-Setup-build'})
    data = urllib.request.urlopen(req, timeout=60).read()
    path = os.path.join(EMB, asset)
    open(path, 'wb').write(data)
    with zipfile.ZipFile(path) as z:
        print('embedded', asset, z.read(top + '/VERSION').decode().strip(), len(data), 'bytes')

subprocess.run([sys.executable, os.path.join(SRC, 'make_icon.py')], check=True)

out = os.path.join(DIST, 'dmxON-Setup.exe')
cmd = [CSC, '/nologo', '/target:winexe', '/optimize+', '/platform:anycpu', '/codepage:65001',
       '/win32icon:' + os.path.join(SRC, 'setup.ico'), '/win32manifest:' + os.path.join(SRC, 'app.manifest'),
       '/r:System.dll', '/r:System.Core.dll', '/r:System.Drawing.dll', '/r:System.Windows.Forms.dll',
       '/r:System.IO.Compression.dll', '/r:System.IO.Compression.FileSystem.dll',
       '/resource:%s,dmxON-FabKit.zip' % os.path.join(EMB, 'dmxON-FabKit.zip'),
       '/resource:%s,dmxON-JLC-Rules.zip' % os.path.join(EMB, 'dmxON-JLC-Rules.zip'),
       '/resource:%s,setup_logo.png' % os.path.join(SRC, 'setup_logo.png'),
       '/out:' + out, os.path.join(SRC, 'Setup.cs')]
r = subprocess.run(cmd, capture_output=True, text=True, encoding='cp866', errors='replace')
print(r.stdout[-4000:], r.stderr[-2000:])
if r.returncode != 0 or not os.path.exists(out):
    sys.exit('build failed')
sha = hashlib.sha256(open(out, 'rb').read()).hexdigest()
print('ok', out, os.path.getsize(out), 'bytes, SHA-256', sha)
