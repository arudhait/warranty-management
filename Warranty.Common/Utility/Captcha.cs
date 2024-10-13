using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Warranty.Common.Utility
{
    public class Captcha
    {
        #region Private Members
        private const int iHeight = 80;
        private const int iWidth = 190;
        public const string HardCaptchaText = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+.{0,8}$";
        public const string MediumCaptchaText = @"^(?=.*[a-z])(?=.*\d).+.{0,6}$";

        private struct CharacterSet
        {
            public const string LowerCaseCharacter = "abcdefghijklmnopqrstuvwxyz";
            public const string UpperCaseCharacter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public const string DigitCharacter = "02345689";
        }
        #endregion

        #region Private Methods
        private static string GenerateCaptchaCode(CaptchaType captchaType)
        {
            string characterSet = string.Empty;
            string randomString = string.Empty;
            switch (captchaType)
            {
                case CaptchaType.Simple:
                    {
                        return GenereteRandomString(CharacterSet.DigitCharacter, 4);
                    }

                case CaptchaType.Medium:
                    {
                        characterSet = string.Format("{0}{1}", CharacterSet.LowerCaseCharacter, CharacterSet.DigitCharacter);

                        while (true)
                        {
                            randomString = GenereteRandomString(characterSet, 6);
                            if (Regex.IsMatch(randomString, MediumCaptchaText))
                                break;
                        }
                        return randomString;

                    }
                case CaptchaType.Hard:
                    {
                        characterSet = string.Format("{0}{1}{2}", CharacterSet.UpperCaseCharacter, CharacterSet.LowerCaseCharacter, CharacterSet.DigitCharacter);
                        while (true)
                        {
                            randomString = GenereteRandomString(characterSet, 8);
                            if (Regex.IsMatch(randomString, HardCaptchaText))
                                break;
                        }
                        return randomString;
                        //return GenereteRandomString(characterSet, 8);
                    }
                default:
                    return string.Empty;
            }
        }
        private static string GenereteRandomString(string characterSet, int length)
        {
            Random random = new Random();

            //The below code will select the random characters from the set
            //and then the array of these characters are passed to string 
            //constructor to make an alphanumeric string
            string randomCode = new string(
                Enumerable.Repeat(characterSet, length)
                    .Select(set => set[random.Next(set.Length)])
                    .ToArray());
            return randomCode;
        }
        private static byte[] GenerateCaptchaImage(string captchaText)
        {
            #region Local variable declaration
            int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
            int[] aTextColor = new int[] { 0, 0, 0 };
            int[] aFontEmSizes = new int[] { 20, 25, 30, 35 };

            string[] aFontNames = new string[]
                                  {
                                   "Comic Sans MS",
                                   "Arial",
                                   "Times New Roman",
                                   "Georgia",
                                   "Verdana",
                                   "Geneva"
                                  };
            FontStyle[] aFontStyles = new FontStyle[]
                        {
                         FontStyle.Bold,
                         FontStyle.Italic,
                         FontStyle.Regular,
                         FontStyle.Strikeout,
                         FontStyle.Underline
                        };
            HatchStyle[] aHatchStyles = new HatchStyle[]
                        {
                         HatchStyle.BackwardDiagonal, HatchStyle.Cross,
                            HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
                         HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
                            HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
                         HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
                            HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                         HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
                            HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
                         HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
                            HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
                         HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
                            HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
                         HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
                            HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
                         HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
                            HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
                         HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
                        };
            #endregion
            string sCaptchaText = captchaText; //iNumber.ToString();

            //Creates an output Bitmap
            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            Random oRandom = new Random();

            //Create a Drawing area
            RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
            //  Brush oBrush = default(Brush);

            //Draw background (Lighter colors RGB 100 to 255)
            Brush oBrush = new HatchBrush(aHatchStyles[oRandom.Next
                (aHatchStyles.Length - 1)], Color.FromArgb((oRandom.Next(100, 255)),
                (oRandom.Next(100, 255)), (oRandom.Next(100, 255))), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            int i = 0;
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = iWidth / (iChars + 1) * i + 10;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                //Draw the letters with Random Font Type, Size and Color
                oGraphics.DrawString
                (
                //Text
                sCaptchaText.Substring(i, 1),
                //Random Font Name and Style
                new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                   aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                   aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                //Random Color (Darker colors RGB 0 to 100)
                new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                   oRandom.Next(0, 100), oRandom.Next(0, 100))),
                x,
                oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }

            MemoryStream oMemoryStream = new MemoryStream();
            oOutputBitmap.Save(oMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] oBytes = oMemoryStream.GetBuffer();

            oOutputBitmap.Dispose();
            oMemoryStream.Close();
            return oBytes;
        }
        #endregion

        #region Public Methods
        public static CaptchaResult Generate(CaptchaType captchaType = CaptchaType.Simple)
        {

            string captchaCode = GenerateCaptchaCode(captchaType);
            CaptchaResult result = new CaptchaResult();
            result.CatpchaCode = captchaCode;
            // bool b1=RegularExpressions.IsValidAlphaNumeric(captchaCode);
            result.CaptchaBase64 = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(GenerateCaptchaImage(captchaCode)));
            return result;
        }
        #endregion
    }
    public enum CaptchaType
    {
        Simple,
        Medium,
        Hard
    }
    public class CaptchaResult
    {
        public string CatpchaCode { get; set; }
        public string CaptchaBase64 { get; set; }
    }
}
