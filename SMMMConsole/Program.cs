using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMMMLib;

namespace SMMMConsole
{
    class Program
    {
        private MinecraftInstance instance;

        static void Main(string[] args)
        {
            new Program(args);
        }
        public Program(string[] args)
        {
            instance = new MinecraftInstance();
            switch (args[0])
            {
                case "list":
                    listMods();
                    break;
                case "set":
                    switch (args[1])
                    {
                        case "id":
                            int i;
                            string source = args[2];
                            if (int.TryParse(source, out i))
                            {
                                setID(i, int.Parse(args[3]));
                            }
                            else
                            {
                                setID(source, int.Parse(args[3]));
                            }
                            break;
                        default:
                            break;
                            
                    }
                    break;
                case "install":
                    instance.installAll();
                    break;
                default:
                    break;
            }
        }
        private void setID(int from, int to)
        {
            Mod target = instance.getMod(from);
            target.ID = to;
        }
        private void setID(string from, int to)
        {
            Mod target = instance.getMod(from);
            target.ID = to;
        }
        private void listMods()
        {
            foreach (Mod m in instance.getInstalledMods())
            {
                Console.WriteLine(m.ID + " " + m.Name + " " + m.Destination);
            }
        }
    }
}
