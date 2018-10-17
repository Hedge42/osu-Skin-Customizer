using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MyExtensions;

namespace osuSkinCustomizer
{
    public static class FileHandler
    {

        public static string CurrentSkinFolder { get; private set; }

        public static string SkinManagerFolder { get { return CurrentSkinFolder + "\\" + "SkinManager"; } }
        public static string BackupsFolder { get { return SkinManagerFolder + "\\Backups"; } }
        public static string HitcircleEditorFolder { get { return SkinManagerFolder + "\\HitcircleEditor"; } }

        /// <summary> Creates subdirectories inside currentSkinFolder </summary>
        public static void CreateSkinSubdirectories()
        {
            if (!CurrentSkinFolder.IsSkin())
                return;

            if (!Directory.Exists(SkinManagerFolder))
                Directory.CreateDirectory(SkinManagerFolder);

            if (!Directory.Exists(BackupsFolder))
                Directory.CreateDirectory(BackupsFolder);

            if (!Directory.Exists(HitcircleEditorFolder))
                Directory.CreateDirectory(HitcircleEditorFolder);
        }

        /// <summary> Event fired upon changing the text of the current skin folder input field </summary>
        public static void CurrentSkinTextChanged(object sender, RoutedEventArgs e)
        {
            SetCurrentSkin(((TextBox)sender).Text);
        }

        /// <summary> Sets the current skin folder to be equal to the input parameter, only if that parameter can be parsed as a skin folder </summary>
        private static void SetCurrentSkin(string input)
        {
            if (input.IsSkin())
            {
                // root folder has to be validated in order to be set
                CurrentSkinFolder = input;

                // change the label if the path is valid
                string skinName = Path.GetFileName(input);
                MainWindow.instance.lblRootFolder.Content = "Current skin: " + skinName;
            }
        }

        public static string[] GetFiles(string folder)
        {
            if (!Directory.Exists(folder))
                return null;

            return Directory.GetFiles(folder);
        }
        public static string[] GetImageFiles(string folder)
        {
            string[] allFiles = GetFiles(folder);

            List<string> imageFiles = new List<string>();
            if (allFiles != null && allFiles.Length > 0)
            {
                foreach (string s in allFiles)
                {
                    string ext = Path.GetExtension(s);
                    if (ext == ".png" || ext == ".jpg")
                        imageFiles.Add(s);
                }
            }

            return imageFiles.ToArray();

        }
        public static string[] GetAudioFiles(string folder)
        {
            string[] files = GetFiles(folder);

            List<string> audioFiles = new List<string>();
            if (files == null || files.Length == 0)
            {
                foreach (string s in files)
                {
                    string ext = Path.GetExtension(s);
                    if (ext == ".mp3" || ext == ".wav" || ext == ".ogg")
                        audioFiles.Add(s);
                }
            }
            return audioFiles.ToArray();
        }

        public static void CopyFiles(string[] files, string folder)
        {
            List<string> filesNamesWithExt = new List<string>();

            foreach (string file in files)
                filesNamesWithExt.Add(Path.GetFileName(file));

            for (int i = 0; i < files.Length; i++)
            {
                string destinationFile = folder + "\\" + filesNamesWithExt[i];
                File.Copy(files[i], destinationFile);
            }
        }
        public static void DeleteFiles(string[] files)
        {
            try
            {
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot delete files\r\n\r\n" + e.Message);
            }
        }
        public static void MoveFilesToBackup(string[] files)
        {
            try
            {
                string parentFolder = Path.GetDirectoryName(files[0]);
                string backupFolder = CreateBackupFolder(parentFolder);
                foreach (string file in files)
                    File.Move(file, parentFolder + "\\" + Path.GetFileName(file));
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot move to backup.\r\n\r\n" + e.Message);
            }
        }
        public static string CreateBackupFolder(string parentFolder)
        {
            try
            {
                // https://stackoverflow.com/questions/22225044/creating-a-new-folder-with-todays-date-on-specific-folder
                string name = "Backup " + DateTime.Now.ToString("MM-dd-yy hh.mm.ss");
                string path = parentFolder + "\\" + name;
                Directory.CreateDirectory(path);
                return path;

                // or return Directory.CreateDirectory(parentFolder + "\\" + name).FullName;
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot create backup\r\n\r\n" + e.Message);
                return "";
            }
        }
        public static void Backup(string[] files)
        {
            // ????
            string parentFolder = new DirectoryInfo(files[0]).FullName;
            string backupFolder = CreateBackupFolder(parentFolder);
            foreach (string file in files)
            {
                string dest = backupFolder + "\\" + file;
                File.Move(file, dest);
            }
        }

        public static string[] GetFileSet(string[] files, string[] positiveTerms, string[] negativeTerms)
        {
            if (files == null) return null;

            List<string> filesList = new List<string>();
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                if (GetMatch(file, positiveTerms, negativeTerms))
                    filesList.Add(file);
            }
            return filesList.ToArray();
        }
        private static bool GetMatch(string fileName, string[] positiveTerms, string[] negativeTerms)
        {
            // Does the file contain any of the positive terms, and NOT contain any of the negative terms?

            // No positive terms == default TRUE
            bool positiveHit = true;
            if (positiveTerms != null && positiveTerms.Length > 0)
                positiveHit = fileName.Contains(positiveTerms);

            // No negative terms == default FALSE
            bool negativeHit = false;
            if (negativeTerms != null && negativeTerms.Length > 0)
                negativeHit = fileName.Contains(negativeTerms);

            return positiveHit && !negativeHit;
        }
    }
}
