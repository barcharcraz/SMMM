using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Xml;
using SMMMLib;

namespace SMMMWPF
{
    class DestinationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate JarTemplate { get; set; }
        public DataTemplate ModsTemplate { get; set; }
        public DataTemplate ComplexTemplate { get; set; }
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            ModDestinations xitem = (item as Mod).Destination;
            //string dest = xitem.SelectSingleNode("Destination").InnerText;
            switch (xitem)
            {
                case ModDestinations.MODS:
                    return ModsTemplate;
                case ModDestinations.COMPLEX:
                    return ComplexTemplate;
                case ModDestinations.JAR:
                    return JarTemplate;
                default:
                    return null;
            }
            
        }
    }
}
