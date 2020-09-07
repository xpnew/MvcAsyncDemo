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
    /// 并列线程写法1，直接在指定异步在后台线程上执行 
    /// </summary>
    public class Await1Controller : TempBase
    {



        // GET: Await1
        public ActionResult Index()
        {
            Say("控制器UI Await1Controller ");

            Say("控制器UI Index： start ");
            Say("线程id: " + Thread.CurrentThread.ManagedThreadId);

            var task = Task.Factory.StartNew(() =>
            {
                Dowork1().ContinueWith(x =>
                {
                    //logging i done the work
                    if (x.IsFaulted) Say("异步出现异常：" + x.Exception);
                });
            }, TaskCreationOptions.LongRunning);

            //task.Start();
            Say("控制器UI Index： end  ");
            return SendHtml("ok");
        }

        //园友推荐的ContinueWith不能阻止上下文丢失的问题，相反会直接抛出异常
        public async Task<ActionResult> Get()
        {
            Say("控制器UI Await1Controller ");

            Say("控制器UI Get： start ");
            Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
            //DoworkAsync 不想堵塞
            // var t=**,no warning
            var t = Dowork1().ContinueWith(x =>
            {
                //logging i done the work
                if (x.IsFaulted) Say("异步出现异常：" + x.Exception);
            }).ConfigureAwait(false);
            Say("控制器UI Get： end  ");
            return await Task.FromResult(SendHtml("ok"));
        }

        protected async Task Dowork1()
        {
            string ss = "";
            await Task.Delay(5000);
            Say("\tDowork1 start ");
            Say("\t线程id: " + Thread.CurrentThread.ManagedThreadId);
            //await Task.Run(() =>
            //{
            //    Say("\t\tDowork1 await 异步任务 ");
            //    System.Threading.Thread.Sleep(6100);
            //    Say("\t\tDowork1 await 异步任务 end ");
            //}); //.ConfigureAwait(false)
            await BuildBGAsync(() =>
            {

                Say("\t\tDowork1 await 异步任务 ");
                Say("\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);
                System.Threading.Thread.Sleep(6100);
                Say("\t\tDowork1 await 异步任务 end ");

            });

            await LargeWaitAsync1();
            var rr = await GetReturn1("abc");
            //var ll = Task.Factory.StartNew<string>(() => { return ""; }, TaskCreationOptions.HideScheduler);
            Say("\tDowork1 异步获取结果： " + rr);
            //运行正常，下面这句可以在日志当中看到
            Say("\tDowork1 end ");
        }


        protected async Task<string> GetReturn1(string urserid)
        {

            Say("\t\tawait 异步任务GetReturn1 ");
            Say("\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);
            string ss = "";
            var Result = await Task.Run(async () =>
            {
                Say("\t\t\tGetReturn1 内部 await 异步任务 ");
                Say("\t\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);
                //System.Threading.Thread.Sleep(70);
                ss = await GetWebAsync(urserid);
                ss += " await ";
                Say("\t\t\ttGetReturn1 内部  await 异步任务 end ");
                return ss;
            });



            return Result;
        }




       


        protected async Task LargeWaitAsync1()
        {
            string ss = "";
            Say("\t\tLargeWaitAsync start ");
            Say("\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);

           
            var GoodReturn = Get(GetServerHost() + "/BaseAwait/GetUserInfo", "");
            //await Task.Run(() =>
            //{
            //    Say("\t\t\tLargeWaitAsync内部 await 异步任务 ");
            //    System.Threading.Thread.Sleep(6100);
            //    ss += " await ";
            //    Say("\t\t\tLargeWaitAsync内部  await 异步任务 end ");
            //});

            await Task.Run(() =>
            {
                Say("\t\t\tLargeWaitAsync内部 await 异步任务 ");
                System.Threading.Thread.Sleep(6100);
                ss += " await ";
                Say("\t\t\tLargeWaitAsync内部  await 异步任务 end ");
            });

            try
            {
                var rr = await GetWebAsync("def");
                ss += rr;
            }
            catch (InvalidOperationException ex)
            {
                Say("异步任务异常：" + ex);
            }
            catch (Exception ex)
            {
                Say("异步任务异常：" + ex);
            }
            ss += " end ";

            Say("\t\tLargeWaitAsync end ");

        }
    }
}