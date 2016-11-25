using System;
namespace ThreadExecutor.thread
{
    public  interface IThreadExecutor
    {
        void add(IRunnable run);
        void kill();
        void monitor();
        bool isBusy();
        void Activity();

        bool isRunning(IRunnable procces);


        void shutdown();
    }
}
