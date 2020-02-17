using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xray.Tools.SpiderHelper
{
    public class QueueHelper<T> where T : class
    {
        int TaskNum { get; set; } = 1;
        public int QueueWaitCount { get; }
        public int Delay { get; }
        public ConcurrentQueue<T> TaskQueue { get; set; } = new ConcurrentQueue<T>();
        bool Runing { get; set; } = false;
        public QueueHelper(int TaskNum = 1, int QueueWaitCount = 0, int Delay = 1000)
        {
            this.TaskNum = TaskNum;
            this.QueueWaitCount = QueueWaitCount;
            this.Delay = Delay;
        }
        /// <summary>
        /// 参数1 调用对象自身 2入参数组 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="parms"></param>
        public void Start(Func<object[], bool> func, params object[] parms)
        {
            if (Runing || TaskNum < 1)
            {
                return;
            }
            Runing = true;
            for (int i = 0; i < TaskNum; i++)
            {
                Task.Run(() =>
                {
                    while (Runing)
                    {
                        try
                        {
                            func.Invoke(parms);
                        }
                        catch
                        {
                            Console.WriteLine("执行委托失败");
                        }
                        Thread.Sleep(Delay);
                    }
                });
            }
        }
        public void Stop()
        {
            Runing = false;
        }
        public bool TaskEnqueue(T taskentity)
        {
            if (taskentity == null || !Runing)
            {
                return false;
            }
            try
            {
                TaskQueue.Enqueue(taskentity);
                return true;

            }
            catch (Exception)
            {
            }
            return false;
        }
        public bool TaskDequeue(out T taskentity)
        {
            if (!Runing)
            {
                taskentity = null;
                return false;
            }
            return TaskQueue.TryDequeue(out taskentity);
        }
    }
}
