using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.Fonts;
using static System.Net.Mime.MediaTypeNames;

namespace Helios.Web.Util
{
	public class Captcha
	{
		public static Random random = new Random();

		private static int width = 200;
		private static int height = 50;
		private static int gridSize = 11;
		private static int fontSize = 45;
		private static int rotationAmplitude = 8;
		private static int scaleAmplitude = 15;

		public static string RandomTextSequence(int length)
		{

			char[] data = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
				'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
				'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
				'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
				'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6',
				'7', '8', '9'};

			char[] index = new char[length - 1];

			var i = 0;

			for (i = 0; i < (index.Length); i++)
			{
				int ran = random.Next(data.Length);
				index[i] = data[ran];
			}

			return new string(index);
		}

		public static byte[] Generate(string text)
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

						int oldX = random.Next(0, width);
						int oldY = random.Next(0, height);

						int newX = random.Next(0, height);
						int newY = random.Next(0, height);

						lines.Add(new int[] { oldX, oldY });
						lines.Add(new int[] { newX, newY });

						PointF[] imageSharpLines = lines
							.Select(r => new PointF(r[0], r[1]))
							.ToArray();

						var linePen = new Pen(Color.Black, 1);
						imageContext.DrawLines(linePen, imageSharpLines);
					}

				});
				int xPos = 20;

				foreach (char ch in text.ToCharArray())
				{
					image.Mutate(imageContext =>
					{
						int charMaxWidth = (width / text.Length) - random.Next(0, 20);
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


		private static void DrawCharacter(IImageProcessingContext imageContext, char ch, int xPos, int boxWidth)
		{
			double degree = (random.NextDouble() * rotationAmplitude * 2) - rotationAmplitude;
			double scale = 1 - (random.NextDouble() * scaleAmplitude / 100);

			var normalFont = SystemFonts.CreateFont("Arial", fontSize, FontStyle.Regular);
			var boldFont = SystemFonts.CreateFont("Arial", fontSize, FontStyle.Bold);

			imageContext.SetDrawingTransform(Matrix3x2Extensions.CreateRotationDegrees((float)degree));

			//imageContext.SetDrawingTransform(Matrix3x2Extensions.CreateScale((float)scale, new PointF(0, 0)));

			int currentX = xPos + (boxWidth / 2);
			int currentY = height / 2;

			var font = random.Next(2) == 0 ? normalFont : boldFont;

			var measure = TextMeasurer.Measure(ch.ToString(), new TextOptions(font));
			int charWidth = (int)measure.Width;
			int charHeight = (int)measure.Height;

			imageContext.DrawText(ch.ToString(), font, Color.Black,
				new PointF(currentX + (-(charWidth / 2)), currentY + (measure.Top - (charHeight / 2))));
		}

		//imageContext.Resize(new Size((int)scale));
	}
}
