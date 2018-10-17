using System;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace osuSkinCustomizer
{
    public static class FollowpointEditor
    {
        private static CheckBox[] cbs;

        public static void Initialize()
        {
            GetCheckBoxes();
            SetHandler();
        }

        private static void GetCheckBoxes()
        {
            cbs = new CheckBox[]
            {
                MainWindow.instance.cbFp0,
                MainWindow.instance.cbFp1,
                MainWindow.instance.cbFp2,
                MainWindow.instance.cbFp3,
                MainWindow.instance.cbFp4,
                MainWindow.instance.cbFp5,
                MainWindow.instance.cbFp6,
                MainWindow.instance.cbFp7,
                MainWindow.instance.cbFp8,
                MainWindow.instance.cbFp9
            };
        }

        private static void SetHandler()
        {
            MainWindow.instance.btnExecuteFollowpoints.Click += ButtonHandler;
        }

        private static void ButtonHandler(object sender, RoutedEventArgs e)
        {
            ReplaceFollowpoints();
        }

        private static void ReplaceFollowpoints()
        {
            string[] followpoints = GetFollowpoints();
            string biggest = GetBiggest(followpoints);
            string blankPng = GetBlankPNG();

            // copy each 
            string backupFolder = FileHandler.CreateBackupFolder(FileHandler.CurrentSkinFolder);
            foreach (string fp in followpoints)
            {
                string dest = backupFolder + "\\" + Path.GetFileName(fp);
                File.Copy(fp, dest);
            }

            string temp = backupFolder + "\\followpointTemplate.png";
            File.Copy(biggest, temp);

            for (int i = 0; i < cbs.Length; i++)
            {
                // delete followpoint-x.png, if it's there
                // replace it with biggest, renamed

                string fileName = FileHandler.CurrentSkinFolder + "\\followpoint-" + i + ".png";

                if ((bool)cbs[i].IsChecked)
                {
                    File.Copy(temp, fileName, true);
                }
                else
                {
                    File.Copy(blankPng, fileName, true);
                }

            }

            File.Delete(temp);

            MessageBox.Show("Followpoint replacement success!");
        }

        private static string[] GetFollowpoints()
        {
            string[] allFiles = FileHandler.GetFiles(FileHandler.CurrentSkinFolder);
            string[] followpoints = ElementFinder.GetGameplayFollowpoints(allFiles);
            string[] relevantFps = Array.FindAll(followpoints, fp => Path.GetFileName(fp).ToLower() != "followpoint.png");
            return relevantFps;
        }
        private static string GetBiggest(string[] files)
        {
            long biggestSize = 0;
            string biggestPng = "";
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                if (info.Length > biggestSize && !IsBlank(file))
                {
                    biggestSize = info.Length;
                    biggestPng = file;
                }
            }

            return biggestPng;
        }
        private static string GetBlankPNG()
        {
            // Should return a blank 1x1 png
            // should add something later to ensure that it chooses the right thing...
            // TODO: find blank file

            string[] imgFiles = FileHandler.GetImageFiles(FileHandler.CurrentSkinFolder);
            foreach (string file in imgFiles)
            {
                if (IsBlank(file))
                    return file;
            }

            return "";
        }
        private static bool IsBlank(string file)
        {
            using (var imageStream = File.OpenRead(file))
            {
                var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile,
                    BitmapCacheOption.Default);
                var height = decoder.Frames[0].PixelHeight;
                var width = decoder.Frames[0].PixelWidth;

                return height == 1 && width == 1;
            }
        }
    }
}
