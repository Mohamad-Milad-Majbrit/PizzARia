# MobileAR

## AR-Build-Einstellungen einrichten
Tutorial für AR Einstellungen auf Unity: youtube.com/watch?v=FWyTf3USDCQ&embeds_referring_euri=https%3A%2F%2Fchatgpt.com%2F&source_ve_path=OTY3MTQ
Geänderte Begriffe für Version 6000.0.46f1:
- Statt ARCore XR Plugin -> Google ARCore XR Plugin
- Statt ARKit XR Plugin -> Apple ARKit XR Plugin
- > Plugins und Buildsettings kommen ins .gitignore, so braucht jeder nur das passende für sein eigenes Smartphone einzurichten (also Android oder iOS)
- Build Einstellungen sind unter Build Profiles
- Player Settins sind unter Services -> General Settings

Smartphone für Build vorbereiten:
- Entwickleroptionen einschalten
- USB-Debugging einschalten

Wenn Hintergrund gelb ist:
- Assets (Ordner) -> Settings -> Project Configuration -> Android Preset --> AR Background Renderer Feature hinzufügen




### Teil 2: Individuelle Einrichtung (Muss von jedem Entwickler einmal durchgeführt werden)

Jeder Entwickler muss diese Schritte auf seinem eigenen Rechner durchführen, um Git richtig zu konfigurieren.

#### 4. UnityYAMLMerge-Pfad finden

Suchen Sie das `UnityYAMLMerge`-Tool im Installationsordner Ihrer Unity-Version. Notieren Sie sich den vollständigen Pfad.

* **Beispiel Windows:** `C:\Program Files\Unity\Editor\Data\Tools\UnityYAMLMerge.exe`
* **Beispiel macOS:** `/Applications/Unity/Editor/Data/Tools/UnityYAMLMerge`

#### 5. Git-Merge-Tool konfigurieren

Öffnen Sie Ihr Terminal (Git Bash/Kommandozeile) und führen Sie die folgenden drei Befehle aus. **Ersetzen Sie dabei `<path to UnityYAMLMerge>` durch den in Schritt 4 gefundenen Pfad.**

* *Hinweis: Wenn Ihr Pfad Leerzeichen enthält (wie im Windows-Beispiel), müssen Sie Anführungszeichen verwenden!*

```bash
# 1. Definiert, welches Programm für den Treiber 'unityyamlmerge' gestartet wird.
git config --global mergetool.unityyamlmerge.cmd '"<path to UnityYAMLMerge>" merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"'

# 2. Weist Git an, den Exit-Code des Tools zu beachten.
git config --global mergetool.unityyamlmerge.trustExitCode false

# 3. Definiert ein Fallback-Tool, falls UnityYAMLMerge den Konflikt nicht lösen kann (optional, aber empfohlen).
# Beispiel für P4Merge (ein gängiges, kostenloses Merge-Tool):
git config --global merge.tool p4merge
