using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExtensions;

namespace osuSkinCustomizer
{
    public static class ElementFinder
    {
        public static string[] GetInterfaceMainMenu(string[] files)
        {
            string[] terms = { "menu-background", "menu-snow", "welcome_text" };
            return FileHandler.GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetInterfaceButton(string[] files)
        {
            string[] terms = { "button-left", "button-middle", "button-right" };
            return FileHandler.GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetInterfaceCursor(string[] files)
        {
            string[] terms = { "cursor" };
            return FileHandler.GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetInterfaceModIcons(string[] files)
        {
            string[] positive = { "mod" };
            string[] negative = { "mode" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceOffsetWizard(string[] imageFiles)
        {
            string[] terms = { "options-offset-tick" };
            return FileHandler.GetFileSet(imageFiles.GetImageFiles(), terms, null);
        }

        // Interface\Playfield
        public static string[] GetInterfacePlayfield(string[] files)
        {
            // Everything before countdown
            string[] positive = { "section", "multi-skipped", "masking-border", "arrow", "play" };
            string[] negative = { "mod", "replay", "reverse" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceCountdown(string[] files)
        {
            string[] positive = { "count", "go", "ready" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceHitBursts(string[] files)
        {
            string[] positive = { "hit" };
            string[] negative = { "circle" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceInputOverlay(string[] files)
        {
            string[] positive = { "inputoverlay" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfacePauseScreen(string[] files)
        {
            string[] positive = { "pause" };
            string[] negative = { "arrow" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceScorebar(string[] files)
        {
            string[] positive = { "scorebar" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceScoreNumbers(string[] files)
        {
            string[] positive = { Skinini.GetScorePrefix(files) + "-" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceComboNumbers(string[] files)
        {
            if (Skinini.GetComboPrefix(files) == Skinini.GetScorePrefix(files))
                return null;

            string[] positive = { Skinini.GetComboPrefix(files) + "-" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        //
        public static string[] GetInterfaceRanking(string[] files)
        {
            // TODO: split into grades & screen
            string[] positive = { "ranking-" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceScoreEntry(string[] files)
        {
            string[] positive = { "scoreentry" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceSongSelection(string[] files)
        {
            string[] positive = { "menu-back", "menu-button-background", "selection", "star", "selection" };
            string[] negative = { "menu-background", "star2" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetInterfaceSongSelectionStar2(string[] files)
        {
            string[] positive = { "star2" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetInterfaceModeSelect(string[] files)
        {
            string[] positive = { "mode" };
            string[] negative = { "selection" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }

        // osu!StandardGameplay\
        public static string[] GetGameplayComboBurst(string[] files)
        {
            string[] terms = { "comboburst" };
            return FileHandler.GetFileSet(files.GetImageFiles(), terms, null);
        }
        public static string[] GetGameplayDefaultNumbers(string[] files)
        {
            string[] terms = { Skinini.GetHitCirclePrefix(files) + "-" };
            return FileHandler.GetFileSet(files.GetImageFiles(), terms, null);
        }

        // osu!StandardGameplay\Hitcircle
        public static string[] GetGameplayHitCircles(string[] files)
        {
            // TODO: Expand hitcircle more
            string[] positive = { "hitcircle", "lighting" };
            string[] negative = { "spinner" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplayApproachCircle(string[] files)
        {
            string[] positive = { "approachcircle" };
            string[] negative = { "spinner" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, negative);
        }
        public static string[] GetGameplayFollowpoints(string[] files)
        {
            string[] positive = { "followpoint" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplaySlider(string[] files)
        {
            // TODO: Expand slider more
            string[] positive = { "slider", "reverse" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplaySpinner(string[] files)
        {
            // TODO: Expand spinner more
            string[] positive = { "spinner" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }
        public static string[] GetGameplayParticles(string[] files)
        {
            string[] positive = { "particle" };
            return FileHandler.GetFileSet(files.GetImageFiles(), positive, null);
        }

        // SOUNDS
        public static string[] GetMainMenuSounds(string[] files)
        {
            string[] terms = { "heart", "seeya", "welcome" };
            return FileHandler.GetFileSet(files, terms, null);
        }
        public static string[] GetKeysSounds(string[] files)
        {
            string[] term = { "key" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), term, null);
        }
        public static string[] GetClicksSounds(string[] files)
        {
            string[] positive = { "check", "menu", "click", "select", "shutter" };
            string[] negative = { "menuclick", "click-short.wav" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positive, negative);
        }
        public static string[] GetHoverSounds(string[] files)
        {
            string[] positive = { "hover", "menuclick", "click-short" };
            string[] negative = { "confirm" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positive, negative);
        }
        public static string[] GetDragSounds(string[] files)
        {
            string[] positiveTerms = { "bar", "whoosh" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetMultiplayerSounds(string[] files)
        {
            string[] positiveTerm = { "match" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }
        public static string[] GetCountdownSounds(string[] files)
        {
            string[] positiveTerms = { "count", "gos", "readys" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetMetronomeSounds(string[] files)
        {
            string[] positiveTerm = { "metronome" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }
        public static string[] GetGameplaySounds(string[] files)
        {
            string[] positiveTerms = { "combo", "fail", "pass", "applause" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetPauseScreenSounds(string[] files)
        {
            string[] positiveTerm = { "pause" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }

        public static string[] GetHitSoundsFull(string[] files)
        {
            string[] positive = { "drum", "soft", "normal", "taiko", "spinner" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positive, null);
        }
        public static string[] GetNormalHitSet(string[] files)
        {
            string[] positiveTerms = { "drum", "soft", "normal" };
            string[] negativeTerms = { "taiko" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerms, negativeTerms);
        }
        public static string[] GetSpinnerSet(string[] files)
        {
            string[] positiveTerms = { "spinner" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerms, null);
        }
        public static string[] GetTaikoSet(string[] files)
        {
            string[] positiveTerm = { "taiko" };
            return FileHandler.GetFileSet(files.GetAudioFiles(), positiveTerm, null);
        }
    }
}
