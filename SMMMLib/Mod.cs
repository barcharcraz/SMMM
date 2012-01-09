using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using SevenZip;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;
namespace SMMMLib
{
    /// <summary>
    /// Represents a compressed mod file
    /// 
    /// the mod is going to go into some directory of the mine craft instillation
    /// </summary>
    public class Mod : CompressedFile
    {
        private List<KeyValuePair<string, string>> tags
        {
            get
            {
                List<KeyValuePair<string, string>> retval = new List<KeyValuePair<string, string>>();
                retval.Add(new KeyValuePair<string, string>("ZIP", FilePath));
                retval.Add(new KeyValuePair<string, string>("MODROOT", ExtractedRoot.FullName));
                retval.Add(new KeyValuePair<string, string>("NAME", Path.GetFileName(FilePath)));
                return retval;
            }
        }
        
        private ICollection<IFSAction> m_installActions;
        public ICollection<IFSAction> InstallActions
        {
            get
            {
                return m_installActions;
            }
            set
            {
                for (int i = 0; i < value.Count; i++)
                {
                    value.ElementAt(i).ExtraTags = tags;
                }
                m_installActions = value;

            }
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(this.FilePath);
            }
        }
        private int m_id;
        public int ID
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
                OnIDChanged(EventArgs.Empty);
            }
        }
        private ModDestinations m_destination;
        /// <summary>
        /// the destination of the mod
        /// 
        /// This is going to be ether JAR, MODS, or COMPLEX
        /// </summary>
        public ModDestinations Destination
        {
            get { return m_destination; }
            set { m_destination = value; }
        }
        
        public Mod(XElement x)
            : base((string)x.Element("Path"))
        {
            Destination = (ModDestinations)Enum.Parse(typeof(ModDestinations), (string)x.Element("Destination"));
            m_id = (int)x.Element("ID");
            XElement instAct = x.Element("InstallActions");
            InstallActions = new List<IFSAction>();
            foreach (XElement xe in instAct.Elements())
            {
                Type t = Type.GetType(xe.Name.ToString());
                Type[] paramTypes = new Type[3];
                paramTypes[0] = typeof(string);
                paramTypes[1] = typeof(string);
                paramTypes[2] = typeof(ICollection<KeyValuePair<string, string>>);
                ConstructorInfo ctor = t.GetConstructor(paramTypes);
                IFSAction act;
                act = (IFSAction)ctor.Invoke(
                    new object[]{(string)xe.Element("source"), 
                        (string)xe.Element("target"),
                        tags});
                InstallActions.Add(act);
            }
        }
        public Mod(string f, int id)
            : base(f)
        {
            InstallActions = new List<IFSAction>();
            Destination = findDestination();
            m_id = id;
            

        }
        public Mod(System.IO.FileInfo f, int id) : this(f.FullName, id) {}
        private ModDestinations findDestination()
        {
            ReadOnlyCollection<ArchiveFileInfo> info;
            
            info = new SevenZipExtractor(FilePath).ArchiveFileData;
            bool couldBeJAR = false;
            bool couldBeMOD = false;
            Regex builtIn = new Regex(@"^[a-z]{1,3}.class");
            Regex mod = new Regex(@"^mod_.+");
            foreach (ArchiveFileInfo finfo in info)
            {
                if (builtIn.Match(finfo.FileName).Success)
                {
                    couldBeJAR = true;
                }
                if (mod.Match(finfo.FileName).Success)
                {
                    couldBeMOD = true;
                }
            }
            if (couldBeJAR)
            {
                InstallActions.Add(new DirectoryCopyAction(@"MODROOT", @"JAR", tags));
                return ModDestinations.JAR;
                
            }
            else if (couldBeMOD)
            {
                InstallActions.Add(new FileCopyAction(@"ZIP", @"MODS/NAME", tags));
                return ModDestinations.MODS;
            }
            else
            {
                return ModDestinations.COMPLEX;
            }
        }
        protected virtual void OnIDChanged(EventArgs e)
        {
            if (IDChanged != null)
            {
                
                IDChanged(this, e);
            }
        }
        public event EventHandler IDChanged;
    }
}
