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
    /* TODO...
     * Remove generator. Stick the files in a folder so they aren't lost forever... Then remove functionality from xaml
    */ 


    public partial class MainWindow : Window
    {
        public static MainWindow instance;

        // tabs
        private Importer importer;

        public MainWindow()
        {
            InitializeComponent();

            // Set class instances...
            instance = this;
            importer = new Importer();

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