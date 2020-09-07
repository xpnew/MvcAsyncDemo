using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAsyncDemo
{
    public class LogHelper
    {


        #region 单例模式
        private static LogHelper _instance = new LogHelper();

        public static LogHelper Self
        {
            get
            {
                return _instance;
            }
        }

        private LogHelper() { }
        #endregion

        private object lockLog = new object(); //日志排他锁
        /// <summary>
        /// 调式日志，用于调式日志输出
        /// </summary>
        /// <param name="log"></param>
        public void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            lock (lockLog) //防止并发异常
            {
                try
                {
                    string LogName = DateTime.Now.ToString("yyyyMMdd") + logname; //按天日志
                    string Dir = HttpRuntime.AppDomainAppPath.ToString() + "log/";
                    if (!System.IO.Directory.Exists(Dir))
                    {
                        System.IO.Directory.CreateDirectory(Dir);
                    }
                    string logfile = Dir + LogName;
                    System.IO.StreamWriter sw = System.IO.File.AppendText(logfile);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                    sw.WriteLine("---------------");
                    sw.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}