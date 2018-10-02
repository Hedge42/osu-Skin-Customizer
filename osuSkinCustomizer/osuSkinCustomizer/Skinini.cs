using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyExtensions;
using System.Windows;

namespace osuSkinCustomizer
{
    public static class Skinini
    {
        public enum Type
        {
            None,

            // General
            Name,
            Author,
            Version,
            AnimationFramerate,
            AllowSliderBallTint,
            ComboBurstRandom,
            CursorCentre,
            CursorExpand,
            CursorRotate,
            CursorTrailRotate,
            CustomComboBurstSounds,
            HitCircleOverlayAboveNumber,
            LayeredHitSounds,
            SliderBallFlip,
            SliderBallFrames,
            SliderStyle,
            SpinnerFadePlayfield,
            SpinnerFrequencyModulate,
            SpinnerNoBlink,

            // Colours
            ComboColours,
            InputOverlayText,
            MenuGlow,
            SliderBall,
            SliderBorder,
            SliderTrackOverride,
            SongSelectActiveText,
            SongSelectInactiveText,
            SpinnerBackground,
            StarBreakAdditive,

            // Fonts
            HitCirclePrefix,
            HitCircleOverlap,
            ScorePrefix,
            ScoreOverlap,
            ComboPrefix,
            ComboOverlap,
        }

        public static void Replace(Type t, string fromFolder, string toFolder)
        {
            if (t == Type.ComboColours)
            {
                ReplaceComboColors(fromFolder, toFolder);
                return;
            }

            string fromLine = GetLine(t, fromFolder);
            string toLine = GetLine(t, toFolder);

            string[] toLinesFull = File.ReadAllLines(GetIniPath(toFolder));
            int index = IndexOf(toLinesFull, toLine);
            toLinesFull[index] = fromLine;

            File.WriteAllLines(GetIniPath(toFolder), toLinesFull);
        }
        public static string GetIniPath(string folder)
        {
            if (folder.IsSkin())
            {
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    if (Path.GetFileName(file).ToLower() == "skin.ini")
                    {
                        return file;
                    }
                }
            }
            return "";
        }
        public static string GetIniPath(string[] files)
        {
            foreach (string file in files)
            {
                if (Path.GetFileName(file).ToLower() == "skin.ini")
                {
                    return file;
                }
            }
            return "";
        }

        public static string GetComboPrefix(string[] files)
        {
            string folder = new FileInfo(files[0]).Directory.FullName;
            return GetValue(GetLine(Type.ComboPrefix, folder));
        }
        public static string GetHitCirclePrefix(string[] files)
        {
            string folder = new FileInfo(files[0]).Directory.FullName;
            return GetValue(GetLine(Type.HitCirclePrefix, folder));
        }
        public static string GetScorePrefix(string[] files)
        {
            string folder = new FileInfo(files[0]).Directory.FullName;
            return GetValue(GetLine(Type.ScorePrefix, folder));
        }

        private static void ReplaceComboColors(string fromFolder, string toFolder)
        {
            string[] fromLines = GetRealLines(fromFolder);
            string[] toLines = GetRealLines(toFolder);

            string[] fromComboColorLines = GetComboColorLines(fromLines);
            string[] toComboColorLines = GetComboColorLines(toLines);

            // get index of each combo color line from allLines in toFolder skin.ini...
            string[] toAllLines = File.ReadAllLines(GetIniPath(toFolder));

            // Turn full file into a list
            List<string> toAllLinesList = new List<string>();
            foreach (string line in toAllLines)
                toAllLinesList.Add(line);

            // Remove each combo color line (except for [0])
            for (int i = 1; i < toComboColorLines.Length; i++)
                toAllLinesList.Remove(toComboColorLines[i]);

            // Replace first combo color with fromComboColorLines[0]
            int firstIndex = toAllLinesList.IndexOf(toComboColorLines[0]);
            toAllLinesList[firstIndex] = fromComboColorLines[0];

            // Insert each fromComboColorLine after the first
            for (int i = 1; i < fromComboColorLines.Length; i++)
                toAllLinesList.Insert(firstIndex + i, fromComboColorLines[i]);

            string[] newLines = toAllLinesList.ToArray();
            File.WriteAllLines(GetIniPath(toFolder), newLines);
        }
        private static string[] GetComboColorLines(string[] fromLines)
        {
            List<string> lines = new List<string>();
            foreach (string line in fromLines)
            {
                if (IsComboColor(line))
                {
                    lines.Add(line);
                }
            }
            return lines.ToArray();
        }
        private static bool IsComboColor(string line)
        {
            return line.ToLower().StartsWith("combo") && int.TryParse(line[5].ToString(), out int a);
        }

        /// <summary> Returns each line in the skin.ini where the line actually compiles </summary>
        private static string[] GetRealLines(string folder)
        {
            string path = GetIniPath(folder);
            if (path == "") return null;

            string[] allLines = File.ReadAllLines(path);
            List<string> relevantLines = new List<string>();
            foreach (string line in allLines)
            {
                if (!line.StartsWith("//") && !line.StartsWith("[") && line.Trim() != "")
                {
                    relevantLines.Add(line);
                }
            }

            return relevantLines.ToArray();
        }

        /// <summary> Returns the full line from the folder's skin.ini where the Type == t </summary>
        private static string GetLine(Type t, string folder)
        {
            string[] lines = GetRealLines(folder);
            foreach (string line in lines)
            {
                if (line.Trim().ToLower().StartsWith(t.ToString().ToLower()))
                {
                    return line;
                }
            }
            return "";
        }

        /// <summary> Gets all text after the ':' character </summary>
        private static string GetValue(string line)
        {
            return line.Substring(line.IndexOf(':') + 1).Trim();
        }

        /// <summary> Finds the index of a specific string within an array. Returns -1 if not found. </summary>
        private static int IndexOf(string[] strings, string value)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (value == strings[i])
                    return i;
            }
            return -1;
        }
    }
}
