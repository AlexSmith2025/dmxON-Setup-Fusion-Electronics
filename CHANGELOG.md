# Changelog

Numbering: `MAJOR.MINOR.PATCH` — new feature → MINOR, fix → PATCH. The version is in `VERSION` and in `AssemblyVersion` of `src/Setup.cs`.

---

## 1.0.0 — 2026-10-16

First release.

- One `.exe`, no administrator rights: installs dmxON FabKit and dmxON JLC Rules into `Documents\dmxON` and the dmxON add-in into Fusion's add-in folder of the current user.
- Takes the latest GitHub releases at install time; checks SHA-256 against the release, the zip structure and the key files.
- Offline fallback: the releases current at build time are built into the `.exe`.
- Update and reinstall keep your own files; every tool folder is replaced in one step with rollback (or copied over when another program holds it open).
- Uninstall entry in Settings → Apps; removal from the installer or `/uninstall`; backups and settings stay.
- Silent mode `/S`, `/uninstall`, `/lang=xx`, `/offline`; log in `%LOCALAPPDATA%\dmxON\setup.log`.
- English, Russian, Spanish, German and Polish; dark dmxON design, high-DPI aware.
- Tested on a clean user profile: install, update with own files, offline install, removal; on Windows 11 with Fusion 2705.1.30.
