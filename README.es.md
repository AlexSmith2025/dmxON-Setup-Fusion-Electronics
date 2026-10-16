<p align="center">
  <img src="docs/assets/banner.svg" alt="dmxON Setup — instalador en un clic de dmxON FabKit, dmxON JLC Rules y el complemento dmxON para Autodesk Fusion Electronics" width="100%">
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest"><img src="https://img.shields.io/github/v/release/AlexSmith2025/dmxON-Setup-Fusion-Electronics?style=for-the-badge&color=b4744a&label=release" alt="Última versión"></a>
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases"><img src="https://img.shields.io/github/downloads/AlexSmith2025/dmxON-Setup-Fusion-Electronics/total?style=for-the-badge&color=24574a&label=downloads" alt="Descargas"></a>
  <img src="https://img.shields.io/badge/Windows-10%20%7C%2011-3b4a5a?style=for-the-badge" alt="Windows 10 | 11">
  <img src="https://img.shields.io/badge/Autodesk%20Fusion-Electronics-e2ab7f?style=for-the-badge&logo=autodesk&logoColor=white" alt="Autodesk Fusion Electronics">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="https://img.shields.io/badge/PayPal-Donate-003087?style=for-the-badge&logo=paypal&logoColor=white" alt="Donar con PayPal"></a>
</p>

<p align="center">
  <a href="https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe"><img src="https://img.shields.io/badge/%E2%AC%87%20Descargar-dmxON--Setup.exe-b4744a?style=for-the-badge" alt="Descargar dmxON-Setup.exe"></a>
</p>

<p align="center">
  <a href="README.md">English</a> · <a href="README.ru.md">Русский</a> · <b>Español</b> · <a href="README.de.md">Deutsch</a> · <a href="README.pl.md">Polski</a>
</p>

**Un solo `.exe` que instala todo dmxON para Autodesk Fusion Electronics (Fusion 360 Electronics) en cualquier equipo con Windows 10/11: descarga las últimas versiones de [dmxON FabKit](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) y [dmxON JLC Rules](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) desde GitHub, las verifica e instala el complemento dmxON: Fusion recibe la pestaña dmxON.**

<p align="center">
  <img src="docs/assets/setup_en_1.png" alt="dmxON Setup — elegir herramientas" width="49%">
  <img src="docs/assets/setup_en_3.png" alt="dmxON Setup — instalado" width="49%">
</p>

---

## Qué hace

