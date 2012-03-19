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

namespace SMMMWPF
{
    /// <summary>
    /// Interaction logic for DirectoryTree.xaml
    /// </summary>
    public partial class DirectoryTree : UserControl
    {
        public FileSystemViewModel vm { get; set; }


        public bool ShowRoot
        {
            get { return (bool)GetValue(ShowRootProperty); }
            set { SetValue(ShowRootProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowRoot.  This enables animation, styling, binding, etc...\

        /// <summary>
        /// Determines weather the root directory of the mod is shown
        /// 
        /// </summary>
        //TODO: implement this
        public static readonly DependencyProperty ShowRootProperty =
            DependencyProperty.Register("ShowRoot", typeof(bool), typeof(DirectoryTree), new UIPropertyMetadata(false));

        
        public static readonly DependencyProperty RootProperty =
            DependencyProperty.Register("Root", typeof(string), typeof(DirectoryTree), new PropertyMetadata(default(string), new PropertyChangedCallback(OnRootChanged) ));

        public string Root
        {
            get { return (string)GetValue(RootProperty); }
            set
            {
                SetValue(RootProperty, value);
            }
        }
        public DirectoryTree()
        {
            //vm = new FileSystemViewModel(@"C:\Program Files (x86)\GOG.com\Freespace 2");
            vm = new FileSystemViewModel();
            DataContext = vm;
            //InitializeComponent();
            
            
        }
        private static void OnRootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DirectoryTree dir = d as DirectoryTree;

            if (e.NewValue != null)
            {
                dir.vm.RootName = e.NewValue as string;
            }
            else
            {
                dir.vm.Root = null;
            }
            
            dir.InitializeComponent();
        }

        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            FileSystemViewModel model = (sender as TextBlock).DataContext as FileSystemViewModel;
            string datString;
            /*if (vm.RootFileName == block)
            {
                datString = vm.RootName;
            }
            else
            {
                IEnumerable<string> data =
                    (from FileSystemViewModel v in vm.children where v.RootFileName == block select v.RootName);
                datString = data.ElementAt(0);
            }*/
            datString = model.RootName;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(sender as TextBlock, datString, DragDropEffects.Link | DragDropEffects.Copy);
                
            }
        }

        private void TextBlock_Drop(object sender, DragEventArgs e)
        {

            Console.WriteLine(e.Data.GetData(DataFormats.StringFormat));
            //Console.WriteLine(e.Source);
            //Console.WriteLine(((e.Source as ContentPresenter).DataContext as FileSystemViewModel).RootName);
        }

        private void TextBlock_DragEnter(object sender, DragEventArgs e)
        {
            TextBlock send = sender as TextBlock;
            send.Background = Brushes.Gray;
        }

        private void TextBlock_DragLeave(object sender, DragEventArgs e)
        {
            TextBlock send = sender as TextBlock;
            send.Background = Brushes.Transparent;
        }
    }
}
