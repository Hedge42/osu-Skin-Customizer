using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using MyExtensions;
using System.Windows.Controls;

namespace osuSkinCustomizer
{
    public class Importer
    {
        public static string importFolder;
        public static string[] importFiles;

        // Controls
        private TextBox tbImportFolder;
        private Label lblImportFolder;
        private TreeView tvImport;
        private TextBlock txtDetails;
        private Button btnExecute;

        // Other var...
        public static Action UpdateDetailsAction;

        public Importer()
        {
            SetMainWindowControls();
            CreateImportTree();
        }

        private Section[] sections;
        private void CreateImportTree()
        {
            List<Section> sList = new List<Section>();

            // Skin\...
            Section ssSounds = sList.Add<Section>(new Section("Sounds", null));
            Section ssInterface = sList.Add<Section>(new Section("Interface", null));
            Section ssStandard = sList.Add<Section>(new Section("osu!Standard", null));

            // Master-Parent objects
            tvImport.Items.Add(ssInterface.item);
            tvImport.Items.Add(ssStandard.item);
            tvImport.Items.Add(ssSounds.item);

            sList.Add(new Section("Main Menu", ssInterface, ElementFinder.GetInterfaceMainMenu));
            sList.Add(new Section("Button", ssInterface, ElementFinder.GetInterfaceButton));
            sList.Add(new Section("Cursor", ssInterface, ElementFinder.GetInterfaceCursor));
            sList.Add(new Section("Mod Icons", ssInterface, ElementFinder.GetInterfaceModIcons));
            sList.Add(new Section("Offset Wizard", ssInterface, ElementFinder.GetInterfaceOffsetWizard));

            Section ssPlayfield = sList.Add<Section>(new Section("Playfield", ssInterface));
            sList.Add(new Section("Playfield", ssPlayfield, ElementFinder.GetInterfacePlayfield));
            sList.Add(new Section("Countdown", ssPlayfield, ElementFinder.GetInterfaceCountdown));
            sList.Add(new Section("Hit bursts", ssPlayfield, ElementFinder.GetInterfaceHitBursts));
            sList.Add(new Section("Input Overlay", ssPlayfield, ElementFinder.GetInterfaceInputOverlay));
            sList.Add(new Section("Pause Screen", ssPlayfield, ElementFinder.GetInterfacePauseScreen));
            sList.Add(new Section("Scorebar", ssPlayfield, ElementFinder.GetInterfaceScorebar));

            Section ssScoreNumbers = sList.Add<Section>(new Section("Score Numbers", ssPlayfield, ElementFinder.GetInterfaceScoreNumbers));
            sList.Add(new Section(".ini ScorePrefix", ssScoreNumbers, Skinini.Option.ScorePrefix));
            sList.Add(new Section(".ini ScoreOverlap", ssScoreNumbers, Skinini.Option.ScoreOverlap));

            Section ssComboNumbers = sList.Add<Section>(new Section("Combo Numbers", ssPlayfield, ElementFinder.GetInterfaceComboNumbers));
            sList.Add(new Section(".ini ComboOverlap", ssComboNumbers, Skinini.Option.ComboOverlap));
            sList.Add(new Section(".ini ComboPrefix", ssComboNumbers, Skinini.Option.ComboPrefix));

            sList.Add(new Section("Ranking", ssInterface, ElementFinder.GetInterfaceRanking));
            sList.Add(new Section("Score Entry", ssInterface, ElementFinder.GetInterfaceScoreEntry));

            Section ssSongSelect = sList.Add<Section>(new Section("Song Selection", ssInterface, ElementFinder.GetInterfaceSongSelection));
            sList.Add(new Section("Mode Select", ssSongSelect, ElementFinder.GetInterfaceModeSelect));
            sList.Add(new Section("Star2", ssSongSelect, ElementFinder.GetInterfaceSongSelectionStar2));

            sList.Add(new Section("Comboburst", ssStandard, ElementFinder.GetGameplayComboBurst));

            Section ssDefaultNumbers = sList.Add<Section>(new Section("Default Numbers", ssStandard, ElementFinder.GetGameplayDefaultNumbers));
            sList.Add(new Section(".ini HitCircleOverlap", ssDefaultNumbers, Skinini.Option.HitCircleOverlap));
            sList.Add(new Section(".ini HitCirclePrefix", ssDefaultNumbers, Skinini.Option.HitCirclePrefix));

            Section ssHitCircle = sList.Add<Section>(new Section("Hit Circle", ssStandard, ElementFinder.GetGameplayHitCircles));
            sList.Add(new Section("Approach Circle", ssHitCircle, ElementFinder.GetGameplayApproachCircle));
            sList.Add(new Section("Followpoints", ssHitCircle, ElementFinder.GetGameplayFollowpoints));
            sList.Add(new Section(".ini ComboColors", ssHitCircle, Skinini.Option.ComboColours));

            Section ssSlider = sList.Add<Section>(new Section("Slider", ssStandard, ElementFinder.GetGameplaySlider));
            sList.Add(new Section(".ini SliderBall", ssSlider, Skinini.Option.SliderBall));
            sList.Add(new Section(".ini SliderBorder", ssSlider, Skinini.Option.SliderBorder));
            sList.Add(new Section(".ini SliderTrackOverride", ssSlider, Skinini.Option.SliderTrackOverride));

            Section ssSpinner = sList.Add<Section>(new Section("Spinner", ssStandard, ElementFinder.GetGameplaySpinner));
            sList.Add(new Section(".ini Spinner Background", ssSpinner, Skinini.Option.SpinnerBackground));

            sList.Add(new Section("Particles", ssStandard, ElementFinder.GetGameplayParticles));

            sList.Add(new Section("Main Menu", ssSounds, ElementFinder.GetMainMenuSounds));
            sList.Add(new Section("Keys", ssSounds, ElementFinder.GetKeysSounds));
            sList.Add(new Section("Clicks", ssSounds, ElementFinder.GetClicksSounds));
            sList.Add(new Section("Hover", ssSounds, ElementFinder.GetHoverSounds));
            sList.Add(new Section("Drag", ssSounds, ElementFinder.GetDragSounds));
            sList.Add(new Section("Multiplayer", ssSounds, ElementFinder.GetMultiplayerSounds));
            sList.Add(new Section("Countdown", ssSounds, ElementFinder.GetCountdownSounds));
            sList.Add(new Section("Metronome", ssSounds, ElementFinder.GetMetronomeSounds));
            sList.Add(new Section("Gameplay", ssSounds, ElementFinder.GetGameplaySounds));
            sList.Add(new Section("Pause Screen", ssSounds, ElementFinder.GetPauseScreenSounds));

            Section ssHitSounds = sList.Add<Section>(new Section("Hit Sounds", ssSounds));
            sList.Add(new Section("Normal Sets", ssHitSounds, ElementFinder.GetNormalHitSet));
            sList.Add(new Section("Spinner Set", ssHitSounds, ElementFinder.GetSpinnerSet));
            sList.Add(new Section("Taiko Set", ssHitSounds, ElementFinder.GetTaikoSet));

            sections = sList.ToArray();
        }

