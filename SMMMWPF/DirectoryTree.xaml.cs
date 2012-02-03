﻿using System;
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
                //dir.vm.Root = null;
            }
            
            dir.InitializeComponent();
        }

        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            IEnumerable<string> data =
                (from FileSystemViewModel v in vm.children where v.RootFileName == block.Text select v.RootName);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(block, data.ElementAt(0), DragDropEffects.Link);
            }
        }

        private void TextBlock_Drop(object sender, DragEventArgs e)
        {

            Console.WriteLine(e.Data.GetData(DataFormats.StringFormat));
            //Console.WriteLine(e.Source);
            Console.WriteLine(((e.Source as TextBlock).DataContext as FileSystemViewModel).RootName);
        }
    }
}
