using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMMMLib
{
    public class Mod : CompressedFile
    {
        private ModDestinations m_destination;

        public ModDestinations Destination
        {
            get { return m_destination; }
            set { m_destination = value; }
        }
        
        public Mod(string f) : base(f) { }
    }
}
