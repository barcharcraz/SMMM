using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace SMMMLib
{
    public class ActionFactory
    {

        
        public static IFSAction GenerateAction(MinecraftPaths p, string source, string target)
        {
            string s = p.resolvePath(source);
            string t = p.resolvePath(target);
            if (Directory.Exists(s))
            {
                return new DirectoryCopyAction(s,t);
            }
            else if (File.Exists(s))
            {
                return new FileCopyAction(s, t);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

       
    }
}
