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
            ModConfig mc = new ModConfig();
            jar.Mods.Add(new Mod("C:\\Users\\Charlie\\Documents\\Visual Studio 2010\\Projects\\SMMM\\SMMMConsole\\bin\\Debug\\minecraftforge-client-1.2.3.zip"));
            mc.addMod(new Mod("C:\\Users\\Charlie\\Documents\\Visual Studio 2010\\Projects\\SMMM\\SMMMConsole\\bin\\Debug\\minecraftforge-client-1.2.3.zip"));
            mc.save();
            jar.installMods();
        }
    }
}
