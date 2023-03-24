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
                db.Enajenacion.Add(enajenacion);

                string rut;
                int porcentaje;
                int check;

                var rutList = Request.Form["Enajenantes[0].RutEnajenante"].Split(',');
                var porcentajeList = Request.Form["Enajenantes[0].PorcentajeEnajenante"].Split(',');
                var checkList = Request.Form["Enajenantes[0].CheckEnajenante"].Split(',');
                for (int i = 0; i < rutList.Length; i++)
                {
                    var enajenante = new Enajenante();
                    enajenante.IdEnajenacion = enajenacion.Id;
                    rut = rutList[i];
                    porcentaje = int.Parse(porcentajeList[i]);
                    check = int.Parse(checkList[i]);

                    enajenante.RutEnajenante = rut;
                    enajenante.PorcentajeEnajenante = porcentaje;
                    enajenante.CheckEnajenante = check;

                    db.Enajenante.Add(enajenante);
                }

                rutList = Request.Form["Adquirientes[0].RutAdquiriente"].Split(',');
                porcentajeList = Request.Form["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
                checkList = Request.Form["Adquirientes[0].CheckAdquiriente"].Split(',');
                for (int i = 0; i < rutList.Length; i++)
                {
                    var adquiriente = new Adquiriente();
                    adquiriente.IdEnajenacion = enajenacion.Id;
                    rut = rutList[i];
                    porcentaje = int.Parse(porcentajeList[i]);
                    check = int.Parse(checkList[i]);

                    adquiriente.RutAdquiriente = rut;
                    adquiriente.PorcentajeAdquiriente = porcentaje;
                    adquiriente.CheckAdquiriente = check;

                    db.Adquiriente.Add(adquiriente);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(enajenacion);
        }



        // GET: Enajenacion/Consult
        public ActionResult Consult()
        {
            return View();
        }

        // POST: Enajenacion/Consult
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Consult([Bind(Include = "Id, CNE, Comuna, Manzana, Predio, Fojas, FechaInscripcion, IdInscripcion")] Enajenacion enajenacion)
        {
            if (ModelState.IsValid)
            {
                db.Enajenacion.Add(enajenacion);

                string rut;
                int porcentaje;
                int check;

                var rutList = Request.Form["Enajenantes[0].RutEnajenante"].Split(',');
                var porcentajeList = Request.Form["Enajenantes[0].PorcentajeEnajenante"].Split(',');
                var checkList = Request.Form["Enajenantes[0].CheckEnajenante"].Split(',');
                for (int i = 0; i < rutList.Length; i++)
                {
                    var enajenante = new Enajenante();
                    enajenante.IdEnajenacion = enajenacion.Id;
                    rut = rutList[i];
                    porcentaje = int.Parse(porcentajeList[i]);
                    check = int.Parse(checkList[i]);

                    enajenante.RutEnajenante = rut;
                    enajenante.PorcentajeEnajenante = porcentaje;
                    enajenante.CheckEnajenante = check;

                    db.Enajenante.Add(enajenante);
                }

                rutList = Request.Form["Adquirientes[0].RutAdquiriente"].Split(',');
                porcentajeList = Request.Form["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
                checkList = Request.Form["Adquirientes[0].CheckAdquiriente"].Split(',');
                for (int i = 0; i < rutList.Length; i++)
                {
                    var adquiriente = new Adquiriente();
                    adquiriente.IdEnajenacion = enajenacion.Id;
                    rut = rutList[i];
                    porcentaje = int.Parse(porcentajeList[i]);
                    check = int.Parse(checkList[i]);

                    adquiriente.RutAdquiriente = rut;
                    adquiriente.PorcentajeAdquiriente = porcentaje;
                    adquiriente.CheckAdquiriente = check;

                    db.Adquiriente.Add(adquiriente);
                }

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
