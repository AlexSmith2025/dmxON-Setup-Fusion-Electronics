<p align="center">
  <img src="docs/assets/banner.svg" alt="dmxON Setup — Ein-Klick-Installer für dmxON FabKit, dmxON JLC Rules und das dmxON-Add-in für Autodesk Fusion Electronics" width="100%">
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest"><img src="https://img.shields.io/github/v/release/AlexSmith2025/dmxON-Setup-Fusion-Electronics?style=for-the-badge&color=b4744a&label=release" alt="Neueste Version"></a>
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases"><img src="https://img.shields.io/github/downloads/AlexSmith2025/dmxON-Setup-Fusion-Electronics/total?style=for-the-badge&color=24574a&label=downloads" alt="Downloads"></a>
  <img src="https://img.shields.io/badge/Windows-10%20%7C%2011-3b4a5a?style=for-the-badge" alt="Windows 10 | 11">
  <img src="https://img.shields.io/badge/Autodesk%20Fusion-Electronics-e2ab7f?style=for-the-badge&logo=autodesk&logoColor=white" alt="Autodesk Fusion Electronics">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="https://img.shields.io/badge/PayPal-Donate-003087?style=for-the-badge&logo=paypal&logoColor=white" alt="Mit PayPal spenden"></a>
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe"><img src="https://img.shields.io/badge/%E2%AC%87%20Download-dmxON--Setup.exe-b4744a?style=for-the-badge" alt="dmxON-Setup.exe herunterladen"></a>
</p>

<p align="center">
  <a href="README.md">English</a> · <a href="README.ru.md">Русский</a> · <a href="README.es.md">Español</a> · <b>Deutsch</b> · <a href="README.pl.md">Polski</a>
</p>

**Eine `.exe`, die das komplette dmxON-Paket für Autodesk Fusion Electronics (Fusion 360 Electronics) auf jedem Windows-10/11-Rechner installiert: Sie lädt die neuesten Versionen von [dmxON FabKit](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) und [dmxON JLC Rules](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) von GitHub, prüft sie und richtet das dmxON-Add-in ein — Fusion erhält den Reiter dmxON.**

<p align="center">
  <img src="docs/assets/setup_en_1.png" alt="dmxON Setup — Werkzeuge wählen" width="49%">
  <img src="docs/assets/setup_en_3.png" alt="dmxON Setup — installiert" width="49%">
</p>

---

## Was es tut

