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




## Teil 2: Individuelle Einrichtung zum Szenen Mergen (Muss von jedem Entwickler einmal durchgeführt werden)

Jeder Entwickler muss diese Schritte auf seinem eigenen Rechner durchführen, um Git richtig zu konfigurieren.

### 1. Alternatives Merge-Tool installieren
Das Programm `p4merge` installieren

### 2. UnityYAMLMerge-Pfad finden

Suchen das `UnityYAMLMerge`-Tool im Installationsordner deiner Unity-Version. Notieren dir den vollständigen Pfad.

* **Beispiel Windows:** `C:\Program Files\Unity\Hub\Editor\6000.2.12f1\Editor\Data\Tools\UnityYAMLMerge.exe`
* **Beispiel macOS:** `/Applications/Unity/Editor/Data/Tools/UnityYAMLMerge`

### 3. Git-Merge-Tool konfigurieren

Öffnen das Terminal (Git Bash!) und führen die folgenden drei Befehle aus. **Ersetzen Sie dabei `<path to UnityYAMLMerge>` durch den in Schritt 4 gefundenen Pfad.**

* *Hinweis: Wenn dein Pfad Leerzeichen enthält (wie im Windows-Beispiel), musst du Anführungszeichen verwenden!*

```bash
# 1. Definiert, welches Programm für den Treiber 'unityyamlmerge' gestartet wird.
git config --global mergetool.unityyamlmerge.cmd '"<path to UnityYAMLMerge>" merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"'

# 2. Weist Git an, den Exit-Code des Tools zu beachten.
git config --global mergetool.unityyamlmerge.trustExitCode false

# 3. Definiert ein Fallback-Tool, falls UnityYAMLMerge den Konflikt nicht lösen kann (optional, aber empfohlen).
# Beispiel für P4Merge (ein gängiges, kostenloses Merge-Tool):
git config --global merge.tool p4merge

```

### 4. Szenen Mergen

Auf mergen in Github Desktop klicken oder per Terminalbefehl.

Den Konflikt in der IDE z.B. Visual Studio Code öffnen.

Im Terminal in den Ordner mit dem Konflikt navigieren z.B. cd `C:\Pfad\MobileAR\Assets\Scenes\BlankScene`

Mergeprozess im Terminal starten: `git mergetool`

Das p4merge Programm öffnet sich und darin können die Konflikte gelöst werden

```diff
- Ich weiß nicht wie fehlerfrei das Mergen mit p4merge ist. Am sichersten ist es, wenn jeder in einer eignen Szene arbeitet und nur einer die Elemente in die Hauptszene verschiebt. Am besten wir probieren beide Methoden, da man nicht immer dran vorbei kommt in der Hauptszene etwas zu ändern.
```

