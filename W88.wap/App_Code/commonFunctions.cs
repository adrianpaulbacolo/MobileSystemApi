using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public static class commonFunctions
{
    public static void cssInclude(string cssPath, System.Web.UI.Page page)
    {
        using (System.Web.UI.HtmlControls.HtmlGenericControl css = new System.Web.UI.HtmlControls.HtmlGenericControl())
        {
            css.TagName = "style";
            css.Attributes.Add("type", "text/css");
            css.InnerHtml = "@import \"" + cssPath + "\";";

            page.Header.Controls.Add(css);
        }
    }
    public static void cssInclude(string cssPath, System.Web.UI.MasterPage masterPage)
    {
        using (System.Web.UI.HtmlControls.HtmlGenericControl css = new System.Web.UI.HtmlControls.HtmlGenericControl())
        {
            css.TagName = "style";
            css.Attributes.Add("type", "text/css");
            css.InnerHtml = "@import \"" + cssPath + "\";";

            masterPage.Page.Header.Controls.Add(css);
        }
    }

    public static void cssInclude(string cssPath, bool useImport, System.Web.UI.Page page)
    {
        if (useImport)
        {
            using (System.Web.UI.HtmlControls.HtmlGenericControl control = new System.Web.UI.HtmlControls.HtmlGenericControl())
            {
                control.TagName = "style";
                control.Attributes.Add("type", "text/css");
                control.InnerHtml = "@import \"" + cssPath + "\";";

                page.Header.Controls.Add(control);
            }
        }
        else
        {
            using (System.Web.UI.HtmlControls.HtmlLink control = new System.Web.UI.HtmlControls.HtmlLink())
            {
                control.Attributes.Add("rel", "Stylesheet");
                control.Attributes.Add("type", "text/css");
                control.Attributes.Add("href", cssPath);

                page.Header.Controls.Add(control);
            }
        }
    }
    public static void cssInclude(string cssPath, bool useImport, System.Web.UI.MasterPage masterPage)
    {
        if (useImport)
        {
            using (System.Web.UI.HtmlControls.HtmlGenericControl control = new System.Web.UI.HtmlControls.HtmlGenericControl())
            {
                control.TagName = "style";
                control.Attributes.Add("type", "text/css");
                control.InnerHtml = "@import \"" + cssPath + "\";";

                masterPage.Page.Header.Controls.Add(control);
            }
        }
        else
        {
            using (System.Web.UI.HtmlControls.HtmlLink control = new System.Web.UI.HtmlControls.HtmlLink())
            {
                control.Attributes.Add("rel", "Stylesheet");
                control.Attributes.Add("type", "text/css");
                control.Attributes.Add("href", cssPath);

                masterPage.Page.Header.Controls.Add(control);
            }
        }
    }

    public static void cssInclude(string cssPath, string mediaType, bool useImport, System.Web.UI.Page page)
    {
        using (System.Web.UI.HtmlControls.HtmlGenericControl control = new System.Web.UI.HtmlControls.HtmlGenericControl())
        {
            if (useImport)
            {
                control.TagName = "style";
                control.Attributes.Add("type", "text/css");
                control.InnerHtml = "@import \"" + cssPath + "\";";
            }
            else
            {
                control.TagName = "link";
                control.Attributes.Add("rel", "Stylesheet");
                control.Attributes.Add("type", "text/css");
                control.Attributes.Add("href", cssPath);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    control.Attributes.Add("media", mediaType);
                }
            }

            page.Header.Controls.Add(control);
        }
    }
    public static void cssInclude(string cssPath, string mediaType, bool useImport, System.Web.UI.MasterPage masterPage)
    {
        using (System.Web.UI.HtmlControls.HtmlGenericControl control = new System.Web.UI.HtmlControls.HtmlGenericControl())
        {
            if (useImport)
            {
                control.TagName = "style";
                control.Attributes.Add("type", "text/css");
                control.InnerHtml = "@import \"" + cssPath + "\";";
            }
            else
            {
                control.TagName = "link";
                control.Attributes.Add("rel", "Stylesheet");
                control.Attributes.Add("type", "text/css");
                control.Attributes.Add("href", cssPath);
                if (!string.IsNullOrEmpty(mediaType))
                {
                    control.Attributes.Add("media", mediaType);
                }
            }

            masterPage.Page.Header.Controls.Add(control);
        }
    }

    public static void jsInclude(string jsPath, System.Web.UI.Page page)
    {
        using (System.Web.UI.HtmlControls.HtmlGenericControl js = new System.Web.UI.HtmlControls.HtmlGenericControl())
        {
            js.TagName = "script";
            js.Attributes.Add("type", "text/javascript");
            js.Attributes.Add("src", jsPath);

            page.Header.Controls.Add(js);
        }
    }
    public static void jsInclude(string jsPath, System.Web.UI.MasterPage masterPage)
    {
        using (System.Web.UI.HtmlControls.HtmlGenericControl js = new System.Web.UI.HtmlControls.HtmlGenericControl())
        {
            js.TagName = "script";
            js.Attributes.Add("type", "text/javascript");
            js.Attributes.Add("src", jsPath);

            masterPage.Page.Header.Controls.Add(js);
        }
    }

    public static void faviconInclude(string faviconPath, System.Web.UI.Page page)
    {
        using (System.Web.UI.HtmlControls.HtmlLink favicon = new System.Web.UI.HtmlControls.HtmlLink())
        {
            favicon.Attributes.Add("rel", "shortcut icon");
            favicon.Attributes.Add("type", "image/x-icon");
            favicon.Attributes.Add("href", faviconPath);

            page.Header.Controls.Add(favicon);
        }
    }
    public static void faviconInclude(string faviconPath, System.Web.UI.MasterPage masterPage)
    {
        using (System.Web.UI.HtmlControls.HtmlLink favicon = new System.Web.UI.HtmlControls.HtmlLink())
        {
            favicon.Attributes.Add("rel", "shortcut icon");
            favicon.Attributes.Add("type", "image/x-icon");
            favicon.Attributes.Add("href", faviconPath);

            masterPage.Page.Header.Controls.Add(favicon);
        }
    }

    public static void metaInclude(string key, string value, System.Web.UI.Page page)
    {
        using (System.Web.UI.HtmlControls.HtmlMeta meta_tag = new System.Web.UI.HtmlControls.HtmlMeta())
        {
            meta_tag.Attributes.Add(key, value);

            page.Header.Controls.Add(meta_tag);
        }
    }
    public static void metaInclude(string key, string value, System.Web.UI.MasterPage masterPage)
    {
        using (System.Web.UI.HtmlControls.HtmlMeta meta_tag = new System.Web.UI.HtmlControls.HtmlMeta())
        {
            meta_tag.Attributes.Add(key, value);

            masterPage.Page.Header.Controls.Add(meta_tag);
        }
    }

    public static void metaInclude(System.Collections.Specialized.NameValueCollection metaAttributes, System.Web.UI.Page page)
    {
        using (System.Web.UI.HtmlControls.HtmlMeta meta_tag = new System.Web.UI.HtmlControls.HtmlMeta())
        {
            foreach (string key in metaAttributes.AllKeys)
            {
                meta_tag.Attributes.Add(key, metaAttributes.Get(key));
            }

            page.Header.Controls.Add(meta_tag);
        }
    }
    public static void metaInclude(System.Collections.Specialized.NameValueCollection metaAttributes, System.Web.UI.MasterPage masterPage)
    {
        using (System.Web.UI.HtmlControls.HtmlMeta meta_tag = new System.Web.UI.HtmlControls.HtmlMeta())
        {
            foreach (string key in metaAttributes.AllKeys)
            {
                meta_tag.Attributes.Add(key, metaAttributes.Get(key));
            }

            masterPage.Page.Header.Controls.Add(meta_tag);
        }
    }

    public static void setLabel(System.Web.UI.WebControls.Literal Object, string text)
    {
        if (Object != null) Object.Text = text;
    }
    public static void setLabel(System.Web.UI.WebControls.Label Object, string text)
    {
        if (Object != null) Object.Text = text;
    }
    public static void setLabel(System.Web.UI.HtmlControls.HtmlGenericControl Object, string text)
    {
        if (Object != null) Object.InnerText = text;
    }

    public static void highlightMenu(string labelId, System.Web.UI.Page page)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl div_highlight = (System.Web.UI.HtmlControls.HtmlGenericControl)page.FindControl("dbtn_" + labelId);
        System.Web.UI.HtmlControls.HtmlGenericControl mimg_highlight = (System.Web.UI.HtmlControls.HtmlGenericControl)page.FindControl("mimg_" + labelId);
        System.Web.UI.HtmlControls.HtmlGenericControl himg_highlight = (System.Web.UI.HtmlControls.HtmlGenericControl)page.FindControl("himg_" + labelId);

        if (div_highlight != null) { div_highlight.Attributes.Add("class", "highlight"); }
        if (mimg_highlight != null) { mimg_highlight.Attributes.Add("class", "selected"); }
        if (himg_highlight != null) { himg_highlight.Attributes.Add("class", "selected"); }
    }


    #region generation
    public static string generateRandom(int length, randomType type)
    {
        string allowed_chars = string.Empty;
        string upper_chars = string.Empty;

        Random random_number;

        char[] chars;

        int allowed_length = 0;
        int charAt = 0;

        random_number = new Random();

        chars = new char[length];

        switch (type)
        {
            case randomType.alpha:
                allowed_chars = "abcdefghijkmnopqrstuvwxyz";
                break;
            case randomType.alphaNumeric:
                allowed_chars = "abcdefghijkmnopqrstuvwxyz1234567890";
                break;
            case randomType.numeric:
                allowed_chars = "0123456789";
                break;
        }

        allowed_length = allowed_chars.Length - 1;

        for (int int_count = 0; int_count < length; int_count++)
        {
            charAt = Convert.ToInt32((allowed_length * random_number.NextDouble()));

            chars[int_count] = allowed_chars[charAt];

            if (Convert.ToInt32(random_number.Next(20)) % 2 == 0)
            {
                upper_chars = chars[int_count].ToString().ToUpper();

                chars[int_count] = upper_chars[0];
            }
        }

        return new string(chars);
    }
    public static string generateJson(System.Data.DataSet dataSet)
    {
        System.Text.StringBuilder string_builder = new System.Text.StringBuilder();

        string_builder.Append("[");

        foreach (System.Data.DataRow data_row in dataSet.Tables[0].Rows)
        {
            string_builder.Append("{");

            foreach (System.Data.DataColumn data_col in dataSet.Tables[0].Columns)
            {
                string_builder.AppendFormat("\"{0}\":\"{1}\",", data_col.ColumnName, data_row[data_col.ColumnName]);
            }

            string_builder.Remove(string_builder.Length - 1, 1);
            string_builder.Append("},");
        }

        string_builder.Remove(string_builder.Length - 1, 1);
        string_builder.Append("]");

        return Convert.ToString(string_builder);
    }
    public static string generateJson(System.Collections.Specialized.NameValueCollection nameval)
    {
        System.Text.StringBuilder string_builder = new System.Text.StringBuilder();

        string_builder.Append("[");

        foreach (string key in nameval.Keys)
        {
            string_builder.Append("{");
            string_builder.AppendFormat("\"{0}\":\"{1}\",", key, nameval.Get(key));
            string_builder.Remove(string_builder.Length - 1, 1);
            string_builder.Append("},");
        }

        string_builder.Remove(string_builder.Length - 1, 1);
        string_builder.Append("]");

        return Convert.ToString(string_builder);
    }
    public static string generateDelimitedString(System.Data.DataSet dataSet, string columnDelimiter, string rowDelimiter)
    {
        System.Text.StringBuilder string_builder = new System.Text.StringBuilder();

        foreach (System.Data.DataRow data_row in dataSet.Tables[0].Rows)
        {
            foreach (System.Data.DataColumn data_col in dataSet.Tables[0].Columns)
            {
                string_builder.AppendFormat("{0}{1}", data_row[data_col.ColumnName], columnDelimiter);
            }

            string_builder.Remove(string_builder.Length - 1, 1);
            string_builder.Append(rowDelimiter);
        }

        return Convert.ToString(string_builder);
    }
    public static void generateCaptcha(HttpContext context, int width, int height)
    {
        System.Drawing.Bitmap obj_bitmap = null;

        System.Drawing.Graphics obj_graphics = null;

        System.Drawing.Rectangle rect_area;

        System.Drawing.Drawing2D.LinearGradientBrush linear_brush = null;

        System.Drawing.Font obj_font = null;

        string random_string = string.Empty;

        char[] char_array = null;

        using (obj_bitmap = new System.Drawing.Bitmap(width, height))
        {
            using (obj_graphics = System.Drawing.Graphics.FromImage(obj_bitmap))
            {
                rect_area = new System.Drawing.Rectangle(0, 0, width, height);

                using (linear_brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect_area, System.Drawing.Color.White, System.Drawing.Color.CornflowerBlue, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    obj_graphics.FillRectangle(linear_brush, 0, 0, width, height);

                    obj_graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

                    random_string = commonFunctions.generateRandom(4, commonFunctions.randomType.alphaNumeric);

                    char_array = new char[random_string.Length + random_string.Length];

                    for (int int_count = 0; int_count < random_string.Length; int_count++)
                    {
                        char_array[int_count + int_count] = random_string[int_count];
                        char_array[int_count + int_count + 1] = " "[0];
                    }

                    context.Session.Add("vCode", random_string);

                    using (obj_font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel))
                    {
                        obj_graphics.DrawString(new string(char_array), obj_font, System.Drawing.Brushes.MidnightBlue, 5, 0);

                        context.Response.ContentType = "image/GIF";

                        obj_bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);

                    }

                }
            }
        }
    }
    /*public static void generateCaptcha(HttpContext context, int length, randomType type, int width, int height, System.Drawing.Color top_color, System.Drawing.Color bottom_color, System.Drawing.Color text_color)
    {
        System.Drawing.Drawing2D.HatchBrush hatchBrush = null;

        string embedded_string = commonFunctions.generateRandom(length, type);

        commonVariables.SetSessionVariable("vCode", commonEncryption.encrypting(embedded_string));

        char[] char_array = null;

        char_array = new char[embedded_string.Length + embedded_string.Length];

        for (int int_count = 0; int_count < embedded_string.Length; int_count++)
        {
            char_array[int_count + int_count] = embedded_string[int_count];
            char_array[int_count + int_count + 1] = " "[0];
        }

        //Captcha String
        System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily("Comic Sans MS");
        // -  Generate Random
        //int randomsize = 5;
        Random random = new Random(DateTime.Now.Millisecond);

        // Create a new 32-bit bitmap image.
        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
        {
            // Create a graphics object for drawing.
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);

                // Fill in the background.
                using (hatchBrush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, top_color, bottom_color))
                {
                    g.FillRectangle(hatchBrush, rect);

                    // Set up the text font.
                    System.Drawing.SizeF size = default(System.Drawing.SizeF);
                    float fontSize = rect.Height + 25;
                    System.Drawing.Font font = null;
                    System.Drawing.StringFormat format = new System.Drawing.StringFormat();
                    format.Alignment = System.Drawing.StringAlignment.Center;
                    format.LineAlignment = System.Drawing.StringAlignment.Center;

                    // Adjust the font size until the text fits within the image.
                    do
                    {
                        fontSize -= 5;
                        font = new System.Drawing.Font(fontFamily, fontSize, System.Drawing.FontStyle.Bold);
                        size = g.MeasureString(new string(char_array), font, new System.Drawing.SizeF(width, height), format);
                    } while (size.Width > rect.Width);

                    // Create a path using the text and warp it randomly.
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddString(new string(char_array), font.FontFamily, Convert.ToInt32(font.Style), font.Size, rect, format);

                    float v = 6f;
                    System.Drawing.PointF[] points = {
                                                         new System.Drawing.PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                                                         new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                                                         new System.Drawing.PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                                                         new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
                                                     };
                    System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                    matrix.Translate(0f, 0f);
                    path.Warp(points, rect, matrix, System.Drawing.Drawing2D.WarpMode.Perspective, 0f);

                    // Draw the text.
                    using (hatchBrush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal, text_color, text_color))
                    {
                        g.FillPath(hatchBrush, path);
                    }

                    font.Dispose();
                }
            }

            context.Response.ContentType = "image/png";
            bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
        }
    }*/
    public static string generateCaptcha(HttpContext context, int length, randomType type, int width, int height, System.Drawing.Color top_color, System.Drawing.Color bottom_color, System.Drawing.Color text_color)
    {
        System.Drawing.Drawing2D.HatchBrush hatchBrush = null;

        string strCode = commonFunctions.generateRandom(length, type);

        commonVariables.SetSessionVariable("vCode", commonEncryption.encrypting(strCode));
        //context.Session["vCode"] = commonEncryption.encrypting(strCode);

        char[] char_array = null;

        char_array = new char[strCode.Length + strCode.Length];

        for (int int_count = 0; int_count < strCode.Length; int_count++)
        {
            char_array[int_count + int_count] = strCode[int_count];
            char_array[int_count + int_count + 1] = " "[0];
        }

        //Captcha String
        System.Drawing.FontFamily fontFamily = new System.Drawing.FontFamily("Comic Sans MS");
        // -  Generate Random
        //int randomsize = 5;
        Random random = new Random(DateTime.Now.Millisecond);

        // Create a new 32-bit bitmap image.
        using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
        {
            // Create a graphics object for drawing.
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, width, height);

                // Fill in the background.
                using (hatchBrush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, top_color, bottom_color))
                {
                    g.FillRectangle(hatchBrush, rect);

                    // Set up the text font.
                    System.Drawing.SizeF size = default(System.Drawing.SizeF);
                    float fontSize = rect.Height + 25;
                    System.Drawing.Font font = null;
                    System.Drawing.StringFormat format = new System.Drawing.StringFormat();
                    format.Alignment = System.Drawing.StringAlignment.Center;
                    format.LineAlignment = System.Drawing.StringAlignment.Center;

                    // Adjust the font size until the text fits within the image.
                    do
                    {
                        fontSize -= 5;
                        font = new System.Drawing.Font(fontFamily, fontSize, System.Drawing.FontStyle.Bold);
                        size = g.MeasureString(new string(char_array), font, new System.Drawing.SizeF(width, height), format);
                    } while (size.Width > rect.Width);

                    // Create a path using the text and warp it randomly.
                    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                    path.AddString(new string(char_array), font.FontFamily, Convert.ToInt32(font.Style), font.Size, rect, format);

                    float v = 6f;
                    System.Drawing.PointF[] points = {
                                                         new System.Drawing.PointF(random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                                                         new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v, random.Next(rect.Height) / v),
                                                         new System.Drawing.PointF(random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v),
                                                         new System.Drawing.PointF(rect.Width - random.Next(rect.Width) / v, rect.Height - random.Next(rect.Height) / v)
                                                     };
                    System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                    matrix.Translate(0f, 0f);
                    path.Warp(points, rect, matrix, System.Drawing.Drawing2D.WarpMode.Perspective, 0f);

                    // Draw the text.
                    using (hatchBrush = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal, text_color, text_color))
                    {
                        g.FillPath(hatchBrush, path);
                    }

                    font.Dispose();
                }
            }

            context.Response.ContentType = "image/png";
            bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
            return strCode;
        }
    }
    #endregion

    #region schemas
    public static string getSchemaPath()
    {
        string filename = System.IO.Path.GetFileName(HttpContext.Current.Request.Path);

        string folderpath = string.Empty;
        string returnpath = string.Empty;

        folderpath = HttpContext.Current.Server.MapPath(".");

        returnpath = folderpath + @"\schemas\" + filename + ".xsd";

        return returnpath;
    }
    public static string getSchemaPath(string mapPath)
    {
        string fileName = System.IO.Path.GetFileName(HttpContext.Current.Request.Path);

        string folderPath = string.Empty;
        string returnPath = string.Empty;

        if (string.IsNullOrEmpty(mapPath))
        {
            folderPath = HttpContext.Current.Server.MapPath(".");
        }
        else
        {
            folderPath = HttpContext.Current.Server.MapPath(mapPath);
        }

        returnPath = folderPath + @"\schemas\" + fileName + ".xsd";

        return returnPath;
    }
    public static string getSchemaPath(string mapPath, string fileName)
    {
        string folderPath = string.Empty;
        string returnPath = string.Empty;

        if (string.IsNullOrEmpty(mapPath))
        {
            folderPath = HttpContext.Current.Server.MapPath(".");
        }
        else
        {
            folderPath = HttpContext.Current.Server.MapPath(mapPath);
        }

        if (string.IsNullOrEmpty(fileName))
        {
            fileName = System.IO.Path.GetFileName(HttpContext.Current.Request.Path);
        }

        returnPath = folderPath + @"\schemas\" + fileName + ".xsd";

        return returnPath;

    }
    public static string getSchemaPath(string fileName, bool root)
    {
        string returnPath = string.Empty;
        string appPath = System.AppDomain.CurrentDomain.BaseDirectory;

        if (string.IsNullOrEmpty(fileName))
        {
            fileName = System.IO.Path.GetFileName(HttpContext.Current.Request.Path);
        }

        returnPath = appPath + @"\schemas\" + fileName + ".xsd";

        return returnPath;
    }
    #endregion

    public enum randomType
    {
        alphaNumeric,
        numeric,
        alpha
    }
}