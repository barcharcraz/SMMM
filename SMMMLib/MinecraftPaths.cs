using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMLib
{
    public class MinecraftPaths
    {
        private static string defaultRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        public List<KeyValuePair<string, string>> DefaultTags { private set; get; }
        private List<KeyValuePair<string, string>> CompressTags { get; set; }
        public string minecraftRoot;
        public string appRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SMMM");
        public string binDir;
        public string jarPath;
        public string resourcesDir;
        public string modsDir;
        public string configDir;
        public string tempDir;
        public string appConfigDir;
        public string modConfigFile;
        public string appModDir;
        public string appLogDir;
        public string appBakDir;
        public MinecraftPaths(string baseDir)
        {
            minecraftRoot = baseDir;
            binDir = baseDir + "\\bin";
            jarPath = binDir + "\\minecraft.jar";
            resourcesDir = baseDir + "\\resources";
            modsDir = baseDir + "\\mods";
            tempDir = appRoot + "\\temp";
            configDir = baseDir + "\\config";
            appConfigDir = appRoot + "\\config";
            modConfigFile = appConfigDir + "\\config.xml";
            appModDir = appRoot + "\\Mods";
            appBakDir = appRoot + "\\bak";
            appLogDir = appRoot;


            createDirs();
            InitTags();
        }

        public MinecraftPaths()
            : this(defaultRoot)
        {

        }
        #region tagStuff
        private void InitTags()
        {
            DefaultTags = new List<KeyValuePair<string, string>>();
            DefaultTags.Add(new KeyValuePair<string, string>("JAR", Path.Combine(tempDir, "minecraft")));
            DefaultTags.Add(new KeyValuePair<string, string>("MODS", modsDir));
            DefaultTags.Add(new KeyValuePair<string, string>("RESOURCES", resourcesDir));
            DefaultTags.Add(new KeyValuePair<string, string>("MCROOT", minecraftRoot));
            DefaultTags.Add(new KeyValuePair<string, string>("BIN", binDir));
            CompressTags = new List<KeyValuePair<string, string>>();
            CompressTags = DefaultTags.ToList();
            modifyTag(CompressTags, "JAR", Path.Combine(binDir, "minecraft.jar"));

        }
        private void modifyTag(List<KeyValuePair<string, string>> input, string tagKey, string newValue)
        {
            for (int i = 0; i < input.Count; i++)
            {
                KeyValuePair<string, string> kv = input[i];
                if (kv.Key == tagKey)
                {
                    kv = new KeyValuePair<string, string>(tagKey, newValue);
                    input[i] = kv;
                }
            }
        }

        #endregion
        private void createDirs()
        {
            if (!Directory.Exists(appRoot))
            {
                Directory.CreateDirectory(appRoot);
            }
            if (!Directory.Exists(appModDir))
            {
                Directory.CreateDirectory(appModDir);
            }
            if (!Directory.Exists(appLogDir))
            {
                Directory.CreateDirectory(appModDir);
            }
            if (!Directory.Exists(appConfigDir))
            {
                Directory.CreateDirectory(appConfigDir);
            }
            if (!Directory.Exists(appBakDir))
            {
                Directory.CreateDirectory(appBakDir);
            }
        }
        #region resolve and compress
        private string resolveDirTag(string dirTag, ICollection<KeyValuePair<string, string>> tags = null)
        {
            if (tags != null)
            {
                foreach (KeyValuePair<string, string> kv in tags)
                {
                    if (dirTag == kv.Key)
                    {
                        return kv.Value;
                    }
                }
            }
            foreach (KeyValuePair<string, string> pair in DefaultTags)
            {
                if (dirTag == pair.Key)
                {
                    return pair.Value;
                }
            }
            return dirTag;

        }
        public string resolvePath(string path, ICollection<KeyValuePair<string, string>> tags = null)
        {
            //Console.WriteLine(Path.GetFileNameWithoutExtension(path));
            string[] components = path.Split('/', '\\');
            string retval ="";
            foreach (string s in components)
            {
                //Console.WriteLine(s);
                retval = Path.Combine(retval, resolveDirTag(s, tags));
            }
            if (retval.ElementAt(retval.IndexOf(":") + 1) != '\\')
            {
                retval = retval.Insert(retval.IndexOf(":")+1, @"\");
            }
            
            return retval;
        }
        public string CompressPath(string path, ICollection<KeyValuePair<string, string>> tags = null)
        {
            KeyValuePair<string, string> possibleMatch = new KeyValuePair<string, string>("", "");
            if (tags != null)
            {
                foreach (KeyValuePair<string, string> pair in tags)
                {
                    if (pair.Value == path)
                    {
                        return pair.Key;
                    }
                }
            }
            foreach (KeyValuePair<string, string> pair in CompressTags)
            {
                //make sure to return the longest tag match, so return JAR and not BIN/minecraft.jar
                
                if (pair.Value == path)
                {
                        if (pair.Value.Length > possibleMatch.Value.Length)
                        {
                            possibleMatch = pair;
                        }
                }

                
            }
            if (possibleMatch.Value != "" && possibleMatch.Key != "")
            {
                return possibleMatch.Key;
            } else if (Directory.GetParent(path) != null)
            {
                return Path.Combine(CompressPath(Directory.GetParent(path).FullName, tags), Path.GetFileName(path));

            }
            else
            {

                return path;
            }
        }
        #endregion
    }
}
