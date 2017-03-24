using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using GameXmlGenerator.Helpers;

namespace GameXmlGenerator
{
    class Program
    {

        static void Main(string[] args)
        {
            var sourcePath = ConfigurationManager.AppSettings["folderLocation"];
            
            if (!Directory.Exists(sourcePath))
            {
                Directory.CreateDirectory(sourcePath);
            }

            if (!Directory.GetFiles(sourcePath).Any())
            {
                Console.WriteLine(string.Format("The source directory is empty {0}", sourcePath));
                Console.Read();
                Environment.Exit(0);
            }

            foreach (string file in Directory.EnumerateFiles(sourcePath, "*.csv"))
            {
                var lines = File.ReadAllLines(file);

                var headers = lines.FirstOrDefault().Split(',');
                var xmlHelp = new XmlHelper(headers, new XElement("Providers"));

                lines = lines.Skip(1).ToArray();
                var xml = new XElement("Games");

                foreach (string line in lines)
                {

                    var filename = Path.GetFileName(file);
                    var game = new XElement("Game");

                    game = xmlHelp.BuildGame(game, line.Split(','), xmlHelp.GetClub(filename));

                    xml.Add(game);
                }

                if (xmlHelp.Providers.HasElements) xml.Add(xmlHelp.Providers);
                var outputfile = file.Replace(".csv", ".xml");
                xml.Save(outputfile);

                MoveFile(outputfile);
            }

            Console.WriteLine("press any key to exit...");
            Console.Read();

            Environment.Exit(0);
        }

        static void MoveFile(string source)
        {
            var savePath = ConfigurationManager.AppSettings["saveLocation"];
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            File.Copy(source, savePath + Path.GetFileName(source), true);
            
            if (File.Exists(source))
                File.Delete(source);

            Console.WriteLine(string.Format("{0} generated \n", savePath + Path.GetFileName(source)));
        }
    }
}