        /// <summary> Tries to execute an Import.</summary>
        public void Import()
        {
            string[] importFiles = GetSelectedFiles(Importer.importFiles);
            string[] rootFiles = GetSelectedFilesInRoot();

            // Create backup by default
            string backupFolder = FileHandler.CreateBackupFolder(FileHandler.CurrentSkinFolder);

            // Move files that will be replaced in the root folder to the backup folder
            foreach (string file in rootFiles)
            {
                string dest = backupFolder + "\\" + Path.GetFileName(file);
                File.Move(file, dest);

                if (GetSelectedIniTypes().Length > 0)
                {
                    string iniPath = Skinini.GetIniPath(FileHandler.CurrentSkinFolder);
                    string iniFileName = Path.GetFileName(iniPath);
                    string iniDestination = backupFolder + "\\" + iniFileName;
                    File.Copy(iniPath, iniDestination);
                }
            }

            // Copy each file to the root folder
            foreach (string file in importFiles)
            {
                string fileName = Path.GetFileName(file);
                string dest = FileHandler.CurrentSkinFolder + "\\" + fileName;
                File.Copy(file, dest);
            }

            // Replace lines in skin.ini
            Skinini.Option[] selectedTypes = GetSelectedIniTypes();
            foreach (Skinini.Option type in selectedTypes)
            {
                Skinini.Replace(type, importFolder, FileHandler.CurrentSkinFolder);
            }
        }

