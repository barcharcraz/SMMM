using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;

namespace SMMMWPF
{
    class NumericStringSorter : IComparer
    {
        public int Compare(object x, object y)
        {

            XmlElement xmlx = x as XmlElement;
            XmlElement xmly = y as XmlElement;
            string xstr = xmlx.SelectSingleNode("ID").InnerText;
            string ystr = xmly.SelectSingleNode("ID").InnerText;
            
            return Convert.ToInt32(xstr) - Convert.ToInt32(ystr);
        }
    }
}
