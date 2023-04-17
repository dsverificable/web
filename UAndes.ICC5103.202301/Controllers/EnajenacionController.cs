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
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.Ajax.Utilities;

namespace UAndes.ICC5103._202301.Controllers
{
    public class EnajenacionController : Controller
    {
        #region Contansts
        private Grupo10ConchaMunozBrDbEntities db = new Grupo10ConchaMunozBrDbEntities();
        #endregion

        #region Public Methods
        // GET: Enajenacion
        public async Task<ActionResult> Index()
        {
            EnajenacionViewModel model = new EnajenacionViewModel();
            model.Enajenacions = db.Enajenacion.ToList();
            model.Descripcion = db.CNEOptions.Select(c => c.Descripcion).ToList();
            model.Comuna = db.ComunaOptions.Select(c => c.Comuna).ToList();

            return View(model);
        }

        // GET: Enajenacion/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!isId(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
            if (!isEnajenacion(enajenacion))
            {
                return HttpNotFound();
            }

            List<Enajenante> enajenantes = await db.Enajenante.Where(e => e.IdEnajenacion == enajenacion.Id).ToListAsync();
            List<Adquiriente> adquirientes = await db.Adquiriente.Where(e => e.IdEnajenacion == enajenacion.Id).ToListAsync();

            EnajenacionViewModel model = new EnajenacionViewModel();
            model.Adquirientes = adquirientes;
            model.Enajenacion = enajenacion;
            model.Enajenantes = enajenantes;
            model.Descripcion = db.CNEOptions.Select(c => c.Descripcion).ToList();
            model.Comuna = db.ComunaOptions.Select(c => c.Comuna).ToList();
            model.SelectDescripcion = model.Descripcion[0];
            model.SelectComuna = model.Comuna[enajenacion.Comuna];

            return View(model);
        }

        // GET: Enajenacion/Create
        public ActionResult Create()
        {
            var model = new EnajenacionViewModel();
            model.CNEOptions = db.CNEOptions.ToList();
            model.ComunaOptions = db.ComunaOptions.ToList();   
            
            return View(model);
        }

