using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.Fonts;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Text.RegularExpressions;
using Avatara.Figure;
using Helios.Web.Helpers;

namespace Helios.Web.Util
{
    public class FigureUtil
    {
        #region Fields

        public static readonly FigureUtil Instance = new FigureUtil();

        #endregion

        public FigureUtil()
        {

        }

        public string GenerateRandomFigure(string genderReq)
        {
            StringBuilder figureOutput = new StringBuilder();

            if (genderReq == null)
            {
                if (CaptchaUtil.Instance.Random.NextBoolean())
                {
                    genderReq = "M";
                }
                else
                {
                    genderReq = "F";
                }
            }

            var randomTypes = new List<FigureSetType>();

            foreach (var setType in FiguredataReader.Instance.FigureSetTypes.Values)
            {
                if (setType.IsMandatory != null && setType.IsMandatory.Value)
                {
                    if (!randomTypes.Any(x => x.Set == setType.Set))
                        randomTypes.Add(setType);
                }
                else if ((setType.IsMaleMandatoryNonHC != null && setType.IsMaleMandatoryNonHC.Value && genderReq == "M") ||
                    (setType.IsFemaleMandatoryNonHC != null && setType.IsFemaleMandatoryNonHC.Value && genderReq == "F"))
                {
                    if (!randomTypes.Any(x => x.Set == setType.Set))
                        randomTypes.Add(setType);
                }
                else
                {
                    // not mandatory item but random chance we may add it :3

                    if (setType.Set != "hr" && setType.Set != "sh")
                        if (CaptchaUtil.Instance.Random.Next(0, 10) < 1)
                            if (!randomTypes.Any(x => x.Set == setType.Set))
                                randomTypes.Add(setType);

                    if (setType.Set == "hr")
                        if (CaptchaUtil.Instance.Random.Next(0, 6) < 5)
                            if (!randomTypes.Any(x => x.Set == setType.Set))
                                randomTypes.Add(setType);

                    if (setType.Set == "sh")
                        if (CaptchaUtil.Instance.Random.Next(0, 6) < 2)
                            if (!randomTypes.Any(x => x.Set == setType.Set))
                                randomTypes.Add(setType);
                }
            }


            foreach (var setType in randomTypes)
            {
                var setTypeList = FiguredataReader.Instance.FigureSets.Values.Where(x => x.SetType == setType.Set && !x.Club && (x.Gender == genderReq || x.Gender == "U") && x.Selectable).ToList();

                if (!setTypeList.Any())
                {
                    continue;
                }

                var randomlySelectedType = setTypeList[CaptchaUtil.Instance.Random.Next(0, setTypeList.Count)];

                if (randomlySelectedType == null)
                {
                    continue;
                }

                var palettes = FiguredataReader.Instance.FigurePalettes[setType.PaletteId].Where(x => x.IsClubRequired == false).ToList();
                var randomPaletteId = randomlySelectedType.Colourable ? int.Parse(palettes[CaptchaUtil.Instance.Random.Next(0, palettes.Count)].ColourId) : 1;

                figureOutput.Append(randomlySelectedType.SetType);
                figureOutput.Append("-");
                figureOutput.Append(randomlySelectedType.Id);
                figureOutput.Append("-");
                figureOutput.Append(randomPaletteId);
                figureOutput.Append(".");
            }

            var figure = figureOutput.ToString();
            
            if (figure.Length> 0)
            {
                figure = figure.Substring(0, figure.Length - 1);
            }

            return figure;
        }
    }
}
