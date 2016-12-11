using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BuroFactor.Models.dao;
using BuroFactor.code.extension.html;

namespace BuroFactor.Areas.Contratos.Controllers
{
    [Security(Roles = "ADMINISTRADOR", NotifyUrl = "~/Principal/Errores/Error404")]
    public class PlanController : Controller
    {
        private burofactorEntities db = new burofactorEntities();

        // GET: Contratos/Plan
        public async Task<ActionResult> Index()
        {

            return View(await db.planconsulta.Where(aux => aux.Activo && aux.FechaVencimiento > DateTime.Now).ToListAsync());
        }

        // GET: Contratos/Plan/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planconsulta planconsulta = await db.planconsulta.FindAsync(id);
            if (planconsulta == null)
            {
                return HttpNotFound();
            }
            return View(planconsulta);
        }

        // GET: Contratos/Plan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contratos/Plan/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Create([Bind(Include = "idPlanConsulta,Nombre,MaxConsultaMes,FechaVencimiento,Precio,Ilimitado")] planconsulta planconsulta)
        {
            if (ModelState.IsValid)
            {
                planconsulta.FechaRegistro = DateTime.Now;
                planconsulta.Activo = true;
                db.planconsulta.Add(planconsulta);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(planconsulta);
        }

        // GET: Contratos/Plan/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planconsulta planconsulta = await db.planconsulta.FindAsync(id);
            if (planconsulta == null)
            {
                return HttpNotFound();
            }
            else if (planconsulta.FechaVencimiento < DateTime.Now || !planconsulta.Activo)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            return View(planconsulta);
        }

        // POST: Contratos/Plan/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Edit([Bind(Include = "idPlanConsulta,MaxConsultaMes,FechaVencimiento,Precio,Ilimitado")] planconsulta planconsulta)
        {
            if (ModelState.IsValid)
            {
                planconsulta plan = await db.planconsulta.FindAsync(planconsulta.idPlanConsulta);
                plan.MaxConsultaMes = planconsulta.MaxConsultaMes;
                plan.FechaVencimiento = planconsulta.FechaVencimiento;
                plan.Precio = planconsulta.Precio;
                plan.Ilimitado = planconsulta.Ilimitado;

                db.Entry(plan).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(planconsulta);
        }

        // GET: Contratos/Plan/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planconsulta planconsulta = await db.planconsulta.FindAsync(id);
            if (planconsulta == null)
            {
                return HttpNotFound();
            }
            return View(planconsulta);
        }

        // POST: Contratos/Plan/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            planconsulta planconsulta = await db.planconsulta.FindAsync(id);
            planconsulta.Activo = false;
            db.Entry(planconsulta).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
