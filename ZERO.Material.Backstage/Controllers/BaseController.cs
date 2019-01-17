using System;
using System.Drawing;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;

namespace ZERO.Material.Backstage.Controllers
{
    public class BaseController<T> : Controller where T : class, new()
    {
        public string GetModelInfo()
        {
            return AssmblyHelper.GetDisplayAttributeInfo<T>();
        }

        public string UploadImage()
        {
            var imageFile = Request.Files[0];

            if (imageFile != null)
            {
                Image image = Image.FromStream(imageFile.InputStream);
                var msg = new
                {
                    code = 0,
                    msg = "",
                    data = ParseHelper.ParseImageToBytes(image)
                };
                return JsonConvert.SerializeObject(msg);
            }
            var errorMsg = new
            {
                code = 1,
                msg = "",
                data = ""
            };
            return JsonConvert.SerializeObject(errorMsg);
        }
    }
}