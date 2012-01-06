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
        public string minecraftRoot = defaultRoot;
        public string binDir = defaultRoot + "\\bin";
        public string jarPath = defaultRoot + "\\bin\\minecraft.jar";
        public string resourcesDir = defaultRoot + "\\resources";
        public string modsDir = defaultRoot + "\\mods";
        public string configDir = defaultRoot + "\\config";
        public string tempDir = ".\\temp";
        public string appConfigDir = ".\\config";
        public MinecraftPaths(string baseDir)
        {
            minecraftRoot = baseDir;
            binDir = baseDir + "\\bin";
            jarPath = binDir + "\\minecraft.jar";
            resourcesDir = baseDir + "\\resources";
            modsDir = baseDir = "\\mods";
            tempDir = ".\\temp";
            configDir = baseDir + "\\config";
            appConfigDir = ".\\config";
        }
        public MinecraftPaths()
        {

        }
        
    }
}
