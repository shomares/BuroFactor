using BuroFactor.code.extension.bag;
using BuroFactor.code.extension.html;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BuroFactor.AspNetForms
{
    public partial class aspnetViewver : Page
    {
        public aspnetViewver()
        {
            Page.Init += Page_Init;
            Page.Unload += Page_Unload;
        }
        private void Visor_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
        }

        private void Page_Unload(object sender, EventArgs e)
        {
        }

        private void Page_Init(object sender, EventArgs e)
        {
            var isPDF = Request.QueryString["generaPDF"];
            ReportSessionObject rso = null;
            ReportDocument rd = null;
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {

                    if (HttpContext.Current.Session["REPORTE"] != null)
                    {
                        rso = (ReportSessionObject)HttpContext.Current.Session["REPORTE"];
                        rd = CrystalBag.Instance.createInstance(rso, HttpContext.Current.Session.SessionID);
                        if (rd != null)
                        {
                            rd.SetDataSource(rso.Datos);
                            foreach (string param in rso.Parametros)
                            {
                                try { rd.SetParameterValue(param.Split('|')[0], param.Split('|')[1]); } catch (Exception) { }
                            }
                        }
                    }
                    if (rd != null)
                    {
                        if (isPDF == null)
                        {
                            Visor.ReportSource = rd;
                            Visor.DataBind();
                        }
                        else
                        {
                            ReportDocument document = rd;

                            document.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Reporte");
                        }
                    }
                    else
                        Response.Redirect("~/Principal/Errores/Error404");
                }
                else
                    Response.Redirect("/");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //}
        }

        protected void Visor_Init(object sender, EventArgs e)
        {
            var isPDF = Request.QueryString["generaPDF"];
            ReportSessionObject rso = null;
            ReportDocument rd = null;
            //if (!IsPostBack)
            //{
            if (User.Identity.IsAuthenticated)
            {

                if (HttpContext.Current.Session["REPORTE"] != null)
                {
                    rso = (ReportSessionObject)HttpContext.Current.Session["REPORTE"];
                    rd = CrystalBag.Instance.createInstance(rso, HttpContext.Current.Session.SessionID);
                    if (rd != null)
                    {
                        rd.SetDataSource(rso.Datos);
                        //if (!IsPostBack)
                        foreach (string param in rso.Parametros)
                        {
                            try { rd.SetParameterValue(param.Split('|')[0], param.Split('|')[1]); } catch (Exception) { }
                        }
                    }
                }
                if (rd != null)
                {
                    if (isPDF == null)
                    {

                        Visor.ReportSource = rd;
                        Visor.DataBind();
                    }
                    else
                    {
                        ReportDocument document = rd;
                        document.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "Reporte");
                    }
                }
                else
                    Response.Redirect("~/Principal/Errores/Error404");
            }
            else
                Response.Redirect("/");
        }
    }
}