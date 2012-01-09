using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace SMMMLib
{
    public interface IFSAction
    {
        void execute(MinecraftPaths p);
        string source { get; set; }
        string target { get; set; }
        ICollection<KeyValuePair<string, string>> ExtraTags { get; set; }
        
    }
}
