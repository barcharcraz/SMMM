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
    /// Interaction logic for ActionsDisplay.xaml
    /// </summary>
    public partial class ActionsDisplay : UserControl
    {
        public string SourcePath { get; set; }
        public string DestPath { get; set; }
        public ActionsDisplay()
        {
            this.DataContext = this;
            InitializeComponent();
        }


    }
}
