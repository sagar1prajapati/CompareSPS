using System;
using System.Collections.Generic;
using System.Text;

namespace CompareSPS
{
    public class FormatPath
    {
        public static string GetFilePath(string path, string item)
        {
            string Path = path + "\\" + item;
            Path = Path.Trim();
            return Path;
        }
        public static string GetFilePath(string path, string subpath, string item)
        {
            string Path = path + "\\" + subpath + "\\" + item;
            Path = Path.Trim();
            return Path;
        }
    }
}
