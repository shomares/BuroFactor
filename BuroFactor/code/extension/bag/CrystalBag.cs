using BuroFactor.code.extension.html;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace BuroFactor.code.extension.bag
{
    public class CrystalDetail : IDisposable
    {
        public DateTime FechaCreacion { get; set; }

        public ReportDocument Report { get; set; }
        public String IDSession { get; set; }

        public void createDocument(String type)
        {
            FechaCreacion = DateTime.Now;
            this.Report = (ReportDocument)Activator.CreateInstance(Type.GetType(type));
        }

        public void Dispose()
        {
            if (this.Report != null)
            {
                this.Report.Close();
                this.Report.Dispose();
                GC.Collect();
            }
        }
    }

    public class CrystalBag
    {
        private ConcurrentDictionary<String, CrystalDetail> Dictionary { get; set; }
        private Int32 Max;
        private static CrystalBag instance;
        private int Sleep;
        public static CrystalBag Instance
        {
            get
            {
                if (instance == null)
                    instance = new CrystalBag();
                return instance;
            }
        }
        private CrystalBag()
        {
            Dictionary = new ConcurrentDictionary<string, CrystalDetail>();
            this.Max = 50;
            //this.Sleep = 3600000;
            this.Sleep = 3600000;
            Thread th = new Thread(() =>
            {
                try
                {
                    Thread.Sleep(this.Sleep);
                    DateTime now = DateTime.Now;
                    var data = Dictionary.Values.Where(aux => (now - aux.FechaCreacion).Minutes > 20).ToList();
                    foreach (var elemento in data)
                    {
                        CrystalDetail document = null;
                        Dictionary.TryRemove(elemento.IDSession, out document);
                        if (document != null)
                            document.Dispose();
                    }
                }
                catch (Exception)
                {

                }
            });
            th.Start();
        }
        public ReportDocument createInstance(ReportSessionObject session, String idSession)
        {
            CrystalDetail report = null;
            closeSession(idSession);
            if (Dictionary.Count() < Max)
            {
                report = new CrystalDetail();
                report.createDocument(session.ReportNameType);
                report.IDSession = idSession;
                Dictionary.TryAdd(idSession, report);
            }
            return report.Report;
        }
        public void closeSession(String idSession)
        {
            CrystalDetail report = null;
            if (Dictionary.ContainsKey(idSession))
            {
                Dictionary.TryRemove(idSession, out report);
                if (report != null)
                    report.Dispose();
            }
        }

    }
}