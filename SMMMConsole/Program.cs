using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMMMLib;

namespace SMMMConsole
{
    class Program
    {
        private MinecraftJar jar;
        static void Main(string[] args)
        {
            new Program();
        }
        public Program()
        {
            jar = new MinecraftJar();
            jar.TempPath = "./Temp";
            jar.extractToTemp();
        }
    }
}
