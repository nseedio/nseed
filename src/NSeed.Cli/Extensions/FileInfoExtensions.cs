using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSeed.Cli.Extensions
{
    internal static class FileInfoExtensions
    {
        public static string GetNameWithoutExtension(this FileInfo fileInfo)
        {
            if (string.IsNullOrEmpty(fileInfo.Name))
                return string.Empty;
            if (string.IsNullOrEmpty(fileInfo.Extension))
                return fileInfo.Name;
            return Regex.Replace(fileInfo.Name, fileInfo.Extension, string.Empty);
        }
    }
}