| | |
|---|---|
| ⬇️ | Al instalar descarga las **últimas versiones** de dmxON FabKit y dmxON JLC Rules desde GitHub: el instalador nunca queda anticuado |
| 🔒 | Verifica cada archivo: **SHA-256 frente a GitHub**, la estructura del zip y los archivos clave; una descarga dañada o alterada se rechaza |
| 🧩 | Instala el **complemento dmxON**: pestaña **dmxON** en los editores de placa y esquema (FabKit, JLC Rules, Copia, Actualizar); en el primer inicio registra las carpetas dmxON en las listas de ULP, reglas y CAM de Fusion |
| 👤 | **Sin derechos de administrador**: todo va al usuario actual de Windows (`Documentos`, `AppData`, `HKCU`) |
| 🌍 | Funciona en **cualquier equipo**: Documentos movidos a OneDrive u otra unidad, nombres de usuario con espacios o cirílico, proxy corporativo, TLS 1.2 |
| 📴 | **Sin conexión**: si GitHub no responde, se instala la copia integrada en el `.exe` |
| ♻️ | **Actualizar y reinstalar** conserva sus archivos (por ejemplo las reglas de placas en `rules\`); cada carpeta se reemplaza de una vez con reversión |
| 🗑️ | Aparece en **Configuración → Aplicaciones → Aplicaciones instaladas** como «dmxON for Autodesk Fusion Electronics»; se elimina desde allí o desde el instalador |
| 🌐 | **5 idiomas**: English, Русский, Español, Deutsch, Polski (según Windows, se cambia en la ventana) |

---

## Instalación

1. [**Descargue `dmxON-Setup.exe`**](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/releases/latest/download/dmxON-Setup.exe) y ejecútelo.
2. SmartScreen puede mostrar «Windows protegió su PC»: el archivo no está firmado. Pulse **Más información → Ejecutar de todas formas**. El SHA-256 de cada versión está en sus notas.
3. Deje marcadas ambas herramientas (o desmarque una) y pulse **Instalar**. Tarda unos segundos.
4. Inicie **Autodesk Fusion** (reinícielo si estaba abierto). Los editores de placa y esquema tienen ahora la pestaña **dmxON**.

Para actualizar: ejecute `dmxON-Setup.exe` otra vez (muestra las versiones instaladas y las últimas y ofrece **Actualizar**) o use **dmxON → Actualizar** dentro de Fusion.

---

## Dónde se instala

| Qué | Dónde |
|---|---|
| dmxON FabKit | `Documentos\dmxON\dmxON-FabKit` |
| dmxON JLC Rules (paquete de reglas en `rules\`) | `Documentos\dmxON\dmxON-JLC-Rules` |
| Complemento dmxON | `%APPDATA%\Autodesk\Autodesk Fusion 360\API\AddIns\dmxON` |
| Copia del instalador para desinstalar, registro | `%LOCALAPPDATA%\dmxON\` (`setup.log`) |
| Entrada de desinstalación | `HKCU\Software\Microsoft\Windows\CurrentVersion\Uninstall\dmxON` |

Los ajustes de carpetas de Fusion se comparten entre todos los equipos de una cuenta de Autodesk; por eso el complemento los guarda como `$HOME\dmxON\…`, válidos en cada equipo con dmxON.

---

## Línea de comandos

```
dmxON-Setup.exe                 ventana
dmxON-Setup.exe /S              instalación o actualización silenciosa (código 0 = bien, 1 = error)
dmxON-Setup.exe /uninstall      desinstalar (ventana; con /S, silencioso)
dmxON-Setup.exe /lang=es        idioma: en, ru, es, de, pl
dmxON-Setup.exe /offline        instalar la copia integrada, sin red
```

---

## Requisitos

- Windows 10 u 11 de 64 bits. .NET Framework 4.8 forma parte de Windows; no hay que instalar nada más.
- Autodesk Fusion con Electronics. Fusion puede instalarse después de dmxON: el complemento se carga en el siguiente inicio de Fusion.
- Acceso a `github.com` para las últimas versiones (opcional: sin él se instala la copia integrada).

---

## Problemas

| Problema | Respuesta |
|---|---|
| «Windows protegió su PC» | **Más información → Ejecutar de todas formas**. El instalador no está firmado; compare su SHA-256 con las notas de la versión |
| No hay pestaña **dmxON** en Fusion | reinicie Fusion; o **Utilities → Add-Ins → Scripts and Add-Ins → dmxON → Run** |
| «GitHub no responde» | se instalaron las versiones integradas; ejecute el instalador más tarde o use **dmxON → Actualizar** |
| Algo falló | **Abrir registro** en el instalador o `%LOCALAPPDATA%\dmxON\setup.log`, y [abra un issue](https://github.com/AlexSmith2025/dmxON-Setup-Fusion-Electronics/issues) |

---

## Compilar desde el código

```
python build.py
```

Necesita Windows, Python 3 con Pillow e internet. Descarga las últimas versiones de dmxON (integradas como copia sin conexión), dibuja el icono y compila `src/Setup.cs` con el compilador C# de .NET Framework 4.8 (incluido en Windows) en `dist\dmxON-Setup.exe`.

---

## Herramientas dmxON

| | |
|---|---|
| [**dmxON FabKit**](https://github.com/AlexSmith2025/dmxON-FabKit-Fusion-Electronics) | Placa de Fusion Electronics → pedido completo a JLCPCB: Gerber, BOM y CPL con piezas LCSC, informe, documentación ISO/GOST, copia del diseño |
| [**dmxON JLC Rules**](https://github.com/AlexSmith2025/dmxON-JLC-Rules-Fusion-Electronics) | Reglas JLCPCB de 2, 4, 6 y 8 capas y configuración de la placa en un clic: apilado, DRC, rejilla de puntos |

---

## Apoye el proyecto

<p align="center">
  <a href="https://www.paypal.com/donate/?business=smith.show2025%40gmail.com&no_recurring=0&item_name=dmxON&currency_code=EUR"><img src="docs/assets/donate.es.svg" alt="Apoye dmxON con PayPal — smith.show2025@gmail.com" width="760"></a>
</p>

---

## Licencia

Uso gratuito; atribución obligatoria; sin modificaciones ni redistribución comercial — vea [LICENSE.md](LICENSE.md). **dmxON by AlexSmith 2026.**
