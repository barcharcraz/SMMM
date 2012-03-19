using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace SMMMLib
{
    public class ActionFactory
    {

        
        public static IFSAction GenerateAction(MinecraftPaths p, string source, string target, ICollection<KeyValuePair<string,string>> extraTags = null,  bool compress=true)
        {
            
            
            string s = p.resolvePath(source,extraTags);
            string t = p.resolvePath(target,extraTags);
            if (Path.GetExtension(t) == ".zip")
            {
                return new DirectoryCompressAction(p.CompressPath(s, extraTags), p.CompressPath(t, extraTags));
            }
            else if (Directory.Exists(s))
            {
                return new DirectoryCopyAction(p.CompressPath(s, extraTags), p.CompressPath(t, extraTags));
            }
            else if (File.Exists(s))
            {
                return new FileCopyAction(p.CompressPath(s, extraTags), Path.Combine(p.CompressPath(t, extraTags), Path.GetFileName(s)));
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

       
    }
}
