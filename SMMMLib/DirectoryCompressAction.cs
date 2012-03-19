using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenZip;
using System.IO;

namespace SMMMLib
{
    public class DirectoryCompressAction : IFSAction, IReversibleFSAction
    {
        public DirectoryCompressAction(string source, string target)
        {
            this.source = source;
            this.target = target;
        }

        public DirectoryCompressAction(string source, string target, ICollection<KeyValuePair<string, string>> tags)
            : this(source, target)
        {
            ExtraTags = tags;
        }


        public void execute(MinecraftPaths p)
        {
            SevenZipCompressor comp = new SevenZipCompressor();
            comp.ArchiveFormat = OutArchiveFormat.Zip;
            comp.CompressDirectory(p.resolvePath(source, ExtraTags), p.resolvePath(target, ExtraTags));
        }

        public string source { get; set; }

        public string target { get; set; }

        public ICollection<KeyValuePair<string, string>> ExtraTags { get; set; }

        public void reverse(MinecraftPaths p)
        {
            File.Delete(p.resolvePath(target));
        }

        public override string ToString()
        {
            return "COMPRESS DIRECTORY " + source + " AS " + target;
        }
    }
}
