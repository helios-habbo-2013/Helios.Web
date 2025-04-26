using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Helios.Web.Util
{
    public class CaptchaUtil
    {
        #region Fields

        public static readonly CaptchaUtil Instance = new CaptchaUtil();

        #endregion

        public List<string> Words { get; }


        public Random Random { get; }

        private static int width = 310;
        private static int height = 50;
        // private static int gridSize = 11;
        private static int fontSize = 45;
        private static int rotationAmplitude = 2;
        private static int scaleAmplitude = 15;

        public CaptchaUtil()
        {
            Words = new List<string>();
            Random = new Random();
        }

        public void Load()
        {
            Words.Clear();

            foreach (var word in File.ReadAllLines("tools/words.txt"))
            {
                if (word.Length < 3 || word.Length > 7) continue;

                Words.Add(word.Trim());
            }
        }

        public string RandomWord()
        {
            return Words[Random.Next(0, Words.Count)];
        }

        public string RandomTextSequence(int length)
        {

            char[] data =
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
                'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
                'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
                'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0'
				
				/*, '1', '2', '3', '4', '5', '6', '7', '8', '9'*/
			};

            char[] index = new char[length - 1];

            var i = 0;

            for (i = 0; i < (index.Length); i++)
            {
                int ran = Random.Next(data.Length);
                index[i] = data[ran];
            }

            return new string(index);
        }

        public byte[] Generate(string text)
        {
            using (var image = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(width, height))
            {

                image.Mutate(imageContext =>
                {
                    imageContext.GetGraphicsOptions().Antialias = true;
                    imageContext.GetGraphicsOptions().AntialiasSubpixelDepth = 1;

                    imageContext.BackgroundColor(Color.White);

                    for (int i = 0; i < 8; i++)
                    {
                        var lines = new List<int[]>();

                        int oldX = Random.Next(0, width);
                        int oldY = Random.Next(0, height);

                        int newX = Random.Next(0, width);
                        int newY = Random.Next(0, height);

                        lines.Add(new int[] { oldX, oldY });
                        lines.Add(new int[] { newX, newY });

                        PointF[] imageSharpLines = lines
                            .Select(r => new PointF(r[0], r[1]))
                            .ToArray();

                        var linePen = new SolidPen(new SolidBrush(Color.Black), 1);
                        imageContext.DrawLine(linePen, imageSharpLines);
                    }

                });

                int xPos = 20;

                foreach (char ch in text.ToCharArray())
                {
                    image.Mutate(imageContext =>
                    {
                        int charMaxWidth = (width / text.Length) - Random.Next(0, 10);
                        DrawCharacter(imageContext, ch, xPos, charMaxWidth);
                        xPos += charMaxWidth;
                    });
                }


                using (var ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    return ms.ToArray();
                }
            }
        }


        private void DrawCharacter(IImageProcessingContext imageContext, char ch, int xPos, int boxWidth)
        {
            double degree = (Random.NextDouble() * rotationAmplitude * 2) - rotationAmplitude;
            double scale = 1 - (Random.NextDouble() * scaleAmplitude / 100);

            var normalFont = SystemFonts.CreateFont("Times New Roman", fontSize, FontStyle.Regular);
            var boldFont = SystemFonts.CreateFont("Times New Roman", fontSize, FontStyle.Bold);
            var italicFont = SystemFonts.CreateFont("Times New Roman", fontSize, FontStyle.Italic);
            var italicBoldFont = SystemFonts.CreateFont("Times New Roman", fontSize, FontStyle.BoldItalic);

            imageContext.SetDrawingTransform(Matrix3x2Extensions.CreateRotationDegrees((float)degree));

            //imageContext.SetDrawingTransform(Matrix3x2Extensions.CreateScale((float)scale, new PointF(0, 0)));

            int currentX = xPos + (boxWidth / 2);
            int currentY = height / 2;

            Font font;

            switch (Random.Next(0, 4))
            {
                case 0:
                    font = normalFont;
                    break;
                case 1:
                    font = boldFont;
                    break;
                case 2:
                    font = italicFont;
                    break;
                default:
                    font = italicBoldFont;
                    break;
            }

            var measure = TextMeasurer.MeasureAdvance(ch.ToString(), new TextOptions(font));
            int charWidth = (int)measure.Width;
            int charHeight = (int)measure.Height;

            imageContext.DrawText(ch.ToString(), font, Color.Black,
                new PointF(currentX + (-(charWidth / 2)), currentY + (measure.Top - (charHeight / 2))));
        }

        //imageContext.Resize(new Size((int)scale));
    }
}
