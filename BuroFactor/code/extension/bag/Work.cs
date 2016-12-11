using System;
using ThreadExecutor.thread;

namespace BuroFactor.code.extension.bag
{
    public class Work : IRunnable
    {
        public String ID { get; set; }

        //Events
        public delegate void OnSucces(Result<Object> resultado, Work id);
        public event OnSucces onSucces;
        public delegate void OnError(Result<Object> resultado, Work id);
        public event OnError onError;
        public event EventIRunnable.OnAvance onAvance;


        public bool isBusy
        {
            get { return workI.isBusy; }
        }
        public void kill()
        {
            workI.kill();
        }

        private IRunnable workI;

        public string idSession { get; set; }

        public IRunnable WorkI
        {
            get { return workI; }
            set
            {
                this.workI = value;
                if (this.workI != null)
                    this.workI.onAvance += work_onavance;
            }
        }

        public void Dispose()
        {
            WorkI.Dispose();
            WorkI = null;

        }

        public bool Almacena { get; internal set; }

        public string Name
        {
            get
            {
                return WorkI.Name;
            }
        }

        public string Result
        {
            get
            {
                return WorkI.Result;
            }
        }

        string IRunnable.Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        string IRunnable.Result
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Result<Object> run()
        {

            Result<Object> resultado = workI.run();

            if (resultado.State == States.StateEnum.OK)
            {
                if (onSucces != null)
                    onSucces(resultado, this);
            }
            else
            {
                if (onError != null)
                    onError(resultado, this);
            }
            return resultado;

        }

        private void work_onavance(float avance, string id)
        {
            if (this.onAvance != null)
                this.onAvance(avance, this.ID);
        }

        public Work()
        {
            Almacena = false;
        }
    }
}