using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMWPF
{
    public class FileSystemViewModel
    {
        public string RootName
        {
            get { return Root.FullName; }
            set
            {
                UpdateRoot(new DirectoryInfo(value));
            }
        }
        public string RootFileName
        {
            get { return Path.GetFileName(RootName); }
        }
        private FileSystemInfo root;
        public FileSystemInfo Root
        {
            get { return root; }
            set
            {
                UpdateRoot(value);
            }
        }
        public ReadOnlyCollection<FileSystemViewModel> children { get; set; }
        public FileSystemViewModel(FileSystemInfo path)
        {
            
            UpdateRoot(path);

        }
        private void UpdateRoot(FileSystemInfo path)
        {

            if (path.Attributes.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo droot = path as DirectoryInfo;
                root = droot;
                if (droot != null)
                {
                    List<FileSystemViewModel> tempList = (from FileSystemInfo d in droot.EnumerateFileSystemInfos() select new FileSystemViewModel(d)).ToList();
                    children = new ReadOnlyCollection<FileSystemViewModel>(tempList);
                }
            }
            else
            {
                root = new FileInfo(path.FullName);
            }
        }

        public FileSystemViewModel(string root)
        {
            RootName = root;
        }
        public FileSystemViewModel()
        {
            
        }
    }
}
