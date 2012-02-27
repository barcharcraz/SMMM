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
        public void CopyDirectory(DirectoryInfo source, DirectoryInfo dest)
        {
            CopyDirectory(source.FullName, dest.FullName);
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
        public static void UndoDirectoryCopy(DirectoryInfo source, DirectoryInfo target)
        {
            
            FileSystemInfo[] contents = source.GetFileSystemInfos();
            
            foreach (FileSystemInfo fs in contents)
            {
                if (fs is DirectoryInfo)
                {
                    DirectoryInfo dirinfo = fs as DirectoryInfo;
                    IEnumerable<DirectoryInfo> matchingInTarget = from DirectoryInfo d in target.GetDirectories()
                                                                  where d.Name == dirinfo.Name
                                                                  select d;
                    UndoDirectoryCopy(fs as DirectoryInfo, matchingInTarget.ElementAt(0));
                }
                else if (fs is FileInfo)
                {
                    FileInfo fi = fs as FileInfo;
                    IEnumerable<FileInfo> matches = from FileInfo f in target.GetFiles()
                                                    where f.Name == fi.Name
                                                    select f;
                    foreach (FileInfo file in matches)
                    {
                        file.Delete();
                    }
                }
            }
        }
        public static void UndoDirectoryCopy(string source, string target)
        {
            UndoDirectoryCopy(new DirectoryInfo(source), new DirectoryInfo(target) );
        }
    }

}
