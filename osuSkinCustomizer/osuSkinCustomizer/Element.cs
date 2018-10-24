using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osuSkinCustomizer
{
    public enum Element
    {
        // can include everything... instead, only include the stuff that may be looked for individually - gameplay elements
        Cursor,
        CursorTrail,
        CursorSmoke,
        Hitcircle,
        HitcircleOverlay,
        ApproachCircle,
        DefaultNumbers, // multiple
        Followpoints, // multiple
        ReverseArrow,
        SliderStartCircle,
        SliderStartCircleOverlay,
        SliderEndCircle,
        SliderEndCircleOverlay,
        SliderFollowCircle,
        SliderBall,
    }
    public enum ElementSection
    {
        // derived from skinning wiki, but with slight changes in the way that makes sense to me

        // interface
        MainMenu,
        Button,
        Cursor,
        ModIcon,
        OffsetWizard,
        Playfield,
        Countdown,
        HitBursts,
        InputOverlay,
        PauseScreen,
        Scorebar,
        ScoreNumbers,
        ComboNumbers,
        Ranking,
        ScoreEntry,
        SongSelection,
        // Star2,
        ModeSelect,

        // Gameplay
        ComboBurst,
        DefaultNumbers,
        Hitcircle,
        Followpoints,
        Slider,
        Sinner,
        Particles, // include star2 here

        // Sounds
        MainMenuSounds,
        KeysSounds,
        ClicksSounds,
        HoverSounds,
        DragSounds,
        MultiplayerSounds,
        CountdownSounds,
        MetronomeSounds,
        GameplaySounds,
        PauseScreenSounds,
        HitSounds,
        TaikoSounds,
    }

    public static class ElementExtensions
    {
        public static List<string> GetPaths(this Element e, string path)
        {
            List<string> paths = new List<string>();

            switch (e)
            {

            }


            return paths;
        }
    }
}
