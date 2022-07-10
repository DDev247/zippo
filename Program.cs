using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

#pragma warning disable CS8600
#pragma warning disable CS8604

namespace Zippo
{
    public static class Info
    {
        public const string PROGRAM_NAME = "zippo";
        public const string PROGRAM_DEV = "DDev / DDev247 @ github.com";
        public const string PROGRAM_VERSION = "0.0.1";
    }
    
    public enum CMDStage
    {
        noargs,
        arg,
        args,
        full,
    }

    public static class Interaction
    {


        public static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                // Display help

                DisplayHelp();
            }
            else
            {
                if(args.Length == 1)
                {
                    if(args[0] == "help")
                    {
                        DisplayHelp();
                    }
                    else if(args[0] == "h")
                    {
                        DisplayHelp();
                    }
                    else if(args[0] == "compress")
                    {
                        bool result = DisplayCompress(CMDStage.noargs);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                    else if(args[0] == "c")
                    {
                        bool result = DisplayCompress(CMDStage.noargs);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                    else if(args[0] == "extract")
                    {
                        bool result = DisplayExtract(CMDStage.noargs);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                    else if(args[0] == "e")
                    {
                        bool result = DisplayExtract(CMDStage.noargs);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                }
                else if(args.Length == 2)
                {
                    if(args[0] == "help")
                    {
                        DisplayHelp();
                    }
                    else if(args[0] == "h")
                    {
                        DisplayHelp();
                    }
                    else if(args[0] == "compress")
                    {
                        bool result = DisplayCompress(CMDStage.arg, args);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                    else if(args[0] == "c")
                    {
                        bool result = DisplayCompress(CMDStage.arg, args);
                        
                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                    else if(args[0] == "extract")
                    {
                        bool result = DisplayCompress(CMDStage.arg, args);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                    else if(args[0] == "e")
                    {
                        bool result = DisplayCompress(CMDStage.arg, args);

                        if(result == false)
                            Console.WriteLine("An error accured");
                    }
                }
            }
        }

        struct CompressInfo
        {
            public string FileName;
            public string OutputName;
            public string method;

            public CompressInfo(string file, string output, string method)
            {
                this.FileName = file;
                this.OutputName = output;
                this.method = method;
            }
        }

        static bool DisplayCompress(CMDStage stage, string[] args = null)
        {
            if(stage != CMDStage.noargs && args == null)
                return false;

            if(stage == CMDStage.noargs)
            {
                boiler();

                Console.Write("Please insert folder path: ");
                string fileName = Console.ReadLine();

                Console.Write("Please insert output file name: ");
                string outputName = Console.ReadLine();

                Console.Write("Please insert compression method (1-zip, 2-tar 3-tar.gz): ");
                string method = Console.ReadLine();

                CompressInfo info = new CompressInfo(fileName, outputName, method);
                // Compress
                return Compress(info);
            }
            else if(stage == CMDStage.arg)
            {
                boiler();

                string fileName = args[1];

                Console.Write("Please insert output file name: ");
                string outputName = Console.ReadLine();

                Console.Write("Please insert compression method (1-zip, 2-tar 3-tar.gz): ");
                string method = Console.ReadLine();

                CompressInfo info = new CompressInfo(fileName, outputName, method);
                // Compress
                return Compress(info);
            }
            else if(stage == CMDStage.args)
            {
                boiler();

                string fileName = args[1];

                string outputName = args[2];

                Console.Write("Please insert compression method (1-zip, 2-tar 3-tar.gz): ");
                string method = Console.ReadLine();

                CompressInfo info = new CompressInfo(fileName, outputName, method);
                // Compress
                return Compress(info);
            }
            else if(stage == CMDStage.full)
            {
                boiler();

                string fileName = args[1];
                string outputName = args[2];
                string method = args[3];

                CompressInfo info = new CompressInfo(fileName, outputName, method); 
                // Compress
                return Compress(info);               
            }
            
            return false;
        }

        static bool Compress(CompressInfo info)
        {
            bool success;

            try
            {
                if(info.method == "1" || info.method == "zip")
                {
                    // Zip file

                    ZipFile.CreateFromDirectory(info.FileName, info.OutputName);
                }
                else if(info.method == "12" || info.method == "tar")
                {
                    // Tar file

                    Console.WriteLine("TAR: CREATING TARBALL...");

                    TarArchive archive = TarArchive.CreateOutputTarArchive(File.Create(info.OutputName), null);
                    List<TarEntry> entries = new List<TarEntry>();

                    DirectoryInfo directoryInfo = new DirectoryInfo(info.FileName);

                    Console.WriteLine("TAR: GETTING SUB FILES...");
                    DirectoryInfo[] infos = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);
                    foreach(DirectoryInfo dir in infos)
                    {
                        FileInfo[] fi = dir.GetFiles();

                        foreach(FileInfo f in fi)
                        {
                            Console.WriteLine("    SUB ENTRY: " + f.FullName);

                            entries.Add(TarEntry.CreateEntryFromFile(f.FullName));
                        }
                    }

                    Console.WriteLine("TAR: GETTING ROOT FILES...");
                    FileInfo[] files = directoryInfo.GetFiles();
                    foreach(FileInfo file in files)
                    {
                        Console.WriteLine("    ROOT ENTRY: " + file.FullName);
                        entries.Add(TarEntry.CreateEntryFromFile(file.FullName));
                    }

                    Console.WriteLine("TAR: WRITING ENTRIES...");
                    foreach(TarEntry entry in entries)
                    {
                        archive.WriteEntry(entry, false);
                    }

                    archive.Close();
                    Console.WriteLine("Finished");
                }
                else if(info.method == "3" || info.method == "targz" || info.method == "tar.gz")
                {
                    // Tar file then GZip it

                    Console.WriteLine("TAR.GZ - TARBALL: CREATING TARBALL...");

                    Stream gzoStream = new GZipOutputStream(File.Create(info.OutputName));

                    TarArchive archive = TarArchive.CreateOutputTarArchive(gzoStream, null);
                    List<TarEntry> entries = new List<TarEntry>();

                    DirectoryInfo directoryInfo = new DirectoryInfo(info.FileName);

                    Console.WriteLine("TAR.GZ - TARBALL: GETTING SUB FILES...");
                    DirectoryInfo[] infos = directoryInfo.GetDirectories("*", SearchOption.AllDirectories);
                    foreach(DirectoryInfo dir in infos)
                    {
                        FileInfo[] fi = dir.GetFiles();

                        foreach(FileInfo f in fi)
                        {
                            Console.WriteLine("    SUB ENTRY: " + f.FullName);

                            entries.Add(TarEntry.CreateEntryFromFile(f.FullName));
                        }
                    }

                    Console.WriteLine("TAR.GZ - TARBALL: GETTING ROOT FILES...");
                    FileInfo[] files = directoryInfo.GetFiles();
                    foreach(FileInfo file in files)
                    {
                        Console.WriteLine("    ROOT ENTRY: " + file.FullName);
                        entries.Add(TarEntry.CreateEntryFromFile(file.FullName));
                    }

                    Console.WriteLine("TAR.GZ - TARBALL: WRITING ENTRIES...");
                    foreach(TarEntry entry in entries)
                    {
                        archive.WriteEntry(entry, false);
                    }

                    Console.WriteLine("TAR -> TAR.GZ");
                    archive.Close();
                    Thread.Sleep(new Random().Next(200, 300));

                    /*
                    for(int i = 1; i < 5; i++)
                    {
                        Console.Write("TAR ");
                        if(i == 1)
                            Console.Write("-");
                        else if(i == 2)
                            Console.Write("--");
                        else if(i == 3)
                            Console.Write("---");
                        else if(i == 4)
                            Console.Write("----");
                        else if(i == 5)
                            Console.Write("-----");
                        Console.Write("> TAR.GZ");



                        //Delete shit
                        if(i != 9)
                        {
                            for(int e = 0; e < 8 + i + 4; i++)
                                Console.Write("\b");
                        }
                    }
                    */

                    Console.WriteLine("Finished");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\nStack trace: " + ex.StackTrace);
                return false;
            }
        }

        static bool DisplayExtract(CMDStage stage, string[] args = null)
        {
            if(stage != CMDStage.noargs && args == null)
                return false;

            
            
            return true;
        }

        static void boiler()
        {
            Console.WriteLine(Info.PROGRAM_NAME + "-" + Info.PROGRAM_VERSION + " by " + Info.PROGRAM_DEV);
            Console.WriteLine("Source code avaiable at https://github.com/DDev247/zippo");
            Console.WriteLine();
        }

        static void DisplayHelp()
        {
            boiler();
            Console.WriteLine("    help     (h) - Get help info");
            Console.WriteLine("    compress (c) - Compress a file or folder");
            Console.WriteLine("    extract  (e) - Extract a file");
            Console.WriteLine();
        }
    }
}
