using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpImageLibrary;
using ImageMagick;

namespace osuSkinCustomizer
{
    public static class ImageManipulator
    {
        // https://github.com/dlemstra/Magick.NET/tree/master/Documentation
        public static void TintImage(string imagePath, System.Windows.Media.Color color)
        {
            using (MagickImage image = new MagickImage(imagePath))
            {
                // MagickColor c = new MagickColor(color.R, color.G, color.B);
                // image.Tint("100000000", c);

                // TODO
                // This isn't how osu colors things. Eventually find a better way to do this
                MagickImage tinted = ChangeWhiteColor(image, color, 50);
                image.Write(imagePath);
            }
        }

        private static MagickImage ChangeWhiteColor(MagickImage Image, System.Windows.Media.Color TargetColor, int fuzz)
        {
            // https://stackoverflow.com/questions/42700059/cannot-replace-color-with-magick-net
            Image.ColorFuzz = new Percentage(fuzz);
            Image.Opaque(MagickColor.FromRgb((byte)255, (byte)255, (byte)255),
                MagickColor.FromRgb(TargetColor.R,
                TargetColor.G,
                TargetColor.B));
            return Image;
        }
    }
}
