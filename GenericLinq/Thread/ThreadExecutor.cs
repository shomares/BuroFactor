using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace ThreadExecutor.thread
{
    public class ThreadExecutorPool : IThreadExecutor
    {

        private List<IRunnable> queque;
        private Exception ex;

        private ThreadPoolState work;

        public ThreadExecutorPool()
        {
            queque = new List<IRunnable>();
            work = ThreadPoolState.Ready;
        }

    
      

        public bool isBusy()
        {


            lock (queque)
                if (queque.Count() > 0)
                    return true;
                else
                {
                    work = ThreadPoolState.Ready;
                    return false;
                }


        }


        public void kill()
        {
            foreach (var run in queque)
                run.kill();
            lock (queque)
                queque = new List<IRunnable>();
            work = ThreadPoolState.Ready;
        }


        public bool isRunning(IRunnable procces)
        {

            //lock (queque)
            //    if ((from aux in queque where aux.idT == procces.idT && aux.isBusy select aux).Count() > 0)
            //        return true;
            //    else
            //    {
            //        if (queque.Count() == 0)
            //            work = ThreadPoolState.Ready;
            //        return false;
            //    }
            return false;

        }


        public void shutdown()
        {
            lock (queque)
                queque = new List<IRunnable>();
            work = ThreadPoolState.Down;
        }


        public void monitor()
        {
            throw new NotImplementedException();
        }

        public void Activity()
        {
            throw new NotImplementedException();
        }

        public void add(IRunnable run)
        {
            throw new NotImplementedException();
        }
    }
}
