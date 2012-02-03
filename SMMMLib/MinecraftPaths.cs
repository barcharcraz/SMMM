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
        public string minecraftRoot;
        public string appRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SMMM");
        public string binDir;
        public string jarPath;
        public string resourcesDir;
        public string modsDir;
        public string configDir;
        public string tempDir;
        public string appConfigDir;
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
        private void InitTags()
        {
            DefaultTags = new List<KeyValuePair<string, string>>();
            DefaultTags.Add(new KeyValuePair<string, string>("JAR", Path.Combine(tempDir, "minecraft")));
            DefaultTags.Add(new KeyValuePair<string, string>("MODS", modsDir));
            DefaultTags.Add(new KeyValuePair<string, string>("RESOURCES", resourcesDir));
            DefaultTags.Add(new KeyValuePair<string, string>("MCROOT", minecraftRoot));
            DefaultTags.Add(new KeyValuePair<string, string>("BIN", binDir));
        }
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
            string[] components = path.Split('/');
            string retval = "";
            foreach (string s in components)
            {
                //Console.WriteLine(s);
                retval = Path.Combine(retval, resolveDirTag(s, tags));
            }
            return retval;
        }
        public string CompressPath(string path)
        {
            foreach (KeyValuePair<string, string> pair in DefaultTags)
            {
                if (pair.Value == path)
                {
                    return pair.Key;
                }
            }
            return CompressPath(Directory.GetParent(path).FullName) + Path.GetFileName(path);
        }
    }
}
