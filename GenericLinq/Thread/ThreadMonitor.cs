using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadExecutor.thread
{
    public class ThreadMonitor
    {
        private IThreadExecutor threadExecutor;

        public IThreadExecutor ThreadExecutor
        {
            get { return threadExecutor; }
            set
            {
                threadExecutor = value;
                Thread hilo = new Thread(threadExecutor.monitor);
                hilo.Start();

            }
        }

  
    }
}
