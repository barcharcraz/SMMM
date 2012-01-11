using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SMMMLib;

namespace SMMMWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    
    public partial class MainWindow : Window
    {
        MinecraftInstance instance;
        MinecraftPaths m_paths;
        public MainWindow()
        {
            m_paths = new MinecraftPaths();
            XmlDataProvider dpro = new XmlDataProvider();
            Uri furi = new Uri(System.IO.Path.Combine(m_paths.appConfigDir));

            dpro.Source = furi;
            
            dpro.XPath = "SMMMconfig";
            Binding bind = new Binding();
            bind.Source = dpro;
            ModsBox.SetBinding(ListBox.ItemsSourceProperty, bind);
            InitializeComponent();
        }
    }
}
