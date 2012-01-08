using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMLib
{
    internal class FileCopyAction : IFSAction
    {
        public string source { get; set; }
        public string target { get; set; }
        public void execute(MinecraftPaths p)
        {
            File.Copy(p.resolvePath(source), p.resolvePath(target));
        }
        public FileCopyAction(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
    }
}
