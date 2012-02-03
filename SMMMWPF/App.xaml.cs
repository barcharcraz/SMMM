using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SMMMLib;

namespace SMMMWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //set the active instance equal to the default %appdata% stuff
            StateProvider.ActiveInstancePaths = new MinecraftPaths();
        }
    }
}
