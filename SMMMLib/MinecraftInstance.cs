using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMLib
{
    public class MinecraftInstance
    {
        private MinecraftPaths m_path;
        private MinecraftJar m_jar;
        private ModConfig m_config;
        private FileSystemWatcher fsWatcher;
        public MinecraftInstance()
        {
            m_path = new MinecraftPaths();
            m_jar = new MinecraftJar(m_path.jarPath);
            m_config = new ModConfig(m_path);
            fsWatcher = new FileSystemWatcher(m_path.appModDir);
            fsWatcher.Changed += new FileSystemEventHandler(fsWatcher_Changed);
            
        }

        void fsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    break;
                case WatcherChangeTypes.Created:
                    m_config.addMod(new Mod(e.FullPath));
                    break;
                case WatcherChangeTypes.Deleted:
                    m_config.removeMod(new Mod(e.FullPath));
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                default:
                    break;
            }
            m_config.save();
            
        }

        private void install(Mod m)
        {
            switch (m.Destination)
            {
                case ModDestinations.JAR:
                    installToJar(m);
                    break;
                case ModDestinations.MODS:
                    installToMods(m);
                    break;
                case ModDestinations.COMPLEX:
                    installToComplex(m);
                    break;
                default:
                    break;
            }
        }
        private void installToJar(Mod m)
        {
            
            SMMMUtil.FileSystemUtils.CopyDirectory(
                m.ExtractedRoot.FullName, 
                m_jar.ExtractedRoot.FullName
                );
            m_jar.deleteMETAINF();
        }
        private void installToMods(Mod m)
        {
            FileInfo modFile = new FileInfo(m.FilePath);
            modFile.CopyTo(m_path.modsDir);
        }
        private void installToComplex(Mod m)
        {
            throw new NotImplementedException();
        }
        
        
    }
}