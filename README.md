# MvcAsyncDemo
MVC（Asp.net）模式下同步异步混用的时候，异步代码可能会丢失上下文，这里是在园友的启发下搞出来的两个办法。

业务需求大致是这样的：
在MVC当中，
需要将原来链条比较长的N个操作,拆分两条并太相交的链条N1和N2.
其中N1是需要通过控制器的Action迅速返回给最终用户&客户端。
N2不需要返回消息，但是需要处理一大堆N1的副产品。
所以N2采用了异步模式写法，但是不等待（await关键字）返回，不阻塞主线程（控制器UI线程）。
业务代码改造之前，已经在控制台程序上写完相关的Demo了，但是移植到MVC上面的时候就会出现问题。
经过在博客园上的提问，猜测可能是线程上下文切换的时候造成了上下文丢失。
最后想到了两个办法。

方案一、强制与UI进程分离、强制使用后台进程。

Await1Controller就是这样的。

   var task = Task.Factory.StartNew(() =>
            {
                Dowork1().ContinueWith(x =>
                {
                    //logging i done the work
                    if (x.IsFaulted) Say("异步出现异常：" + x.Exception);
                });
            }, TaskCreationOptions.LongRunning);


方案二、分支、辅助线程里面的异步方法，很一次await都添加.ConfigureAwait(false);

Await2Controller

