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
using System.Windows.Shapes;
using SMMMLib;
using System.Collections.ObjectModel;

namespace SMMMWPF
{
    /// <summary>
    /// Interaction logic for EditActions.xaml
    /// </summary>
    public partial class EditActions : Window
    {
        
        public static readonly DependencyProperty targetModProperty =
            DependencyProperty.Register("targetMod", typeof (Mod), typeof (EditActions), new PropertyMetadata(default(Mod)));

        public Mod targetMod
        {
            get { return (Mod) GetValue(targetModProperty); }
            set { SetValue(targetModProperty, value); }
        }


        public string ModName
        {
            get { return (string)GetValue(ModNameProperty); }
            set { SetValue(ModNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModNameProperty =
            DependencyProperty.Register("ModName", typeof(string), typeof(EditActions), new UIPropertyMetadata("ERROR: NO NAME"));

        public string ModTempDir { get; set; }
        public string MinecraftBase { get; set; }
        public ObservableCollection<IFSAction> Actions { get; set; }
        public EditActions(Mod m)
        {
            targetMod = m;
            ModTempDir = m.ExtractedRoot.FullName;
            MinecraftBase = StateProvider.ActiveInstancePaths.minecraftRoot;
            ModName = m.Name;
            Actions = new ObservableCollection<IFSAction>(m.InstallActions);
            
            InitializeComponent();
            
        }

        private void ActionsDisplay_Drop(object sender, DragEventArgs e)
        {
            
            TextBlock origSource = e.OriginalSource as TextBlock;
            
            string target = (origSource.DataContext as FileSystemViewModel).RootName;
            string source = e.Data.GetData(DataFormats.StringFormat) as string;
            if (e.KeyStates == DragDropKeyStates.ControlKey)
            {
                target = System.IO.Path.Combine(target, targetMod.Name + ".zip");
            }
            IFSAction act = ActionFactory.GenerateAction(
                StateProvider.ActiveInstancePaths,
                source,
                target,targetMod.tags);
            Actions.Add(act);
            
            Console.WriteLine(act);
            Console.WriteLine(e.OriginalSource);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            targetMod.InstallActions = Actions;
            ModConfig conf = new ModConfig(new MinecraftPaths(StateProvider.ActiveInstancePaths.modConfigFile));
            conf.updateMod(targetMod);
            conf.save();
            Close();
        }

        private void ActionsList_KeyDown(object sender, KeyEventArgs e)
        {
            ListBox list = sender as ListBox;
            switch (e.Key)
            {
                case Key.Delete:
                    List<object> toRemove = new List<object>();
                    foreach (object sel in list.SelectedItems )
                    {
                        toRemove.Add(sel);
                    }
                    foreach (object current in toRemove)
                    {
                        Actions.Remove(current as IFSAction);
                    }
                    break;
            }
        }
    }
}
