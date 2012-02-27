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
using System.ComponentModel;
using System.Xml;
using SMMMLib;

namespace SMMMWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    
    public partial class MainWindow : Window
    {
        MinecraftInstance instance;
        //MinecraftPaths m_paths;
        

        public MainWindow()
        {
            
            InitializeComponent();
            instance = new MinecraftInstance();
            setConfigPath();
            
            
        }
        private void setConfigPath()
        {
            XmlDataProvider dp = this.MainView.Resources["Config"] as XmlDataProvider;
            Uri confuri = new Uri(System.IO.Path.Combine(instance.Paths.appConfigDir, "config.xml"));
            dp.Source = confuri;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void save(object sender, RoutedEventArgs e)
        {
            XmlDataProvider dp = this.MainView.Resources["Config"] as XmlDataProvider;
            dp.Document.Save(dp.Source.LocalPath);
        }

        private void install_Click(object sender, RoutedEventArgs e)
        {
            instance.installAll();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            ListCollectionView dataView = 
                CollectionViewSource.GetDefaultView(ModsView.ItemsSource) as ListCollectionView;
            dataView.CustomSort = new NumericStringSorter();
            
            dataView.Refresh();
        }

        private void EditActions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XmlElement xMod = ModsView.SelectedItem as XmlElement;
            System.Xml.Linq.XElement xlMod = System.Xml.Linq.XElement.Parse(xMod.OuterXml);
            Mod mod = new Mod(xlMod);
            new EditActions(mod).ShowDialog();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            instance.Clean();
        }


    }
}
