<p align="center">
  <img src="docs/assets/banner.svg" alt="dmxON Setup — instalator jednym kliknięciem dla dmxON FabKit, dmxON JLC Rules i dodatku dmxON do Autodesk Fusion Electronics" width="100%">
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest"><img src="https://img.shields.io/github/v/release/AlexSmith2025/dmxON-Setup-Fusion-Electronics?style=for-the-badge&color=b4744a&label=release" alt="Najnowsze wydanie"></a>
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases"><img src="https://img.shields.io/github/downloads/AlexSmith2025/dmxON-Setup-Fusion-Electronics/total?style=for-the-badge&color=24574a&label=downloads" alt="Pobrania"></a>
  <img src="https://img.shields.io/badge/Windows-10%20%7C%2011-3b4a5a?style=for-the-badge" alt="Windows 10 | 11">
  <img src="https://img.shields.io/badge/Autodesk%20Fusion-Electronics-e2ab7f?style=for-the-badge&logo=autodesk&logoColor=white" alt="Autodesk Fusion Electronics">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="https://img.shields.io/badge/PayPal-Donate-003087?style=for-the-badge&logo=paypal&logoColor=white" alt="Wesprzyj przez PayPal"></a>
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe"><img src="https://img.shields.io/badge/%E2%AC%87%20Pobierz-dmxON--Setup.exe-b4744a?style=for-the-badge" alt="Pobierz dmxON-Setup.exe"></a>
</p>

<p align="center">
  <a href="README.md">English</a> · <a href="README.ru.md">Русский</a> · <a href="README.es.md">Español</a> · <a href="README.de.md">Deutsch</a> · <b>Polski</b>
</p>

**Jeden plik `.exe`, który instaluje cały zestaw dmxON dla Autodesk Fusion Electronics (Fusion 360 Electronics) na każdym komputerze z Windows 10/11: pobiera najnowsze [dmxON FabKit](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) i [dmxON JLC Rules](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) z GitHuba, sprawdza je i instaluje dodatek dmxON — w Fusion pojawia się karta dmxON.**

<p align="center">
  <img src="docs/assets/setup_en_1.png" alt="dmxON Setup — wybór narzędzi" width="49%">
  <img src="docs/assets/setup_en_3.png" alt="dmxON Setup — zainstalowano" width="49%">
</p>

---

## Co robi

