using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Common.Models;

namespace Common.Helper
{
    public static class ExtensionHelper
    {
        private readonly static string _originalWith = ConfigurationManager.AppSettings["UploadImageOriginalWidth"] ?? "1024";
        private readonly static string _originalHeight = ConfigurationManager.AppSettings["UploadImageOriginalHeight"] ?? "768";
        private readonly static string _thumbnailWith = ConfigurationManager.AppSettings["UploadImageThumbnailWidth"] ?? "218";
        private readonly static string _thumbnailHeight = ConfigurationManager.AppSettings["UploadImageThumbnailHeight"] ?? "138";
        private readonly static string _iconHeight = ConfigurationManager.AppSettings["UploadIconHeight"] ?? "80";
        private readonly static string _iconWidth = ConfigurationManager.AppSettings["UploadIconWidth"] ?? "80";
        /// <summary>
        /// 去除HTML字符串中的特殊字符，获取纯文本
        /// </summary>
        /// <param name="oldString">HTML字符串</param>
        /// <returns></returns>
        public static string TrimHTML(this string oldString)
        {
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = regex.Replace(oldString, "");
            strOutput = strOutput.Replace("&nbsp;", " ");
            return strOutput;
        }

        public static string DecodeHtmlString(this string htmlEncode)
        {
            return HttpUtility.HtmlDecode(htmlEncode);
        }
        /// <summary>
        /// 获取html字符串中的img元素
        /// </summary>
        /// <param name="imgString">HTML字符串</param>
        /// <returns></returns>
        public static string GetImgElement(this string imgString)
        {
            Regex regex = new Regex("<img[^>]+>", RegexOptions.IgnoreCase);
            Match mat = regex.Match(imgString);
            return mat.Value;
        }
        /// <summary>
        /// 获取HTML中img的src路径
        /// </summary>
        /// <param name="imgString">HTML字符串</param>
        /// <returns></returns>
        public static string GetImgSrc(this string imgString)
        {
            string imgElement = GetImgElement(imgString);
            string result = string.Empty;
            Regex regex = new Regex("(?<=src=['|\"])\\S+(?=['|\"])", RegexOptions.IgnoreCase);
            Match mat = regex.Match(imgElement);
            if (mat.Success)
            {
                result = mat.Value;
            }
            return result;
        }

        /// <summary>
        /// Add the image prefix to html image element
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static string ReplaceHtmlImagePath(this string htmlString, string prefix)
        {
            if (!string.IsNullOrEmpty(htmlString))
            {
                string replaceString = Regex.Replace(htmlString, "(?<=src=['|\"](?!http|https))\\S+(?=['|\"])", prefix + "$0",
               RegexOptions.Compiled | RegexOptions.IgnoreCase);
                return replaceString;
            }
            return "";
        }

        public static string ReplaceHtmlImagePath(this string htmlString)
        {
            string prifx = "/";
            return ReplaceHtmlImagePath(htmlString, prifx);
        }

