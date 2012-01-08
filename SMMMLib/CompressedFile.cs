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
        private DirectoryInfo m_extractedRoot;

        public DirectoryInfo ExtractedRoot
        {
            get
            {
                if (m_extractedRoot == null)
                {
                    extractToTemp();
                }
                return m_extractedRoot;
            }
            private set
            {
                m_extractedRoot = value;
            }
        }
        public string TempPath { get; set; }
        public string FilePath { get; set; }
        /// <summary>
        /// Creates a compressed file around the given file path
        /// </summary>
        /// <param name="f"> the file to wrap </param>
        public CompressedFile(string f)
        {
            FilePath = f;
            TempPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "temp";
            
        }
        public DirectoryInfo extractToTemp()
        {
            SevenZipExtractor extractor = new SevenZipExtractor(FilePath);
            DirectoryInfo temp = new DirectoryInfo(TempPath);
            temp.CreateSubdirectory(Path.GetFileNameWithoutExtension(extractor.FileName));
            temp = temp.GetDirectories(Path.GetFileNameWithoutExtension(extractor.FileName))[0];
            
            extractor.ExtractArchive(temp.FullName);
            ExtractedRoot = temp;
            return temp;
            
        }
        public DirectoryInfo extractToTemp(string subdir)
        {
            SevenZipExtractor extractor = new SevenZipExtractor(FilePath);
            DirectoryInfo temp = new DirectoryInfo(TempPath);
            temp.CreateSubdirectory(subdir);
            temp = temp.GetDirectories(subdir)[0];
            extractor.ExtractArchive(temp.FullName);
            ExtractedRoot = temp;
            return temp;
        }
        /// <summary>
        /// recompress the archive to an archive with a specified filename
        /// </summary>
        /// <param name="fileName">the filename of the new archive</param>
        public void reCompress(string fileName)
        {
            SevenZipCompressor compressor = new SevenZipCompressor();
            compressor.CompressDirectory(ExtractedRoot.FullName, fileName);
        }
        /// <summary>
        /// recompresses the archive to an archive with the same name as the source
        /// </summary>
        public void reCompress()
        {
            SevenZipExtractor extractor = new SevenZipExtractor(FilePath);
            reCompress(TempPath + Path.DirectorySeparatorChar + Path.GetFileName(extractor.FileName));
        }
        

    }
}
