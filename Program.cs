using System;
using System.IO;

class WinSweep
{
    static int deletedFiles = 0;
    static int skippedFiles = 0;

    static void Main()
    {
        Console.WriteLine("+--------------------------------------+");
        Console.WriteLine("|--------|    WinSweep  v1    |--------|");
        Console.WriteLine("+--------------------------------------+\n");
        Console.WriteLine("Lightweight CLI for fast Windows cleanup");
        Console.WriteLine("Please wait.. Cleaning system junk files...\n");
        
        string userTemp = Path.GetTempPath();
        string windowsTemp = @"C:\Windows\Temp";
        string prefetch = @"C:\Windows\Prefetch";
        
        CleanDirectory(userTemp);
        CleanDirectory(windowsTemp);
        CleanDirectory(prefetch);

        CleanRecycleBin();

        Console.WriteLine("\n=== SUMMARY ===");
        Console.WriteLine($"Deleted Files : {deletedFiles}");
        Console.WriteLine($"Skipped Files : {skippedFiles}");
        Console.WriteLine("Done.");
    }

    static void CleanDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Console.WriteLine($"[!] Not found: {path}");
            return;
        }

        Console.WriteLine($"\n[Cleaning] {path}");

        try
        {
            foreach (string file in Directory.GetFiles(path))
            {
                try
                {
                    File.Delete(file);
                    deletedFiles++;
                    Console.WriteLine($"[DEL] {file}");
                }
                catch
                {
                    skippedFiles++;
                    Console.WriteLine($"[SKIP] {file}");
                }
            }

            foreach (string dir in Directory.GetDirectories(path))
            {
                try
                {
                    Directory.Delete(dir, true);
                    deletedFiles++;
                    Console.WriteLine($"[DEL DIR] {dir}");
                }
                catch
                {
                    skippedFiles++;
                    Console.WriteLine($"[SKIP DIR] {dir}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] {path} : {ex.Message}");
        }
    }

    static void CleanRecycleBin()
    {
        Console.WriteLine("\n[Cleaning] Recycle Bin");

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/c rd /s /q C:\\$Recycle.Bin",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit();

            Console.WriteLine("[OK] Recycle Bin cleaned");
        }
        catch
        {
            Console.WriteLine("[SKIP] Failed to clean Recycle Bin");
        }
    }
}


// Coded by Akay https://github.com/ridhoyaka