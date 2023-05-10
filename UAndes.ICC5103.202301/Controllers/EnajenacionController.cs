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
            model.SelectDescripcion = model.Descripcion[enajenacion.CNE];
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
            Enajenacion last_enajenacion = GetLastUpdateOfAndSpecificEnajenacion(enajenaciones);

            if (ModelState.IsValid)
            {
                FormCollection formCollection = new FormCollection(Request.Form);

                if (isRdp(enajenacion.CNE))
                {
                    List<Adquiriente> adquirientes = PastFormToAdquirienteModel(formCollection, enajenacion);
                    AddAdquirientesToHistory(adquirientes, enajenacion);
                    AddAdquirientesToDb(adquirientes);
                }
                else
                {
                    List<Adquiriente> adquirientes = PastFormToAdquirienteModel(formCollection, enajenacion);
                    List<Adquiriente> enajenantes = PastFormToEnajenanteModel(formCollection, enajenacion);
                    List<Adquiriente> newEnajenatesOfEnajenacion = CompraventaCases(enajenacion, last_enajenacion, adquirientes, enajenantes);
                    AddAdquirientesToDb(newEnajenatesOfEnajenacion);
                    AddAdquirientesToHistory(adquirientes, enajenacion);
                    AddEnajenantesToHistory(enajenantes, enajenacion);
                }

                //UpdateHistoricalInformation(enajenacion);
                db.Enajenacion.Add(enajenacion);
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
                Enajenacion enajenacion = GetLastUpdateOfAndSpecificEnajenacion(enajenaciones);
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

                adquirientes.Add(adquiriente);
            }

            return adquirientes;
        }

        private void AddAdquirientesToHistory(List<Adquiriente> adquirientes, Enajenacion enajenacion)
        {

            for (int i = 0; i < adquirientes.Count; i++)
            {
                var historico = new Historial();

                historico.IdEnajenacion = enajenacion.Id;
                historico.Comuna = enajenacion.Comuna;
                historico.Manzana = enajenacion.Manzana;
                historico.Predio = enajenacion.Predio;
                historico.Fojas = enajenacion.Fojas;
                historico.FechaInscripcion = enajenacion.FechaInscripcion;
                historico.IdInscripcion = enajenacion.IdInscripcion;
                historico.Rut = adquirientes[i].RutAdquiriente;
                historico.Porcentaje = adquirientes[i].PorcentajeAdquiriente;
                historico.CNE = enajenacion.CNE;
                historico.Check = adquirientes[i].CheckAdquiriente;
                historico.Participante = "adquiriente";

                db.Historial.Add(historico);
            }
        }

        private List<Adquiriente> PastFormToEnajenanteModel(FormCollection formCollection, Enajenacion enajenacion)
        {
            List<Adquiriente> enajenantes = new List<Adquiriente>();

            var ruts = formCollection["Enajenantes[0].RutEnajenante"].Split(',');
            var percentagesCheck = formCollection["Enajenantes[0].PorcentajeEnajenante"].Split(',');
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

                enajenantes.Add(adquiriente);
            }

            return enajenantes;
        }

        private void AddEnajenantesToHistory(List<Adquiriente> enajenanates, Enajenacion enajenacion)
        {

            for (int i = 0; i < enajenanates.Count; i++)
            {
                var historico = new Historial();

                historico.IdEnajenacion = enajenacion.Id;
                historico.Comuna = enajenacion.Comuna;
                historico.Manzana = enajenacion.Manzana;
                historico.Predio = enajenacion.Predio;
                historico.Fojas = enajenacion.Fojas;
                historico.FechaInscripcion = enajenacion.FechaInscripcion;
                historico.IdInscripcion = enajenacion.IdInscripcion;
                historico.Rut = enajenanates[i].RutAdquiriente;
                historico.Porcentaje = enajenanates[i].PorcentajeAdquiriente;
                historico.CNE = enajenacion.CNE;
                historico.Check = enajenanates[i].CheckAdquiriente;
                historico.Participante = "enajenante";

                db.Historial.Add(historico);
            }
        }

        private List<Adquiriente> UpdateEnajenatePercentageTotalTransfer(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes)
        {
            float transferValue = 0;
            foreach (var enajenante in enajenantes)
            {
                foreach (var currentEnajenante in currentEnajenantes)
                {
                    if (enajenante.RutAdquiriente == currentEnajenante.RutAdquiriente)
                    {
                        enajenante.PorcentajeAdquiriente = transferValue;
                      
                    }
                }
            }

            return enajenantes;
        }

        private List<Adquiriente> UpdateEnajenatePercentageByRights(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes)
        {
            foreach (var enajenante in enajenantes)
            {
                foreach (var currentEnajenante in currentEnajenantes)
                {
                    if (enajenante.RutAdquiriente == currentEnajenante.RutAdquiriente)
                    {
                        float ratioPercentage = RatioPercentage((float)enajenante.PorcentajeAdquiriente, (float)currentEnajenante.PorcentajeAdquiriente);
                        enajenante.PorcentajeAdquiriente = currentEnajenante.PorcentajeAdquiriente - ratioPercentage;
                    }
                }
            }

            return enajenantes;
        }

        private List<Adquiriente> UpdateEnajenatePercentageByDomain(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes)
        {
            foreach (var enajenante in enajenantes)
            {
                foreach (var currentEnajenante in currentEnajenantes)
                {
                    if (enajenante.RutAdquiriente == currentEnajenante.RutAdquiriente)
                    {
                        enajenante.PorcentajeAdquiriente = currentEnajenante.PorcentajeAdquiriente - enajenante.PorcentajeAdquiriente;
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
                            .Where(e => !enajenantes.Any(a => a.RutAdquiriente == e.RutAdquiriente))
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
        
        private List<Adquiriente> DeleteEnajenanteWithoutPercentage(List<Adquiriente> enajenantes)
        {
            List<Adquiriente> newEnajenates = enajenantes
                            .Where(e => e.PorcentajeAdquiriente > 0)
                            .ToList();

            return newEnajenates;
        }
        
        private List<Adquiriente> GetCurrentAdquirientes(Enajenacion enajenacion, int idEnajenacion)
        {
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>();

            if (isEnajenacion(enajenacion))
            {
                currentEnajenantes = db.Adquiriente
                           .Where(a => a.IdEnajenacion == enajenacion.Id)
                           .ToList();

                currentEnajenantes.ForEach(a => a.IdEnajenacion = idEnajenacion);

                return currentEnajenantes;  
            }
            else
            {
                return currentEnajenantes;
            }
        }
        
        private List<Adquiriente> CompraventaCases(Enajenacion enajenacion, Enajenacion lastEnajenacion, List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {
            float totalPercentagesEnajenantes;

            List<Adquiriente> currentEnajenantes = GetCurrentAdquirientes(lastEnajenacion, enajenacion.Id); 
            List<Adquiriente> enajenantesNotInForm = EnjanenatesNotInTheForm(currentEnajenantes, adquirientes, enajenantes);

            if (isSumAdquirienteEqual100(adquirientes))
            {
                totalPercentagesEnajenantes = TotalPercentageEnajenantes(enajenantes, currentEnajenantes);
                adquirientes.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalPercentagesEnajenantes));
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
                enajenantes = UpdateEnajenatePercentageTotalTransfer(currentEnajenantes, enajenantes);
                enajenantes = DeleteEnajenanteWithoutPercentage(enajenantes);    
            }
            else if (isOnlyOneAquirerAndAlienating(adquirientes, enajenantes))
            {
                totalPercentagesEnajenantes = TotalPercentageEnajenantes(enajenantes, currentEnajenantes);
                adquirientes.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalPercentagesEnajenantes));
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
                enajenantes = UpdateEnajenatePercentageByRights(currentEnajenantes, enajenantes);
            }
            else
            {
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
                enajenantes = UpdateEnajenatePercentageByDomain(currentEnajenantes, enajenantes);
            }

            List<Adquiriente> newEnajenatesOfEnajenacion = CombineListsForNewData(enajenantesNotInForm, adquirientes, enajenantes);
            newEnajenatesOfEnajenacion = ParceNegativePercentage(newEnajenatesOfEnajenacion);
            float totalSumPercentege = TotalSumFormPercentage(newEnajenatesOfEnajenacion);

            if (!isSumEqualTo100(totalSumPercentege))
            {
                newEnajenatesOfEnajenacion.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalSumPercentege));
            }

            return newEnajenatesOfEnajenacion;
        }

        private Enajenacion GetLastUpdateOfAndSpecificEnajenacion(List<Enajenacion> enajenaciones)
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
        
        private List<Historial> GetLogsOfEnajenacionAfter(int manzana, int predio, int comuna, DateTime fechaInscripcion)
        {
            List<Historial> historiales = db.Historial
                   .Where(l => l.Manzana == manzana && l.Predio == predio && l.FechaInscripcion > fechaInscripcion && l.Comuna == comuna)
                   .ToList();

            return historiales;
        }

        public List<DateTime> GetDistinctEnajenacionDates(List<Historial> historiales) 
        {
            var distinctDates = historiales
                .Select(e => e.FechaInscripcion)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            return distinctDates;
        }

        private List<Historial> FilterLogsOfEnajenacionByDate(DateTime fechaInscripcion, List<Historial> historiales)
        {
            List<Historial> filterHistoriales = historiales
                   .Where(l => l.FechaInscripcion == fechaInscripcion)
                   .ToList();

            return filterHistoriales;
        }

        private Enajenacion FilterEnajenacion(int manzana, int predio, int comuna, DateTime fechaInscripcion)
        {
            Enajenacion enajenacion = db.Enajenacion
               .FirstOrDefault(e => e.Manzana == manzana && e.Predio == predio && e.FechaInscripcion >= fechaInscripcion && e.Comuna == comuna);
               
            return enajenacion;
        }

        private int GetOperation(List<Historial> historiales)
        {
            int operation = historiales.FirstOrDefault().CNE;

            return operation;
        }

        private List<Adquiriente> PastLogsToEnajenanteModel(List<Historial> historiales)
        {
            List<Historial> filterHistoriales = historiales
                   .Where(l => l.Participante == "enajenante")
                   .ToList();
            List<Adquiriente> enajenantes = new List<Adquiriente>();

            foreach (var hist in filterHistoriales)
            {
                var adquiriente = new Adquiriente();
                adquiriente.RutAdquiriente = hist.Rut;
                adquiriente.IdEnajenacion = hist.IdEnajenacion;
                adquiriente.PorcentajeAdquiriente = hist.Porcentaje;
                adquiriente.CheckAdquiriente = hist.Check;

                enajenantes.Add(adquiriente);
            }

            return enajenantes;
        }

        private List<Adquiriente> PastLogsToAdquirientesModel(List<Historial> historiales)
        {
            List<Historial> filterHistoriales = historiales
                   .Where(l => l.Participante == "adquiriente")
                   .ToList();
            List<Adquiriente> adquirientes = new List<Adquiriente>();

            foreach (var hist in filterHistoriales)
            {
                var adquiriente = new Adquiriente();
                adquiriente.RutAdquiriente = hist.Rut;
                adquiriente.IdEnajenacion = hist.IdEnajenacion;
                adquiriente.PorcentajeAdquiriente = hist.Porcentaje;
                adquiriente.CheckAdquiriente = hist.Check;

                adquirientes.Add(adquiriente);
            }

            return adquirientes;
        }

        private async void RemoveFromAdquirientes(int idEnajenacion)
        {
            List<Adquiriente> adquirientes = await db.Adquiriente.Where(e => e.IdEnajenacion == idEnajenacion).ToListAsync();
            db.Adquiriente.RemoveRange(adquirientes);
        }

        private void UpdateHistoricalInformation(Enajenacion enajenacion)
        {
            List<Historial> historiales = GetLogsOfEnajenacionAfter(enajenacion.Manzana, enajenacion.Predio, enajenacion.Comuna, enajenacion.FechaInscripcion);
            List<DateTime> datesAfterHistory = GetDistinctEnajenacionDates(historiales); 

            foreach (var date in datesAfterHistory)
            {
                List<Historial> filterHistorial = FilterLogsOfEnajenacionByDate(date, historiales);
                Enajenacion lastEnajenacion = FilterEnajenacion(enajenacion.Manzana, enajenacion.Predio, enajenacion.Comuna, date);
                int operation = GetOperation(historiales);

                if (isRdp(operation)) 
                {
                    List<Adquiriente> adquirientes = PastLogsToAdquirientesModel(filterHistorial);
                    RemoveFromAdquirientes(lastEnajenacion.Id);
                    AddAdquirientesToDb(adquirientes);
                }
                else
                {
                    List<Adquiriente> adquirientes = PastLogsToAdquirientesModel(filterHistorial);
                    List<Adquiriente> enajenantes = PastLogsToEnajenanteModel(filterHistorial);
                    List<Adquiriente> newEnajenatesOfEnajenacion = CompraventaCases(enajenacion, lastEnajenacion, adquirientes, enajenantes);
                    RemoveFromAdquirientes(lastEnajenacion.Id);
                    AddAdquirientesToDb(newEnajenatesOfEnajenacion);
                }
            }
        }
        #endregion
    }
}
