# 🐸 Croak and Dagger

A 2D platformer game built with **Godot 4.6** and **C#**. Jump, sprint, and battle your way through levels as a sword-wielding frog facing off against bats and knights.

---

## Prerequisites

Before you can run this project, make sure you have the following installed:

1. **Godot Engine 4.6** (with .NET / Mono support)
   - Download from the [official Godot website](https://godotengine.org/download)
   - Make sure you download the **.NET version** — the standard version does not support C#

2. **.NET 8.0 SDK**
   - Download from [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
   - After installing, verify it's working by opening a terminal and running:
     ```
     dotnet --version
     ```
     You should see a version number starting with `8.`.

---

## Getting the Project

### Option A — Download as ZIP (no Git required)

1. Go to the repository page on GitHub.
2. Click the green **Code** button near the top right.
3. Select **Download ZIP**.
4. Once downloaded, **extract** the ZIP to a folder of your choice.

### Option B — Clone with Git

If you have Git installed, open a terminal and run:

```bash
git clone https://github.com/TheCodester01/croak-and-dagger.git
```
---

## Opening the Project in Godot

1. **Launch Godot 4.6 (.NET version)**.

2. On the **Project Manager** screen, click **Import**.

3. In the file browser that opens, navigate to the folder where you extracted or cloned the project.

4. Open the `2dPlatformerProject` folder and select the **`project.godot`** file, then click **Open**.

5. Godot will import the project. This may take a moment the first time.

6. Once imported, click **Edit** to open the project in the Godot editor.

---

## Building the C# Scripts

Because this project uses C#, you need to build the scripts before running the game. Skipping this step will result in errors.

1. In the Godot editor, go to the top menu bar and click **Build** (or press the build icon — a hammer symbol — in the top-right toolbar).

2. Wait for the build to complete. You should see a **Build** panel at the bottom of the editor showing `Build Succeeded` with no errors.

> **Tip:** If you see errors about missing assemblies or .NET SDK, double-check that the .NET 8.0 SDK is correctly installed and that Godot can find it. You may need to restart Godot after installing .NET.

---

## Running the Game

Once the build succeeds:

1. Press **F5** (or click the **Play** button ▶ in the top-right corner of the editor) to run the game from the main scene.

2. The game will launch starting from the splash screen.

---

## Controls

| Action | Key |
|--------|-----|
| Move Left | `←` Arrow or `A` |
| Move Right | `→` Arrow or `D` |
| Jump | `Space` |
| Sprint | `Shift` |
| Pause | `Escape` |

---

## Project Structure

```
2dPlatformerProject/
├── Assets/          # Sprites, audio, and other media
├── Scenes/          # Godot scene files (.tscn) and scene-level scripts
├── Scripts/         # C# gameplay scripts (Player, Enemies, UI, etc.)
├── project.godot    # Godot project configuration (open this to import)
└── 2D-Platformer-Project.csproj  # C# project file
```

---

## Troubleshooting

**"No C# support" error when opening Godot**
Make sure you downloaded the **.NET version** of Godot 4.6, not the standard version. They are separate downloads on the Godot website.

**Build errors about missing .NET SDK**
Ensure the .NET 8.0 SDK is installed and that your system `PATH` includes the `dotnet` command. Try running `dotnet --version` in a terminal to confirm.

**Scripts show errors but the build succeeded**
Try closing and reopening Godot. Occasionally the editor needs a restart to fully recognize a fresh build.

**Game launches but crashes immediately**
Make sure you opened the project from the `project.godot` file inside the `2dPlatformerProject` folder, not from a parent directory.
