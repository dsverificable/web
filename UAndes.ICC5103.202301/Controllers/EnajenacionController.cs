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
        private Grupo10ConchaMunozBrDbEntities db = new Grupo10ConchaMunozBrDbEntities();

        private bool checkIfIsRdp(int cne)
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

        private bool GetValueOfTheCheck(int percentage)
        {
            //  1 = Regularizacion De Patrimonio
            if (percentage == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetDifferencePorcentageOfNotAcredited(string[] percentages)
        {
            int totalSum = 0;
            int peopleWithoutPorcentage = 0;
            int difference = 0;


            foreach (var percentage in percentages)
            {
                int intPercentage = int.Parse(percentage);
                totalSum += intPercentage;

                if(intPercentage == 0)
                {
                    peopleWithoutPorcentage++;
                }
            }

            if (peopleWithoutPorcentage > 0)
            {
               difference = (100 - totalSum) / peopleWithoutPorcentage;
            }

            return difference;
        }

        private int GetValuePorcentage(int percentage, int differencePercentage)
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
            var rutList = formCollection["Enajenantes[0].RutEnajenante"].Split(',');
            var porcentajeList = formCollection["Enajenantes[0].PorcentajeEnajenante"].Split(',');
            int differencePorcentage = GetDifferencePorcentageOfNotAcredited(porcentajeList);

            for (int i = 0; i < rutList.Length; i++)
            {
                var enajenante = new Enajenante();
                string rut = rutList[i];
                int percentage = int.Parse(porcentajeList[i]);

                enajenante.IdEnajenacion = enajenacionId;
                enajenante.RutEnajenante = rut;
                enajenante.PorcentajeEnajenante = GetValuePorcentage(percentage, differencePorcentage);
                enajenante.CheckEnajenante = GetValueOfTheCheck(percentage);

                db.Enajenante.Add(enajenante);
            }
        }

        private void AddAdquirientesToDb(FormCollection formCollection, int enajenacionId)
        {
            var rutList = formCollection["Adquirientes[0].RutAdquiriente"].Split(',');
            var porcentajeList = formCollection["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
            int differencePorcentage = GetDifferencePorcentageOfNotAcredited(porcentajeList);

            for (int i = 0; i < rutList.Length; i++)
            {
                var adquiriente = new Adquiriente();
                string rut = rutList[i];
                int percentage = int.Parse(porcentajeList[i]);

                adquiriente.IdEnajenacion = enajenacionId;
                adquiriente.RutAdquiriente = rut;
                adquiriente.PorcentajeAdquiriente = GetValuePorcentage(percentage, differencePorcentage);
                adquiriente.CheckAdquiriente = GetValueOfTheCheck(percentage);

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

        // GET: Enajenacion
        public async Task<ActionResult> Index()
        {
            var model = new EnajenacionViewModel();
            model.Enajenacions = db.Enajenacion.ToList();

            var cneoptions = db.CNEOptions.ToList();
            List<String> Descripcion = new List<string>();
            for (int i = 0; i < 2 ;i++)
            {
                Descripcion.Add(cneoptions[i].Descripcion);
            }
            model.Descripcion = Descripcion;

            var comunaoptions = db.ComunaOptions.ToList();
            List<String> Comunas = new List<string>();
            for (int i = 0; i <= 325; i++)
            {
                Comunas.Add(comunaoptions[i].Comuna);
            }
            model.Comuna = Comunas;

            return View(model);
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

            var cneoptions = db.CNEOptions.ToList();
            List<String> Descripcion = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                Descripcion.Add(cneoptions[i].Descripcion);
            }

            viewModel.Descripcion = Descripcion;
            viewModel.SelectDescripcion = Descripcion[enajenacion.CNE];

            var comunasoptions = db.ComunaOptions.ToList();
            List<String> Comuna = new List<string>();
            for (int i = 0; i <= 325; i++)
            {
                Comuna.Add(comunasoptions[i].Comuna);
            }

            viewModel.Comuna = Comuna;
            viewModel.SelectComuna = Comuna[enajenacion.Comuna];

            return View(viewModel);
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
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
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

                if (checkIfIsRdp(enajenacion.CNE))
                {
                    AddAdquirientesToDb(formCollection, enajenacion.Id);
                    db.Enajenacion.Add(enajenacion);
                }
                else
                {
                    // Add Logic
                    //AddAdquirientesToDb(formCollection, enajenacion.Id);
                    //AddEnajenanteToDb(formCollection, enajenacion.Id);
                    //db.Enajenacion.Add(enajenacion);
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
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Consult(EnajenacionViewModel viewModel)
        {
            // Data from query
            int comuna = viewModel.Enajenacion.Comuna;
            int manzana = viewModel.Enajenacion.Manzana;
            int predio = viewModel.Enajenacion.Predio;
            int year = viewModel.Year;

            // New Model
            var model = new EnajenacionViewModel();
            model.ComunaOptions = db.ComunaOptions.ToList();

            List<Enajenacion> enajenaciones = await db.Enajenacion
                   .Where(e => e.Manzana == manzana && e.Predio == predio && e.FechaInscripcion.Year <= year && e.ComunaOptions.Valor == comuna)
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
