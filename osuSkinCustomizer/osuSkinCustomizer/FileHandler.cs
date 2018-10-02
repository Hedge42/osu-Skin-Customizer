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
        public static string rootFolder;

        public static void RootFolderTextChangedEventHandler(object sender, RoutedEventArgs e)
        {
            SetRootFolder(((TextBox)sender).Text);
        }
        private static void SetRootFolder(string input)
        {
            if (input.IsSkin())
            {
                // root folder has to be validated in order to be set
                rootFolder = input;

                // change the label if the path is valid
                string skinName = Path.GetFileName(input);
                MainWindow.instance.lblRootFolder.Content = "Current skin: " + skinName;
            }
        }

        // TODO: better naming convention?
        // split into different classes

        // Interface\ https://osu.ppy.sh/help/wiki/Skinning/Interface
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

        public static string[] GetInterfaceMainMenu(string[] files)
        {
            string[] terms = { "menu-background", "menu-snow", "welcome_text" };
            return GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetInterfaceButton(string[] files)
        {
            string[] terms = { "button-left", "button-middle", "button-right" };
            return GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetInterfaceCursor(string[] files)
        {
            string[] terms = { "cursor" };
            return GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetInterfaceModIcons(string[] files)
        {
            string[] positive = { "mod" };
            string[] negative = { "mode" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceOffsetWizard(string[] imageFiles)
        {
            string[] terms = { "options-offset-tick" };
            return GetFileSet(imageFiles.GetImageFiles(), terms, null);
        }

        // Interface\Playfield
        public static string[] GetInterfacePlayfield(string[] files)
        {
            // Everything before countdown
            string[] positive = { "section", "multi-skipped", "masking-border", "arrow", "play" };
            string[] negative = { "mod", "replay", "reverse" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceCountdown(string[] files)
        {
            string[] positive = { "count", "go", "ready" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceHitBursts(string[] files)
        {
            string[] positive = { "hit" };
            string[] negative = { "circle" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceInputOverlay(string[] files)
        {
            string[] positive = { "inputoverlay" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfacePauseScreen(string[] files)
        {
            string[] positive = { "pause" };
            string[] negative = { "arrow" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceScorebar(string[] files)
        {
            string[] positive = { "scorebar" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceScoreNumbers(string[] files)
        {
            string[] positive = { Skinini.GetScorePrefix(files) + "-" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceComboNumbers(string[] files)
        {
            if (Skinini.GetComboPrefix(files) == Skinini.GetScorePrefix(files))
                return null;

            string[] positive = { Skinini.GetComboPrefix(files) + "-" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        //
        public static string[] GetInterfaceRanking(string[] files)
        {
            // TODO: split into grades & screen
            string[] positive = { "ranking-" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceScoreEntry(string[] files)
        {
            string[] positive = { "scoreentry" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceSongSelection(string[] files)
        {
            string[] positive = { "menu-back", "menu-button-background", "selection", "star", "selection" };
            string[] negative = { "menu-background", "star2" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceSongSelectionStar2(string[] files)
        {
            string[] positive = { "star2" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceModeSelect(string[] files)
        {
            string[] positive = { "mode" };
            string[] negative = { "selection" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }

        // osu!StandardGameplay\
        public static string[] GetGameplayComboBurst(string[] files)
        {
            string[] terms = { "comboburst" };
            return GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetGameplayDefaultNumbers(string[] files)
        {
            string[] terms = { Skinini.GetHitCirclePrefix(files) + "-" };
            return GetFileSet(files.GetImageFiles(), terms, null);
        }

        // osu!StandardGameplay\Hitcircle
        public static string[] GetGameplayHitCircles(string[] files)
        {
            // TODO: Expand hitcircle more
            string[] positive = { "hitcircle", "lighting" };
            string[] negative = { "spinner" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplayApproachCircle(string[] files)
        {
            string[] positive = { "approachcircle" };
            string[] negative = { "spinner" };
            return GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetGameplayFollowpoints(string[] files)
        {
            string[] positive = { "followpoint" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplaySlider(string[] files)
        {
            // TODO: Expand slider more
            string[] positive = { "slider", "reverse" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplaySpinner(string[] files)
        {
            // TODO: Expand spinner more
            string[] positive = { "spinner" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplayParticles(string[] files)
        {
            string[] positive = { "particle" };
            return GetFileSet(files.GetImageFiles(), positive, null);
        }

        // SOUNDS
        public static string[] GetMainMenuSounds(string[] files)
        {
            string[] terms = { "heart", "seeya", "welcome" };
            return GetFileSet(files, terms, null);
        }
        public static string[] GetKeysSounds(string[] files)
        {
            string[] term = { "key" };
            return GetFileSet(files.GetAudioFiles(), term, null);
        }
        public static string[] GetClicksSounds(string[] files)
        {
            string[] positive = { "check", "menu", "click", "select", "shutter" };
            string[] negative = { "menuclick", "click-short.wav" };
            return GetFileSet(files.GetAudioFiles(), positive, negative);
        }
        public static string[] GetHoverSounds(string[] files)
        {
            string[] positive = { "hover", "menuclick", "click-short" };
            string[] negative = { "confirm" };
            return GetFileSet(files.GetAudioFiles(), positive, negative);
        }
        public static string[] GetDragSounds(string[] files)
        {
            string[] positiveTerms = { "bar", "whoosh" };
            return GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetMultiplayerSounds(string[] files)
        {
            string[] positiveTerm = { "match" };
            return GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }
        public static string[] GetCountdownSounds(string[] files)
        {
            string[] positiveTerms = { "count", "gos", "readys" };
            return GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetMetronomeSounds(string[] files)
        {
            string[] positiveTerm = { "metronome" };
            return GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }
        public static string[] GetGameplaySounds(string[] files)
        {
            string[] positiveTerms = { "combo", "fail", "pass", "applause" };
            return GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetPauseScreenSounds(string[] files)
        {
            string[] positiveTerm = { "pause" };
            return GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }

        public static string[] GetHitSoundsFull(string[] files)
        {
            string[] positive = { "drum", "soft", "normal", "taiko", "spinner" };
            return GetFileSet(files.GetAudioFiles(), positive, null);
        }
        public static string[] GetNormalHitSet(string[] files)
        {
            string[] positiveTerms = { "drum", "soft", "normal" };
            string[] negativeTerms = { "taiko" };
            return GetFileSet(files.GetAudioFiles(), positiveTerms, negativeTerms);
        }
        public static string[] GetSpinnerSet(string[] files)
        {
            string[] positiveTerms = { "spinner" };
            return GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetTaikoSet(string[] files)
        {
            string[] positiveTerm = { "taiko" };
            return GetFileSet(files.GetAudioFiles(), positiveTerm, null);
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

        private static string[] GetFileSet(string[] files, string[] positiveTerms, string[] negativeTerms)
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
