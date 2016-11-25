using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ThreadExecutor.thread
{
    public class ThreadExe : Runnable<Int32>
    {

        Process procces;
        public String rutaAplicacion { get; set; }
        public String arguments { get; set; }
        private int resultado;
        private float avance;




        private bool ocupado= true;
        public void run()
        {
            avance=0.0f;
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = rutaAplicacion;
            if (arguments != null)
                startInfo.Arguments = arguments;
            try
            {

                procces = Process.Start(startInfo);
                procces.WaitForExit();
            }
            catch (Exception)
            {
            }
            ocupado = false;
            if ( procces!= null)
                resultado = procces.ExitCode;
            avance = 100.0f;

        }
        public int regreso
        {
            get
            {
                if (!isBusy)

                    if (procces != null)
                        return resultado;
                    else
                        throw new Exception("Espere a que termine el programa");
                else
                    throw new Exception("Espere a que termine el programa");
            }
        }

        public bool isBusy
        {
            get
            {
                return ocupado;
            }
        }
        public void kill()
        {
            if (procces != null)
                procces.Kill();
        }






        public string idT
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public float Avance
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

        public object Value
        {
            get { throw new NotImplementedException(); }
        }


        public object Error
        {
            get { throw new NotImplementedException(); }
        }


        object IRunnable.run()
        {
            throw new NotImplementedException();
        }
    }
}
