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
        public MinecraftPaths Paths
        {
            get
            {
                return m_path;
            }
        }
        private MinecraftJar m_jar;
        private ModConfig m_config;
        private FileSystemWatcher fsWatcher;
        public MinecraftInstance()
        {
            m_path = new MinecraftPaths();
            m_jar = new MinecraftJar(m_path.jarPath);
            m_jar.TempPath = m_path.tempDir;
            CompressedFile.defaultTempDir = m_path.tempDir;
            m_config = new ModConfig(m_path);
            //fsWatcher = new FileSystemWatcher(m_path.appModDir);
            //fsWatcher.Changed += new FileSystemEventHandler(fsWatcher_Changed);
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
        public void removeMod(Mod m)
        {
            DirectoryInfo tempRoot = m.ExtractedRoot;
            if (m.Destination == ModDestinations.COMPLEX)
            {
                foreach (IFSAction act in m.InstallActions)
                {

                }
            }
            tempRoot.Delete(true);
            
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
            m_jar.extractToTemp();
            foreach (Mod m in m_config.getAllMods())
            {
                Console.WriteLine(m.Name);
                foreach (IFSAction act in m.InstallActions)
                {
                    act.execute(m_path);
                }
            }
            m_jar.installJAR();
        }
        public void addAction(Mod m, IFSAction action)
        {
            m.InstallActions.Add(action);
        }
        public void addDirCopyAction(Mod m, string source, string dest)
        {
            DirectoryCopyAction act = new DirectoryCopyAction(source, dest);
            m.InstallActions.Add(act);
            m_config.updateMod(m);
        }
        public void addFileCopyAction(Mod m, string source, string dest)
        {
            FileCopyAction act = new FileCopyAction(source, dest);
            m.InstallActions.Add(act);
            m_config.updateMod(m);
        }
        public void addDirCopyAction(int id, string source, string dest)
        {
            addDirCopyAction(getMod(id), source, dest);
        }
        public void addFileCopyAction(int id, string source, string dest)
        {
            addFileCopyAction(getMod(id), source, dest);
        }
        public void addDirCopyAction(string m, string source, string dest)
        {
            addDirCopyAction(getMod(m), source, dest);
        }
        public void addFileCopyAction(string m, string source, string dest)
        {
            addFileCopyAction(getMod(m), source, dest);
        }
        
        
    }
}