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
            addAllMods();
            pruneConfig();
            
        }
        private void addAllMods()
        {
            DirectoryInfo dir = new DirectoryInfo(m_path.appModDir);
            FileInfo log = new FileInfo(Path.Combine(m_path.appLogDir, "log.txt"));
            StreamWriter logout = new StreamWriter(log.FullName);
            
            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    m_config.addMod(new Mod(file, m_config.getNextID()));
                }
                catch (exceptions.DuplicateItemException e)
                {
                    logout.WriteLine(e);
                }
                
            }
            logout.Close();
            m_config.save();
        }
        private void pruneConfig()
        {
            foreach (Mod m in m_config.getAllMods())
            {
                if (!(File.Exists(m.FilePath)))
                {
                    m_config.removeMod(m);
                }
            }
        }
        public IEnumerable<Mod> getInstalledMods()
        {
            return m_config.getAllMods();
        }
        void fsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    break;
                case WatcherChangeTypes.Created:
                    m_config.addMod(new Mod(e.FullPath, m_config.getNextID()));
                    break;
                case WatcherChangeTypes.Deleted:
                    m_config.removeMod(new Mod(e.FullPath, m_config.getNextID()));
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                default:
                    break;
            }
            m_config.save();
            
        }
        public Mod getMod(int id)
        {
            return m_config.getMod(id);
        }
        public Mod getMod(string name)
        {
            return m_config.getMod(name);
        }
        public void installAll()
        {
            foreach (Mod m in m_config.getAllMods())
            {
                foreach (IFSAction act in m.InstallActions)
                {
                    act.execute(m_path);
                }
            }
        }
        private void install(Mod m)
        {
            m.TempPath = m_path.tempDir;
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
            modFile.CopyTo(Path.Combine(m_path.modsDir,modFile.Name), true);
        }
        private void installToComplex(Mod m)
        {
            throw new NotImplementedException();
        }
        
        
    }
}