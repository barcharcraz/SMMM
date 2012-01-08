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
        private MinecraftPaths paths;
        public ModConfig(MinecraftPaths p)
        {
            paths = p;
            
            if (!File.Exists(Path.Combine(p.appConfigDir, "config.xml")))
            {
                XDeclaration dec = new XDeclaration("1.0", "UTF-8", "yes");
                document = new XDocument();     
                document.Declaration = dec;
                document.Add(new XElement("SMMMconfig"));
                document.Save(Path.Combine(paths.appConfigDir, "config.xml"));
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
                    new XElement("ID", m.ID)
                );
            XElement installActions = new XElement("InstallActions");
            foreach (IFSAction act in m.InstallActions)
            {
                installActions.Add(new XElement(act.GetType().ToString(),
                    new XElement("source", act.source),
                    new XElement("target", act.target)));
            }
            modElement.Add(installActions);
            IEnumerable<XElement> modTest = from c in document.Descendants("Mod")
                                            where (string)c.Element("Path") == m.FilePath
                                            select c;
            if (modTest.Count() > 0)
            {
                throw new exceptions.DuplicateItemException("mod: " + m.Name + "is already in the config file");
            }
            document.Root.Add(modElement);
            m_numMods++;
            
        }
        
        public Mod getMod(string name)
        {

            XElement xE = getXMod(name);
            Mod retVal = new Mod(xE);
            retVal.IDChanged += IDChangeHandler;
            return retVal;
        }
        public Mod getMod(int id)
        {
            XElement xE = getXMod(id);
            
            Mod retVal = new Mod(xE);
            retVal.IDChanged += IDChangeHandler;
            return retVal;
        }
        private XElement getXMod(int id)
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
            return xE;
        }
        private XElement getXMod(string name)
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
            return xE;
        }
        public IEnumerable<Mod> getAllMods()
        {
            List<Mod> retVal = new List<Mod>();
            IEnumerable<XElement> modElements = from c in document.Descendants("Mod")
                                                orderby (int)c.Element("ID")
                                                select c;
            foreach (XElement x in modElements)
            {
                Mod current = new Mod(x);
                current.IDChanged += IDChangeHandler;
                retVal.Add(current);
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
        public void updateID(Mod m)
        {
            XElement xMod = getXMod(m.Name).Element("ID");
            xMod.Value = m.ID.ToString();

            try
            {
                Mod modInID = getMod(m.ID);
                
                if (!(modInID.FilePath == m.FilePath))
                {
                    modInID.ID++;
                }
            }
            catch (exceptions.ElementNotFoundException e)
            {
                StreamWriter log = new StreamWriter(Path.Combine(paths.appLogDir, "log.txt"));
                log.WriteLine("mod not found, reached the end of ID update");
                log.Close();
            }
            save();
            
            
        }
        public void IDChangeHandler(object sender, EventArgs e)
        {
            updateID((Mod)sender);
        }
        public void save()
        {
           
            document.Save(Path.Combine(paths.appConfigDir, "config.xml"));
        }
        public int getNextID()
        {
            int retVal = m_numMods;
            return retVal;
            
        }
    }
}
