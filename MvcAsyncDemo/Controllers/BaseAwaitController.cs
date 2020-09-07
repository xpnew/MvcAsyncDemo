using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcAsyncDemo.Controllers
{
    public class BaseAwaitController : TempBase
    {
        // GET: BaseAwait
        public ActionResult Index()
        {
            x.Say("控制器： start ");
            x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);

            TestReturn2();
            return SendHtml("ok");
        }


        public async Task<string> Get()
        {
            string AAA = "AAA";

            string ss = await GetArticleContentAsync();
            string ss2 = await Good2();
            TestReturn2();
            return AAA;
        }
        private async Task<string> GetArticleContentAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://www.cnblogs.com/rosanshao/p/3728108.html");
                var buffer = await response.Content.ReadAsByteArrayAsync();
                return Encoding.UTF8.GetString(buffer);
            }
        }

        private async Task<string> Good2()
        {
            string ss = "";
            await Task.Run(() =>
            {
                x.Say("Dowork2 await 异步任务 ");
                System.Threading.Thread.Sleep(6100);
                ss += " await ";
                x.Say("ss:" + ss);
                x.Say("Dowork2 await 异步任务 end ");
            }).ConfigureAwait(false);
            return ss;
        }



        protected async Task TestReturn2()
        {
            x.Say("TestReturn2  start ");
            x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);

            x.Say("TestReturn2 同步任务开始  ");
            //await Task.Run(() =>
            //{
            //    x.Say("TestReturn2  额外线程 start ");
            //    x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
            //    x.Say("TestReturn2 await 异步任务 ");
            //    System.Threading.Thread.Sleep(6100);
                
            //    x.Say("TestReturn2 await 异步任务 end ");
            //});


            await Dowork2();//.ConfigureAwait(false);

            var ss2 = await GetReturn3("232332");
            x.Say("TestReturn2 同步任务结束  ");


        }
        private async Task Dowork2()
        {
            string ss = "";
            //await Task.Delay(5000);
            //System.Threading.Thread.Sleep(1000);
            x.Say("Dowork2 start ");
            x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
            //Task.Run(() =>
            //{
            //    x.Say("Dowork2 无await 异步任务 ");
            //    System.Threading.Thread.Sleep(8100);
            //    x.Say("Dowork2 无await  异步任务 end ");
            //});

            var GoodReturn = Get("http://localhost:51191/BaseAwait/GetUserInfo", "");
            x.Say("GoodReturn" + GoodReturn);
            await Task.Run(() =>
            {
                x.Say("Dowork2 await 异步任务 ");
                System.Threading.Thread.Sleep(6100);
                ss += " await ";
                x.Say("ss:" + ss);
                x.Say("Dowork2 await 异步任务 end ");
            }).ConfigureAwait(false);

            //await GetReturn2();
            try
            {
                var rr = await GetReturn("abc");
                ss += rr;

            }
            catch (InvalidOperationException ex)
            {
                x.Say("异步任务异常：" + ex);
            }
            catch (Exception ex)
            {
                x.Say("异步任务异常：" + ex);
            }
            ss += " end ";

            x.Say("Dowork2 end ");
            x.Say("ss:" + ss);


            //Task.Run(() =>
            //{
            //    x.Say("Dowork2 无await 异步任务2 ");
            //    System.Threading.Thread.Sleep(1000);
            //    x.Say("Dowork2 无await  异步任务2 end ");
            //});


        }

        protected async Task GetReturn2()
        {
            x.Say("Dowork2 await 异步任务GetReturn2 ");
            x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
            //await Task.Delay(1000);
            x.Say("Dowork2 await 异步任务GetReturn2 end ");
        }
        [AsyncTimeout(100000)]
        protected async Task<string> GetReturn(string urserid)
        {

            x.Say("Dowork2 await 异步任务GetReturn ");
            x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
            string ss = "";
            var Result = await Task.Run(async () =>
            {
                x.Say("Dowork2 await 异步任务 ");
                x.Say("线程id: " + Thread.CurrentThread.ManagedThreadId);
                //System.Threading.Thread.Sleep(70);
                //ss = Get("http://localhost:51191/BaseAwait/GetUserInfo", "");
                ss = await GetReturn3("aaa");
                ss += " await ";
                x.Say("ss:" + ss);
                x.Say("Dowork2 await 异步任务 end ");
                return ss;

            }).ConfigureAwait(false);

            //var Result = await Task.Run(() => { return ss; });



            return Result;
        }

        protected async Task<string> GetReturn3(string urserid)
        {
            return await GetAsync("http://localhost:51191/BaseAwait/GetUserInfo", "");
        }

        protected async Task Good555() { }


     

        public ActionResult Error()
        {
            int a = 1;
            if (0 < a)
            {
                throw new Exception("Test ...");
            }

            return View();
        }

    }
}