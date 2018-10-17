using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using MyExtensions;

namespace osuSkinCustomizer
{
    // static? probably, right?
    public static class HitcircleEditor
    {
        private enum Element
        {
            None,
            Hitcircle,
            HitcircleOverlay,
            ApproachCircle,
            Followpoints,
            SliderStartCircle,
            SliderEndCircle,
            DefaultNumbers,
            Cursor
        }

        // Controls
        private static Image imgHitcircle;
        private static Image imgHitcircleOverlay;
        private static Button btnShow;
        private static Button btnMakeChanges;
        private static RadioButton rbBlack;
        private static RadioButton rbGray;
        private static RadioButton rbWhite;
        private static Grid grdHitcircle;
        private static CheckBox cbHitcircle;
        private static CheckBox cbHitcircleOverlay;
        private static CheckBox cbDefaultNumbers;
        private static CheckBox cbFollowpoint;
        private static CheckBox cbApproachCircle;
        private static CheckBox cbSliderStartCircle;
        private static CheckBox cbSliderEndCircle;
        private static StackPanel spHitcircles;

        // Var
        private static string[] imageFiles;

        // Init
        public static void Initialize()
        {
            // Get references...
            imgHitcircle = MainWindow.instance.imgHitcircle;
            imgHitcircleOverlay = MainWindow.instance.imgHitcircleOverlay;

            btnShow = MainWindow.instance.btnShowHitcircle;
            btnMakeChanges = MainWindow.instance.btnExecuteHitcircles;

            rbBlack = MainWindow.instance.rbBlack;
            rbGray = MainWindow.instance.rbGray;
            rbWhite = MainWindow.instance.rbWhite;

            cbHitcircle = MainWindow.instance.cbHitcircle;
            cbHitcircleOverlay = MainWindow.instance.cbHitcircleOverlay;
            cbDefaultNumbers = MainWindow.instance.cbDefaultNumbers;
            cbApproachCircle = MainWindow.instance.cbApproachCircle;
            cbFollowpoint = MainWindow.instance.cbFollowpoint;
            cbSliderStartCircle = MainWindow.instance.cbSliderStartCircle;
            cbSliderEndCircle = MainWindow.instance.cbSliderEndCircle;
            spHitcircles = MainWindow.instance.spHitcircles;

            grdHitcircle = MainWindow.instance.grdHitcircle;

            // Set event handlers
            btnShow.Click += UpdateImageEventHandler;
            btnMakeChanges.Click += ExecuteEventHandler;

            rbBlack.Click += RadioButtonEventHandler;
            rbGray.Click += RadioButtonEventHandler;
            rbWhite.Click += RadioButtonEventHandler;
        }

        // Event Handlers
        private static void UpdateImageEventHandler(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private static void ExecuteEventHandler(object sender, RoutedEventArgs e)
        {
            // TODO
        }
        private static void RadioButtonEventHandler(object sender, RoutedEventArgs e)
        {
            if ((bool)rbBlack.IsChecked)
            {
                grdHitcircle.Background = Brushes.Black;
            }
            else if ((bool)rbGray.IsChecked)
            {
                grdHitcircle.Background = Brushes.Gray;
            }
            else if ((bool)rbWhite.IsChecked)
            {
                grdHitcircle.Background = Brushes.White;
            }
            else
            {
                MessageBox.Show("wtf?");
            }
        }

        public static void Update()
        {
            // Update paths to files...
            imageFiles = FileHandler.GetImageFiles(FileHandler.CurrentSkinFolder);

            List<string> foundHitcircleFiles = GetImages();
            List<string> copiedFiles = CopyToHitcircleEditorFolder(foundHitcircleFiles);

            PopulateHitcircleDropdown(FindElements(Element.Hitcircle, false));

            List<Color> comboColors = Skinini.GetComboColors();
            TintHitcircle(copiedFiles, comboColors[0]);

            SetImages(copiedFiles);
        }



        /// <summary> Returns all file paths relevant to the hitcircle </summary>
        private static List<string> GetImages()
        {
            List<string> relevantPaths = new List<string>();
            foreach (Element e in Enum.GetValues(typeof(Element)))
            {
                string filePath = FindPath(e);
                if (filePath != "")
                {
                    relevantPaths.Add(filePath);
                }
            }
            return relevantPaths;
        }

        /// <summary> Carries out what should be done with each path </summary>
        private static void SetImages(List<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                Element e = GetElement(path);

                switch (e)
                {
                    case Element.Hitcircle:
                        if ((bool)cbHitcircle.IsChecked)
                            SetImageSource(imgHitcircle, path);
                        else
                            imgHitcircle.Source = null;
                        break;
                    case Element.HitcircleOverlay:
                        if ((bool)cbHitcircleOverlay.IsChecked)
                            SetImageSource(imgHitcircleOverlay, path);
                        else
                            imgHitcircleOverlay.Source = null;
                        break;
                }
            }
        }

        /// <summary> Properly sets the source on an image </summary>
        private static void SetImageSource(Image image, string filePath)
        {
            /*
             Using this instead of just setting image.Source = filePath
             Not exactly sure what this does, but I think it creates a copy?
             */

            // https://stackoverflow.com/questions/16908383/how-to-release-image-from-image-source-in-wpf
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = new Uri(filePath);
            bitmapImage.EndInit();
            image.Source = bitmapImage;
        }

        /// <summary> Finds which element is associated with the input path </summary>
        private static Element GetElement(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            foreach (Element e in Enum.GetValues(typeof(Element)))
            {
                if (e.ToString().ToLower() == fileName.ToLower())
                {
                    return e;
                }
            }
            return Element.None;
        }

