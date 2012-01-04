using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SMMMLib
{
    struct Paths
    {
        string minecraftRoot;
        string binDir;
        string jarPath;
        string resourcesDir;
        string modsDir;
        string configDir;
        public Paths(string baseDir)
        {
            minecraftRoot = baseDir;
            binDir = baseDir + "\\bin";
            jarPath = binDir + "\\minecraft.jar";
            resourcesDir = baseDir + "\\resources";
            modsDir = baseDir = "\\mods";
            configDir = baseDir + "\\config";
        }

    }
}
