using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

using System.Text;

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
                ConsoleColor clr = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("Error: " + ex.Message + "\nStack trace: " + ex.StackTrace);              

                Console.ForegroundColor = clr;

                return false;
            }
        }

        struct ExtractInfo
        {
            public string FileName;
            public string OutputName;

            public ExtractInfo(string file, string output)
            {
                this.FileName = file;
                this.OutputName = output;
            }
        }

        static bool DisplayExtract(CMDStage stage, string[] args = null)
        {
            if(stage != CMDStage.noargs && args == null)
                return false;

            if(stage == CMDStage.noargs)
            {
                boiler();

                Console.Write("Please insert folder path: ");
                string fileName = Console.ReadLine();

                Console.Write("Please insert output folder name: ");
                string outputName = Console.ReadLine();

                ExtractInfo info = new ExtractInfo(fileName, outputName);
                // Extract
                return Extract(info);
            }
            else if(stage == CMDStage.arg)
            {
                boiler();

                string fileName = args[1];

                Console.Write("Please insert output folder name: ");
                string outputName = Console.ReadLine();

                ExtractInfo info = new ExtractInfo(fileName, outputName);
                // Extract
                return Extract(info);
            }
            else if(stage == CMDStage.args)
            {
                boiler();

                string fileName = args[1];

                string outputName = args[2];

                ExtractInfo info = new ExtractInfo(fileName, outputName);
                // Extract
                return Extract(info);
            }
            else if(stage == CMDStage.full)
            {
                boiler();

                string fileName = args[1];
                string outputName = args[2];
                // Ignore third arg string method = args[3];
                ConsoleColor clr = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
         
                Console.WriteLine("Warning: Argument not needed for extraction: method");

                Console.ForegroundColor = clr;

                ExtractInfo info = new ExtractInfo(fileName, outputName);
                // Extract
                return Extract(info);             
            }
            
            return false;
        }

        static bool Extract(ExtractInfo info)
        {
            try
            {
                if(info.FileName.EndsWith(".zip"))
                {
                    // Zip file
                    if(!Directory.Exists(info.OutputName))
                        Directory.CreateDirectory(info.OutputName);

                    ZipFile.ExtractToDirectory(info.FileName, info.OutputName);
                }
                else if(info.FileName.EndsWith(".tar"))
                {
                    // Tar file
                    if(!Directory.Exists(info.OutputName))
                        Directory.CreateDirectory(info.OutputName);

                    Tar.ExtractTar(info.FileName, info.OutputName);
                }
                else if(info.FileName.EndsWith(".tar.gz"))
                {
                    // Tar GZip file
                    if(!Directory.Exists(info.OutputName))
                        Directory.CreateDirectory(info.OutputName);

                    Tar.ExtractTarGz(info.FileName, info.OutputName);
                }
                else
                {
                    throw new NotSupportedException("File type not supported: " + Path.GetExtension(info.FileName));
                }
            
                return true;
            }
            catch (Exception ex)
            {
                ConsoleColor clr = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("Error: " + ex.Message + "\nStack trace: " + ex.StackTrace);              

                Console.ForegroundColor = clr;

                return false;
            }
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

public class Tar
	{
		/// <summary>
		/// Extracts a <i>.tar.gz</i> archive to the specified directory.
		/// </summary>
		/// <param name="filename">The <i>.tar.gz</i> to decompress and extract.</param>
		/// <param name="outputDir">Output directory to write the files.</param>
		public static void ExtractTarGz(string filename, string outputDir)
		{
			using (var stream = File.OpenRead(filename))
				ExtractTarGz(stream, outputDir);
		}

		/// <summary>
		/// Extracts a <i>.tar.gz</i> archive stream to the specified directory.
		/// </summary>
		/// <param name="stream">The <i>.tar.gz</i> to decompress and extract.</param>
		/// <param name="outputDir">Output directory to write the files.</param>
		public static void ExtractTarGz(Stream stream, string outputDir)
		{
			// A GZipStream is not seekable, so copy it first to a MemoryStream
			using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
			{
				const int chunk = 4096;
				using (var memStr = new MemoryStream())
				{
					int read;
					var buffer = new byte[chunk];
					do
					{
						read = gzip.Read(buffer, 0, chunk);
						memStr.Write(buffer, 0, read);
					} while (read == chunk);

					memStr.Seek(0, SeekOrigin.Begin);
					ExtractTar(memStr, outputDir);
				}
			}
		}

		/// <summary>
		/// Extractes a <c>tar</c> archive to the specified directory.
		/// </summary>
		/// <param name="filename">The <i>.tar</i> to extract.</param>
		/// <param name="outputDir">Output directory to write the files.</param>
		public static void ExtractTar(string filename, string outputDir)
		{
			using (var stream = File.OpenRead(filename))
				ExtractTar(stream, outputDir);
		}

		/// <summary>
		/// Extractes a <c>tar</c> archive to the specified directory.
		/// </summary>
		/// <param name="stream">The <i>.tar</i> to extract.</param>
		/// <param name="outputDir">Output directory to write the files.</param>
		public static void ExtractTar(Stream stream, string outputDir)
		{
			var buffer = new byte[100];
			while (true)
			{
				stream.Read(buffer, 0, 100);
				var name = Encoding.ASCII.GetString(buffer).Trim('\0');
				if (String.IsNullOrWhiteSpace(name))
					break;
				stream.Seek(24, SeekOrigin.Current);
				stream.Read(buffer, 0, 12);
				var size = Convert.ToInt64(Encoding.UTF8.GetString(buffer, 0, 12).Trim('\0').Trim(), 8);

				stream.Seek(376L, SeekOrigin.Current);

				var output = Path.Combine(outputDir, name);
				if (!Directory.Exists(Path.GetDirectoryName(output)))
					Directory.CreateDirectory(Path.GetDirectoryName(output));
                if (!name.Equals("./", StringComparison.InvariantCulture)) 
                {
                    Console.WriteLine(output);
                    using (var str = File.Open(output, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        var buf = new byte[size];
                        stream.Read(buf, 0, buf.Length);
                        str.Write(buf, 0, buf.Length);
                    }
                }

				var pos = stream.Position;
	
				var offset = 512 - (pos  % 512);
				if (offset == 512)
					offset = 0;

				stream.Seek(offset, SeekOrigin.Current);
			}
		}
	}
