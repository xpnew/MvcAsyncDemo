using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcAsyncDemo.Controllers
{

    /// <summary>
    /// 精简的模式，只是为了验证异常情况会出现
    /// </summary>
    public class MiniAwaitController : TempBase
    {
        public ActionResult Index()
        {
            Say("控制器UI Index： start ");
            Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
            Dowork();
            Say("控制器UI Index： end  ");
            return SendHtml("ok");
        }

   
        private async Task Dowork()
        {
            //这里的线程ID和控制器UI是一样的。
            Say("Dowork  线程id: " + Thread.CurrentThread.ManagedThreadId);
            await Task.Run(() =>
            {
                Say("Dowork  Task.Run 线程id: " + Thread.CurrentThread.ManagedThreadId);
                Say("Dowork await 异步任务 ");
                //以下三种方式任意一种，都会出现异常
                System.Threading.Thread.Sleep(1000);
                //string ss = Get("http://localhost:51191/BaseAwait/GetUserInfo", "");
                //for (long i = 0; i < 9999; i++)
                //{
                //    if (0 == i % 2000)
                //    {
                //        Say("i: " + i);
                //    }
                //}
                Say("Dowork await 异步任务 end ");
            }); //.ConfigureAwait(false);
            //这里不会执行
            Say("Dowork 同步任务结束  ");
        }
    }
}