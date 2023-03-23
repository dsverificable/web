using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UAndes.ICC5103._202301.Models;
using System.Drawing.Printing;

namespace UAndes.ICC5103._202301.Controllers
{
    public class EnajenacionController : Controller
    {
        private InscripcionesBrDbEntities db = new InscripcionesBrDbEntities();

        // GET: Enajenacion
        public async Task<ActionResult> Index()
        {
            return View(await db.Enajenacion.ToListAsync());
        }

        // GET: Enajenacion/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
            if (enajenacion == null)
            {
                return HttpNotFound();
            }

            List<Enajenante> enajenantes = await db.Enajenante.Where(e => e.IdEnajenacion == enajenacion.Id).ToListAsync();
            List<Adquiriente> adquirientes = await db.Adquiriente.Where(e => e.IdEnajenacion == enajenacion.Id).ToListAsync();


            EnajenacionViewModel viewModel = new EnajenacionViewModel
            {
                Enajenacion = enajenacion,
                Enajenantes = enajenantes,
                Adquirientes = adquirientes
            };

            return View(viewModel);
        }

        // GET: Enajenacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Enajenacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id, CNE, Comuna, Manzana, Predio, Fojas, FechaInscripcion, IdInscripcion")] Enajenacion enajenacion)
        {
            if (ModelState.IsValid)
            {
                //Create([Bind(Include = "RutEnajenante, PorcentajeEnajenante, CheckEnajenante")] Enajenante enajenante)
                db.Enajenacion.Add(enajenacion);


                var enajenante = new Enajenante();

                string rut = Request.Form["Enajenantes[0].RutEnajenante"];
                int porcentaje = int.Parse(Request.Form["Enajenantes[0].PorcentajeEnajenante"]);
                int check = int.Parse(Request.Form["Enajenantes[0].CheckEnajenante"]);

                enajenante.RutEnajenante = rut;
                enajenante.PorcentajeEnajenante = porcentaje;
                enajenante.CheckEnajenante = check;
                enajenante.IdEnajenacion = enajenacion.Id;

                db.Enajenante.Add(enajenante);


                var adquiriente = new Adquiriente();

                rut = Request.Form["Adquirientes[0].RutAdquiriente"];
                porcentaje = int.Parse(Request.Form["Adquirientes[0].PorcentajeAdquiriente"]);
                check = int.Parse(Request.Form["Adquirientes[0].CheckAdquiriente"]);

                adquiriente.RutAdquiriente = rut;
                adquiriente.PorcentajeAdquiriente = porcentaje;
                adquiriente.CheckAdquiriente = check;
                adquiriente.IdEnajenacion = enajenacion.Id;

                db.Adquiriente.Add(adquiriente);

                var a = 1;


                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(enajenacion);
        }







        // GET: Enajenacion/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
            if (enajenacion == null)
            {
                return HttpNotFound();
            }
            return View(enajenacion);
        }

        // POST: Enajenacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id, CNE, Comuna, Manzana, Predio, RutEnajenante, PorcentajeEnajenante, CheckEnajenante, RutAdquiriente, PorcentajeAdquiriente, CheckAdquiriente, Fojas, FechaInscripcion, IdInscripcion.")] Enajenacion enajenacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enajenacion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(enajenacion);
        }

        // GET: Enajenacion/Delete/5
        public async Task<ActionResult> Delete(int? id, char option = 'a', int insideId = 0)
        {
            if (option == 'a')
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
                if (enajenacion == null)
                {
                    return HttpNotFound();
                }
                return View(enajenacion);
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
                if (enajenacion == null)
                {
                    return HttpNotFound();
                }

                Enajenante enajenante = await db.Enajenante.FindAsync(insideId);
                if (enajenante == null)
                {
                    return HttpNotFound();
                }
                return View(enajenacion);
            }
        }

        // POST: Enajenacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id, char option, int insideId)
        {   
            if (option == 'a')
            {
                Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
                db.Enajenacion.Remove(enajenacion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                Enajenante enajenante = await db.Enajenante.FindAsync(insideId);
                db.Enajenante.Remove(enajenante);
                await db.SaveChangesAsync();
                return RedirectToAction("Details/"+ id.ToString());
            }
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
