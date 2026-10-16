// dmxON Setup — установщик dmxON FabKit, dmxON JLC Rules и надстройки dmxON для Autodesk Fusion Electronics.
// Один exe без зависимостей (.NET Framework 4.8 есть в Windows 10/11), без прав администратора: всё ставится
// в профиль пользователя. Берёт последние выпуски с GitHub; без сети — версии, встроенные в exe.
// Ключи: /S (тихо), /uninstall, /lang=xx, /offline. Журнал: %LOCALAPPDATA%\dmxON\setup.log
// dmxON by AlexSmith 2026 · сборка: build.py (csc.exe из .NET Framework)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

[assembly: AssemblyTitle("dmxON Setup")]
[assembly: AssemblyDescription("Installer for dmxON FabKit, dmxON JLC Rules and the dmxON add-in for Autodesk Fusion Electronics")]
[assembly: AssemblyCompany("dmxON by AlexSmith")]
[assembly: AssemblyProduct("dmxON for Autodesk Fusion Electronics")]
[assembly: AssemblyCopyright("dmxON by AlexSmith 2026")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace DmxonSetup
{
    // ------------------------------------------------------------------ тексты на пяти языках
    static class T
    {
        public static readonly string[] Codes = { "en", "ru", "es", "de", "pl" };
        public static readonly string[] Names = { "English", "Русский", "Español", "Deutsch", "Polski" };
        public static int Lang = 0;
        static readonly Dictionary<string, string[]> D = new Dictionary<string, string[]>();

        static void A(string k, string en, string ru, string es, string de, string pl) { D[k] = new[] { en, ru, es, de, pl }; }

        static T()
        {
            A("title", "dmxON for Autodesk Fusion Electronics", "dmxON для Autodesk Fusion Electronics", "dmxON para Autodesk Fusion Electronics", "dmxON für Autodesk Fusion Electronics", "dmxON dla Autodesk Fusion Electronics");
            A("lead", "Installs the latest versions from GitHub into this Windows user — no administrator rights needed.", "Ставит последние версии с GitHub для этого пользователя Windows — права администратора не нужны.", "Instala las últimas versiones desde GitHub para este usuario de Windows, sin derechos de administrador.", "Installiert die neuesten Versionen von GitHub für diesen Windows-Benutzer — ohne Administratorrechte.", "Instaluje najnowsze wersje z GitHuba dla tego użytkownika Windows — bez uprawnień administratora.");
            A("fk_desc", "Fusion board → complete JLCPCB order: Gerber, BOM/CPL with LCSC parts, report, ISO/GOST documentation", "Плата Fusion → полный заказ JLCPCB: Gerber, BOM/CPL с деталями LCSC, отчёт, документация ISO/ГОСТ", "Placa de Fusion → pedido completo a JLCPCB: Gerber, BOM/CPL con piezas LCSC, informe, documentación ISO/GOST", "Fusion-Platine → komplette JLCPCB-Bestellung: Gerber, BOM/CPL mit LCSC-Teilen, Bericht, ISO/GOST-Dokumentation", "Płytka Fusion → pełne zamówienie JLCPCB: Gerber, BOM/CPL z częściami LCSC, raport, dokumentacja ISO/GOST");
            A("jr_desc", "JLCPCB design rules for 2–8 layers and a one-click board setup with DRC", "Правила JLCPCB для 2–8 слоёв и настройка платы в один клик с DRC", "Reglas de diseño JLCPCB de 2 a 8 capas y configuración de la placa en un clic con DRC", "JLCPCB-Designregeln für 2–8 Lagen und Platinen-Setup mit einem Klick inkl. DRC", "Reguły JLCPCB dla 2–8 warstw i konfiguracja płytki jednym kliknięciem z DRC");
            A("ad_desc", "dmxON tab in Fusion: FabKit, JLC Rules, backups, updates from GitHub", "Вкладка dmxON во Fusion: FabKit, JLC Rules, бэкапы, обновления с GitHub", "Pestaña dmxON en Fusion: FabKit, JLC Rules, copias, actualizaciones desde GitHub", "Reiter dmxON in Fusion: FabKit, JLC Rules, Sicherungen, Updates von GitHub", "Karta dmxON w Fusion: FabKit, JLC Rules, kopie, aktualizacje z GitHuba");
            A("ad_name", "dmxON add-in for Fusion", "Надстройка dmxON для Fusion", "Complemento dmxON para Fusion", "dmxON-Add-in für Fusion", "Dodatek dmxON dla Fusion");
            A("required", "required", "обязательно", "obligatorio", "erforderlich", "wymagany");
            A("installed", "installed {0}", "стоит {0}", "instalado {0}", "installiert {0}", "zainstalowano {0}");
            A("latest", "latest {0}", "последняя {0}", "última {0}", "neueste {0}", "najnowsza {0}");
            A("checking", "checking GitHub…", "проверка GitHub…", "consultando GitHub…", "GitHub wird geprüft…", "sprawdzanie GitHuba…");
            A("offline_v", "offline: built-in {0}", "без сети: встроенная {0}", "sin conexión: integrada {0}", "offline: eingebaute {0}", "offline: wbudowana {0}");
            A("fus_ok", "Autodesk Fusion found", "Autodesk Fusion найден", "Autodesk Fusion encontrado", "Autodesk Fusion gefunden", "Znaleziono Autodesk Fusion");
            A("fus_run", "Autodesk Fusion is running — restart it after installing to see the dmxON tab", "Autodesk Fusion запущен — после установки перезапустите его, чтобы появилась вкладка dmxON", "Autodesk Fusion está abierto: reinícielo tras instalar para ver la pestaña dmxON", "Autodesk Fusion läuft — nach der Installation neu starten, dann erscheint der Reiter dmxON", "Autodesk Fusion jest uruchomiony — po instalacji uruchom go ponownie, aby pojawiła się karta dmxON");
            A("fus_no", "Autodesk Fusion not found on this computer — install it from autodesk.com; dmxON is installed anyway and loads with Fusion", "Autodesk Fusion на этом компьютере не найден — поставьте его с autodesk.com; dmxON всё равно установится и загрузится вместе с Fusion", "No se encontró Autodesk Fusion en este equipo: instálelo desde autodesk.com; dmxON se instala igualmente y se cargará con Fusion", "Autodesk Fusion wurde nicht gefunden — von autodesk.com installieren; dmxON wird trotzdem installiert und mit Fusion geladen", "Nie znaleziono Autodesk Fusion — zainstaluj go z autodesk.com; dmxON i tak zostanie zainstalowany i wczyta się razem z Fusion");
            A("where", "Installs to", "Куда", "Destino", "Ziel", "Miejsce");
            A("install", "Install", "Установить", "Instalar", "Installieren", "Zainstaluj");
            A("update", "Update", "Обновить", "Actualizar", "Aktualisieren", "Aktualizuj");
            A("repair", "Reinstall", "Переустановить", "Reinstalar", "Neu installieren", "Zainstaluj ponownie");
            A("remove", "Remove", "Удалить", "Eliminar", "Entfernen", "Usuń");
            A("cancel", "Cancel", "Отмена", "Cancelar", "Abbrechen", "Anuluj");
            A("close", "Close", "Закрыть", "Cerrar", "Schließen", "Zamknij");
            A("back", "Back", "Назад", "Atrás", "Zurück", "Wstecz");
            A("st_fusion", "Check Autodesk Fusion", "Проверка Autodesk Fusion", "Comprobar Autodesk Fusion", "Autodesk Fusion prüfen", "Sprawdzenie Autodesk Fusion");
            A("st_dl", "Download {0}", "Загрузка {0}", "Descargar {0}", "{0} herunterladen", "Pobieranie {0}");
            A("st_verify", "Verify the archives", "Проверка архивов", "Verificar los archivos", "Archive prüfen", "Sprawdzenie archiwów");
            A("st_files", "Install the tools", "Установка инструментов", "Instalar las herramientas", "Werkzeuge installieren", "Instalacja narzędzi");
            A("st_addin", "Install the dmxON add-in", "Установка надстройки dmxON", "Instalar el complemento dmxON", "dmxON-Add-in installieren", "Instalacja dodatku dmxON");
            A("st_reg", "Register in Windows", "Регистрация в Windows", "Registrar en Windows", "In Windows registrieren", "Rejestracja w Windows");
            A("working", "Installing…", "Установка…", "Instalando…", "Installation…", "Instalowanie…");
            A("removing", "Removing…", "Удаление…", "Eliminando…", "Entfernen…", "Usuwanie…");
            A("done", "dmxON is installed", "dmxON установлен", "dmxON está instalado", "dmxON ist installiert", "dmxON jest zainstalowany");
            A("removed", "dmxON is removed", "dmxON удалён", "dmxON se ha eliminado", "dmxON wurde entfernt", "dmxON został usunięty");
            A("failed", "Installation failed", "Установка не удалась", "La instalación falló", "Installation fehlgeschlagen", "Instalacja nie powiodła się");
            A("next1", "Start Autodesk Fusion: the PCB and schematic editors get a dmxON tab (FabKit, JLC Rules, Backup, Updates). On the first start the add-in registers the dmxON folders in Fusion — Automation → Run ULP and DRC → Load… list them.", "Запустите Autodesk Fusion: в редакторах платы и схемы появится вкладка dmxON (FabKit, JLC Rules, Бэкап, Обновления). При первом запуске надстройка зарегистрирует папки dmxON во Fusion — они появятся в Automation → Run ULP и DRC → Load….", "Inicie Autodesk Fusion: los editores de placa y esquema tendrán la pestaña dmxON (FabKit, JLC Rules, Copia, Actualizar). En el primer inicio el complemento registra las carpetas dmxON en Fusion: aparecen en Automation → Run ULP y DRC → Load….", "Autodesk Fusion starten: Platinen- und Schaltplan-Editor erhalten den Reiter dmxON (FabKit, JLC Rules, Sicherung, Updates). Beim ersten Start registriert das Add-in die dmxON-Ordner in Fusion — sie stehen unter Automation → Run ULP und DRC → Load….", "Uruchom Autodesk Fusion: edytory płytki i schematu dostaną kartę dmxON (FabKit, JLC Rules, Kopia, Aktualizacje). Przy pierwszym uruchomieniu dodatek zarejestruje foldery dmxON w Fusion — pojawią się w Automation → Run ULP i DRC → Load….");
            A("next_run", "Fusion is running: close it and start it again to load the dmxON add-in.", "Fusion запущен: закройте его и запустите снова — тогда загрузится надстройка dmxON.", "Fusion está abierto: ciérrelo y vuelva a iniciarlo para cargar el complemento dmxON.", "Fusion läuft: schließen und neu starten, dann wird das dmxON-Add-in geladen.", "Fusion jest uruchomiony: zamknij go i uruchom ponownie, aby wczytać dodatek dmxON.");
            A("next_rm", "Fusion is running: restart it to unload the dmxON tab.", "Fusion запущен: перезапустите его, чтобы вкладка dmxON исчезла.", "Fusion está abierto: reinícielo para quitar la pestaña dmxON.", "Fusion läuft: neu starten, damit der Reiter dmxON verschwindet.", "Fusion jest uruchomiony: uruchom go ponownie, aby karta dmxON zniknęła.");
            A("start_fus","Start Fusion", "Запустить Fusion", "Iniciar Fusion", "Fusion starten", "Uruchom Fusion");
            A("open_dir", "Open folder", "Открыть папку", "Abrir carpeta", "Ordner öffnen", "Otwórz folder");
            A("open_log", "Open log", "Открыть журнал", "Abrir registro", "Protokoll öffnen", "Otwórz dziennik");
            A("rm_q", "Remove dmxON FabKit, dmxON JLC Rules and the dmxON add-in from this computer?", "Удалить dmxON FabKit, dmxON JLC Rules и надстройку dmxON с этого компьютера?", "¿Eliminar dmxON FabKit, dmxON JLC Rules y el complemento dmxON de este equipo?", "dmxON FabKit, dmxON JLC Rules und das dmxON-Add-in von diesem Computer entfernen?", "Usunąć dmxON FabKit, dmxON JLC Rules i dodatek dmxON z tego komputera?");
            A("rm_keep", "Backups (Documents\\dmxON\\Backups) and settings are kept.", "Бэкапы (Документы\\dmxON\\Backups) и настройки остаются.", "Las copias (Documentos\\dmxON\\Backups) y los ajustes se conservan.", "Sicherungen (Dokumente\\dmxON\\Backups) und Einstellungen bleiben erhalten.", "Kopie (Dokumenty\\dmxON\\Backups) i ustawienia zostają.");
            A("src_gh", "GitHub, release {0}", "GitHub, выпуск {0}", "GitHub, versión {0}", "GitHub, Release {0}", "GitHub, wydanie {0}");
            A("src_emb", "built into the installer ({0}) — GitHub is not reachable", "встроенная в установщик ({0}) — GitHub недоступен", "integrada en el instalador ({0}): GitHub no responde", "im Installer enthalten ({0}) — GitHub nicht erreichbar", "wbudowana w instalator ({0}) — GitHub niedostępny");
            A("sha_ok", "SHA-256 matches GitHub", "SHA-256 совпадает с GitHub", "SHA-256 coincide con GitHub", "SHA-256 stimmt mit GitHub überein", "SHA-256 zgodny z GitHubem");
            A("sha_bad", "SHA-256 does not match GitHub — download rejected", "SHA-256 не совпадает с GitHub — загрузка отклонена", "SHA-256 no coincide con GitHub: descarga rechazada", "SHA-256 stimmt nicht mit GitHub überein — Download verworfen", "SHA-256 niezgodny z GitHubem — pobranie odrzucone");
            A("bad_zip", "the archive is damaged or incomplete", "архив повреждён или неполный", "el archivo está dañado o incompleto", "das Archiv ist beschädigt oder unvollständig", "archiwum jest uszkodzone lub niepełne");
            A("kept_user", "{0} own files kept", "сохранено своих файлов: {0}", "{0} archivos propios conservados", "{0} eigene Dateien behalten", "zachowano własnych plików: {0}");
            A("ad_kept", "a newer add-in {0} is already installed — kept", "уже стоит более новая надстройка {0} — оставлена", "ya hay un complemento más nuevo {0}: se conserva", "ein neueres Add-in {0} ist schon installiert — behalten", "nowszy dodatek {0} jest już zainstalowany — pozostawiono");
            A("lang", "Language", "Язык", "Idioma", "Sprache", "Język");
            A("support", "Free and open source. Support new versions: PayPal smith.show2025@gmail.com", "Бесплатно и с открытым кодом. Поддержать новые версии: PayPal smith.show2025@gmail.com", "Gratis y de código abierto. Apoye nuevas versiones: PayPal smith.show2025@gmail.com", "Kostenlos und quelloffen. Neue Versionen unterstützen: PayPal smith.show2025@gmail.com", "Darmowe i otwarte. Wesprzyj nowe wersje: PayPal smith.show2025@gmail.com");
        }

        public static string S(string k, params object[] a)
        {
            string[] v;
            if (!D.TryGetValue(k, out v)) return k;
            string s = v[Lang] ?? v[0];
            return a.Length > 0 ? string.Format(s, a) : s;
        }

        public static void Detect()
        {
            string c = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
            if (c == "uk" || c == "be") c = "ru";
            int i = Array.IndexOf(Codes, c);
            Lang = i >= 0 ? i : 0;
        }
    }

    // ------------------------------------------------------------------ журнал
    static class Log
    {
        public static string Path;
        static readonly object L = new object();
        public static void W(string s)
        {
            lock (L)
            {
                try { File.AppendAllText(Path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + s + "\r\n", Encoding.UTF8); }
                catch { }
            }
        }
    }

    // ------------------------------------------------------------------ один инструмент
    class Tool
    {
        public string Id, Name, Dir, Ulp, Repo, Asset, DescKey;
        public bool Selected = true;
        public string Installed = "", Latest = "", Tag = "", Source = "";
        public byte[] Zip;
        public bool FromGitHub;
    }

    // ------------------------------------------------------------------ установка (без интерфейса)
    class Installer
    {
        public static string Docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string Base = System.IO.Path.Combine(Docs, "dmxON");
        public static string AddIn = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                           "Autodesk", "Autodesk Fusion 360", "API", "AddIns", "dmxON");
        public static string AppDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "dmxON");
        static string UninstKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\dmxON";

        // проверка «как на чистой машине»: всё — в отдельную папку, свой ключ удаления
        public static void Sandbox(string root)
        {
            root = System.IO.Path.GetFullPath(root);
            Docs = System.IO.Path.Combine(root, "Documents");
            Base = System.IO.Path.Combine(Docs, "dmxON");
            AddIn = System.IO.Path.Combine(root, "AppData", "Roaming", "Autodesk", "Autodesk Fusion 360", "API", "AddIns", "dmxON");
            AppDir = System.IO.Path.Combine(root, "AppData", "Local", "dmxON");
            UninstKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\dmxON-sandbox";
        }

        public readonly List<Tool> Tools = new List<Tool>
        {
            new Tool { Id = "fabkit", Name = "dmxON FabKit", Dir = "dmxON-FabKit", Ulp = "dmxON_FabKit.ulp",
                       Repo = "AlexSmith2025/dmxON-FabKit-Fusion-Electronics", Asset = "dmxON-FabKit.zip", DescKey = "fk_desc" },
            new Tool { Id = "jlc", Name = "dmxON JLC Rules", Dir = "dmxON-JLC-Rules", Ulp = "dmxON_JLC_Setup.ulp",
                       Repo = "AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics", Asset = "dmxON-JLC-Rules.zip", DescKey = "jr_desc" },
        };
        public string AddInInstalled = "", AddInNew = "";
        public bool Offline;

        // события для интерфейса
        public Action<int, int, string> Step = delegate { };      // шаг, состояние (1 идёт, 2 готово, 3 ошибка, 4 пропуск), подпись
        public Action<double> Progress = delegate { };

        public Installer()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;   // GitHub: TLS 1.2+ (1.3 не включаем — на старых сборках Windows ломает соединение)
            try { WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials; } catch { }
        }

        // ---------------------------------------------------------------- состояние машины
        public static string FusionExe()
        {
            foreach (string root in new[] {
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Autodesk", "webdeploy", "production"),
                System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Autodesk", "webdeploy", "production") })
            {
                try
                {
                    if (!Directory.Exists(root)) continue;
                    var exe = Directory.GetDirectories(root).Select(d => System.IO.Path.Combine(d, "Fusion360.exe"))
                                       .Where(File.Exists).OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault();
                    if (exe != null) return exe;
                }
                catch { }
            }
            return null;
        }

        public static bool FusionRunning()
        {
            try { return Process.GetProcessesByName("Fusion360").Length > 0; } catch { return false; }
        }

        public static void StartFusion()
        {
            var cand = new List<string>();
            foreach (var sf in new[] { Environment.SpecialFolder.Programs, Environment.SpecialFolder.CommonPrograms, Environment.SpecialFolder.DesktopDirectory })
            {
                string d = Environment.GetFolderPath(sf);
                foreach (string n in new[] { @"Autodesk\Autodesk Fusion.lnk", "Autodesk Fusion.lnk", @"Autodesk\Autodesk Fusion 360.lnk", "Autodesk Fusion 360.lnk" })
                    cand.Add(System.IO.Path.Combine(d, n));
            }
            string wd = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Autodesk", "webdeploy", "production");
            try { if (Directory.Exists(wd)) cand.AddRange(Directory.GetDirectories(wd).Select(x => System.IO.Path.Combine(x, "FusionLauncher.exe"))); } catch { }
            foreach (string c in cand)
            {
                if (!File.Exists(c)) continue;
                try { Process.Start(new ProcessStartInfo(c) { UseShellExecute = true }); Log.W("start Fusion: " + c); return; }
                catch (Exception e) { Log.W("start Fusion failed: " + c + " " + e.Message); }
            }
        }

        static string ReadVersion(string file)
        {
            try { return File.Exists(file) ? File.ReadAllText(file).Trim() : ""; } catch { return ""; }
        }

        static string ManifestVersion(string json)
        {
            var m = Regex.Match(json ?? "", "\"version\"\\s*:\\s*\"([^\"]+)\"");
            return m.Success ? m.Groups[1].Value : "";
        }

        public static int Cmp(string a, string b)
        {
            Func<string, int[]> p = s => (s ?? "").Split('.').Select(x => { int v; return int.TryParse(Regex.Match(x, @"\d+").Value, out v) ? v : 0; }).ToArray();
            int[] x1 = p(a), x2 = p(b);
            for (int i = 0; i < Math.Max(x1.Length, x2.Length); i++)
            {
                int u = i < x1.Length ? x1[i] : 0, w = i < x2.Length ? x2[i] : 0;
                if (u != w) return u.CompareTo(w);
            }
            return 0;
        }

        public bool AnyInstalled
        {
            get { return Tools.Any(t => t.Installed != "") || AddInInstalled != ""; }
        }

        public void ReadInstalled()
        {
            foreach (var t in Tools) t.Installed = ReadVersion(System.IO.Path.Combine(Base, t.Dir, "VERSION"));
            try { AddInInstalled = File.Exists(System.IO.Path.Combine(AddIn, "dmxON.manifest")) ? ManifestVersion(File.ReadAllText(System.IO.Path.Combine(AddIn, "dmxON.manifest"))) : ""; }
            catch { AddInInstalled = ""; }
        }

        // ---------------------------------------------------------------- сеть
        static HttpWebRequest Req(string url)
        {
            var r = (HttpWebRequest)WebRequest.Create(url);
            r.UserAgent = "dmxON-Setup/1.0 (+https://github.com/AlexSmith2025)";
            r.Timeout = 30000;
            r.ReadWriteTimeout = 60000;
            r.AllowAutoRedirect = true;
            r.Proxy = WebRequest.DefaultWebProxy;
            return r;
        }

        static string GetText(string url)
        {
            var r = Req(url);
            r.Accept = "application/vnd.github+json";
            using (var resp = (HttpWebResponse)r.GetResponse())
            using (var sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                return sr.ReadToEnd();
        }

        // тег последнего выпуска без API: github.com/<repo>/releases/latest → …/releases/tag/vX.Y.Z
        public static string LatestTag(string repo)
        {
            // без перехода по редиректу: тег — в заголовке Location (страницу выпуска не грузим)
            var sw = Stopwatch.StartNew();
            var r = Req("https://github.com/" + repo + "/releases/latest");
            r.Method = "HEAD";
            r.AllowAutoRedirect = false;
            using (var resp = (HttpWebResponse)r.GetResponse())
            {
                string loc = resp.Headers["Location"] ?? resp.ResponseUri.AbsoluteUri;
                var m = Regex.Match(loc, @"/releases/tag/([^/?#]+)");
                Log.W("latest " + repo + ": " + (m.Success ? m.Groups[1].Value : "?") + " in " + sw.ElapsedMilliseconds + " ms");
                return m.Success ? Uri.UnescapeDataString(m.Groups[1].Value) : "";
            }
        }

        byte[] Download(string url, Action<double> prog)
        {
            Exception last = null;
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    var r = Req(url);
                    using (var resp = (HttpWebResponse)r.GetResponse())
                    using (var s = resp.GetResponseStream())
                    using (var ms = new MemoryStream())
                    {
                        long total = resp.ContentLength, got = 0;
                        var buf = new byte[65536];
                        int n;
                        while ((n = s.Read(buf, 0, buf.Length)) > 0)
                        {
                            ms.Write(buf, 0, n);
                            got += n;
                            if (total > 0) prog(Math.Min(1.0, got / (double)total));
                        }
                        if (total > 0 && got != total) throw new IOException("incomplete download " + got + "/" + total);
                        return ms.ToArray();
                    }
                }
                catch (Exception e)
                {
                    last = e;
                    Log.W("download " + url + " attempt " + attempt + ": " + e.Message);
                    Thread.Sleep(1500 * attempt);
                }
            }
            throw last;
        }

        // SHA-256 из описания выпуска (поле digest у файла); "" — если GitHub его не дал
        static string GitHubDigest(string repo, string tag, string asset)
        {
            try
            {
                string j = GetText("https://api.github.com/repos/" + repo + "/releases/tags/" + Uri.EscapeDataString(tag));
                var nm = Regex.Match(j, "\"name\"\\s*:\\s*\"" + Regex.Escape(asset) + "\"");
                if (!nm.Success) return "";
                int p = nm.Index;
                int e = j.IndexOf("\"browser_download_url\"", p, StringComparison.Ordinal);
                string seg = e > p ? j.Substring(p, e - p) : j.Substring(p, Math.Min(2000, j.Length - p));
                var m = Regex.Match(seg, "\"digest\"\\s*:\\s*\"sha256:([0-9a-fA-F]{64})\"");
                return m.Success ? m.Groups[1].Value.ToLowerInvariant() : "";
            }
            catch (Exception e) { Log.W("digest " + repo + ": " + e.Message); return ""; }
        }

        static string Sha256(byte[] b)
        {
            using (var h = SHA256.Create()) return BitConverter.ToString(h.ComputeHash(b)).Replace("-", "").ToLowerInvariant();
        }

        static byte[] Embedded(string asset)
        {
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(asset))
            {
                if (s == null) return null;
                using (var ms = new MemoryStream()) { s.CopyTo(ms); return ms.ToArray(); }
            }
        }

        static string ZipVersion(byte[] zip, string dir)
        {
            try
            {
                using (var za = new ZipArchive(new MemoryStream(zip), ZipArchiveMode.Read))
                {
                    var e = za.GetEntry(dir + "/VERSION");
                    if (e == null) return "";
                    using (var sr = new StreamReader(e.Open())) return sr.ReadToEnd().Trim();
                }
            }
            catch { return ""; }
        }

        // что лежит на GitHub (для первой страницы)
        public void ProbeLatest()
        {
            foreach (var t in Tools)
            {
                try { t.Tag = Offline ? "" : LatestTag(t.Repo); }
                catch (Exception e) { t.Tag = ""; Log.W("probe " + t.Repo + ": " + e.Message); }
                if (t.Tag != "") t.Latest = t.Tag.TrimStart('v', 'V');
                else
                {
                    var emb = Embedded(t.Asset);
                    t.Latest = emb != null ? ZipVersion(emb, t.Dir) : "";
                    t.Source = "embedded";
                }
            }
        }

        // ---------------------------------------------------------------- проверка архива
        void Verify(Tool t)
        {
            using (var za = new ZipArchive(new MemoryStream(t.Zip), ZipArchiveMode.Read))
            {
                string pre = t.Dir + "/";
                foreach (var need in new[] { "VERSION", t.Ulp, "INSTALL.ulp", "fusion-addin/dmxON/dmxON.manifest", "fusion-addin/dmxON/dmxON.py" })
                    if (za.GetEntry(pre + need) == null) throw new InvalidDataException(t.Asset + ": " + T.S("bad_zip") + " (" + need + ")");
                foreach (var e in za.Entries)
                {
                    if (!e.FullName.StartsWith(pre, StringComparison.Ordinal) || e.FullName.Contains(".."))
                        throw new InvalidDataException(t.Asset + ": " + T.S("bad_zip") + " (" + e.FullName + ")");
                    if (e.Length > 0) using (var s = e.Open()) { var b = new byte[8192]; while (s.Read(b, 0, b.Length) > 0) { } }   // CRC
                }
            }
        }

        // ---------------------------------------------------------------- установка папки инструмента: целиком, с откатом
        int InstallTool(Tool t)
        {
            Directory.CreateDirectory(Base);
            string target = System.IO.Path.Combine(Base, t.Dir);
            string stage = System.IO.Path.Combine(Base, ".setup_" + t.Dir);
            string old = target + ".old_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (Directory.Exists(stage)) Directory.Delete(stage, true);
            Directory.CreateDirectory(stage);
            using (var za = new ZipArchive(new MemoryStream(t.Zip), ZipArchiveMode.Read))
            {
                foreach (var e in za.Entries)
                {
                    string dst = System.IO.Path.GetFullPath(System.IO.Path.Combine(stage, e.FullName.Replace('/', '\\')));
                    if (!dst.StartsWith(stage + "\\", StringComparison.OrdinalIgnoreCase)) continue;
                    if (e.FullName.EndsWith("/")) { Directory.CreateDirectory(dst); continue; }
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dst));
                    e.ExtractToFile(dst, true);
                }
            }
            string fresh = System.IO.Path.Combine(stage, t.Dir);
            int kept = 0;
            bool hadOld = Directory.Exists(target);
            if (hadOld)
            {
                try { Directory.Move(target, old); }
                catch (Exception e)
                {
                    // папку держит другая программа (Проводник, Fusion) — копируем поверх, свои файлы остаются
                    Log.W("move " + target + " failed (" + e.Message + "), copying over");
                    foreach (string f in Directory.GetFiles(fresh, "*", SearchOption.AllDirectories))
                    {
                        string nf = System.IO.Path.Combine(target, f.Substring(fresh.Length + 1));
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(nf));
                        File.Copy(f, nf, true);
                    }
                    try { Directory.Delete(stage, true); } catch { }
                    return 0;
                }
            }
            try
            {
                Directory.Move(fresh, target);
                if (hadOld)
                {
                    // свои файлы пользователя (например, правила под плату в rules\) переносятся в новую версию
                    foreach (string f in Directory.GetFiles(old, "*", SearchOption.AllDirectories))
                    {
                        string rel = f.Substring(old.Length + 1);
                        string nf = System.IO.Path.Combine(target, rel);
                        if (File.Exists(nf)) continue;
                        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(nf));
                        File.Copy(f, nf);
                        kept++;
                    }
                }
            }
            catch
            {
                if (hadOld && !Directory.Exists(target)) Directory.Move(old, target);
                throw;
            }
            finally
            {
                try { if (Directory.Exists(stage)) Directory.Delete(stage, true); } catch { }
            }
            if (hadOld) try { Directory.Delete(old, true); } catch (Exception e) { Log.W("old folder kept: " + old + " " + e.Message); }
            return kept;
        }

        // надстройка: самая новая из пакетов; более новую уже установленную не трогать
        string InstallAddIn()
        {
            string best = "", bestVer = "";
            foreach (var t in Tools.Where(x => x.Zip != null))
            {
                string src = System.IO.Path.Combine(Base, t.Dir, "fusion-addin", "dmxON");
                string v = ManifestVersion(File.Exists(System.IO.Path.Combine(src, "dmxON.manifest")) ? File.ReadAllText(System.IO.Path.Combine(src, "dmxON.manifest")) : "");
                if (v != "" && (best == "" || Cmp(v, bestVer) > 0)) { best = src; bestVer = v; }
            }
            if (best == "") throw new InvalidDataException("dmxON add-in: " + T.S("bad_zip"));
            AddInNew = bestVer;
            if (AddInInstalled != "" && Cmp(AddInInstalled, bestVer) > 0) return T.S("ad_kept", AddInInstalled);
            string pyc = System.IO.Path.Combine(AddIn, "__pycache__");
            try { if (Directory.Exists(pyc)) Directory.Delete(pyc, true); } catch { }
            foreach (string f in Directory.GetFiles(best, "*", SearchOption.AllDirectories))
            {
                string nf = System.IO.Path.Combine(AddIn, f.Substring(best.Length + 1));
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(nf));
                File.Copy(f, nf, true);
            }
            if (!File.Exists(System.IO.Path.Combine(AddIn, "dmxON.py"))) throw new IOException("dmxON add-in not written: " + AddIn);
            return bestVer;
        }

        // язык вкладки dmxON — как у установщика, если пользователь его ещё не выбирал
        void AddInLanguage()
        {
            string f = System.IO.Path.Combine(Base, "dmxON-addin.json");
            try
            {
                string j = File.Exists(f) ? File.ReadAllText(f, Encoding.UTF8) : "";
                if (Regex.IsMatch(j, "\"lang\"\\s*:\\s*\"[a-z]{2}\"")) return;
                if (j.Trim() == "" || !j.TrimStart().StartsWith("{")) j = "{\n \"lang\": \"" + T.Codes[T.Lang] + "\"\n}\n";
                else if (Regex.IsMatch(j, "\"lang\"\\s*:\\s*\"\"")) j = Regex.Replace(j, "\"lang\"\\s*:\\s*\"\"", "\"lang\": \"" + T.Codes[T.Lang] + "\"");
                else j = j.TrimStart().Substring(1).Trim() == "}" ? "{\n \"lang\": \"" + T.Codes[T.Lang] + "\"\n}\n" : "{\n \"lang\": \"" + T.Codes[T.Lang] + "\"," + j.TrimStart().Substring(1);
                File.WriteAllText(f, j, new UTF8Encoding(false));
            }
            catch (Exception e) { Log.W("settings: " + e.Message); }
        }

        static long DirSizeKb(string d)
        {
            try { return Directory.GetFiles(d, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length) / 1024; } catch { return 0; }
        }

        void RegisterUninstall()
        {
            Directory.CreateDirectory(AppDir);
            string self = Assembly.GetExecutingAssembly().Location;
            string copy = System.IO.Path.Combine(AppDir, "dmxON-Setup.exe");
            if (!string.Equals(System.IO.Path.GetFullPath(self), System.IO.Path.GetFullPath(copy), StringComparison.OrdinalIgnoreCase))
                File.Copy(self, copy, true);
            var fk = Tools.First(t => t.Id == "fabkit");
            string ver = ReadVersion(System.IO.Path.Combine(Base, fk.Dir, "VERSION"));
            using (var k = Registry.CurrentUser.CreateSubKey(UninstKey))
            {
                k.SetValue("DisplayName", "dmxON for Autodesk Fusion Electronics");
                k.SetValue("DisplayVersion", ver);
                k.SetValue("Publisher", "dmxON by AlexSmith");
                k.SetValue("DisplayIcon", copy + ",0");
                k.SetValue("InstallLocation", Base);
                k.SetValue("UninstallString", "\"" + copy + "\" /uninstall");
                k.SetValue("QuietUninstallString", "\"" + copy + "\" /uninstall /S");
                k.SetValue("ModifyPath", "\"" + copy + "\"");
                k.SetValue("URLInfoAbout", "https://github.com/" + fk.Repo);
                k.SetValue("HelpLink", "https://github.com/" + fk.Repo);
                k.SetValue("NoRepair", 1, RegistryValueKind.DWord);
                k.SetValue("EstimatedSize", (int)(DirSizeKb(Base) + DirSizeKb(AddIn) + 1), RegistryValueKind.DWord);
                k.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
            }
        }

        // ---------------------------------------------------------------- полный цикл установки
        // шаги: 0 Fusion, 1..n загрузки, n+1 проверка, n+2 файлы, n+3 надстройка, n+4 регистрация
        public List<string> Notes = new List<string>();

        public void Install()
        {
            var sel = Tools.Where(t => t.Selected).ToList();
            int n = Tools.Count;
            Notes.Clear();
            Log.W("install: " + string.Join(", ", sel.Select(t => t.Name)) + (Offline ? " (offline)" : ""));

            Step(0, 1, "");
            string fus = FusionExe();
            Step(0, 2, fus != null ? (FusionRunning() ? T.S("fus_run") : T.S("fus_ok")) : T.S("fus_no"));
            Log.W("fusion: " + (fus ?? "not found") + (FusionRunning() ? " (running)" : ""));

            for (int i = 0; i < n; i++)
            {
                var t = Tools[i];
                if (!t.Selected) { Step(1 + i, 4, ""); continue; }
                Step(1 + i, 1, "");
                t.Zip = null;
                t.FromGitHub = false;
                if (!Offline)
                {
                    try
                    {
                        if (t.Tag == "") t.Tag = LatestTag(t.Repo);
                        string url = "https://github.com/" + t.Repo + "/releases/download/" + Uri.EscapeDataString(t.Tag) + "/" + t.Asset;
                        int ii = i;
                        var sw = Stopwatch.StartNew();
                        t.Zip = Download(url, p => Progress((ii + p) / n));
                        Log.W("downloaded " + t.Asset + " " + t.Tag + ": " + t.Zip.Length + " bytes in " + sw.ElapsedMilliseconds + " ms");
                        sw.Restart();
                        string dg = GitHubDigest(t.Repo, t.Tag, t.Asset);
                        Log.W("sha256 " + (dg != "" ? (dg == Sha256(t.Zip) ? "ok" : "MISMATCH") : "not published") + " in " + sw.ElapsedMilliseconds + " ms");
                        if (dg != "" && dg != Sha256(t.Zip))
                        {
                            Log.W(t.Asset + ": sha256 mismatch " + Sha256(t.Zip) + " != " + dg);
                            throw new InvalidDataException(T.S("sha_bad"));
                        }
                        t.FromGitHub = true;
                        t.Source = T.S("src_gh", t.Tag) + (dg != "" ? " · " + T.S("sha_ok") : "");
                    }
                    catch (Exception e) { Log.W(t.Asset + ": " + e.Message); t.Zip = null; }
                }
                if (t.Zip == null)
                {
                    t.Zip = Embedded(t.Asset);
                    if (t.Zip == null) throw new IOException(t.Name + ": GitHub is not reachable and no built-in copy");
                    t.Source = T.S("src_emb", ZipVersion(t.Zip, t.Dir));
                }
                Step(1 + i, 2, t.Source);
            }
            Progress(1);

            Step(n + 1, 1, "");
            foreach (var t in sel) Verify(t);
            Step(n + 1, 2, string.Join(" · ", sel.Select(t => t.Name + " " + ZipVersion(t.Zip, t.Dir))));

            Step(n + 2, 1, "");
            var msg = new List<string>();
            foreach (var t in sel)
            {
                int kept = InstallTool(t);
                t.Installed = ReadVersion(System.IO.Path.Combine(Base, t.Dir, "VERSION"));
                msg.Add(t.Name + " " + t.Installed + (kept > 0 ? " (" + T.S("kept_user", kept) + ")" : ""));
                Log.W("installed " + t.Name + " " + t.Installed + " → " + System.IO.Path.Combine(Base, t.Dir) + ", kept " + kept);
            }
            Step(n + 2, 2, string.Join(" · ", msg));

            Step(n + 3, 1, "");
            string ad = InstallAddIn();
            AddInLanguage();
            ReadInstalled();
            Step(n + 3, 2, ad);
            Log.W("add-in " + AddInInstalled + " → " + AddIn);

            Step(n + 4, 1, "");
            RegisterUninstall();
            Step(n + 4, 2, "");
            Log.W("done");
        }

        // ---------------------------------------------------------------- удаление
        public void Uninstall()
        {
            Log.W("uninstall");
            foreach (var t in Tools)
            {
                string d = System.IO.Path.Combine(Base, t.Dir);
                try { if (Directory.Exists(d)) Directory.Delete(d, true); Log.W("removed " + d); }
                catch (Exception e) { Log.W("remove " + d + ": " + e.Message); }
            }
            try { if (Directory.Exists(AddIn)) Directory.Delete(AddIn, true); Log.W("removed " + AddIn); }
            catch (Exception e) { Log.W("remove add-in: " + e.Message); }
            foreach (string f in new[] { ".dmxon-addin" })
                try { File.Delete(System.IO.Path.Combine(Base, f)); } catch { }
            try { Registry.CurrentUser.DeleteSubKeyTree(UninstKey, false); } catch { }
            // копия установщика удаляет себя после выхода
            string copy = System.IO.Path.Combine(AppDir, "dmxON-Setup.exe");
            if (File.Exists(copy))
            {
                try
                {
                    Process.Start(new ProcessStartInfo("cmd.exe", "/c ping 127.0.0.1 -n 3 >nul & del /f /q \"" + copy + "\"")
                    { CreateNoWindow = true, UseShellExecute = false, WindowStyle = ProcessWindowStyle.Hidden });
                }
                catch { }
            }
            ReadInstalled();
        }
    }

    // ------------------------------------------------------------------ оформление
    static class Th
    {
        public static readonly Color Bg = Color.FromArgb(28, 35, 43), Side = Color.FromArgb(23, 58, 50), Card = Color.FromArgb(36, 45, 55),
            CardHi = Color.FromArgb(44, 55, 67), Cu = Color.FromArgb(180, 116, 74), CuHi = Color.FromArgb(226, 171, 127),
            Mask = Color.FromArgb(36, 87, 74), Text = Color.FromArgb(243, 238, 231), Dim = Color.FromArgb(160, 170, 178),
            Line = Color.FromArgb(58, 70, 82), Ok = Color.FromArgb(124, 196, 150), Warn = Color.FromArgb(226, 171, 127), Bad = Color.FromArgb(232, 112, 104);
        public static float K = 1f;
        public static int P(float v) { return (int)Math.Round(v * K); }
        public static Font F(float size, FontStyle st = FontStyle.Regular) { return new Font("Segoe UI", size, st, GraphicsUnit.Point); }
        public static Font Semi(float size) { return new Font("Segoe UI Semibold", size, FontStyle.Regular, GraphicsUnit.Point); }

        public static GraphicsPath Round(RectangleF r, float rad)
        {
            var p = new GraphicsPath();
            float d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public static void Hq(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }
    }

    class FlatButton : Control
    {
        public bool Primary;
        bool hover, down;
        public FlatButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Cursor = Cursors.Hand;
            Font = Th.Semi(10);
            BackColor = Color.Transparent;
        }
        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; down = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { down = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { down = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Th.Hq(g);
            g.Clear(Parent != null ? Parent.BackColor : Th.Bg);
            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (var p = Th.Round(r, Th.P(8)))
            {
                Color fill = Primary ? (down ? Th.Cu : hover ? Th.CuHi : Th.Cu) : (hover ? Th.CardHi : Th.Card);
                if (!Enabled) fill = Th.Card;
                using (var b = new SolidBrush(fill)) g.FillPath(b, p);
                if (!Primary) using (var pen = new Pen(hover ? Th.Cu : Th.Line, Th.K)) g.DrawPath(pen, p);
            }
            Color tc = !Enabled ? Th.Dim : Primary ? Color.FromArgb(28, 22, 18) : Th.Text;
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, tc, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }
    }

    // карточка инструмента с переключателем
    class ToolCard : Control
    {
        public string Title = "", Desc = "", Badge = "", Badge2 = "";
        public bool Checked = true, Locked;
        public event EventHandler Toggled = delegate { };
        bool hover;
        public ToolCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
        }
        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); }
        protected override void OnClick(EventArgs e)
        {
            if (Locked) return;
            Checked = !Checked; Invalidate(); Toggled(this, EventArgs.Empty);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Th.Hq(g);
            g.Clear(Th.Bg);
            var r = new RectangleF(0.5f, 0.5f, Width - 1.5f, Height - 1.5f);
            using (var p = Th.Round(r, Th.P(10)))
            {
                using (var b = new SolidBrush(hover && !Locked ? Th.CardHi : Th.Card)) g.FillPath(b, p);
                using (var pen = new Pen(Checked ? Th.Cu : Th.Line, Th.K)) g.DrawPath(pen, p);
            }
            // флажок
            int s = Th.P(20), x = Th.P(16), y = Th.P(16);
            var box = new RectangleF(x, y, s, s);
            using (var p = Th.Round(box, Th.P(5)))
            {
                if (Checked) using (var b = new SolidBrush(Locked ? Th.Mask : Th.Cu)) g.FillPath(b, p);
                using (var pen = new Pen(Checked ? (Locked ? Th.Mask : Th.Cu) : Th.Dim, 1.5f * Th.K)) g.DrawPath(pen, p);
            }
            if (Checked)
                using (var pen = new Pen(Th.Text, 2.2f * Th.K) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                    g.DrawLines(pen, new[] { new PointF(x + s * 0.24f, y + s * 0.52f), new PointF(x + s * 0.43f, y + s * 0.71f), new PointF(x + s * 0.78f, y + s * 0.3f) });
            int tx = x + s + Th.P(14);
            using (var f = Th.Semi(11)) TextRenderer.DrawText(g, Title, f, new Point(tx, Th.P(12)), Th.Text);
            int bx = Width - Th.P(14);
            foreach (var bt in new[] { Badge2, Badge })
            {
                if (string.IsNullOrEmpty(bt)) continue;
                using (var f = Th.F(8.5f))
                {
                    var sz = TextRenderer.MeasureText(bt, f);
                    var br = new RectangleF(bx - sz.Width - Th.P(12), Th.P(12), sz.Width + Th.P(12), Th.P(22));
                    using (var p = Th.Round(br, Th.P(11))) using (var b = new SolidBrush(bt == Badge ? Th.Mask : Th.Line)) g.FillPath(b, p);
                    TextRenderer.DrawText(g, bt, f, Rectangle.Round(br), Th.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    bx = (int)br.X - Th.P(6);
                }
            }
            using (var f = Th.F(9))
                TextRenderer.DrawText(g, Desc, f, new Rectangle(tx, Th.P(38), Width - tx - Th.P(14), Height - Th.P(42)), Th.Dim, TextFormatFlags.WordBreak);
        }
    }

    // список шагов установки
    class StepList : Control
    {
        public List<string> Names = new List<string>();
        public List<int> State = new List<int>();          // 0 ждёт, 1 идёт, 2 готово, 3 ошибка, 4 пропуск
        public List<string> Notes = new List<string>();
        public double Prog;
        float spin;
        readonly System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer { Interval = 40 };
        public StepList()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            tm.Tick += (s, e) => { spin += 12; Invalidate(); };
            tm.Start();
        }
        protected override void Dispose(bool d) { if (d) tm.Dispose(); base.Dispose(d); }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Th.Hq(g);
            g.Clear(Th.Bg);
            int row = Th.P(46), y = 0;
            for (int i = 0; i < Names.Count; i++, y += row)
            {
                int st = State[i];
                float cx = Th.P(14), cy = y + Th.P(14), rr = Th.P(10);
                var c = new RectangleF(cx - rr, cy - rr, rr * 2, rr * 2);
                if (st == 2) { using (var b = new SolidBrush(Th.Cu)) g.FillEllipse(b, c);
                    using (var pen = new Pen(Th.Bg, 2f * Th.K) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                        g.DrawLines(pen, new[] { new PointF(cx - rr * 0.45f, cy), new PointF(cx - rr * 0.08f, cy + rr * 0.38f), new PointF(cx + rr * 0.5f, cy - rr * 0.38f) }); }
                else if (st == 3) { using (var b = new SolidBrush(Th.Bad)) g.FillEllipse(b, c);
                    using (var pen = new Pen(Th.Bg, 2f * Th.K)) { g.DrawLine(pen, cx - rr * 0.4f, cy - rr * 0.4f, cx + rr * 0.4f, cy + rr * 0.4f); g.DrawLine(pen, cx + rr * 0.4f, cy - rr * 0.4f, cx - rr * 0.4f, cy + rr * 0.4f); } }
                else if (st == 1) { using (var pen = new Pen(Th.Line, 2.5f * Th.K)) g.DrawEllipse(pen, c);
                    using (var pen = new Pen(Th.CuHi, 2.5f * Th.K) { StartCap = LineCap.Round, EndCap = LineCap.Round }) g.DrawArc(pen, c, spin, 100); }
                else using (var pen = new Pen(st == 4 ? Th.Line : Th.Dim, 1.5f * Th.K)) g.DrawEllipse(pen, c);
                Color tc = st == 0 || st == 4 ? Th.Dim : Th.Text;
                using (var f = st == 1 ? Th.Semi(10.5f) : Th.F(10.5f)) TextRenderer.DrawText(g, Names[i], f, new Point(Th.P(36), y + Th.P(3)), tc);
                if (!string.IsNullOrEmpty(Notes[i]))
                    using (var f = Th.F(8.5f))
                        TextRenderer.DrawText(g, Notes[i], f, new Rectangle(Th.P(36), y + Th.P(23), Width - Th.P(40), Th.P(20)), st == 3 ? Th.Bad : Th.Dim, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
            }
            // общая полоса прогресса
            var bar = new RectangleF(Th.P(2), Height - Th.P(10), Width - Th.P(4), Th.P(8));
            using (var p = Th.Round(bar, Th.P(4))) using (var b = new SolidBrush(Th.Card)) g.FillPath(b, p);
            if (Prog > 0.002)
            {
                var fill = new RectangleF(bar.X, bar.Y, Math.Max(bar.Height, (float)(bar.Width * Prog)), bar.Height);
                using (var p = Th.Round(fill, Th.P(4)))
                using (var b = new LinearGradientBrush(fill, Th.Cu, Th.CuHi, 0f)) g.FillPath(b, p);
            }
        }
    }

    // ------------------------------------------------------------------ окно
    class MainForm : Form
    {
        [DllImport("dwmapi.dll")] static extern int DwmSetWindowAttribute(IntPtr h, int a, ref int v, int s);

        readonly Installer inst;
        readonly Panel side = new Panel(), body = new Panel();
        readonly Image logo;
        int page;       // 0 выбор, 1 установка, 2 готово, 3 ошибка, 4 удалено
        string err = "";
        ToolCard[] cards;
        StepList steps;
        Label status;
        bool busy;

        public MainForm(Installer i)
        {
            inst = i;
            using (var g = CreateGraphics()) Th.K = g.DpiX / 96f;
            AutoScaleMode = AutoScaleMode.None;
            Text = "dmxON Setup";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(Th.P(820), Th.P(560));
            BackColor = Th.Bg;
            ForeColor = Th.Text;
            Font = Th.F(9.5f);
            try { Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location); } catch { }
            try { using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("setup_logo.png")) if (s != null) logo = Image.FromStream(s); } catch { }
            side.SetBounds(0, 0, Th.P(250), ClientSize.Height);
            side.BackColor = Th.Side;
            side.Paint += PaintSide;
            body.SetBounds(Th.P(250), 0, ClientSize.Width - Th.P(250), ClientSize.Height);
            body.BackColor = Th.Bg;
            Controls.Add(body);
            Controls.Add(side);
            Show0();
            Shown += (s, e) => Probe();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try { int on = 1; if (DwmSetWindowAttribute(Handle, 20, ref on, 4) != 0) DwmSetWindowAttribute(Handle, 19, ref on, 4); } catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (busy) e.Cancel = true;
            base.OnFormClosing(e);
        }

        void PaintSide(object s, PaintEventArgs e)
        {
            var g = e.Graphics;
            Th.Hq(g);
            int x = Th.P(28);
            if (logo != null) g.DrawImage(logo, new Rectangle(x, Th.P(32), Th.P(64), Th.P(64)));
            using (var f = new Font("Segoe UI Black", 26, FontStyle.Regular, GraphicsUnit.Point))
            {
                TextRenderer.DrawText(g, "dmx", f, new Point(x - Th.P(4), Th.P(108)), Th.Text);
                var w = TextRenderer.MeasureText(g, "dmx", f, Size.Empty, TextFormatFlags.NoPadding).Width;
                TextRenderer.DrawText(g, "ON", f, new Point(x - Th.P(4) + w + Th.P(2), Th.P(108)), Th.CuHi);
            }
            using (var f = Th.F(9.5f))
                TextRenderer.DrawText(g, "FabKit · JLC Rules · add-in\nAutodesk Fusion Electronics", f, new Rectangle(x, Th.P(160), side.Width - x - Th.P(16), Th.P(48)), Th.Dim, TextFormatFlags.WordBreak);
            // этапы
            bool rm = page >= 4;
            string[] st = rm ? new[] { T.S("remove"), T.S("removing"), T.S("removed") } : new[] { T.S("install"), T.S("working"), T.S("done") };
            int y = Th.P(250);
            for (int i = 0; i < 3; i++, y += Th.P(36))
            {
                int cur = page == 0 || page == 5 ? 0 : page == 1 || page == 6 ? 1 : 2;
                bool on = i == cur, past = i < cur;
                using (var b = new SolidBrush(on ? Th.CuHi : past ? Th.Cu : Color.FromArgb(70, 120, 105)))
                    g.FillEllipse(b, x, y + Th.P(6), Th.P(10), Th.P(10));
                using (var f = on ? Th.Semi(10.5f) : Th.F(10.5f))
                    TextRenderer.DrawText(g, st[i].TrimEnd('…', '.'), f, new Point(x + Th.P(22), y), on ? Th.Text : Color.FromArgb(170, 200, 190));
            }
            using (var f = Th.F(8))
                TextRenderer.DrawText(g, "dmxON by AlexSmith 2026\nsetup " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                                      f, new Rectangle(x, side.Height - Th.P(58), side.Width - x, Th.P(40)), Color.FromArgb(150, 185, 175), TextFormatFlags.WordBreak);
        }

        Label L(string text, Font f, Color c, int x, int y, int w, int h)
        {
            var l = new Label { Text = text, Font = f, ForeColor = c, BackColor = Color.Transparent, AutoSize = false };
            l.SetBounds(x, y, w, h);
            body.Controls.Add(l);
            return l;
        }

        FlatButton B(string text, bool primary, int x, int y, int w, EventHandler click)
        {
            var b = new FlatButton { Text = text, Primary = primary };
            b.SetBounds(x, y, w, Th.P(40));
            b.Click += click;
            body.Controls.Add(b);
            return b;
        }

        void Clear()
        {
            foreach (Control c in body.Controls.Cast<Control>().ToList()) { body.Controls.Remove(c); c.Dispose(); }
            side.Invalidate();
        }

        int W { get { return body.Width - Th.P(64); } }

        // ---------------------------------------------------------------- страница 0: выбор
        void Show0()
        {
            page = 0;
            Clear();
            int x = Th.P(32), y = Th.P(28);
            L(T.S("title"), Th.Semi(17), Th.Text, x, y, W - Th.P(150), Th.P(70));
            var lang = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, BackColor = Th.Card, ForeColor = Th.Text, Font = Th.F(9.5f),
                                      DrawMode = DrawMode.OwnerDrawFixed, ItemHeight = Th.P(22), TabStop = false };
            lang.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                bool hi = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;
                using (var b = new SolidBrush(hi ? Th.Mask : Th.Card)) e.Graphics.FillRectangle(b, e.Bounds);
                TextRenderer.DrawText(e.Graphics, T.Names[e.Index], lang.Font, new Rectangle(e.Bounds.X + Th.P(6), e.Bounds.Y, e.Bounds.Width, e.Bounds.Height),
                                      Th.Text, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
            };
            lang.Items.AddRange(T.Names);
            lang.SelectedIndex = T.Lang;
            lang.SetBounds(body.Width - Th.P(32) - Th.P(130), y + Th.P(4), Th.P(130), Th.P(28));
            lang.SelectedIndexChanged += (s, e) => { T.Lang = lang.SelectedIndex; Show0(); };
            body.Controls.Add(lang);
            L(T.S("lead"), Th.F(10), Th.Dim, x, y + Th.P(74), W, Th.P(40));
            y += Th.P(122);
            cards = new ToolCard[3];
            for (int i = 0; i < 3; i++)
            {
                var c = new ToolCard();
                if (i < 2)
                {
                    var t = inst.Tools[i];
                    c.Title = t.Name; c.Desc = T.S(t.DescKey); c.Checked = t.Selected;
                    c.Badge = t.Latest == "" ? T.S("checking") : (t.Source == "embedded" ? T.S("offline_v", t.Latest) : T.S("latest", t.Latest));
                    c.Badge2 = t.Installed != "" ? T.S("installed", t.Installed) : "";
                    int ii = i;
                    c.Toggled += (s, e) => { inst.Tools[ii].Selected = ((ToolCard)s).Checked; UpdateMain(); };
                }
                else
                {
                    c.Title = T.S("ad_name"); c.Desc = T.S("ad_desc"); c.Checked = true; c.Locked = true; c.Cursor = Cursors.Default;
                    c.Badge = T.S("required");
                    c.Badge2 = inst.AddInInstalled != "" ? T.S("installed", inst.AddInInstalled) : "";
                }
                c.SetBounds(x, y, W, Th.P(78));
                body.Controls.Add(c);
                cards[i] = c;
                y += Th.P(88);
            }
            string fus = Installer.FusionExe();
            bool run = Installer.FusionRunning();
            status = L((fus == null ? "⚠  " + T.S("fus_no") : run ? "●  " + T.S("fus_run") : "✓  " + T.S("fus_ok")) + "\n" + T.S("where") + ": " + Installer.Base,
                       Th.F(9), fus == null ? Th.Warn : run ? Th.Warn : Th.Ok, x, y + Th.P(2), W, Th.P(64));
            int by = body.Height - Th.P(64);
            mainBtn = B("", true, body.Width - Th.P(32) - Th.P(170), by, Th.P(170), (s, e) => Run());
            if (inst.AnyInstalled) B(T.S("remove"), false, x, by, Th.P(130), (s, e) => AskRemove());
            B(T.S("cancel"), false, body.Width - Th.P(32) - Th.P(170) - Th.P(12) - Th.P(120), by, Th.P(120), (s, e) => Close());
            UpdateMain();
        }

        FlatButton mainBtn;

        void UpdateMain()
        {
            if (mainBtn == null) return;
            bool any = inst.Tools.Any(t => t.Selected);
            bool upd = inst.Tools.Any(t => t.Selected && t.Installed != "" && t.Latest != "" && Installer.Cmp(t.Latest, t.Installed) > 0);
            bool all = inst.Tools.Where(t => t.Selected).All(t => t.Installed != "");
            mainBtn.Text = upd ? T.S("update") : (all && any ? T.S("repair") : T.S("install"));
            mainBtn.Enabled = any;
        }

        void Probe()
        {
            new Thread(() =>
            {
                inst.ProbeLatest();
                BeginInvoke((Action)(() =>
                {
                    if (page != 0) return;
                    for (int i = 0; i < 2; i++)
                    {
                        var t = inst.Tools[i];
                        cards[i].Badge = t.Latest == "" ? "?" : (t.Source == "embedded" ? T.S("offline_v", t.Latest) : T.S("latest", t.Latest));
                        cards[i].Invalidate();
                    }
                    UpdateMain();
                }));
            }) { IsBackground = true }.Start();
        }

        // ---------------------------------------------------------------- страница 1: установка
        void Run()
        {
            page = 1;
            busy = true;
            Clear();
            int x = Th.P(32), y = Th.P(28);
            L(T.S("working"), Th.Semi(17), Th.Text, x, y, W, Th.P(44));
            steps = new StepList();
            steps.Names.Add(T.S("st_fusion"));
            foreach (var t in inst.Tools) steps.Names.Add(T.S("st_dl", t.Name));
            steps.Names.AddRange(new[] { T.S("st_verify"), T.S("st_files"), T.S("st_addin"), T.S("st_reg") });
            foreach (var n in steps.Names) { steps.State.Add(0); steps.Notes.Add(""); }
            steps.SetBounds(x, y + Th.P(64), W, body.Height - Th.P(64) - Th.P(110));
            body.Controls.Add(steps);
            inst.Step = (i, st, note) => BeginInvoke((Action)(() => { steps.State[i] = st; steps.Notes[i] = note ?? ""; steps.Invalidate(); }));
            inst.Progress = p => BeginInvoke((Action)(() => { steps.Prog = p; steps.Invalidate(); }));
            new Thread(() =>
            {
                try
                {
                    inst.Install();
                    BeginInvoke((Action)(() => { busy = false; Show2(); }));
                }
                catch (Exception ex)
                {
                    Log.W("ERROR " + ex);
                    BeginInvoke((Action)(() =>
                    {
                        busy = false;
                        int cur = steps.State.FindIndex(s2 => s2 == 1);
                        if (cur >= 0) { steps.State[cur] = 3; steps.Notes[cur] = ex.Message; }
                        err = ex.Message;
                        Show3();
                    }));
                }
            }) { IsBackground = true }.Start();
        }

        // ---------------------------------------------------------------- страница 2: готово
        void Show2()
        {
            page = 2;
            Clear();
            int x = Th.P(32), y = Th.P(28);
            L("✓  " + T.S("done"), Th.Semi(17), Th.Ok, x, y, W, Th.P(44));
            y += Th.P(60);
            foreach (var t in inst.Tools.Where(t => t.Selected))
            {
                L(t.Name + "  " + t.Installed, Th.Semi(11), Th.Text, x, y, W, Th.P(24));
                L(t.Source, Th.F(8.5f), Th.Dim, x, y + Th.P(22), W, Th.P(20)).AutoEllipsis = true;
                y += Th.P(50);
            }
            L(T.S("ad_name") + "  " + inst.AddInInstalled, Th.Semi(11), Th.Text, x, y, W, Th.P(24));
            L(T.S("ad_desc"), Th.F(8.5f), Th.Dim, x, y + Th.P(22), W, Th.P(20)).AutoEllipsis = true;
            y += Th.P(50);
            L(T.S("where") + ":  " + Installer.Base, Th.F(8.5f), Th.Dim, x, y, W, Th.P(20)).AutoEllipsis = true;
            y += Th.P(36);
            bool run = Installer.FusionRunning();
            L(run ? T.S("next_run") : T.S("next1"), Th.F(10), run ? Th.Warn : Th.Text, x, y, W, Th.P(90));
            L(T.S("support"), Th.F(8.5f), Th.Dim, x, body.Height - Th.P(110), W, Th.P(36));
            int by = body.Height - Th.P(64);
            B(T.S("close"), !(Installer.FusionExe() != null && !run), body.Width - Th.P(32) - Th.P(130), by, Th.P(130), (s, e) => Close());
            if (Installer.FusionExe() != null && !run)
                B(T.S("start_fus"), true, body.Width - Th.P(32) - Th.P(130) - Th.P(12) - Th.P(170), by, Th.P(170), (s, e) => { Installer.StartFusion(); Close(); });
            B(T.S("open_dir"), false, x, by, Th.P(150), (s, e) => Process.Start(new ProcessStartInfo(Installer.Base) { UseShellExecute = true }));
        }

        // ---------------------------------------------------------------- страница 3: ошибка
        void Show3()
        {
            page = 3;
            var keep = steps;
            body.Controls.Remove(keep);
            Clear();
            int x = Th.P(32), y = Th.P(28);
            L("✕  " + T.S("failed"), Th.Semi(17), Th.Bad, x, y, W, Th.P(44));
            keep.SetBounds(x, y + Th.P(64), W, body.Height - Th.P(64) - Th.P(150));
            body.Controls.Add(keep);
            L(err, Th.F(9), Th.Bad, x, body.Height - Th.P(128), W, Th.P(56));
            int by = body.Height - Th.P(64);
            B(T.S("close"), false, body.Width - Th.P(32) - Th.P(130), by, Th.P(130), (s, e) => Close());
            B(T.S("back"), true, body.Width - Th.P(32) - Th.P(130) - Th.P(12) - Th.P(140), by, Th.P(140), (s, e) => { inst.ReadInstalled(); Show0(); });
            B(T.S("open_log"), false, x, by, Th.P(150), (s, e) => { try { Process.Start(new ProcessStartInfo(Log.Path) { UseShellExecute = true }); } catch { } });
        }

        // ---------------------------------------------------------------- удаление
        // страницы удаления: 5 подтверждение, 6 идёт, 4 готово
        public void AskRemove()
        {
            page = 5;
            Clear();
            int x = Th.P(32), y = Th.P(28);
            L(T.S("remove"), Th.Semi(17), Th.Text, x, y, W, Th.P(44));
            L(T.S("rm_q"), Th.F(11), Th.Text, x, y + Th.P(64), W, Th.P(56));
            y += Th.P(130);
            foreach (var t in inst.Tools.Where(t => t.Installed != ""))
            {
                L("•  " + t.Name + "  " + t.Installed + "   " + System.IO.Path.Combine(Installer.Base, t.Dir), Th.F(9), Th.Dim, x, y, W, Th.P(22)).AutoEllipsis = true;
                y += Th.P(26);
            }
            if (inst.AddInInstalled != "")
            {
                L("•  " + T.S("ad_name") + "  " + inst.AddInInstalled + "   " + Installer.AddIn, Th.F(9), Th.Dim, x, y, W, Th.P(22)).AutoEllipsis = true;
                y += Th.P(26);
            }
            L(T.S("rm_keep"), Th.F(9.5f), Th.Ok, x, y + Th.P(12), W, Th.P(40));
            int by = body.Height - Th.P(64);
            B(T.S("remove"), true, body.Width - Th.P(32) - Th.P(150), by, Th.P(150), (s, e) => DoRemove());
            B(T.S("back"), false, body.Width - Th.P(32) - Th.P(150) - Th.P(12) - Th.P(120), by, Th.P(120), (s, e) => Show0());
        }

        void DoRemove()
        {
            page = 6;
            busy = true;
            Clear();
            L(T.S("removing"), Th.Semi(17), Th.Text, Th.P(32), Th.P(28), W, Th.P(44));
            new Thread(() =>
            {
                inst.Uninstall();
                BeginInvoke((Action)(() =>
                {
                    busy = false;
                    page = 4;
                    Clear();
                    L("✓  " + T.S("removed"), Th.Semi(17), Th.Ok, Th.P(32), Th.P(28), W, Th.P(44));
                    L(T.S("rm_keep") + (Installer.FusionRunning() ? "\n\n" + T.S("next_rm") : ""), Th.F(10), Th.Dim, Th.P(32), Th.P(96), W, Th.P(80));
                    B(T.S("close"), true, body.Width - Th.P(32) - Th.P(130), body.Height - Th.P(64), Th.P(130), (s, e) => Close());
                }));
            }) { IsBackground = true }.Start();
        }
    }

    // ------------------------------------------------------------------ вход
    static class Program
    {
        [DllImport("user32.dll")] static extern bool SetProcessDPIAware();

        [STAThread]
        static int Main(string[] args)
        {
            T.Detect();
            bool quiet = false, uninstall = false, offline = false;
            foreach (var a0 in args)
            {
                string a = a0.ToLowerInvariant().TrimStart('/', '-');
                if (a.StartsWith("sandbox=")) Installer.Sandbox(a0.Substring(a0.IndexOf('=') + 1).Trim('"'));
                else if (a == "s" || a == "silent" || a == "quiet" || a == "verysilent") quiet = true;
                else if (a == "uninstall" || a == "remove") uninstall = true;
                else if (a == "offline") offline = true;
                else if (a.StartsWith("lang="))
                {
                    int i = Array.IndexOf(T.Codes, a.Substring(5));
                    if (i >= 0) T.Lang = i;
                }
            }
            Directory.CreateDirectory(Installer.AppDir);
            Log.Path = System.IO.Path.Combine(Installer.AppDir, "setup.log");
            Log.W("---- dmxON Setup " + Assembly.GetExecutingAssembly().GetName().Version + " · " + Environment.OSVersion + " · .NET " + Environment.Version +
                  " · user docs " + Installer.Docs + (quiet ? " · quiet" : "") + (uninstall ? " · uninstall" : "") + (offline ? " · offline" : ""));
            Log.W("args: " + string.Join(" | ", args));

            // один экземпляр
            bool fresh;
            using (var mx = new Mutex(true, "dmxON-Setup-" + Environment.UserName, out fresh))
            {
                if (!fresh) return 2;
                var inst = new Installer { Offline = offline };
                inst.ReadInstalled();
                if (quiet)
                {
                    try
                    {
                        if (uninstall) inst.Uninstall();
                        else { inst.ProbeLatest(); inst.Install(); }
                        return 0;
                    }
                    catch (Exception e) { Log.W("ERROR " + e); return 1; }
                }
                SetProcessDPIAware();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += (s, e) => { Log.W("UI " + e.Exception); MessageBox.Show(e.Exception.Message, "dmxON Setup"); };
                var form = new MainForm(inst);
                if (uninstall) form.Shown += (s, e) => form.AskRemove();     // из «Приложений и возможностей»
                Application.Run(form);
                return 0;
            }
        }
    }
}
