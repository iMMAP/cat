using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace iMMAP.iMPROVE.Core.Services
{
    public interface IIOService
    {
        string GetDatabasePath();
        string GetUsersDatabasePath();
        string GetUniqueFilePath(string filePath);
    }

    public class IOService: IIOService
    {
        public string GetDatabasePath()
        {
            string relPath = System.Configuration.ConfigurationManager.AppSettings["Database"];
            string absPath = HttpContext.Current.Server.MapPath(relPath);
            return absPath;
        }

        public string GetUsersDatabasePath()
        {
            string relPath = System.Configuration.ConfigurationManager.AppSettings["UsersDatabase"];
            string absPath = HttpContext.Current.Server.MapPath(relPath);
            return absPath;
        }

        public string GetUniqueFilePath(string filePath)
        {
            if (File.Exists(filePath))
            {
                string folder = Path.GetDirectoryName(filePath);
                string filename = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                int number = 1;

                Match regex = Regex.Match(filePath, @"(.+)_(\d+)\.\w+");

                if (regex.Success)
                {
                    filename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    filePath = Path.Combine(folder, string.Format("{0}_{1}{2}", filename, number, extension));
                }
                while (File.Exists(filePath));
            }

            return filePath;
        }
    }
}