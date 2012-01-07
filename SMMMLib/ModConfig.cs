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
        private int m_numMods;
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
            m_numMods = document.Descendants().Count();


        }
        public ModConfig() : this(new MinecraftPaths()) { }

        public void addMod(Mod m)
        {
            XElement modElement = new XElement("Mod",
                    new XElement("Name", Path.GetFileNameWithoutExtension(m.FilePath)),
                    new XElement("Destination", m.Destination),
                    new XElement("Path", m.FilePath),
                    new XElement("ID", m_numMods)
                );
            document.Add(modElement);
            m_numMods++;
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
            Mod retVal = new Mod(xE);
            return retVal;
        }
        public Mod getMod(int id)
        {
            IEnumerable<XElement> xMod = from c in document.Descendants("Mod")
                                         where (int)c.Element("ID") == id
                                         select c;
            if (xMod.Count() > 1)
            {
                throw new exceptions.DuplicateItemException("There can not be more than one mod with the same ID: " + id);
            }
            if (xMod.Count() == 0)
            {
                throw new exceptions.ElementNotFoundException("Mod is not in the config file, did you forget to call addMod(string path)? " + id);
            }

            XElement xE = xMod.ElementAt(0);
            Mod retVal = new Mod(xE);
            return retVal;
        }
        public IEnumerable<Mod> getAllMods()
        {
            List<Mod> retVal = new List<Mod>();
            IEnumerable<XElement> modElements = from c in document.Descendants("Mod")
                                                orderby c.Element("ID")
                                                select c;
            foreach (XElement x in modElements)
            {
                retVal.Add(new Mod(x));
            }
            return retVal;
        }
        public void removeMod(string path)
        {
            IEnumerable<XElement> toDeleteList = from c in document.Descendants("Mod")
                                where (string)c.Element("Path") == path
                                select c;
            if (toDeleteList.Count() > 1)
            {
                throw new exceptions.DuplicateItemException("That path was in the XML config file more than once:" + path);
            }
            if (toDeleteList.Count() == 0)
            {
                throw new exceptions.ElementNotFoundException("that path is not in the XML config file: " + path);
            }
            XElement toDelete = toDeleteList.ElementAt(0);
            toDelete.Remove();
        }
        public void removeMod(Mod m)
        {
            removeMod(m.FilePath);
        }
        public void save()
        {
            MinecraftPaths p = new MinecraftPaths();
            document.Save(Path.Combine(p.appConfigDir, "config.xml"));
        }
    }
}