| | |
|---|---|
| ⬇️ | Lädt bei der Installation die **neuesten Releases** von dmxON FabKit und dmxON JLC Rules von GitHub — der Installer veraltet nie |
| 🔒 | Prüft jedes Archiv: **SHA-256 gegen GitHub**, Zip-Struktur und wichtige Dateien; beschädigte oder manipulierte Downloads werden verworfen |
| 🧩 | Installiert das **dmxON-Add-in**: Reiter **dmxON** im Platinen- und Schaltplan-Editor (FabKit, JLC Rules, Sicherung, Updates); beim ersten Start registriert es die dmxON-Ordner in den ULP-, Regel- und CAM-Listen von Fusion |
| 👤 | **Keine Administratorrechte**: alles geht in den aktuellen Windows-Benutzer (`Dokumente`, `AppData`, `HKCU`) |
| 🌍 | Läuft auf **jedem Rechner**: Dokumente nach OneDrive oder auf ein anderes Laufwerk verschoben, Benutzernamen mit Leerzeichen oder Kyrillisch, Firmen-Proxy, TLS 1.2 |
| 📴 | **Offline**: ist GitHub nicht erreichbar, wird die in der `.exe` enthaltene Kopie installiert |
| ♻️ | **Update und Neuinstallation** behalten eigene Dateien (z. B. Platinenregeln in `rules\`); jeder Werkzeugordner wird in einem Schritt mit Rollback ersetzt |
| 🗑️ | Erscheint unter **Einstellungen → Apps → Installierte Apps** als «dmxON for Autodesk Fusion Electronics» — dort oder im Installer entfernen |
| 🌐 | **5 Sprachen**: English, Русский, Español, Deutsch, Polski (nach Windows-Sprache, im Fenster umschaltbar) |

---

## Installation

1. [**`dmxON-Setup.exe` herunterladen**](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe) und starten.
2. SmartScreen meldet eventuell «Der Computer wurde durch Windows geschützt» — die Datei ist nicht signiert. **Weitere Informationen → Trotzdem ausführen**. Die SHA-256 jeder Version steht in ihren Release-Notes.
3. Beide Werkzeuge angehakt lassen (oder eines abwählen) und **Installieren** drücken. Das dauert wenige Sekunden.
4. **Autodesk Fusion** starten (neu starten, falls es offen war). Platinen- und Schaltplan-Editor haben jetzt den Reiter **dmxON**.

Später aktualisieren: `dmxON-Setup.exe` erneut starten (zeigt installierte und neueste Versionen und bietet **Aktualisieren** an) oder **dmxON → Updates** in Fusion.

---

## Wohin was kommt

| Was | Wohin |
|---|---|
| dmxON FabKit | `Dokumente\dmxON\dmxON-FabKit` |
| dmxON JLC Rules (Regelpaket in `rules\`) | `Dokumente\dmxON\dmxON-JLC-Rules` |
| dmxON-Add-in | `%APPDATA%\Autodesk\Autodesk Fusion 360\API\AddIns\dmxON` |
| Installer-Kopie zum Entfernen, Protokoll | `%LOCALAPPDATA%\dmxON\` (`setup.log`) |
| Deinstallationseintrag | `HKCU\Software\Microsoft\Windows\CurrentVersion\Uninstall\dmxON` |

Die Ordnereinstellungen von Fusion gelten für alle Rechner eines Autodesk-Kontos; deshalb speichert das Add-in sie als `$HOME\dmxON\…` — gültig auf jedem Rechner mit dmxON.

---

## Befehlszeile

```
dmxON-Setup.exe                 Fenster
dmxON-Setup.exe /S              stille Installation oder Aktualisierung (Exitcode 0 = OK, 1 = Fehler)
dmxON-Setup.exe /uninstall      entfernen (Fenster; mit /S still)
dmxON-Setup.exe /lang=de        Sprache: en, ru, es, de, pl
dmxON-Setup.exe /offline        eingebaute Kopie ohne Netz installieren
```

---

## Voraussetzungen

- Windows 10 oder 11, 64 Bit. .NET Framework 4.8 ist Teil von Windows — sonst nichts zu installieren.
- Autodesk Fusion mit Electronics. Fusion kann auch nach dmxON installiert werden: Das Add-in wird beim nächsten Fusion-Start geladen.
- Zugang zu `github.com` für die neuesten Versionen (optional: sonst wird die eingebaute Kopie installiert).

---

## Fehlerbehebung

| Problem | Antwort |
|---|---|
| «Der Computer wurde durch Windows geschützt» | **Weitere Informationen → Trotzdem ausführen**. Der Installer ist nicht signiert; SHA-256 bei Bedarf mit den Release-Notes vergleichen |
| Kein Reiter **dmxON** in Fusion | Fusion neu starten; oder **Utilities → Add-Ins → Scripts and Add-Ins → dmxON → Run** |
| «GitHub nicht erreichbar» | die eingebauten Versionen wurden installiert; später erneut starten oder **dmxON → Updates** |
| Fehler | **Protokoll öffnen** im Installer oder `%LOCALAPPDATA%\dmxON\setup.log`, und [ein Issue anlegen](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/issues) |

---

## Aus dem Quellcode bauen

```
python build.py
```

Benötigt Windows, Python 3 mit Pillow und Internet. Lädt die neuesten dmxON-Releases (eingebettet als Offline-Kopie), zeichnet das Symbol und kompiliert `src/Setup.cs` mit dem C#-Compiler von .NET Framework 4.8 (Teil von Windows) nach `dist\dmxON-Setup.exe`.

---

## Die dmxON-Werkzeuge

| | |
|---|---|
| [**dmxON FabKit**](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) | Fusion-Electronics-Platine → komplette JLCPCB-Bestellung: Gerber, BOM und CPL mit LCSC-Teilen, Bericht, ISO/GOST-Dokumentation, Design-Sicherung |
| [**dmxON JLC Rules**](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) | JLCPCB-Designregeln für 2, 4, 6 und 8 Lagen und Platinen-Setup mit einem Klick: Lagenaufbau, DRC, Punktraster |

---

## Projekt unterstützen

<p align="center">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="docs/assets/donate.de.svg" alt="dmxON mit PayPal unterstützen — smith.show2025@gmail.com" width="760"></a>
</p>

---

## Lizenz

Kostenlos nutzbar; Namensnennung erforderlich; keine Änderungen und keine kommerzielle Weitergabe — siehe [LICENSE.md](LICENSE.md). **dmxON by AlexSmith 2026.**
