using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMUtil
{
    public class FileSystemUtils
    {
        /// <summary>
        /// Code for copying directories taken from Microsoft example/how-to
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void CopyDirectory(string source, string dest)
        {
            DirectoryInfo srcInfo = new DirectoryInfo(source);
            DirectoryInfo[] subdirs = srcInfo.GetDirectories();
            if (!srcInfo.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " +
                source);
            }

            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            //get the file infoz of the files to copy
            FileInfo[] files = srcInfo.GetFiles();

            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(dest, file.Name);
                file.CopyTo(tempPath, true);
            }

            foreach (DirectoryInfo dir in subdirs)
            {
                string tempPath = Path.Combine(dest, dir.Name);
                CopyDirectory(dir.FullName, tempPath);
            }
        }

        public static void DeleteDirectory(string dir)
        {
            DirectoryInfo target = new DirectoryInfo(dir);
            DirectoryInfo[] dirs = target.GetDirectories();
            if (!target.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " +
                target);
            }
            FileInfo[] files = target.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            foreach (DirectoryInfo d in dirs)
            {
                DeleteDirectory(d.FullName);
            }
            target.Delete();
            
        }
    }
}
