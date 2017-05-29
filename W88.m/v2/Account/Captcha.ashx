<%@ WebHandler Language="C#" Class="Captcha" %>

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Web;
using System.Web.SessionState;

public class Captcha : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string[] fontNames = new string[] { 
            "Comic Sans MS",
            "Arial", 
            "Times New Roman",
            "Georgia", 
            "Trebuchet MS", 
            "Verdana",
            "Courier New",
        };

        FontStyle[] fontStyles = new FontStyle[] {
            FontStyle.Bold, 
            FontStyle.Italic, 
            FontStyle.Regular, 
            FontStyle.Strikeout,
            FontStyle.Underline 
        };

        HatchStyle[] hatchStyles = new HatchStyle[]{
            HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal, HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
            HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross, HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, 
            HatchStyle.Horizontal, HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal,
            HatchStyle.LightHorizontal, HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical, HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal, HatchStyle.NarrowVertical,
            HatchStyle.OutlinedDiamond,HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,HatchStyle.SmallConfetti, HatchStyle.SmallGrid, HatchStyle.SolidDiamond,
            HatchStyle.Sphere, HatchStyle.Trellis,HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
        };
        
        // Generate Random
        Random random = new Random(DateTime.Now.Millisecond);

        // Generate Captcha Code
        string strCode = commonFunctions.generateRandom(4, commonFunctions.randomType.numeric);
        commonVariables.SetSessionVariable("vCode", commonEncryption.encrypting(strCode));

        char[] char_array = null;

        char_array = new char[strCode.Length + strCode.Length];

        for (int int_count = 0; int_count < strCode.Length; int_count++)
        {
            char_array[int_count + int_count] = strCode[int_count];
            char_array[int_count + int_count + 1] = " "[0];
        }

        int width = 600;
        int height = 90;

        // Create a new 32-bit bitmap image.
        using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
        {
            // Create a graphics object for drawing.
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighSpeed;
                Rectangle rect = new Rectangle(0, 0, width, height);

                // Fill in the background. (Darker colors RGB 0 to 100)
                Color top_color = Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100));
                Color bottom_color = Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100));
                using (HatchBrush styleBackground = new HatchBrush(hatchStyles[random.Next(hatchStyles.Length - 1)], top_color, bottom_color))
                {
                    g.FillRectangle(styleBackground, rect);

                    // Set up the text font.
                    SizeF size = default(SizeF);
                    float fontSize = rect.Height + 25;

                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    string fontName = fontNames[random.Next(fontNames.Length - 1)];
                    FontFamily fontFamily = new FontFamily(fontName);

                    using (Font font = new Font(fontFamily, fontSize, fontStyles[random.Next(fontStyles.Length - 1)]))
                    {
                        // Adjust the font size until the text fits within the image.
                        do
                        {
                            fontSize -= 5;
                            size = g.MeasureString(new string(char_array), font, new SizeF(width, height), format);
                        } while (size.Width > rect.Width);

                        // Create a path using the text and warp it randomly.
                        GraphicsPath path = new GraphicsPath();
                        path.AddString(new string(char_array), font.FontFamily, Convert.ToInt32(font.Style), font.Size, rect, format);

                        float v = 6f;
                        PointF[] points = {
                            new PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                            new PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                            new PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                            new PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
                        };

                        Matrix matrix = new Matrix();
                        matrix.Translate(0f, 0f);
                        path.Warp(points, rect, matrix, WarpMode.Perspective, 0f);

                        // Draw the text. (Lighter colors RGB 100 to 255)
                        Color text_color = Color.FromArgb(random.Next(100, 255), random.Next(100, 255), random.Next(100, 255));
                        using (HatchBrush styleText = new HatchBrush(hatchStyles[random.Next(hatchStyles.Length - 1)], text_color, text_color))
                        {
                            g.FillPath(styleText, path);
                        }
                    }
                }
            }

            context.Response.ContentType = "image/png";
            bitmap.Save(context.Response.OutputStream, ImageFormat.Png);
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}