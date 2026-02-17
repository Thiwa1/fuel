# Publishing the Pay Bill Application

If you encounter errors when trying to publish the application, follow these steps to create a standalone executable that includes all dependencies.

## Option 1: Using the Command Line (Recommended)

1.  Open a terminal (Command Prompt or PowerShell) in the solution folder (where `fuel.sln` is).
2.  Run the following command:

    ```bash
    dotnet publish -c Release -r win-x64 --self-contained true
    ```

3.  Once the command finishes, your application will be located in:
    `WpfApp1\bin\Release\net8.0-windows\win-x64\publish\`

4.  Inside that folder, you will find `WpfApp1.exe`. This is the file you run.
    *   **Note:** Make sure `appsettings.json` is in the same folder as `WpfApp1.exe`. The build process should copy it automatically.

## Option 2: Using Visual Studio

1.  Right-click on the `WpfApp1` project in the Solution Explorer.
2.  Select **Publish**.
3.  Create a new publish profile (Folder Profile is easiest):
    *   **Target:** Folder
    *   **Specific Target:** `bin\Release\net8.0-windows\publish\`
4.  Click **Finish**.
5.  Click **Show all settings** (or the pencil icon).
6.  Set the following options:
    *   **Configuration:** Release
    *   **Target Runtime:** win-x64
    *   **Deployment Mode:** Self-contained
    *   **File Publish Options:** check "Produce single file" (optional but cleaner)
7.  Click **Save**.
8.  Click **Publish**.

## Troubleshooting

*   **"Assets file project.assets.json not found":** Run `dotnet restore` first.
*   **Database Errors:** Ensure `appsettings.json` is configured correctly with your MySQL database details.
*   **Missing DLLs:** Using "Self-contained" mode ensures all required libraries (like `MiniExcel` and `MySql`) are included in the output folder.
