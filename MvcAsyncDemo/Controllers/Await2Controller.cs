using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Threading;



namespace MvcAsyncDemo.Controllers
{
    public class Await2Controller : TempBase
    {
        // GET: Await2
        public ActionResult Index()
        {
            Say("控制器UI Await2Controller ");
            Say("控制器UI Index： start ");
            Say("线程id: " + Thread.CurrentThread.ManagedThreadId);

            Dowork2();

            Say("控制器UI Index： end  ");
            return SendHtml("ok");
        }

        protected async Task Dowork2()
        {
            string ss = "";
            //await Task.Delay(5000);
            Say("\tDowork2 start ");
            Say("\t线程id: " + Thread.CurrentThread.ManagedThreadId);

            //▲▲▲ Task.Run().ConfigureAwait(false); 
            //▲▲▲ await LargeWaitAsync().ConfigureAwait(false); 
            //▲▲▲ ↑↑↑二选一，至少在方法当中使用一次 ConfigureAwait(false)
            //▲▲▲ 否则会出现上下文丢失，出现“空引用”异常 
            await Task.Run(() =>
            {
                Say("\t\tDowork2 内部 await 异步任务 ");
                System.Threading.Thread.Sleep(6100);
                Say("\t\tDowork2 内部 await 异步任务 end ");
            }).ConfigureAwait(false);

            await LargeWaitAsync().ConfigureAwait(false);
            var rr = await GetReturn2("abc");
            Say("\tDowork2 异步获取结果： " + rr);
            //运行正常，下面这句可以在日志当中看到
            Say("\tDowork2 end ");
        }
       
        protected async Task<string> GetReturn2(string urserid)
        {

            Say("\t\tawait 异步任务GetReturn2 ");
            Say("\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);
            string ss = "";
            var Result = await Task.Run(async () =>
            {
                Say("\t\t\tGetReturn2 内部 await 子异步任务 ");
                Say("\t\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);
                //System.Threading.Thread.Sleep(70);
                ss = await GetWebAsync(urserid).ConfigureAwait(false); 
                ss += " await ";
                Say("\t\t\tGetReturn2 内部 await 异步任务 end ");
                return ss;
            }).ConfigureAwait(false);
            return Result;
        }

   

    }
}