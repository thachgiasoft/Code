using DevExpress.Web;
using DevExpress.Web.Demos;
using DevExpress.Web.Internal;
using EIS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EIS.FEW.Helper
{
    public class UploadControlHelper
    {
        public const string LocalHost = "~/eis/Content/UploadControl/UploadFolder/";
        public const string UploadDirectory = "~/Content/UploadControl/UploadFolder/";
        
        public static readonly UploadControlValidationSettings UploadValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".xml", ".xls", ".xlsx" },
            MaxFileSize = 83886080,
            NotAllowedFileExtensionErrorText = "Văn bản không đúng định dạng yêu cầu!",
            GeneralErrorText = "Có lỗi trong quá trình upload văn bản!"
        };
        public static readonly UploadControlValidationSettings UploadValidationSettings_format_impor = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".bmp", ".png", ".pdf", ".xml", ".rar", ".zip" },
            MaxFileSize = 83886080,
            NotAllowedFileExtensionErrorText = "Văn bản không đúng định dạng yêu cầu!",
            GeneralErrorText = "Có lỗi trong quá trình upload văn bản!"
        };
        public static void ucMultiSelection_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            string resultFileName = Path.GetRandomFileName() + "_" + e.UploadedFile.FileName;
            string resultFileUrl = UploadDirectory + resultFileName;
            //Custom host show file
            string hostFileUrl = LocalHost + resultFileName;
            string resultFilePath = HttpContext.Current.Request.MapPath(resultFileUrl);
            e.UploadedFile.SaveAs(resultFilePath);

            //UploadingUtils.RemoveFileWithDelay(resultFileName, resultFilePath, 5);

            IUrlResolutionService urlResolver = sender as IUrlResolutionService;
            if (urlResolver != null)
            {
                string url = urlResolver.ResolveClientUrl(hostFileUrl);
                e.CallbackData = GetCallbackData(e.UploadedFile, url);
            }
            if (System.Web.HttpContext.Current.Session["URL"] == null)
	        {
                System.Web.HttpContext.Current.Session["URL"] = resultFilePath;
	        }
            else
            {
                System.Web.HttpContext.Current.Session["URL"] += '|' + resultFilePath;
            }
            
        }
        static string GetCallbackData(UploadedFile uploadedFile, string fileUrl)
        {
            string name = uploadedFile.FileName;
            long sizeInKilobytes = uploadedFile.ContentLength / 1024;
            string sizeText = sizeInKilobytes.ToString() + " KB";

            return name + "|" + fileUrl + "|" + sizeText;
        }
    }
}