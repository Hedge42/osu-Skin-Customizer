using System.Windows;
using System.Media;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;
using MyExtensions;
using System;

namespace osuSkinCustomizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;

        // tabs
        private Importer importer;
        private Generator generator;

        public MainWindow()
        {
            InitializeComponent();

            // Set class instances...
            instance = this;
            importer = new Importer();
            generator = new Generator();

            FollowpointEditor.Initialize();
            HitcircleEditor.Initialize();

            // Set handlers...
            tbRootFolder.TextChanged += FileHandler.CurrentSkinTextChanged;


            // Experimental...

            // https://stackoverflow.com/questions/3991933/get-path-for-my-exe
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;

            // is the .exe in a skin folder?
            // get the directory from the file...
            string exeFolder = Path.GetDirectoryName(exePath);

            if (exeFolder.IsSkin())
            {
                // MessageBox.Show("Hooray!");
                // FileHandler.currentSkinFolder = exeFolder;
            }
        }
    }
}