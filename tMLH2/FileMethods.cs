using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tMLH2
{
    static class FileMethods
    {
        /// <summary>
        /// Reads all lines of the file without causing a lock.
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <param name="fileName">The file to read.</param>
        /// <returns>The contents of the file.</returns>
        public static string SilentlyReadAllLines(string fileName) 
        {
            using (FileStream fs = File.Open(fileName,
                        FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
