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

namespace BuroFactor.Areas.Contratos.Controllers
{
    public class FinancieraController : Controller
    {
        private burofactorEntities db = new burofactorEntities();

        // GET: Contratos/Financiera
        public async Task<ActionResult> Index()
        {
            var financiera = db.financiera.Include(f => f.persona);
            return View(await financiera.Where(aux=>aux.Activa==true).ToListAsync());
        }

        // GET: Contratos/Financiera/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            financiera financiera = await db.financiera.FindAsync(id);
            if (financiera == null)
            {
                return HttpNotFound();
            }
            return View(financiera);
        }

        // GET: Contratos/Financiera/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contratos/Financiera/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(financiera financiera)
        {
            if (ModelState.IsValid)
            {
                //Hay que validar el RFC
                var financieraDb= db.financiera.Include(f => f.persona);

                var lista= await financieraDb.Where(aux => aux.persona.RFC == financiera.persona.RFC).ToListAsync();

                if (lista.Count == 0)
                {
                    financiera.Activa = true;
                    db.persona.Add(financiera.persona);
                    db.financiera.Add(financiera);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else 
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(financiera);
        }

        // GET: Contratos/Financiera/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            financiera financiera = await db.financiera.FindAsync(id);
            if (financiera == null)
            {
                return HttpNotFound();
            }
            return View(financiera);
        }

        // POST: Contratos/Financiera/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( financiera financiera)
        {
            if (ModelState.IsValid)
            {
                financiera financieraAux = await db.financiera.FindAsync(financiera.idFinanciera);
                financieraAux.Regulada = financiera.Regulada;
                db.Entry(financieraAux).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(financiera);
        }

        // GET: Contratos/Financiera/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            financiera financiera = await db.financiera.FindAsync(id);
            if (financiera == null)
            {
                return HttpNotFound();
            }
            return View(financiera);
        }

        // POST: Contratos/Financiera/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            financiera financieraAux = await db.financiera.FindAsync(id);
            financieraAux.Activa = false;
            db.Entry(financieraAux).State = EntityState.Modified;
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
