using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAsyncDemo
{
    public class ErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 在发生异常时触发调用
        /// </summary>
        public override void OnException(ExceptionContext filterContext)
        {
            //if (!filterContext.ExceptionHandled && filterContext.Exception is NullReferenceException)
            if (!filterContext.ExceptionHandled)
            {
                //获取出现异常的controller名和action名，用于记录
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                //定义一个HandErrorInfo，用于Error视图展示异常信息
                HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                string thisTime = DateTime.Now.ToShortDateString().Replace("/", "");
                string errorDetails = $"出错时间：{DateTime.Now.ToString()},错误发生在{model.ControllerName}控制器的{model.ActionName},错误类型：{model.Exception.Message}";
                string splitLine = "——————————————————————分割线——————————————————————";

                string LogDir = filterContext.HttpContext.Server.MapPath("~/ErrorTxt/");
                if (!System.IO.Directory.Exists(LogDir))
                {
                    System.IO.Directory.CreateDirectory(LogDir);
                }



                using (System.IO.StreamWriter file = new System.IO.StreamWriter(LogDir + thisTime, true))
                {
                    file.WriteLine(errorDetails);
                    file.WriteLine(model.Exception.StackTrace);
                    file.WriteLine(splitLine);

                }

                ViewResult result = new ViewResult
                {
                    ViewName = this.View,//设置异常时跳转的404页面
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model)  //定义ViewData，泛型
                };
                filterContext.Result = result;
                filterContext.ExceptionHandled = true;//设置异常已处理
            }

        }
    }
}