| | |
|---|---|
| ⬇️ | Przy instalacji pobiera **najnowsze wydania** dmxON FabKit i dmxON JLC Rules z GitHuba — instalator się nie starzeje |
| 🔒 | Sprawdza każde archiwum: **SHA-256 według GitHuba**, strukturę zip i kluczowe pliki; uszkodzone lub podmienione pobranie jest odrzucane |
| 🧩 | Instaluje **dodatek dmxON**: karta **dmxON** w edytorach płytki i schematu (FabKit, JLC Rules, Kopia, Aktualizacje); przy pierwszym uruchomieniu rejestruje foldery dmxON na listach ULP, reguł i CAM w Fusion |
| 👤 | **Bez uprawnień administratora**: wszystko trafia do bieżącego użytkownika Windows (`Dokumenty`, `AppData`, `HKCU`) |
| 🌍 | Działa na **każdym komputerze**: Dokumenty przeniesione do OneDrive lub na inny dysk, nazwy użytkownika ze spacjami lub cyrylicą, firmowy proxy, TLS 1.2 |
| 📴 | **Bez sieci**: gdy GitHub jest niedostępny, instalowana jest kopia wbudowana w `.exe` |
| ♻️ | **Aktualizacja i ponowna instalacja** zachowują Twoje pliki (np. reguły płytek w `rules\`); folder narzędzia jest zastępowany w jednym kroku z wycofaniem |
| 🗑️ | Widoczny w **Ustawienia → Aplikacje → Zainstalowane aplikacje** jako «dmxON for Autodesk Fusion Electronics» — usuwasz stamtąd lub z instalatora |
| 🌐 | **5 języków**: English, Русский, Español, Deutsch, Polski (według Windows, przełączane w oknie) |

---

## Instalacja

1. [**Pobierz `dmxON-Setup.exe`**](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe) i uruchom.
2. SmartScreen może pokazać «System Windows ochronił ten komputer» — plik nie jest podpisany. Kliknij **Więcej informacji → Uruchom mimo to**. SHA-256 każdej wersji jest w opisie wydania.
3. Zostaw oba narzędzia zaznaczone (albo odznacz jedno) i kliknij **Zainstaluj**. Trwa to kilka sekund.
4. Uruchom **Autodesk Fusion** (uruchom ponownie, jeśli był otwarty). Edytory płytki i schematu mają teraz kartę **dmxON**.

Aktualizacja: uruchom ponownie `dmxON-Setup.exe` (pokaże wersje zainstalowane i najnowsze oraz zaproponuje **Aktualizuj**) albo użyj **dmxON → Aktualizacje** w Fusion.

---

## Co gdzie trafia

| Co | Gdzie |
|---|---|
| dmxON FabKit | `Dokumenty\dmxON\dmxON-FabKit` |
| dmxON JLC Rules (pakiet reguł w `rules\`) | `Dokumenty\dmxON\dmxON-JLC-Rules` |
| Dodatek dmxON | `%APPDATA%\Autodesk\Autodesk Fusion 360\API\AddIns\dmxON` |
| Kopia instalatora do usunięcia, dziennik | `%LOCALAPPDATA%\dmxON\` (`setup.log`) |
| Wpis odinstalowania | `HKCU\Software\Microsoft\Windows\CurrentVersion\Uninstall\dmxON` |

Ustawienia folderów Fusion są wspólne dla wszystkich komputerów konta Autodesk, dlatego dodatek zapisuje je jako `$HOME\dmxON\…` — poprawne na każdym komputerze z dmxON.

---

## Wiersz poleceń

```
dmxON-Setup.exe                 okno
dmxON-Setup.exe /S              cicha instalacja lub aktualizacja (kod 0 = OK, 1 = błąd)
dmxON-Setup.exe /uninstall      usunięcie (okno; z /S po cichu)
dmxON-Setup.exe /lang=pl        język: en, ru, es, de, pl
dmxON-Setup.exe /offline        instalacja wbudowanej kopii bez sieci
```

---

## Wymagania

- Windows 10 lub 11, 64-bit. .NET Framework 4.8 jest częścią Windows — nic więcej nie trzeba instalować.
- Autodesk Fusion z Electronics. Fusion można zainstalować także po dmxON: dodatek wczyta się przy następnym uruchomieniu Fusion.
- Dostęp do `github.com` dla najnowszych wersji (opcjonalnie: bez niego instalowana jest wbudowana kopia).

---

## Problemy

| Problem | Odpowiedź |
|---|---|
| «System Windows ochronił ten komputer» | **Więcej informacji → Uruchom mimo to**. Instalator nie jest podpisany; w razie wątpliwości porównaj SHA-256 z opisem wydania |
| Brak karty **dmxON** w Fusion | uruchom ponownie Fusion; albo **Utilities → Add-Ins → Scripts and Add-Ins → dmxON → Run** |
| «GitHub niedostępny» | zainstalowano wersje wbudowane; uruchom instalator później lub użyj **dmxON → Aktualizacje** |
| Błąd | **Otwórz dziennik** w instalatorze lub `%LOCALAPPDATA%\dmxON\setup.log`, i [zgłoś issue](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/issues) |

---

## Budowanie ze źródeł

```
python build.py
```

Wymaga Windows, Pythona 3 z Pillow i internetu. Pobiera najnowsze wydania dmxON (wbudowane jako kopia bez sieci), rysuje ikonę i kompiluje `src/Setup.cs` kompilatorem C# z .NET Framework 4.8 (część Windows) do `dist\dmxON-Setup.exe`.

---

## Narzędzia dmxON

| | |
|---|---|
| [**dmxON FabKit**](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) | Płytka Fusion Electronics → pełne zamówienie JLCPCB: Gerber, BOM i CPL z częściami LCSC, raport, dokumentacja ISO/GOST, kopia projektu |
| [**dmxON JLC Rules**](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) | Reguły JLCPCB dla 2, 4, 6 i 8 warstw oraz konfiguracja płytki jednym kliknięciem: stos warstw, DRC, siatka punktowa |

---

## Wesprzyj projekt

<p align="center">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="docs/assets/donate.pl.svg" alt="Wesprzyj dmxON przez PayPal — smith.show2025@gmail.com" width="760"></a>
</p>

---

## Licencja

Darmowe użytkowanie; wymagane podanie autora; bez modyfikacji i komercyjnej redystrybucji — zob. [LICENSE.md](LICENSE.md). **dmxON by AlexSmith 2026.**
