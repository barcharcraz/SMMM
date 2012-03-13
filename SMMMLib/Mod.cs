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

        /// <summary>
        /// These are tags that are specific to this mod, things in the mod, that kind of stuff
        /// Any tag that needs to know about the mod's structure to resolve should go here
        /// </summary>
        public List<KeyValuePair<string, string>> tags
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
        /// <summary>
        /// Weather the mod is active, active mods will get installed the next time install is called
        /// Inactive mods will not
        /// </summary>
        private bool m_active;
        public bool Active
        {
            get
            {
                return m_active;
            }
            set
            {
                m_active = value;
                OnActiveChanged(EventArgs.Empty);
                
            }
        }

        private ICollection<IFSAction> m_installActions;
        /// <summary>
        /// The actions to take when installing the mod
        /// SMMM will try and make a guess at what kind of mod you are installing
        /// and where it should go. However should that fail the user
        /// can modify these actions himself.
        /// </summary>
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
        /// <summary>
        /// the name of the mod
        /// </summary>
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(this.FilePath);
            }
        }
        private int m_id;
        /// <summary>
        /// the ID of the mod, this determines the order in which mods are installed
        /// </summary>
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
        /// 
        /// COMPLEX indicates that the mod could not be auto-dectected
        /// and the user will have to manually enter installation actions
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
            m_active = (bool)x.Element("Active");
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
        protected virtual void OnActiveChanged(EventArgs e)
        {
            if (ActiveChanged != null)
            {
                ActiveChanged(this, e);
            }
        }
        /// <summary>
        /// Fired whenever the ID of the mod changes
        /// </summary>
        public event EventHandler IDChanged;
        public event EventHandler ActiveChanged;
    }
}
