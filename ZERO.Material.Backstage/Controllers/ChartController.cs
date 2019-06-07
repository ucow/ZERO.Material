using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ZERO.Material.Model.Other;

namespace ZERO.Material.Backstage.Controllers
{
    public class ChartController : Controller
    {
        #region 全局变量

        private readonly ITypeBll _typeBll = UnityContainerHelper.Server<ITypeBll>();
        private readonly IBaseTypeBll _baseTypeBll = UnityContainerHelper.Server<IBaseTypeBll>();
        private readonly IUseApplyBll _userApplyBll = UnityContainerHelper.Server<IUseApplyBll>();
        private readonly ITeacherBll _teacherBll = UnityContainerHelper.Server<ITeacherBll>();

        #endregion 全局变量

        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }

        #region 类别数量统计

        public ActionResult TypeChart()
        {
            return View();
        }

        [HttpPost]
        public string GetTypeChartData()
        {
            List<Material_Type> materialTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == "000000");

            List<string> xAxisData = materialTypes.Select(m => m.Material_Type_Name).ToList();
            List<int> yAxisData = new List<int>();
            foreach (Material_Type materialType in materialTypes)
            {
                yAxisData.Add(GetTypeNumber(materialType.Material_Type_Id));
            }

            var dataJson = new
            {
                xAxis = xAxisData,
                yAxis = yAxisData
            };

            return JsonConvert.SerializeObject(dataJson);
        }

        #endregion 类别数量统计

        #region 展示借用数量前十

        public ActionResult ApplyMost()
        {
            return View();
        }

        [HttpPost]
        public string GetApplyMost()
        {
            List<ApplyMostModel> result = _userApplyBll.ExecuteSqlCommand<ApplyMostModel>(
                "  select top(10) Material_Name as [Name], sum(Apply_Count) as [Count] from  [ZERO_Material].[dbo].[Use_Apply] group by Material_Name order by [Count] desc");

            var dataJson = new
            {
                xAxis = result.Select(m => m.Name).ToList(),
                yAxis = result.Select(m => m.Count).ToList()
            };
            return JsonConvert.SerializeObject(dataJson);
        }

        #endregion 展示借用数量前十

        #region 教师日申请数量

        public ActionResult TeacherApply()
        {
            return View();
        }

        [HttpPost]
        public string GetTeacherApply()
        {
            Dictionary<string, List<TeacherApplyModel>> resultDictionary = new Dictionary<string, List<TeacherApplyModel>>();
            List<string> teacherNames = _userApplyBll.ExecuteSqlCommand<string>("select top(5) Teacher_Name from[ZERO_Material].[dbo].[Use_Apply] group by Teacher_Name order by sum(Apply_Count) desc");

            foreach (string teacherName in teacherNames)
            {
                List<TeacherApplyModel> result = _userApplyBll.ExecuteSqlCommand<TeacherApplyModel>(
                    $"select Apply_Time as ApplyTime,sum(Apply_Count) as [Count] from  [ZERO_Material].[dbo].[Use_Apply] where Teacher_Name = '{teacherName}' group by Apply_Time ");
                resultDictionary.Add(teacherName, result);
            }

            List<DateTime> applyTimes = new List<DateTime>();
            foreach (string key in resultDictionary.Keys)
            {
                resultDictionary[key].ForEach(m =>
                {
                    if (!applyTimes.Contains(m.ApplyTime))
                    {
                        applyTimes.Add(m.ApplyTime);
                    }
                });
            }
            applyTimes.Sort();
            foreach (string key in resultDictionary.Keys)
            {
                foreach (DateTime applyTime in applyTimes)
                {
                    if (resultDictionary[key].Count(m => m.ApplyTime.Date == applyTime.Date) == 0)
                    {
                        resultDictionary[key].Add(new TeacherApplyModel
                        {
                            ApplyTime = applyTime,
                            Count = 0
                        });
                    }
                }
            }

            List<string> legend = new List<string>();
            List<string> xAxis = new List<string>();
            Dictionary<string, List<int>> yAxises = new Dictionary<string, List<int>>();

            foreach (string key in resultDictionary.Keys)
            {
                legend.Add(key);
                resultDictionary[key].Sort((m, n) => m.ApplyTime.CompareTo(n.ApplyTime));
                foreach (TeacherApplyModel model in resultDictionary[key])
                {
                    if (!xAxis.Contains(model.ApplyTime.ToString("yyyy-MM-dd")))
                    {
                        xAxis.Add(model.ApplyTime.ToString("yyyy-MM-dd"));
                    }
                    if (yAxises.ContainsKey(key))
                    {
                        if (yAxises[key] == null)
                        {
                            yAxises[key] = new List<int>();
                        }
                    }
                    else
                    {
                        yAxises.Add(key, new List<int>());
                    }
                    yAxises[key].Add(model.Count);
                }
            }
            var dataJson = new
            {
                legend,
                xAxis,
                yAxis = yAxises
            };

            return JsonConvert.SerializeObject(dataJson);
        }

        #endregion 教师日申请数量

        #region 教师系别占比统计

        public ActionResult TeacherDepartStatis()
        {
            return View();
        }

        [HttpPost]
        public string GetTeacherDepartStatis()
        {
            List<TeacherDepartStatisModel> teacherDeparts = _teacherBll.ExecuteSqlCommand<TeacherDepartStatisModel>(
                "select  Teacher_Depart  as Depart,count(Teacher_Depart) as [Count] FROM [ZERO_Material].[dbo].[Material_Teacher] group by Teacher_Depart");
            List<string> legend = teacherDeparts.Select(m => m.Depart).ToList();
            var dataJson = new
            {
                legend,
                serise = teacherDeparts
            };
            return JsonConvert.SerializeObject(dataJson);
        }

        #endregion 教师系别占比统计

        #region 私有方法

        private int GetTypeNumber(string typeId)
        {
            int count = 0;
            List<Material_Type> materialTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == typeId);
            if (materialTypes.Count == 0)
            {
                return count + _baseTypeBll.GetEntities(m => m.Material_Type_Id == typeId).Count;
            }

            foreach (Material_Type materialType in materialTypes)
            {
                count += GetTypeNumber(materialType.Material_Type_Id);
            }

            return count;
        }

        #endregion 私有方法
    }
}