using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Xml;

namespace SMMMWPF
{
    class DestinationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate JarTemplate { get; set; }
        public DataTemplate ModsTemplate { get; set; }
        public DataTemplate ComplexTemplate { get; set; }
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            XmlElement xitem = item as XmlElement;
            string dest = xitem.SelectSingleNode("Destination").InnerText;
            switch (dest)
            {
                case "MODS":
                    return ModsTemplate;
                case "COMPLEX":
                    return ComplexTemplate;
                case "JAR":
                    return JarTemplate;
                default:
                    return null;
            }
            
        }
    }
}
