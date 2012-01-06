using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace SMMMLib
{
    public class ModConfig
    {
        private XDocument document;
        public ModConfig(MinecraftPaths p)
        {

            
            if (!File.Exists(Path.Combine(p.appConfigDir, "config.xml")))
            {
                XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
                document = new XDocument();
                document.Declaration = dec;
            }
            else
            {
                document = XDocument.Load(Path.Combine(p.appConfigDir, "config.xml"));
            }


        }
        public ModConfig() : this(new MinecraftPaths()) { }

        public void addMod(Mod m)
        {
            XElement modElement = new XElement("Mod",
                    new XElement("Name", Path.GetFileNameWithoutExtension(m.FilePath)),
                    new XElement("Destination", m.Destination),
                    new XElement("Path", m.FilePath)
                );
            document.Add(modElement);
        }
        public Mod getMod(string name)
        {
            IEnumerable<XElement> xMod = from c in document.Descendants("Mod")
                                         where (string)c.Element("Name") == name
                                         select c;
            if (xMod.Count() > 1)
            {
                throw new exceptions.DuplicateItemException("There can not be more than one mod with the same name: " + name);
            }
            if (xMod.Count() == 0)
            {
                throw new exceptions.ElementNotFoundException("Mod is not in the config file, did you forget to call addMod(string path)? " + name);
            }
            XElement xE = xMod.ElementAt(0);
            Mod retVal = new Mod((string)xE.Element("Path"));
            retVal.Destination = (ModDestinations)Enum.Parse(typeof(ModDestinations), (string)xE.Element("Destination"));
            return retVal;
        }
        public void save()
        {
            MinecraftPaths p = new MinecraftPaths();
            document.Save(Path.Combine(p.appConfigDir, "config.xml"));
        }
    }
}
