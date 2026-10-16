<p align="center">
  <img src="docs/assets/banner.svg" alt="dmxON Setup — установщик dmxON FabKit, dmxON JLC Rules и надстройки dmxON для Autodesk Fusion Electronics в один клик" width="100%">
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest"><img src="https://img.shields.io/github/v/release/AlexSmith2025/dmxON-Setup-Fusion-Electronics?style=for-the-badge&color=b4744a&label=release" alt="Последний выпуск"></a>
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases"><img src="https://img.shields.io/github/downloads/AlexSmith2025/dmxON-Setup-Fusion-Electronics/total?style=for-the-badge&color=24574a&label=downloads" alt="Загрузки"></a>
  <img src="https://img.shields.io/badge/Windows-10%20%7C%2011-3b4a5a?style=for-the-badge" alt="Windows 10 | 11">
  <img src="https://img.shields.io/badge/Autodesk%20Fusion-Electronics-e2ab7f?style=for-the-badge&logo=autodesk&logoColor=white" alt="Autodesk Fusion Electronics">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="https://img.shields.io/badge/PayPal-Donate-003087?style=for-the-badge&logo=paypal&logoColor=white" alt="Поддержать через PayPal"></a>
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe"><img src="https://img.shields.io/badge/%E2%AC%87%20%D0%A1%D0%BA%D0%B0%D1%87%D0%B0%D1%82%D1%8C-dmxON--Setup.exe-b4744a?style=for-the-badge" alt="Скачать dmxON-Setup.exe"></a>
</p>

<p align="center">
  <a href="README.md">English</a> · <b>Русский</b> · <a href="README.es.md">Español</a> · <a href="README.de.md">Deutsch</a> · <a href="README.pl.md">Polski</a>
</p>

**Один `.exe`, который ставит весь набор dmxON для Autodesk Fusion Electronics (Fusion 360 Electronics) на любой компьютер с Windows 10/11: скачивает последние [dmxON FabKit](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) и [dmxON JLC Rules](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) с GitHub, проверяет их и устанавливает надстройку dmxON — во Fusion появляется вкладка dmxON.**

<p align="center">
  <img src="docs/assets/setup_ru_1.png" alt="dmxON Setup — выбор инструментов" width="49%">
  <img src="docs/assets/setup_en_3.png" alt="dmxON Setup — установлено" width="49%">
</p>

---

## Что делает

