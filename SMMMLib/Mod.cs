using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        
        public Mod(string f) : base(f) { }
    }
}
