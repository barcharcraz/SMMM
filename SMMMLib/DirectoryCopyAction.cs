using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using SevenZip;

namespace SMMMLib
{
    public class DirectoryCopyAction : IFSAction
    {
        
        public string source { get; set; }
        public string target { get; set; }
        public ICollection<KeyValuePair<string, string>> ExtraTags { get; set; }
        public void execute(MinecraftPaths p)
        {
            Regex zip = new Regex(@"ZIP$");
            if (zip.IsMatch(target))
            {
                SevenZip.SevenZipCompressor comp = new SevenZipCompressor();
                Match m = zip.Match(target);

                target = target.Remove(m.Index) + "NAME";
                comp.CompressDirectory(p.resolvePath(source, ExtraTags), p.resolvePath(target, ExtraTags));

            }
            else
            {
                SMMMUtil.FileSystemUtils.CopyDirectory(p.resolvePath(source, ExtraTags), p.resolvePath(target, ExtraTags));

            }
        }
        public DirectoryCopyAction(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
        
        public DirectoryCopyAction(string source, string target, ICollection<KeyValuePair<string, string>> tags)
            : this(source, target)
        {
            ExtraTags = tags;
        }
        public override string ToString()
        {
            return "COPY DIRECTORY FROM " + source + " TO " + target;
        }
        
    }
}
