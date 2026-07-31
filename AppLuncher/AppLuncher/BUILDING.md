# AppLuncher

AppLuncher is a Windows Forms application targeting .NET Framework 4.8.

## Build

1. Install Visual Studio 2022 with the **.NET desktop development** workload and the .NET Framework 4.8 targeting pack.
2. Open `AppLuncher.slnx`.
3. Restore NuGet packages. The project uses `Newtonsoft.Json` 13.0.3.
4. Build and run the `AppLuncher` project.

On first run, choose an existing `.json` file or enter a new file name. AppLuncher creates a valid empty database when the selected file does not exist and stores the path in per-user application settings.

`SampleData\AppLuncher.sample.json` demonstrates the supported JSON structure. Replace its example program, working-directory, and icon paths before using it.

Every successful save writes formatted JSON and keeps the previous version beside it as `<database>.bak`.

Launcher icons can be loaded from `.ico`, `.exe`, or `.dll` files. The selected source file path is stored in JSON.
