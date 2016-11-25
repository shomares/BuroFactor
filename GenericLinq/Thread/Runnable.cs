using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadExecutor.thread
{

    public class States
    {
        public enum StateEnum { ERROR, OK };
    }
    public class EventIRunnable
    {
        public delegate void OnAvance(float avance, String id);
        

    }

    public class Result<T> {
        public object Objeto { get; set; }
        public States.StateEnum State { get; set; }
    }

    public interface IRunnable
    {

        //For .NET 4.0
        //void run();

        //For .NET 4.5
        Result<Object> run();
        bool isBusy { get; }
        void kill();
        event ThreadExecutor.thread.EventIRunnable.OnAvance onAvance;
    }

    public interface Runnable<T> : IRunnable
    {
        T regreso { get; }
    }
}
