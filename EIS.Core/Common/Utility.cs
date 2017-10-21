using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace EIS.Core.Common
{
    public class Utility
    {
        public byte[] GetCaptchaImage(string checkCode)
        {

            Bitmap image = new Bitmap(Convert.ToInt32(Math.Ceiling((decimal)(checkCode.Length * 15))), 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Random random = new Random();
                Color col = ColorTranslator.FromHtml("#0093DD");
                g.Clear(col);
                Font font = new Font("Arial",12, FontStyle.Italic);
                string str = "";
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.FromArgb(0, 147, 221), Color.DarkRed, 1.2f, true);
                //for (int i = 0; i < checkCode.Length; i++)
                //{
                //    str = str + checkCode.Substring(i, 1);}
                g.DrawString(checkCode, font, new SolidBrush(Color.White), 3, 2);
                g.Flush();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();}
        }

        public byte[] VerificationTextGenerator()
        {

            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < 4; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                //if (temp != -1 && temp == t)
                //{
                //    VerificationTextGenerator();
                //}
                temp = t;
                randomCode += allCharArray[t];
            }
            //GetCaptchaImage(randomCode);
            HttpContext.Current.Session["Captcha"] = randomCode;
            return GetCaptchaImage(randomCode);
        }

    }
}