        /// <summary> Searches through imageFiles to look first for HD (@2x) elements, then for SD </summary>
        private static string FindPath(Element element)
        {

            // File names to look for. 
            // ie, "hitcircle@2x.png" and "hitcircle.png"
            string hdFileName = element.ToString().ToLower() + "@2x.png";
            string sdFileName = element.ToString().ToLower() + ".png";

            // string hdLocation = HitcircleEditorIO.TempLocation + "\\" + hdFileName;
            // string sdLocation = HitcircleEditorIO.TempLocation + "\\" + sdFileName;

            // look for HD first
            foreach (string file in imageFiles)
            {
                // "anyelement.png" == "hitcircle@2x.png" ?
                if (Path.GetFileName(file).ToLower() == hdFileName)
                    return file;
            }

            // if here, no HD element was found, so look for SD
            foreach (string file in imageFiles)
            {
                if (Path.GetFileName(file).ToLower() == sdFileName)
                    return file;
            }

            // return blank string if no element was found at all
            return "";
        }

        /// <summary> Searches through a specific folder to find the file relating to element </summary>
        private static string FindPath(Element element, string folder)
        {
            // File names to look for. 
            // ie, "hitcircle@2x.png" and "hitcircle.png"
            string hdFileName = element.ToString().ToLower() + "@2x.png";
            string sdFileName = element.ToString().ToLower() + ".png";

            // string hdLocation = HitcircleEditorIO.TempLocation + "\\" + hdFileName;
            // string sdLocation = HitcircleEditorIO.TempLocation + "\\" + sdFileName;

            // look for HD first
            string[] _imageFiles = FileHandler.GetImageFiles(folder);

            foreach (string file in _imageFiles)
            {
                // "anyelement.png" == "hitcircle@2x.png" ?
                if (Path.GetFileName(file).ToLower() == hdFileName)
                    return file;
            }

            // if here, no HD element was found, so look for SD
            foreach (string file in _imageFiles)
            {
                if (Path.GetFileName(file).ToLower() == sdFileName)
                    return file;
            }

            // return blank string if no element was found at all
            return "";
        }

        /// <summary> Creates a temp folder if it doesn't exist already, then copies each file to it and returns the new list </summary>
        private static List<string> CopyToHitcircleEditorFolder(List<string> paths)
        {
            FileHandler.CreateSkinSubdirectories();
            DeleteHitcircleEditorFiles();

            List<string> filesInHitcircleEditorFolder = new List<string>();
            foreach (string path in paths)
            {
                string destPath = FileHandler.HitcircleEditorFolder + "\\" + Path.GetFileName(path);
                File.Copy(path, destPath);
                filesInHitcircleEditorFolder.Add(destPath);
            }

            return filesInHitcircleEditorFolder;
        }

        /// <summary> Performs a recursive delete on the temp folder </summary>
        private static void DeleteHitcircleEditorFiles()
        {
            // Instead: just delete the files
            if (Directory.Exists(FileHandler.HitcircleEditorFolder))
            {
                // delete all files
                string[] files = Directory.GetFiles(FileHandler.HitcircleEditorFolder);
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        /// <summary> Colors the hitcircle file and replaces it in the dedicated folder </summary>
        private static void TintHitcircle(List<string> relevantPaths, Color c)
        {
            // get the hitcircle from relevantPaths...
            // search from relevant files where element = element.hitcircle
            foreach (string path in relevantPaths)
            {
                if (GetElement(path) == Element.Hitcircle)
                {
                    ImageManipulator.TintImage(path, c);
                }
            }
        }

        /// <summary> 
        /// <para>True = looks through a dedicated folder (TODO) for elements </para>
        /// <para>False = looks through all skin folders</para>
        /// </summary>
        private static List<string> FindElements(Element e, bool useDedicatedFolder)
        {
            List<string> elements = new List<string>();

            // go though folders...
            // dedicated assets folder? or look through skins?


            if (useDedicatedFolder)
            {
                // TODO
            }
            else
            {
                // search through already-existing skin folders
                // which should all be in the parent folder of the root folder
                string currentFolder = FileHandler.CurrentSkinFolder;
                string skinFolder = Directory.GetParent(currentFolder).FullName;

                string[] folders = Directory.GetDirectories(skinFolder);
                foreach (string folder in folders)
                {
                    if (folder.IsSkin())
                    {
                        string filePath = FindPath(e, folder);
                        if (filePath != "")
                            elements.Add(filePath);
                    }
                }
            }

            return elements;
        }

        private static void PopulateHitcircleDropdown(List<string> hitcirclePaths)
        {
            // Only do this when there is a currentSkin

            foreach (string path in hitcirclePaths)
            {
                // instead, just get the name of the directory
                string folderName = Path.GetFileName(Path.GetDirectoryName(path));
                new ExpanderItem(spHitcircles, folderName, path, delegate { Replace(path); });
            }
        }

        private static void Replace(string fileToCopy)
        {
            // get the associated element in the hitcircle folder, if there is one
            if (!string.IsNullOrEmpty(fileToCopy))
            {
                Element eFromPath = GetElement(fileToCopy);
                string existing = FindPath(eFromPath, FileHandler.HitcircleEditorFolder);

                if (File.Exists(existing))
                {
                    File.Delete(existing);
                    MessageBox.Show("Deleted: " + existing);
                }

                File.Copy(fileToCopy, FileHandler.HitcircleEditorFolder + "\\" + Path.GetFileName(fileToCopy), true);

                // reload
                SetImages(FileHandler.GetFiles(FileHandler.HitcircleEditorFolder).ToList());
            }
        }
    }
}
