using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
using System.Collections.ObjectModel;

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
        public ICollection<Mod> AllMods
        {
            get
            {
                return m_config.getAllMods() as ICollection<Mod>;
            }
        }
        private MinecraftJar m_jar;
        private ModConfig m_config;
        private FileSystemWatcher fsWatcher;
        private const string INIT_BAK_NAME = "origBak";
        public MinecraftInstance()
        {
            m_path = new MinecraftPaths();
            m_jar = new MinecraftJar(m_path.jarPath);
            m_jar.TempPath = m_path.tempDir;
            CompressedFile.defaultTempDir = m_path.tempDir;
            m_config = new ModConfig(m_path);
            
            //fsWatcher = new FileSystemWatcher(m_path.appModDir);
            //fsWatcher.Changed += new FileSystemEventHandler(fsWatcher_Changed);
            InitInstance();
            addAllMods();
            pruneConfig();
            
            
        }
        private void InitInstance()
        {
            if (!File.Exists(Path.Combine(Paths.appBakDir, INIT_BAK_NAME + ".bak")))
            {
                
                makeBakup(INIT_BAK_NAME);
            }
        }
        private void makeBakup(string name)
        {
            SevenZip.SevenZipCompressor comp = new SevenZipCompressor();
            //blocking compress, think about putting a progress dialog in later builds, or implement
            //a system to watch for progress on the minecraft instance
            comp.CompressDirectory(Paths.minecraftRoot, Path.Combine(Paths.appBakDir, name + ".bak"));
        }
        private void RestoreBackup(string name)
        {
            SevenZip.SevenZipExtractor ext = new SevenZipExtractor(Path.Combine(Paths.appBakDir, name + ".bak"));
            DeleteRoot();
            ext.ExtractArchive(Paths.minecraftRoot);
        }
        /// <summary>
        /// deletes the .minecraft directory(default) or whatever else might be set as the 
        /// minecraft root directory, not a great thing to call on a regular basis,
        /// it will make you sad
        /// </summary>
        private void DeleteRoot()
        {
            Directory.Delete(Paths.minecraftRoot, true);
        }
        /// <summary>
        /// cleans the minecraft install by first deleteing the minecraft root and then
        /// restoring the original backup
        /// </summary>
        public void Clean()
        {
            RestoreBackup(INIT_BAK_NAME);
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
                    if (act is IReversibleFSAction)
                    {
                        (act as IReversibleFSAction).reverse(Paths);
                    }
                }
            }
            tempRoot.Delete(true);
            
        }
        public ObservableCollection<Mod> getInstalledMods()
        {
            return new ObservableCollection<Mod>(m_config.getAllMods() as IEnumerable<Mod>);
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
            foreach (Mod m in m_config.getActiveMods())
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