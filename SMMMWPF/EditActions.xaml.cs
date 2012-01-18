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
        public string modName { get; set; }
        public IEnumerable<IFSAction> Actions { get; set; }
        public EditActions(Mod m)
        {
            targetMod = m;
            modName = m.Name;
            Actions = m.InstallActions;
            InitializeComponent();
            
        }
    }
}