| | |
|---|---|
| ⬇️ | При установке скачивает **последние выпуски** dmxON FabKit и dmxON JLC Rules с GitHub — установщик не устаревает |
| 🔒 | Проверяет каждый архив: **SHA-256 по данным GitHub**, структуру zip и ключевые файлы; повреждённая или подменённая загрузка отклоняется |
| 🧩 | Ставит **надстройку dmxON**: вкладка **dmxON** в редакторах платы и схемы (FabKit, JLC Rules, Бэкап, Обновления); при первом запуске она регистрирует папки dmxON в списках ULP, правил и CAM Fusion |
| 👤 | **Без прав администратора**: всё ставится в текущего пользователя Windows (`Документы`, `AppData`, `HKCU`) |
| 🌍 | Работает на **любой машине**: «Документы» перенесены в OneDrive или на другой диск, имя пользователя с пробелами или кириллицей, корпоративный прокси, TLS 1.2 |
| 📴 | **Без сети**: если GitHub недоступен, ставится копия, встроенная в `.exe` |
| ♻️ | **Обновление и переустановка** сохраняют ваши файлы (например, правила плат в `rules\`); папка инструмента заменяется целиком с откатом |
| 🗑️ | Видна в **Параметры → Приложения → Установленные приложения** как «dmxON for Autodesk Fusion Electronics» — удаляется оттуда или из установщика |
| 🌐 | **5 языков**: English, Русский, Español, Deutsch, Polski (по языку Windows, переключается в окне) |

---

## Установка

1. [**Скачайте `dmxON-Setup.exe`**](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe) и запустите.
2. SmartScreen может написать «Система Windows защитила ваш компьютер» — файл не подписан сертификатом. Нажмите **Подробнее → Выполнить в любом случае**. SHA-256 каждой сборки — в описании её выпуска.
3. Оставьте оба инструмента отмеченными (или снимите один) и нажмите **Установить**. Это занимает несколько секунд.
4. Запустите **Autodesk Fusion** (перезапустите, если был открыт). В редакторах платы и схемы появится вкладка **dmxON**.

Обновление: снова запустите `dmxON-Setup.exe` (он покажет установленные и последние версии и предложит **Обновить**) или нажмите **dmxON → Обновления** во Fusion.

<p align="center">
  <img src="docs/assets/setup_ru_rm.png" alt="dmxON Setup — удаление" width="49%">
</p>

---

## Куда что ставится

| Что | Куда |
|---|---|
| dmxON FabKit | `Документы\dmxON\dmxON-FabKit` |
| dmxON JLC Rules (пакет правил — в `rules\`) | `Документы\dmxON\dmxON-JLC-Rules` |
| Надстройка dmxON | `%APPDATA%\Autodesk\Autodesk Fusion 360\API\AddIns\dmxON` |
| Копия установщика для удаления, журнал | `%LOCALAPPDATA%\dmxON\` (`setup.log`) |
| Запись для удаления | `HKCU\Software\Microsoft\Windows\CurrentVersion\Uninstall\dmxON` |

Настройки папок Fusion общие для всех компьютеров аккаунта Autodesk, поэтому надстройка хранит их как `$HOME\dmxON\…` — они верны на каждом компьютере, где стоит dmxON.

---

## Командная строка

```
dmxON-Setup.exe                 окно
dmxON-Setup.exe /S              тихая установка или обновление (код выхода 0 — успех, 1 — ошибка)
dmxON-Setup.exe /uninstall      удаление (окно; с /S — тихо)
dmxON-Setup.exe /lang=ru        язык: en, ru, es, de, pl
dmxON-Setup.exe /offline        поставить встроенную копию без сети
```

Удобно для развёртывания на нескольких рабочих местах: `dmxON-Setup.exe /S` для каждого пользователя.

---

## Требования

- Windows 10 или 11, 64-бит. .NET Framework 4.8 входит в Windows — ставить ничего не нужно.
- Autodesk Fusion с Electronics. Fusion можно поставить и после dmxON: надстройка загрузится при следующем запуске Fusion.
- Доступ к `github.com` для последних версий (необязательно: без него ставится встроенная копия).

---

## Если что-то не так

| Проблема | Ответ |
|---|---|
| «Система Windows защитила ваш компьютер» | **Подробнее → Выполнить в любом случае**. Установщик не подписан; при сомнениях сверьте SHA-256 с описанием выпуска |
| Во Fusion нет вкладки **dmxON** | перезапустите Fusion; или **Utilities → Add-Ins → Scripts and Add-Ins → dmxON → Run** |
| «GitHub недоступен» | поставлены встроенные версии; позже запустите установщик снова или нажмите **dmxON → Обновления** |
| Ошибка установки | **Открыть журнал** в установщике или `%LOCALAPPDATA%\dmxON\setup.log`, и [создайте issue](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/issues) с ним |

---

## Сборка из исходников

```
python build.py
```

Нужны Windows, Python 3 с Pillow и интернет. Скрипт скачивает последние выпуски dmxON (встраиваются как запасной вариант без сети), рисует иконку и компилирует `src/Setup.cs` компилятором C# из .NET Framework 4.8 (`%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe`, входит в Windows) в `dist\dmxON-Setup.exe`. Исходник — один файл: интерфейс WinForms, загрузка с повторами и прокси, проверка SHA-256 по выпуску GitHub, проверка zip, замена папки одним шагом с откатом, установка надстройки, запись для удаления.

---

## Инструменты dmxON

| | |
|---|---|
| [**dmxON FabKit**](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) | Плата Fusion Electronics → полный заказ JLCPCB: Gerber, BOM и CPL с деталями LCSC, отчёт, документация ISO/ГОСТ, бэкап проекта |
| [**dmxON JLC Rules**](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) | Правила JLCPCB для 2, 4, 6 и 8 слоёв и настройка платы в один клик: стек, DRC, сетка точками |

---

## Поддержать проект

<p align="center">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="docs/assets/donate.ru.svg" alt="Поддержать dmxON через PayPal — smith.show2025@gmail.com" width="760"></a>
</p>

---

## Лицензия

Бесплатно; с указанием авторства; без изменений и коммерческого распространения — см. [LICENSE.md](LICENSE.md). **dmxON by AlexSmith 2026.**
