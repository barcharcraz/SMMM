using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMLib
{
    public class FileCopyAction : IFSAction
    {
        public string source { get; set; }
        public string target { get; set; }
        public ICollection<KeyValuePair<string, string>> ExtraTags { get; set; }
        public void execute(MinecraftPaths p)
        {
            File.Copy(p.resolvePath(source, ExtraTags), p.resolvePath(target, ExtraTags),true);
        }
        public FileCopyAction(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
        public FileCopyAction(string source, string target, ICollection<KeyValuePair<string, string>> tags)
            : this(source, target)
        {
            ExtraTags = tags;
        }
        public override string ToString()
        {
            return "COPY FILE FROM " + source + " TO " + target;
        }
    }
}
