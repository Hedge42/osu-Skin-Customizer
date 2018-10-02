using System.Windows;
using System.Media;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;
using MyExtensions;

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

            // Set handlers...
            tbRootFolder.TextChanged += FileHandler.RootFolderTextChangedEventHandler;
        }
    }
}
