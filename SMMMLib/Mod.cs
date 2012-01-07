using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using SevenZip;
using System.Text.RegularExpressions;
namespace SMMMLib
{
    /// <summary>
    /// Represents a compressed mod file
    /// 
    /// the mod is going to go into some directory of the mine craft instillation
    /// </summary>
    public class Mod : CompressedFile
    {
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
        }
        public Mod(string f)
            : base(f)
        {
            Destination = findDestination();

        }
        private ModDestinations findDestination()
        {
            ReadOnlyCollection<ArchiveFileInfo> info;

            info = this.Extractor.ArchiveFileData;
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
                return ModDestinations.JAR;
            }
            else if (couldBeMOD)
            {
                return ModDestinations.MODS;
            }
            else
            {
                return ModDestinations.COMPLEX;
            }
        }
    }
}
