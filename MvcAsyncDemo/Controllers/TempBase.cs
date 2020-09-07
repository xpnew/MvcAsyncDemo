
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace MvcAsyncDemo.Controllers
{
    public class TempBase : Controller
    {

        protected async Task<string> GetWebAsync(string urserid)
        {
            return await GetAsync(GetServerHost() + "/BaseAwait/GetUserInfo?id=" + urserid, "");
        }
        public ActionResult GetUserInfo()
        {
            string UserId = "jack";
            if (!String.IsNullOrEmpty(Request["id"]))
            {
                UserId = Request["id"];
            }
            return SendHtml("user is " + UserId + " !");



        }


        protected string GetServerHost()
        {

            return "http://" + Request.Url.Host + ":" + Request.Url.Port;
        }

        protected async Task LargeWaitAsync()
        {
            string ss = "";
            Say("\t\tLargeWaitAsync start ");
            Say("\t\t线程id: " + Thread.CurrentThread.ManagedThreadId);
            var GoodReturn = Get(GetServerHost() + "/BaseAwait/GetUserInfo", "");
            await Task.Run(() =>
            {
                Say("\t\t\tLargeWaitAsync内部 await 异步任务 ");
                System.Threading.Thread.Sleep(6100);
                ss += " await ";
                Say("\t\t\tLargeWaitAsync内部  await 异步任务 end ");
            }).ConfigureAwait(false);

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


        public Task BuildBGAsync(Action a)
        {
            return Task.Factory.StartNew(a, TaskCreationOptions.HideScheduler);
        }
        //public Task<TResult> BuildBGAsync(Func<TResult> function)
        //{
        //  var aa = Task.Factory.StartNew(function, TaskCreationOptions.HideScheduler);

        //    return  aa;

        //}


        public ActionResult SendHtml(string html)
        {
            ContentResult result = new ContentResult();
            result.ContentType = "";
            result.ContentEncoding = UTF8Encoding.UTF8;
            result.Content = html;
            return result;
        }

        public static string Get(string Url, string postDataStr = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static async Task<string> GetAsync(string Url, string postDataStr = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            var p = await request.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse)p;
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        protected void Say(string msg)
        {

            x.Say(msg);

            LogHelper.Self.Debuglog(msg);

        }

    }

    public class x
    {

        public static void Say(string msg)
        {
            Trace.WriteLine(msg);


        }
    }




}