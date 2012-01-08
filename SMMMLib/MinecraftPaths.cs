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
            appLogDir = appRoot;
            
            createDirs();
        }
        public MinecraftPaths() : this(defaultRoot)
        {

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
        }
        private string resolveDirTag(string dirTag)
        {
            switch (dirTag)
            {
                
                case "JAR":
                    return Path.Combine(tempDir, "minecraft");
                case "MODS":
                    return modsDir;
                
                default:
                    return dirTag;
            }
        }
        public string resolvePath(string path)
        {
            string[] components = path.Split('/');
            string retval = "";
            foreach (string s in components)
            {
                retval = Path.Combine(retval, resolveDirTag(s));
            }
            return retval;
        }
    }
}
