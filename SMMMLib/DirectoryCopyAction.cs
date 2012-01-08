using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMLib
{
    internal class DirectoryCopyAction : IFSAction
    {
        
        public string source { get; set; }
        public string target { get; set; }
        public void execute(MinecraftPaths p)
        {
            SMMMUtil.FileSystemUtils.CopyDirectory(p.resolvePath(source), p.resolvePath(target));
        }
        public DirectoryCopyAction(string source, string target)
        {
            this.source = source;
            this.target = target;
        }
        


        
    }
}