        /// <summary>
        /// Get SingleChoice quesiotn answer
        /// </summary>
        /// <param name="oldString"></param>
        /// <returns></returns>
        public static string GetChoiceAnswer(string oldString)
        {
            string answer = string.Empty;
            string plainText = TrimHTML(oldString);

            string regex = ConfigurationManager.AppSettings["RegexSingleChoiceAnswer"];
            regex = HttpUtility.HtmlDecode(regex);
            MatchCollection collection = Regex.Matches(plainText, regex, RegexOptions.IgnoreCase);
            foreach (Match col in collection)
            {
                if (col.Success)
                {
                    answer += col.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(answer))
            {
                answer = answer.Remove(answer.LastIndexOf(","));
            }
            return answer;
        }

        public static string GetCustomQuestionOperator(string oldstring)
        {
            string plainText = TrimHTML(oldstring);

            string regex = ConfigurationManager.AppSettings["RegexCustomOperator"];
            regex = HttpUtility.HtmlDecode(regex);
            MatchCollection collection = Regex.Matches(plainText, regex, RegexOptions.IgnoreCase);
            if (collection.Count > 0)
            {
                return collection[0].Value;
            }
            else
            {
                return "";
            }
        }

        public static string GetCustomQuestionAnswer(string oldString)
        {
            string answer = string.Empty;
            string plainText = TrimHTML(oldString);

            string regex = ConfigurationManager.AppSettings["RegexCustomAnswer"];
            regex = HttpUtility.HtmlDecode(regex);
            MatchCollection collection = Regex.Matches(plainText, regex, RegexOptions.IgnoreCase);
            foreach (Match col in collection)
            {
                if (col.Success)
                {
                    answer += col.Value + ",";
                }
            }
            if (!string.IsNullOrEmpty(answer))
            {
                answer = answer.Remove(answer.LastIndexOf(","));
            }
            return answer;
        }

        public static string RemoveRegexForSingleChoice(string body)
        {
            string regex = ConfigurationManager.AppSettings["RegexRemoveSingleChoice"];
            string regexAnswer = ConfigurationManager.AppSettings["RegexRemoveSingleChoiceAnswer"];
            regex = HttpUtility.HtmlDecode(regex);
            string newBody = Regex.Replace(body, regex, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            newBody = Regex.Replace(newBody, regexAnswer, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return newBody;
        }

        public static string RemoveRegexAnswerForSingleChoice(string body)
        {
            string regexAnswer = ConfigurationManager.AppSettings["RegexRemoveSingleChoiceAnswer"];
            string newBody = body.Replace("##", "");
            newBody = Regex.Replace(newBody, regexAnswer, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return newBody;
        }


        public static string RemoveRegexForMultiChoice(string body)
        {
            string regex = ConfigurationManager.AppSettings["RegexRemoveMultipleChoice"];
            string regexAnswer = ConfigurationManager.AppSettings["RegexRemoveMultipleChoiceAnswer"];
            regex = HttpUtility.HtmlDecode(regex);
            string newBody = Regex.Replace(body, regex, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            newBody = Regex.Replace(newBody, regexAnswer, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return newBody;
        }

        public static string RemoveRegexAnswerForMultiChoice(string body)
        {
            string regexAnswer = ConfigurationManager.AppSettings["RegexRemoveMultipleChoiceAnswer"];
            string newBody = body.Replace("##", "");
            newBody = Regex.Replace(newBody, regexAnswer, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return newBody;
        }

        public static string RemoveRegexForFillBlank(string body)
        {
            string regex = ConfigurationManager.AppSettings["RegexRemoveFillBlank"];
            regex = HttpUtility.HtmlDecode(regex);
            string newBody = Regex.Replace(body, regex, "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return newBody.Replace("()", "____");
        }

        public static string MonthToSimpleString(this string monthString)
        {
            var m = "";
            switch (monthString)
            {
                case "1":
                    m = "Jan";
                    break;
                case "2":
                    m = "Feb";
                    break;
                case "3":
                    m = "Mar";
                    break;
                case "4":
                    m = "Apr";
                    break;
                case "5":
                    m = "May";
                    break;
                case "6":
                    m = "Jun";
                    break;
                case "7":
                    m = "Jul";
                    break;
                case "8":
                    m = "Aug";
                    break;
                case "9":
                    m = "Sep";
                    break;
                case "10":
                    m = "Oct";
                    break;
                case "11":
                    m = "Nov";
                    break;
                case "12":
                    m = "Dec";
                    break;
            }
            return m;
        }

       
        public static Stream MakeThumbnail(Stream originalImageStream, Stream thumbStream, ThumbType thumbType, int width, int height, System.Drawing.Imaging.ImageFormat format, bool isMakeThmb = true)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(originalImageStream);

            int towidth = width;
            int toheight = height;
            string borderColor = "#FFFFFF";
            int desWidth = width;
            int desHeight = height;
            int penwidth = 0;
            int penhight = 0;
            int top = 0, left = 0;
            int twidth = 0, theight = 0;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            if (!isMakeThmb)
            {
                width = ow;
                height = oh;
                desWidth = width;
                desHeight = height;
                towidth = ow;
                toheight = oh;
            }
            switch (thumbType)
            {
                case ThumbType.HW:
                    break;
                case ThumbType.W:
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbType.H:
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbType.All:
                    string hw = GetImageSize(ow, oh, width, height);
                    string[] aryhw = hw.Split(';');
                    twidth = System.Convert.ToInt32(aryhw[0]);
                    theight = System.Convert.ToInt32(aryhw[1]);
                    if (twidth < desWidth & theight == desHeight)
                    {
                        penwidth = desWidth - twidth;
                    }
                    else if (twidth == desWidth && theight < desHeight)
                    {
                        penhight = desHeight - theight;
                    }
                    else if (twidth < desWidth && theight < desHeight)
                    {
                        penwidth = desWidth - twidth;
                        penhight = desHeight - theight;
                    }
                    top = penhight / 2;
                    left = penwidth / 2;

                    break;
                case ThumbType.CUT:
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(System.Drawing.Color.White);

            if (thumbType == ThumbType.All)
            {
                g.DrawImage(originalImage, new System.Drawing.Rectangle(left, top, twidth, theight), new System.Drawing.Rectangle(0, 0, ow, oh), System.Drawing.GraphicsUnit.Pixel);
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.ColorTranslator.FromHtml(borderColor));
                //g.DrawRectangle(pen, 0, 0, desWidth - 2, desHeight - 2);
                g.DrawRectangle(pen, 0, 0, desWidth, desHeight);
            }
            else
            {
                //在指定位置并且按指定大小绘制原图片的指定部分 
                g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                        new System.Drawing.Rectangle(x, y, ow, oh),
                        System.Drawing.GraphicsUnit.Pixel);
            }

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbStream, format);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            return thumbStream;
        }

        //获取缩略图的高与宽
        private static string GetImageSize(int loadImgW, int loadImgH, int width, int height)
        {
            int xh = 0;
            int xw = 0;
            //容器高与宽
            int oldW = width;
            int oldH = height;
            //图片的高宽与容器的相同
            if (loadImgH == oldH && loadImgW == (oldW))
            {//1.正常显示 
                xh = loadImgH;
                xw = loadImgW;
            }
            if (loadImgH == oldH && loadImgW > (oldW))
            {//2、原高==容高，原宽>容宽 以原宽为基础 
                xw = (oldW);
                xh = loadImgH * xw / loadImgW;
            }
            if (loadImgH == oldH && loadImgW < (oldW))
            {//3、原高==容高，原宽<容宽  正常显示    
                xw = loadImgW;
                xh = loadImgH;
            }
            if (loadImgH > oldH && loadImgW == (oldW))
            {//4、原高>容高，原宽==容宽 以原高为基础    
                xh = oldH;
                xw = loadImgW * xh / loadImgH;
            }
            if (loadImgH > oldH && loadImgW > (oldW))
            {//5、原高>容高，原宽>容宽            
                if ((loadImgH / oldH) > (loadImgW / (oldW)))
                {//原高大的多，以原高为基础 
                    xh = oldH;
                    xw = loadImgW * xh / loadImgH;
                }
                else
                {//以原宽为基础 
                    xw = (oldW);
                    xh = loadImgH * xw / loadImgW;
                }
            }
            if (loadImgH > oldH && loadImgW < (oldW))
            {//6、原高>容高，原宽<容宽 以原高为基础         
                xh = oldH;
                xw = loadImgW * xh / loadImgH;
            }
            if (loadImgH < oldH && loadImgW == (oldW))
            {//7、原高<容高，原宽=容宽 正常显示        
                xh = loadImgH;
                xw = loadImgW;
            }
            if (loadImgH < oldH && loadImgW > (oldW))
            {//8、原高<容高，原宽>容宽 以原宽为基础     
                xw = (oldW);
                xh = loadImgH * xw / loadImgW;
            }
            if (loadImgH < oldH && loadImgW < (oldW))
            {//9、原高<容高，原宽<容宽//正常显示     
                xh = loadImgH;
                xw = loadImgW;
            }
            return xw + ";" + xh;
        }

        public enum ThumbType
        {
            /// <summary>
            /// 指定高宽缩放（可能变形）  
            /// </summary>
            HW,
            /// <summary>
            /// 指定宽，高按比例   
            /// </summary>
            H,
            /// <summary>
            /// 指定高，宽按比例 
            /// </summary>
            W,
            /// <summary>
            /// 指定高宽裁减（不变形）  
            /// </summary>
            CUT,
            All
        }

        public static string GetDomain(this HttpRequest request)
        {
            return string.Format("{0}://{1}/", request.Url.Scheme, request.UrlReferrer != null ? request.UrlReferrer.Authority : request.Url.Authority);
        }

        public static Uri GetBaseUri(this HttpRequest request)
        {
            return new Uri(string.Format("{0}://{1}/", request.Url.Scheme, request.UrlReferrer != null ? request.UrlReferrer.Authority : request.Url.Authority));
        }

        public static string GetDomain(this HttpRequestBase request)
        {
            return request.Url != null ? string.Format("{0}://{1}/", request.Url.Scheme, request.Url.Authority) : string.Empty;
        }

        public static decimal FormatDoubleToThreeDecimalPoint(this double value)
        {
            return Decimal.Round((decimal)value, 3, MidpointRounding.AwayFromZero);
        }

        public static decimal FormatThreeDecimalPoint(this decimal value)
        {
            return Decimal.Round(value, 3, MidpointRounding.AwayFromZero);
        }

        public static decimal FormatFiveDecimalPoint(this decimal value)
        {
            return Decimal.Round(value, 5, MidpointRounding.AwayFromZero);
        }
    }
}
