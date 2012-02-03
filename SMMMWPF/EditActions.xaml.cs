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
using System.Windows.Shapes;
using SMMMLib;

namespace SMMMWPF
{
    /// <summary>
    /// Interaction logic for EditActions.xaml
    /// </summary>
    public partial class EditActions : Window
    {
        public Mod targetMod{get; set;}


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
        public ICollection<IFSAction> Actions { get; set; }
        public EditActions(Mod m)
        {
            targetMod = m;
            ModTempDir = m.ExtractedRoot.FullName;
            MinecraftBase = StateProvider.ActiveInstancePaths.minecraftRoot;
            ModName = m.Name;
            
            Actions = m.InstallActions;
            InitializeComponent();
            
        }

        private void ActionsDisplay_Drop(object sender, DragEventArgs e)
        {
            TextBlock origSource = e.OriginalSource as TextBlock;
            string target = (origSource.DataContext as FileSystemViewModel).RootName;
            string source = e.Data.GetData(DataFormats.StringFormat) as string;
            IFSAction act = ActionFactory.GenerateAction(
                StateProvider.ActiveInstancePaths,
                source,
                target);
            Console.WriteLine(act);
            Console.WriteLine(e.OriginalSource);
        }
    }
}
