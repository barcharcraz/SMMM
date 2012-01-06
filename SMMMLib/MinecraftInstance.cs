using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMMMLib
{
    public class MinecraftInstance
    {
        private MinecraftPaths m_path;
        private MinecraftJar m_jar;
        private ModConfig m_config;
        public MinecraftInstance()
        {
            m_path = new MinecraftPaths();
            m_jar = new MinecraftJar(m_path.jarPath);
            m_config = new ModConfig(m_path);
        }
        
        
    }
}