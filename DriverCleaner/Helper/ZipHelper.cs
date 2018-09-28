using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml.Linq;


namespace DriverCleaner.Helper
{
    public static class ZipHelper
    {
        public static void ExtractFileToDirectory(string zipFileName, string outputDirectory)
        {
            if (Directory.Exists(outputDirectory))
            {
                var di = new DirectoryInfo(outputDirectory);
                di.Attributes &= ~FileAttributes.ReadOnly;

                foreach (var file in di.GetFiles("*", SearchOption.AllDirectories))
                    file.Attributes &= ~FileAttributes.ReadOnly;

                Directory.Delete(outputDirectory, true);
            }

            Directory.CreateDirectory(outputDirectory);

            string zPath = "7za.exe"; //add to proj and set CopyToOuputDir
            try
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.FileName = zPath;
                pro.Arguments = string.Format("x \"{0}\" -y -o\"{1}\"", zipFileName, outputDirectory);
                Process x = Process.Start(pro);
                x.WaitForExit();
            }
            catch (System.Exception Ex)
            {
                //handle error
            }

            var directories = Directory.GetDirectories(outputDirectory);
            foreach (var dir in directories)
            {
                var dirName = new DirectoryInfo(dir).Name;
                if (!Properties.Settings.Default.directories.Contains(dirName))
                {
                    var di = new DirectoryInfo(dir);
                    di.Attributes &= ~FileAttributes.ReadOnly;
                    foreach (var file in di.GetFiles("*", SearchOption.AllDirectories))
                        file.Attributes &= ~FileAttributes.ReadOnly;

                    Directory.Delete(dir, true);
                }
            }

           
        }

        public static void CleanCfg(string fileNameCfg)
        {
            XElement cfgXml = XElement.Load(fileNameCfg);
            var elems = cfgXml.Element("manifest")?.Elements()
                .Where(p => Properties.Settings.Default.eulaStr.Contains(p.Attribute("name")?.Value));
            elems?.Remove();

            cfgXml.Save(fileNameCfg);


        }
    }
}
