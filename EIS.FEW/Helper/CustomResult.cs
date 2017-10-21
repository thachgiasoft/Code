using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace EIS.FEW.HtmlHelpers
{
    public class CustomResult : ActionResult
    {
        public CustomResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        /// <summary>
        /// When set MaxJsonLength passed to the JavaScriptSerializer.
        /// </summary>
        public int? MaxJsonLength { get; set; }

        /// <summary>
        /// When set RecursionLimit passed to the JavaScriptSerializer.
        /// </summary>
        public int? RecursionLimit { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Not allowd get method");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            else
            {
                response.ContentEncoding = Encoding.UTF8;
            }
            if (Data != null)
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                };
                if (RecursionLimit.HasValue)
                {
                    jsonSettings.MaxDepth = RecursionLimit.Value;
                }

                var result = JsonConvert.SerializeObject(Data, Formatting.Indented, jsonSettings);
                response.Write(result);
            }
        }
    }
}