        /// <summary> Returns the type of each ini parameter that has been selected </summary>
        private Skinini.Option[] GetSelectedIniTypes()
        {
            List<Skinini.Option> selectedTypes = new List<Skinini.Option>();
            foreach (Section s in sections)
            {
                if (s.iniType != Skinini.Option.None && (bool)s.checkBox.IsChecked)
                {
                    selectedTypes.Add(s.iniType);
                }
            }
            return selectedTypes.ToArray();
        }

        /// <summary> Gets controls from MainWindow for easier access and sets their Event Handlers. </summary>
        private void SetMainWindowControls()
        {
            tbImportFolder = MainWindow.instance.tbImportFolder;
            tbImportFolder.TextChanged += ImportFolderTextChangedEventHandler;

            lblImportFolder = MainWindow.instance.lblImportFolder;

            tvImport = MainWindow.instance.tvImportTree;

            txtDetails = MainWindow.instance.txtDetails;

            btnExecute = MainWindow.instance.btnExectute;
            btnExecute.Click += ExecuteEventHandler;

            UpdateDetailsAction = UpdateDetails;
        }

        /// <summary> Also sets importFiles </summary>
        private void SetImportFolder(string path)
        {
            string input = tbImportFolder.Text;
            if (input.IsSkin())
            {
                importFolder = input;

                string skinName = Path.GetFileName(input);
                lblImportFolder.Content = "Import from: " + skinName;

                importFiles = FileHandler.GetFiles(importFolder);
            }
        }

        /// <summary> Updates details textBlock to show each file that will be imported </summary>
        private void UpdateDetails()
        {
            string[] files = FileHandler.GetFiles(importFolder);
            if (files == null || files.Length == 0) return;

            string[] selected = GetSelectedFiles(files);

            string text = "";
            foreach (string file in selected)
                text += file + "\r\n";

            text += "\r\n";

            Skinini.Option[] iniTypes = GetSelectedIniTypes();
            foreach (Skinini.Option type in iniTypes)
                text += "skin.ini " + type.ToString() + "\r\n";

            txtDetails.Text = text;
        }

        /// <summary> Uses search function attached to each section in the tree to return the path to each file </summary>
        private string[] GetSelectedFiles(string[] from)
        {
            List<string> fileList = new List<string>();

            foreach (Section s in sections)
            {
                if (s.checkBox != null && (bool)s.checkBox.IsChecked)
                {
                    if (s.searchFunction != null)
                    {
                        string[] files = s.searchFunction.Invoke(from);
                        if (files != null && files.Length > 0)
                        {
                            foreach (string file in files)
                            {
                                fileList.Add(file);
                            }
                        }
                    }
                }
            }

            return fileList.ToArray();
        }

        /// <summary> Runs GetSelectedFiles(rootFiles) </summary>
        private string[] GetSelectedFilesInRoot()
        {
            string[] rootFiles = FileHandler.GetFiles(FileHandler.CurrentSkinFolder);
            return GetSelectedFiles(rootFiles);
        }

        // Event Handlers...
        public void ImportFolderTextChangedEventHandler(object sender, RoutedEventArgs e)
        {
            SetImportFolder(tbImportFolder.Text);
        }
        public void ExecuteEventHandler(object sender, RoutedEventArgs e)
        {
            Import();
        }
    }
}