        // POST: Enajenacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id, CNE, Comuna, Manzana, Predio, Fojas, FechaInscripcion, IdInscripcion")] Enajenacion enajenacion)
        {
            var model = new EnajenacionViewModel();
            model.Enajenacion = enajenacion;
            model.CNEOptions = db.CNEOptions.ToList();
            model.ComunaOptions = db.ComunaOptions.ToList();

            if (ModelState.IsValid)
            {
                FormCollection formCollection = new FormCollection(Request.Form);

                if (isRdp(enajenacion.CNE))
                {
                    AddAdquirientesToDb(formCollection, enajenacion.Id);
                    db.Enajenacion.Add(enajenacion);
                }
                
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        // GET: Enajenacion/Consult
        public ActionResult Consult()
        {
            var model = new EnajenacionViewModel();
            model.ComunaOptions = db.ComunaOptions.ToList();
            
            return View(model);
        }

        // POST: Enajenacion/Consult
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Consult(EnajenacionViewModel viewModel)
        {
            var model = new EnajenacionViewModel();
            model.ComunaOptions = db.ComunaOptions.ToList();

            int comunaId = viewModel.Enajenacion.Comuna;
            int manzana = viewModel.Enajenacion.Manzana;
            int predio = viewModel.Enajenacion.Predio;
            int year = viewModel.Year;

            List<Enajenacion> enajenaciones = await db.Enajenacion
                   .Where(e => e.Manzana == manzana && e.Predio == predio && e.FechaInscripcion.Year <= year && e.ComunaOptions.Valor == comunaId)
                   .ToListAsync();

            if (enajenaciones.Count == 0)
            {
                return View(model);
            }
            else
            {
                Enajenacion enajenacion = getLastUpdateOfAndSpecificEnajenacion(enajenaciones, year);
                List<Adquiriente> adquirientes = await db.Adquiriente
                            .Where(a => a.IdEnajenacion == enajenacion.Id)
                            .ToListAsync();

                model.Adquirientes = adquirientes;
                model.Enajenacion = enajenacion;

                return View(model);
            }
        }

        // GET: Enajenacion/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!isId(id))     
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
            if (!isEnajenacion(enajenacion))
            {
                return HttpNotFound();
            }

            return View(enajenacion);
        }

        // POST: Enajenacion/Edit/5
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
                if (!isId(id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
                if (!isEnajenacion(enajenacion))
                {
                    return HttpNotFound();
                }
                return View(enajenacion);
            }
            else
            {
                if (!isId(id))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);
                if (!isEnajenacion(enajenacion))
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
        #endregion

        #region Protected Methods
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Private Methods
        private bool isRdp(int cne)
        {
            //  1 = Regularizacion De Patrimonio
            if (cne == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isId(int? id)
        {
            if (id != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isEnajenacion(Enajenacion enajenacion)
        {
            if (enajenacion != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckValue(float percentage)
        {
            if (percentage == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        } 

        private float DifferencePercentage(string[] percentages)
        {
            int peopleWithoutPorcentage = 0;
            float percentageSum = 0;
            float difference = 0;

            foreach (var percentage in percentages)
            {
                float parsePercentage = float.Parse(percentage);
                percentageSum += parsePercentage;

                if (parsePercentage == 0)
                {
                    peopleWithoutPorcentage++;
                }
            }

            if (peopleWithoutPorcentage > 0)
            {
                difference = (100 - percentageSum) / peopleWithoutPorcentage;
            }

            return difference;
        }

        private float PercentageValue(float percentage, float differencePercentage)
        {
            if (percentage > 0)
            {
                return percentage;
            }
            else
            {
                return differencePercentage;
            }
        }

        private void AddEnajenanteToDb(FormCollection formCollection, int enajenacionId)
        {
            var ruts = formCollection["Enajenantes[0].RutEnajenante"].Split(',');
            var percentages = formCollection["Enajenantes[0].PorcentajeEnajenante"].Split(',');
            float differencePercentage = DifferencePercentage(percentages);

            for (int i = 0; i < ruts.Length; i++)
            {
                var enajenante = new Enajenante();
                string rut = ruts[i];
                float percentage = float.Parse(percentages[i]);

                enajenante.IdEnajenacion = enajenacionId;
                enajenante.RutEnajenante = rut;
                enajenante.PorcentajeEnajenante = PercentageValue(percentage, differencePercentage);
                enajenante.CheckEnajenante = CheckValue(percentage);

                db.Enajenante.Add(enajenante);
            }
        }

        private void AddAdquirientesToDb(FormCollection formCollection, int enajenacionId)
        {
            var ruts = formCollection["Adquirientes[0].RutAdquiriente"].Split(',');
            var percentages = formCollection["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
            float differencePercentage = DifferencePercentage(percentages);

            for (int i = 0; i < ruts.Length; i++)
            {
                var adquiriente = new Adquiriente();
                string rut = ruts[i];
                float percentage = float.Parse(percentages[i]);

                adquiriente.IdEnajenacion = enajenacionId;
                adquiriente.RutAdquiriente = rut;
                adquiriente.PorcentajeAdquiriente = PercentageValue(percentage, differencePercentage);
                adquiriente.CheckAdquiriente = CheckValue(percentage);

                db.Adquiriente.Add(adquiriente);
            }
        }

        private Enajenacion getLastUpdateOfAndSpecificEnajenacion(List<Enajenacion> enajenaciones, int year)
        {
            int maxYear = enajenaciones.Max(e => e.FechaInscripcion.Year);
            enajenaciones = enajenaciones.Where(e => e.FechaInscripcion.Year == maxYear).ToList();

            if (enajenaciones.Count == 1)
            {
                return enajenaciones.FirstOrDefault();
            }
            else
            {
                int maxIdInscripcion = enajenaciones.Max(e => e.IdInscripcion);
                enajenaciones = enajenaciones.Where(e => e.IdInscripcion == maxIdInscripcion).ToList();

                return enajenaciones.LastOrDefault();
            }
        }
        #endregion
    }
}
