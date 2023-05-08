using Antlr.Runtime.Misc;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using UAndes.ICC5103._202301.Models;

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

            List<Enajenacion> enajenaciones = await db.Enajenacion
                   .Where(e => e.Manzana == enajenacion.Manzana && e.Predio == enajenacion.Predio && e.FechaInscripcion.Year <= enajenacion.FechaInscripcion.Year && e.Comuna == enajenacion.Comuna)
                   .ToListAsync();
            Enajenacion last_enajenacion = getLastUpdateOfAndSpecificEnajenacion(enajenaciones);

            if (ModelState.IsValid)
            {
                FormCollection formCollection = new FormCollection(Request.Form);

                if (isRdp(enajenacion.CNE))
                {
                    List<Adquiriente> adquirientes = PastFormToAdquirienteModel(formCollection, enajenacion);
                    AddAdquirientesToDb(adquirientes);
                    db.Enajenacion.Add(enajenacion); // delete when compraventa works correctly
                }
                else
                {
                    List<Adquiriente> adquirientes = PastFormToAdquirienteModel(formCollection, enajenacion);
                    List<Adquiriente> enajenantes = PastFormToEnajenanteModel(formCollection, enajenacion);
                    List<Adquiriente> newEnajenatesOfEnajenacion = CompraventaCases(enajenacion, last_enajenacion, adquirientes, enajenantes);
                    AddAdquirientesToDb(adquirientes);
                }

                //db.Enajenacion.Add(enajenacion); discomment when all the code is ready
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
                Enajenacion enajenacion = getLastUpdateOfAndSpecificEnajenacion(enajenaciones);
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

                return RedirectToAction("Details/" + id.ToString());
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

        private bool isSumAdquirienteEqual100(List<Adquiriente> adquirientes)
        {
            float sumPercentage = (float)adquirientes.Sum(a => a.PorcentajeAdquiriente);

            if (sumPercentage == 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isOnlyOneAquirerAndAlienating(List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {
            if(adquirientes.Count == 1 && enajenantes.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isSumEqualTo100(float totalSumPercenteges)
        {
            if(totalSumPercenteges == 100)
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

        private void AddAdquirientesToDb(List<Adquiriente> adquirientes)
        {
            foreach (var adquiriente in adquirientes)
            {
                db.Adquiriente.Add(adquiriente);
            }
        }

        private float TotalSumFormPercentage(List<Adquiriente> enajenantes)
        {
            float totalSum = (float)enajenantes.Sum(a => a.PorcentajeAdquiriente);
          
            return totalSum;
        }

        private float TotalPercentageEnajenantes(List<Adquiriente> enajenantes, List<Adquiriente> currentEnajenantes)
        {
            float totalPercentage = 0;

            foreach (var enajenante in enajenantes)
            {
                foreach (var currentEnajenante in currentEnajenantes)
                {
                    if (enajenante.RutAdquiriente == currentEnajenante.RutAdquiriente)
                    {
                        totalPercentage += (float)currentEnajenante.PorcentajeAdquiriente;
                        break;
                    }
                }
            }

            return totalPercentage;
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

        private float RatioPercentage(float userPercentage, float totalPercentage)
        {
            float percentage;

            if (totalPercentage > 100)
            {
                percentage = (100 / totalPercentage) * userPercentage;
            }
            else
            {
                percentage = (userPercentage * totalPercentage) / 100;
            }

            return percentage;
        }

        private List<float> PercentagesToListAdquiriente(string[] percentages)
        {
            float differencePercentage = DifferencePercentage(percentages);
            var floatPercentages = percentages.Select(p => float.Parse(p)).ToList();
            var newPercentages = floatPercentages.Select(p => PercentageValue(p, differencePercentage)).ToList();

            return newPercentages;
        }

        private List<float> PercentagesToListEnajenante(string[] percentages)
        {
            var floatPercentages = percentages.Select(p => float.Parse(p)).ToList();

            return floatPercentages;
        }

        private List<Adquiriente> PastFormToAdquirienteModel(FormCollection formCollection, Enajenacion enajenacion)
        {
            List<Adquiriente> adquirientes = new List<Adquiriente>();

            var ruts = formCollection["Adquirientes[0].RutAdquiriente"].Split(',');
            var percentagesCheck = formCollection["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
            var percentages = formCollection["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
            List<float> percentagesParce = PercentagesToListAdquiriente(percentages);

            for (int i = 0; i < ruts.Length; i++)
            {
                var adquiriente = new Adquiriente();
                string rut = ruts[i];
                adquiriente.RutAdquiriente = rut;
                adquiriente.IdEnajenacion = enajenacion.Id;
                adquiriente.PorcentajeAdquiriente = percentagesParce[i];
                adquiriente.CheckAdquiriente = CheckValue(float.Parse(percentagesCheck[i]));
            }

            return adquirientes;
        }

        private List<Adquiriente> PastFormToEnajenanteModel(FormCollection formCollection, Enajenacion enajenacion)
        {
            List<Adquiriente> enajenantes = new List<Adquiriente>();

            var ruts = formCollection["Adquirientes[0].RutEnajenante"].Split(',');
            var percentagesCheck = formCollection["Adquirientes[0].PorcentajeEnajenante"].Split(',');
            var percentages = formCollection["Enajenantes[0].PorcentajeEnajenante"].Split(',');
            List<float> percentagesParce = PercentagesToListEnajenante(percentages);

            for (int i = 0; i < ruts.Length; i++)
            {
                var adquiriente = new Adquiriente();
                string rut = ruts[i];
                adquiriente.RutAdquiriente = rut;
                adquiriente.IdEnajenacion = enajenacion.Id;
                adquiriente.PorcentajeAdquiriente = percentagesParce[i];
                adquiriente.CheckAdquiriente = CheckValue(float.Parse(percentagesCheck[i]));
            }

            return enajenantes;
        }

        private List<Adquiriente> UpdateEnajenatePercentage(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes, int option)
        {
            foreach (var enajenante in enajenantes)
            {
                foreach (var currentEnajenante in currentEnajenantes)
                {
                    if (enajenante.RutAdquiriente == currentEnajenante.RutAdquiriente)
                    {
                        if (option == 1)
                        {
                            enajenante.PorcentajeAdquiriente = 0;
                        }
                        else if (option == 2)
                        {
                            enajenante.PorcentajeAdquiriente = (float)currentEnajenante.PorcentajeAdquiriente - RatioPercentage((float)enajenante.PorcentajeAdquiriente, (float)currentEnajenante.PorcentajeAdquiriente);
                        }
                        else if (option == 3)
                        {
                            enajenante.PorcentajeAdquiriente = (float)currentEnajenante.PorcentajeAdquiriente - enajenante.PorcentajeAdquiriente;
                        }
                    }
                }
            }

            return enajenantes;
        }

        private List<Adquiriente> UpdateAdquirientesPercentage(List<Adquiriente> currentEnajenantes, List<Adquiriente> adquirientes)
        {
            foreach (var adquiriente in adquirientes)
            {
                foreach (var currentEnajenante in currentEnajenantes)
                {
                    if (adquiriente.RutAdquiriente == currentEnajenante.RutAdquiriente)
                    {
                        adquiriente.PorcentajeAdquiriente = currentEnajenante.PorcentajeAdquiriente + adquiriente.PorcentajeAdquiriente;
                    }
                }
            }

            return adquirientes;
        }

        private List<Adquiriente> ParceNegativePercentage(List<Adquiriente> adquirientes)
        {
            foreach (var adquiriente in adquirientes)
            {
                if (adquiriente.PorcentajeAdquiriente < 0)
                {
                    adquiriente.PorcentajeAdquiriente = 0;
                }
            }

            return adquirientes;
        }

        private List<Adquiriente> EnjanenatesNotInTheForm(List<Adquiriente> currentEnajenantes, List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {
            List<Adquiriente> enajenantesNotInTheForm = currentEnajenantes
                            .Where(e => !adquirientes.Any(a => a.RutAdquiriente == e.RutAdquiriente)
                                        && !enajenantes.Any(en => en.RutAdquiriente == e.RutAdquiriente))
                            .ToList();
    
            return enajenantesNotInTheForm;
        }

        private List<Adquiriente> CombineListsForNewData(List<Adquiriente> currentEnajenantes, List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {
            List<Adquiriente> combinedList = currentEnajenantes
                          .Concat(adquirientes)
                          .Concat(enajenantes)
                          .ToList();

            return combinedList;
        }

        private List<Adquiriente> CompraventaCases(Enajenacion enajenacion, Enajenacion last_enajenacion, List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {
            float totalPercentagesEnajenantes;    
            
            List<Adquiriente> currentEnajenantes = db.Adquiriente
                            .Where(a => a.IdEnajenacion == last_enajenacion.Id)
                            .ToList();
            List<Adquiriente> enajenantesNotInForm = EnjanenatesNotInTheForm(currentEnajenantes, adquirientes, enajenantes);

            if (isSumAdquirienteEqual100(adquirientes))
            {
                totalPercentagesEnajenantes = TotalPercentageEnajenantes(enajenantes, currentEnajenantes);
                adquirientes.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalPercentagesEnajenantes));
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
                enajenantes = UpdateEnajenatePercentage(currentEnajenantes, enajenantes, 1);

                // TODO: delete enejenante of the table
            }
            else if (isOnlyOneAquirerAndAlienating(adquirientes, enajenantes))
            {
                totalPercentagesEnajenantes = TotalPercentageEnajenantes(enajenantes, currentEnajenantes);
                adquirientes.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalPercentagesEnajenantes));
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
                enajenantes = UpdateEnajenatePercentage(currentEnajenantes, enajenantes, 2);
            }
            else
            {
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
                enajenantes = UpdateEnajenatePercentage(currentEnajenantes, enajenantes, 3);
            }

            List<Adquiriente> newEnajenatesOfEnajenacion = CombineListsForNewData(currentEnajenantes, adquirientes, enajenantes);
            newEnajenatesOfEnajenacion = ParceNegativePercentage(newEnajenatesOfEnajenacion);
            float totalSumPercentege = TotalSumFormPercentage(newEnajenatesOfEnajenacion);

            if (!isSumEqualTo100(totalSumPercentege))
            {
                newEnajenatesOfEnajenacion.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalSumPercentege));
            }

            return newEnajenatesOfEnajenacion;
        }

        private Enajenacion getLastUpdateOfAndSpecificEnajenacion(List<Enajenacion> enajenaciones)
        {

            if (enajenaciones.Count == 0)
            {
                return null;
            }
            else
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
        }
        #endregion
    }
}
