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
            ModsView.DataContext = instance;
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
            if (dataView.SortDescriptions.Count() == 0)
            {
                dataView.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Descending));
            }
            else if (dataView.SortDescriptions.ElementAt(0).Direction == ListSortDirection.Ascending)
            {
                dataView.SortDescriptions.Clear();
                dataView.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Descending));
            }
            else if (dataView.SortDescriptions.ElementAt(0).Direction == ListSortDirection.Descending)
            {
                dataView.SortDescriptions.Clear();
                dataView.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Ascending));
            }

            dataView.Refresh();
        }

        private void EditActions_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Mod mod = ModsView.SelectedItem as Mod;
            new EditActions(mod).ShowDialog();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            instance.Clean();
        }

        private void ModsView_MouseMove(object sender, MouseEventArgs e)
        {
            ListViewItem item = FindAnchestor<ListViewItem>(e.OriginalSource as DependencyObject);
            if (item != null)
            {
                Mod movedMod = item.DataContext as Mod;

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(sender as ListView, movedMod, DragDropEffects.Move);
                }
            }
        }
        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
        private void ModsView_Drop(object sender, DragEventArgs e)
        {
            ListViewItem target = FindAnchestor<ListViewItem>(e.OriginalSource as DependencyObject);
            Mod targetMod = target.DataContext as Mod;
            Mod moved = e.Data.GetData(typeof(Mod)) as Mod;
            moved.ID = targetMod.ID;
            ModsView.ItemsSource = instance.AllMods;
            
        }


    }
}
