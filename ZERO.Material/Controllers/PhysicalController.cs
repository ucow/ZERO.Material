using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZERO.Material.Models;

namespace ZERO.Material.Controllers
{
    public class PhysicalController : Controller
    {
        // GET: Physical
        public ActionResult Index()
        {
            List<Physical> physicals = new List<Physical>();
            for (int i = 0; i < 10; i++)
            {
                physicals.Add(new Physical()
                {
                    Id = 100001,
                    Count = 100,
                    Name = "干冰",
                    Price = 3.25,
                    RemainCount = 50,
                    Remark = "nothing",
                    Unit = "KG"
                });
            }
            return View(physicals);
        }
    }
}