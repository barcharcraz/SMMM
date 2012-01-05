using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SMMMLib
{
    public class MinecraftJar : CompressedFile
    {
        private ICollection<Mod> m_mods;
        public ICollection<Mod> Mods
        {
            get
            {
                return m_mods;
            }
            private set
            {
                m_mods = value;
            }
        }
        public MinecraftJar() : base(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\.minecraft\\bin\\minecraft.jar")
        {
            
            Mods = new System.Collections.Generic.List<Mod>();
        }
        public MinecraftJar(string path)
            : base(path)
        {
            Mods = new List<Mod>();
        }
        /// <summary>
        /// install mods to minecraft.jar and recompress it
        /// 
        /// this does not actually move minecraft.jar back to the .minecraft/bin folder
        /// </summary>
        public void installMods()
        {
            DirectoryInfo jarDir = this.extractToTemp();
            List<DirectoryInfo> dirs = new List<DirectoryInfo>();
            foreach (Mod m in Mods)
            {
                dirs.Add(m.extractToTemp());
            }
            foreach (DirectoryInfo info in dirs)
            {
                
                
                info.MoveTo(this.ExtractedRoot.FullName);
            }
            this.reCompress();
        }
    }
}
