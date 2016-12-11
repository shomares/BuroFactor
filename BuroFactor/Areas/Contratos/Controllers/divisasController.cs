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
    public class divisasController : Controller
    {
        private burofactorEntities db = new burofactorEntities();

        // GET: Contratos/divisas
        public async Task<ActionResult> Index()
        {
            return View(await db.divisa.ToListAsync());
        }

        // GET: Contratos/divisas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            divisa divisa = await db.divisa.FindAsync(id);
            if (divisa == null)
            {
                return HttpNotFound();
            }
            return View(divisa);
        }

        // GET: Contratos/divisas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contratos/divisas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Create([Bind(Include = "idDivisa,nombre")] divisa divisa)
        {
            if (ModelState.IsValid)
            {
                db.divisa.Add(divisa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(divisa);
        }

        // GET: Contratos/divisas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            divisa divisa = await db.divisa.FindAsync(id);
            if (divisa == null)
            {
                return HttpNotFound();
            }
            return View(divisa);
        }

        // POST: Contratos/divisas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<ActionResult> Edit([Bind(Include = "idDivisa,nombre")] divisa divisa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(divisa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(divisa);
        }

        // GET: Contratos/divisas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            divisa divisa = await db.divisa.FindAsync(id);
            if (divisa == null)
            {
                return HttpNotFound();
            }
            return View(divisa);
        }

        // POST: Contratos/divisas/Delete/5
        [HttpPost, ActionName("Delete")]

        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            divisa divisa = await db.divisa.FindAsync(id);
            db.divisa.Remove(divisa);
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
