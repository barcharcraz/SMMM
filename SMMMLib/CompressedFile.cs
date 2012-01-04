using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenZip;
using System.IO;

namespace SMMMLib
{
    public class CompressedFile
    {
        private SevenZipCompressor compressor;
        private SevenZipExtractor extractor;
        private string m_tempPath = Directory.GetCurrentDirectory();
        public string TempPath { get; set; }
        /// <summary>
        /// Creates a compresseed file around the given file path
        /// </summary>
        /// <param name="f"> the file to wrap </param>
        public CompressedFile(string f)
        {
            compressor = new SevenZipCompressor();
            extractor = new SevenZipExtractor(f);
            
        }
        public DirectoryInfo extractToTemp()
        {
            DirectoryInfo temp = new DirectoryInfo(TempPath);
            temp.CreateSubdirectory(Path.GetFileNameWithoutExtension(extractor.FileName));
            temp = temp.GetDirectories(Path.GetFileNameWithoutExtension(extractor.FileName))[0];
            
            extractor.ExtractArchive(temp.FullName);
            return temp;
            
        }
        public DirectoryInfo extractToTemp(string subdir)
        {
            DirectoryInfo temp = new DirectoryInfo(TempPath);
            temp.CreateSubdirectory(subdir);
            temp = temp.GetDirectories(subdir)[0];
            extractor.ExtractArchive(temp.FullName);
            return temp;
        }
        

    }
}
