using Antlr.Runtime.Misc;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Globalization;
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
        #region Constants
        private Grupo10ConchaMunozBrDbEntities db = new Grupo10ConchaMunozBrDbEntities();

        Dictionary<string, string> peopleCategories = new Dictionary<string, string>()
            {
                {"adquiriente", "adquiriente"},
                {"enajenante" , "enajenante"}
            };

        Dictionary<string, int> cneId = new Dictionary<string, int>()
            {
                {"Compraventa", 0},
                {"Regularizacion De Patrimonio" , 1}
            };

        bool vigencia = true;
        bool noVigencia = false;

        public class EnajenantesAndAdquirientes
        {
            public List<Adquiriente> Enajenantes { get; set; }
            public List<Adquiriente> Adquirientes { get; set; }
        }
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

        // GET: Enajenacion/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {


            Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);

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

        // POST: Enajenacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enajenacion enajenacion = await db.Enajenacion.FindAsync(id);

            if (enajenacion != null)
            {
                // Eliminar los registros relacionados en la tabla Adquiriente
                var adquirientesRelacionados = db.Adquiriente.Where(a => a.IdEnajenacion == id);
                db.Adquiriente.RemoveRange(adquirientesRelacionados);

                // Eliminar la entidad Enajenacion
                db.Enajenacion.Remove(enajenacion);

                // Guardar los cambios en la base de datos
                await db.SaveChangesAsync();

                // Redirigir a la página principal o a donde corresponda
                return RedirectToAction("Index");
            }

            // Si no se encuentra la entidad Enajenacion, manejar el error o retornar una vista de error
            return HttpNotFound();
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
            enajenacion.Vigente = vigencia;

            List<Enajenacion> enajenaciones = await db.Enajenacion
                   .Where(e => e.Manzana == enajenacion.Manzana 
                            && e.Predio == enajenacion.Predio 
                            && e.FechaInscripcion.Year <= enajenacion.FechaInscripcion.Year 
                            && e.Comuna == enajenacion.Comuna)
                   .ToListAsync();
            Enajenacion last_enajenacion = GetLastUpdateOfAndSpecificEnajenacion(enajenaciones);

            if (ModelState.IsValid)
            {
                FormCollection formCollection = new FormCollection(Request.Form);

                if (isRdp(enajenacion.CNE))
                {
                    List<Adquiriente> adquirientes = PastFormToAdquirienteModel(formCollection, enajenacion);
                    AddAdquirientesToHistory(formCollection, enajenacion);
                    AddAdquirientesToDb(adquirientes);
                }
                else
                {
                    List<Adquiriente> adquirientes = PastFormToAdquirienteModel(formCollection, enajenacion);
                    List<Adquiriente> enajenantes = PastFormToEnajenanteModel(formCollection, enajenacion);
                    List<Adquiriente> newEnajenatesOfEnajenacion = CompraventaCases(enajenacion, last_enajenacion, adquirientes, enajenantes);
                    AddAdquirientesToDb(newEnajenatesOfEnajenacion);
                    AddAdquirientesToHistory(formCollection, enajenacion);
                    AddEnajenantesToHistory(formCollection, enajenacion);
                }

                
                db.Enajenacion.Add(enajenacion);
                await db.SaveChangesAsync();

                if (isInscripcionIsDuplicate(enajenacion))
                {
                    await ChangeVigenciaEnajenacion(enajenacion);
                }

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
            if (cne == cneId["Regularizacion De Patrimonio"])
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

        private bool isInscripcionIsDuplicate(Enajenacion enajenacion)
        {
            bool exists = db.Enajenacion.Count(i => i.FechaInscripcion.Year == enajenacion.FechaInscripcion.Year
                                                 && i.IdInscripcion == enajenacion.IdInscripcion) > 1;

            return exists;
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

        private bool isEnajenateFantasma(List<Adquiriente> enajenantesFantasmas)
        {
            return enajenantesFantasmas.Count > 0;
        }

        private bool isEnajenateFantasmaEqualToEnajenantes(List<Adquiriente> enajenantesFantasmas, List<Adquiriente> enajenantes)
        {
            return enajenantesFantasmas.Count == enajenantes.Count;
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

        private bool isOnlyOneAdquirienteAndOneEnajenante(List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
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
            var culture = new CultureInfo("en-US");
            var floatPercentages = percentages.Select(p => float.Parse(p, culture)).ToList();
            var newPercentages = floatPercentages.Select(p => PercentageValue(p, differencePercentage)).ToList();

            return newPercentages;
        }

        private List<float> PercentagesToListEnajenante(string[] percentages)
        {
            var culture = new CultureInfo("en-US");
            var floatPercentages = percentages.Select(p => float.Parse(p, culture)).ToList();

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
                adquiriente.Fojas = enajenacion.Fojas;
                adquiriente.IdEnajenacion = enajenacion.Id;
                adquiriente.PorcentajeAdquiriente = percentagesParce[i];
                adquiriente.CheckAdquiriente = CheckValue(float.Parse(percentagesCheck[i]));

                adquirientes.Add(adquiriente);
            }

            return adquirientes;
        }

        private async Task ChangeVigenciaEnajenacion(Enajenacion enajenacion)
        {
            var matchingRecords = db.Enajenacion
                                    .Where(i => i.FechaInscripcion.Year == enajenacion.FechaInscripcion.Year
                                             && i.IdInscripcion == enajenacion.IdInscripcion)
                                    .OrderByDescending(i => i.Id)
                                    .ToList();

            for (int i = 0; i < matchingRecords.Count; i++)
            {
                var record = matchingRecords[i];
                record.Vigente = (i == 0) ? vigencia : noVigencia;
            }

            await db.SaveChangesAsync();
        }

        private void AddAdquirientesToHistory(FormCollection formCollection, Enajenacion enajenacion)
        {
            var ruts = formCollection["Adquirientes[0].RutAdquiriente"].Split(',');
            var percentagesCheck = formCollection["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
            var percentages = formCollection["Adquirientes[0].PorcentajeAdquiriente"].Split(',');
            List<float> percentagesParce = PercentagesToListAdquiriente(percentages);

            for (int i = 0; i < ruts.Length; i++)
            {
                var historico = new Historial();

                historico.Eliminado = false;
                historico.IdEnajenacion = enajenacion.Id;
                historico.Comuna = enajenacion.Comuna;
                historico.Manzana = enajenacion.Manzana;
                historico.Predio = enajenacion.Predio;
                historico.Fojas = enajenacion.Fojas;
                historico.FechaInscripcion = enajenacion.FechaInscripcion;
                historico.IdInscripcion = enajenacion.IdInscripcion;
                string rut = ruts[i];
                historico.Rut = rut;
                historico.Porcentaje = percentagesParce[i];
                historico.CNE = enajenacion.CNE;
                historico.Check = CheckValue(float.Parse(percentagesCheck[i]));
                historico.Participante = peopleCategories["adquiriente"];

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
                adquiriente.Fojas = enajenacion.Fojas;
                adquiriente.PorcentajeAdquiriente = percentagesParce[i];
                adquiriente.CheckAdquiriente = CheckValue(float.Parse(percentagesCheck[i]));

                enajenantes.Add(adquiriente);
            }

            return enajenantes;
        }

        private void AddEnajenantesToHistory(FormCollection formCollection, Enajenacion enajenacion)
        {
            var ruts = formCollection["Enajenantes[0].RutEnajenante"].Split(',');
            var percentagesCheck = formCollection["Enajenantes[0].PorcentajeEnajenante"].Split(',');
            var percentages = formCollection["Enajenantes[0].PorcentajeEnajenante"].Split(',');
            List<float> percentagesParce = PercentagesToListEnajenante(percentages);

            for (int i = 0; i < ruts.Length; i++)
            {
                var historico = new Historial();

                historico.Eliminado = false;
                historico.IdEnajenacion = enajenacion.Id;
                historico.Comuna = enajenacion.Comuna;
                historico.Manzana = enajenacion.Manzana;
                historico.Predio = enajenacion.Predio;
                historico.Fojas = enajenacion.Fojas;
                historico.FechaInscripcion = enajenacion.FechaInscripcion;
                historico.IdInscripcion = enajenacion.IdInscripcion;
                string rut = ruts[i];
                historico.Rut = rut;
                historico.Porcentaje = percentagesParce[i];
                historico.CNE = enajenacion.CNE;
                historico.Check = CheckValue(float.Parse(percentagesCheck[i]));
                historico.Participante = peopleCategories["enajenante"];

                db.Historial.Add(historico);
            }
        }

        private List<Adquiriente> UpdateEnajenatePercentageTotalTransfer(List<Adquiriente> enajenantes)
        {
            float transferValue = 0;
            foreach (var enajenante in enajenantes)
            {
                enajenante.PorcentajeAdquiriente = transferValue;     
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
                        enajenante.Fojas = currentEnajenante.Fojas;
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

        private List<Adquiriente> UpdatePercentageForEnajenantesFantasmas(List<Adquiriente> enajenantes, List<Adquiriente> enajenantesFantasmas)
        {
            foreach (var enajenante in enajenantes)
            {
                var matchingEnajenanteFantasma = enajenantesFantasmas.FirstOrDefault(e => e.RutAdquiriente == enajenante.RutAdquiriente);

                if (matchingEnajenanteFantasma != null)
                {
                    enajenante.PorcentajeAdquiriente = 0;
                }
            }

            return enajenantes;
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

        private List<Adquiriente> EnjanenatesNotInTheForm(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes)
        {
            List<Adquiriente> enajenantesNotInTheForm = currentEnajenantes
                            .Where(e => !enajenantes.Any(a => a.RutAdquiriente == e.RutAdquiriente))
                            .ToList();
    
            return enajenantesNotInTheForm;
        }

        private List<Adquiriente> AdquirientesNotInTheForm(List<Adquiriente> currentEnajenantes, List<Adquiriente> adquirientes)
        {
            List<Adquiriente> adquirientesNotInTheForm = currentEnajenantes
                            .Where(e => !adquirientes.Any(a => a.RutAdquiriente == e.RutAdquiriente))
                            .ToList();

            return adquirientesNotInTheForm;
        }

        private List<Adquiriente> CombineListsForNewData(List<Adquiriente> currentEnajenantes, List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {
            List<Adquiriente> combinedList = currentEnajenantes
                          .Concat(adquirientes)
                          .Concat(enajenantes)
                          .ToList();

            return combinedList;
        }

        private List<Adquiriente> AddEnajenantesFantasmasToCurrentEnajenantes(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes)
        {
            List<Adquiriente> combinedList = currentEnajenantes
                                       .Concat(enajenantes
                                           .Select(e => new Adquiriente
                                           {
                                               RutAdquiriente = e.RutAdquiriente,
                                               Fojas = e.Fojas,
                                               IdEnajenacion = e.IdEnajenacion,
                                               PorcentajeAdquiriente = e.PorcentajeAdquiriente,
                                               CheckAdquiriente = e.CheckAdquiriente
                                           }))
                                       .ToList();

            return combinedList;
        }

        private List<Adquiriente> CurrentEnajenteIsFantasmaChangePercentage(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantesFantasmas)
        {
            foreach (var enajenante in currentEnajenantes)
            {
                if (enajenantesFantasmas.Any(e => e.RutAdquiriente == enajenante.RutAdquiriente))
                {
                    enajenante.PorcentajeAdquiriente = 100;
                }
            }

            return currentEnajenantes;
        }

        private List<Adquiriente> DeleteEnajenanteWithoutPercentage(List<Adquiriente> enajenantes)
        {
            List<Adquiriente> newEnajenates = enajenantes
                            .Where(e => e.PorcentajeAdquiriente > 0)
                            .ToList();

            return newEnajenates;
        }
        
        private List<Adquiriente> GetCurrentOwners(Enajenacion enajenacion, int idEnajenacion)
        {
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>();

            if (isEnajenacion(enajenacion))
            {
                List<Adquiriente> currentEnajenantesFromDataBase = db.Adquiriente
                           .Where(a => a.IdEnajenacion == enajenacion.Id)
                           .ToList();

                foreach (var enajenante in currentEnajenantesFromDataBase)
                {
                    var adquiriente = new Adquiriente();
                    adquiriente.RutAdquiriente = enajenante.RutAdquiriente;
                    adquiriente.IdEnajenacion = idEnajenacion;
                    adquiriente.Fojas = enajenacion.Fojas;
                    adquiriente.PorcentajeAdquiriente = enajenante.PorcentajeAdquiriente;
                    adquiriente.CheckAdquiriente = enajenante.CheckAdquiriente;

                    currentEnajenantes.Add(adquiriente);
                }

                return currentEnajenantes;  
            }
            else
            {
                return currentEnajenantes;
            }
        }

        private List<Adquiriente> GetEnajenatesFantasmas(List<Adquiriente> currentEnajenates, List<Adquiriente> enajenantes)
        {
            List<Adquiriente> enajenatesFantasmas = enajenantes.Where(e => !currentEnajenates.Any(c => c.RutAdquiriente == e.RutAdquiriente)).ToList();
            return enajenatesFantasmas;
        }

        private EnajenantesAndAdquirientes CaseSumAdquirienteEqual100(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes, 
            List<Adquiriente> adquirientes, List<Adquiriente> enajenantesFantasmas)
        {
            if (!isEnajenateFantasmaEqualToEnajenantes(enajenantesFantasmas, enajenantes))
            {
                float totalPercentagesEnajenantes = TotalPercentageEnajenantes(enajenantes, currentEnajenantes);
                adquirientes.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalPercentagesEnajenantes));
                adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);  
            }

            enajenantes = UpdateEnajenatePercentageTotalTransfer(enajenantes);
            enajenantes = DeleteEnajenanteWithoutPercentage(enajenantes);

            return new EnajenantesAndAdquirientes
            {
                Enajenantes = enajenantes,
                Adquirientes = adquirientes
            };
        }

        private EnajenantesAndAdquirientes CaseOnlyOneAdquirienteAndOneEnajenante(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes, 
            List<Adquiriente> adquirientes, List<Adquiriente> enajenantesFantasmas)
        {

            if (isEnajenateFantasma(enajenantesFantasmas))
            {
                currentEnajenantes = AddEnajenantesFantasmasToCurrentEnajenantes(currentEnajenantes, enajenantes);
                currentEnajenantes = CurrentEnajenteIsFantasmaChangePercentage(currentEnajenantes, enajenantesFantasmas);
            }

            float totalPercentagesEnajenantes = TotalPercentageEnajenantes(enajenantes, currentEnajenantes);
            adquirientes.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalPercentagesEnajenantes));
            adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
            enajenantes = UpdateEnajenatePercentageByRights(currentEnajenantes, enajenantes);


            return new EnajenantesAndAdquirientes
            {
                Enajenantes = enajenantes,
                Adquirientes = adquirientes
            };
        }

        private EnajenantesAndAdquirientes CaseDefault(List<Adquiriente> currentEnajenantes, List<Adquiriente> enajenantes, 
            List<Adquiriente> adquirientes, List<Adquiriente> enajenantesFantasmas, List<Adquiriente> enajenantesNotInForm)
        {
            adquirientes = UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);
            enajenantes = UpdateEnajenatePercentageByDomain(currentEnajenantes, enajenantes);

            if (isEnajenateFantasma(enajenantesFantasmas))
            {
                enajenantes = UpdatePercentageForEnajenantesFantasmas(enajenantes, enajenantesFantasmas);
                List<Adquiriente> newEnajenatesOfEnajenacion = CombineListsForNewData(enajenantesNotInForm, adquirientes, enajenantes);
                float totalSumPercentege = TotalSumFormPercentage(newEnajenatesOfEnajenacion);

                if (!isSumEqualTo100(totalSumPercentege))
                {
                    enajenantes.ForEach(a =>
                    {
                        if (a.PorcentajeAdquiriente == 0)
                        {
                            a.PorcentajeAdquiriente = (100 - totalSumPercentege) / enajenantesFantasmas.Count;
                        }
                    });
                }
            }

            return new EnajenantesAndAdquirientes
            {
                Enajenantes = enajenantes,
                Adquirientes = adquirientes
            };
        }

        private List<Adquiriente> CompraventaCases(Enajenacion enajenacion, Enajenacion lastEnajenacion, List<Adquiriente> adquirientes, List<Adquiriente> enajenantes)
        {  
            List<Adquiriente> currentEnajenantes = GetCurrentOwners(lastEnajenacion, enajenacion.Id);
            List<Adquiriente> newcurrentEnajenantes = EnjanenatesNotInTheForm(currentEnajenantes, enajenantes);
            List<Adquiriente> enajenantesNotInForm = AdquirientesNotInTheForm(newcurrentEnajenantes, adquirientes); 
            List<Adquiriente> enajenantesFantasmas = GetEnajenatesFantasmas(currentEnajenantes, enajenantes);

            if (isSumAdquirienteEqual100(adquirientes))
            {
                var result = CaseSumAdquirienteEqual100(currentEnajenantes, enajenantes, adquirientes, enajenantesFantasmas);
                enajenantes = result.Enajenantes;
                adquirientes = result.Adquirientes;
            }
            else if (isOnlyOneAdquirienteAndOneEnajenante(adquirientes, enajenantes))
            {
                var result = CaseOnlyOneAdquirienteAndOneEnajenante(currentEnajenantes, enajenantes, adquirientes, enajenantesFantasmas);
                enajenantes = result.Enajenantes;
                adquirientes = result.Adquirientes;
            }
            else
            {
                var result = CaseDefault(currentEnajenantes, enajenantes, adquirientes, enajenantesFantasmas, enajenantesNotInForm);
                enajenantes = result.Enajenantes;
                adquirientes = result.Adquirientes;
            }

            List<Adquiriente> newEnajenatesOfEnajenacion = CombineListsForNewData(enajenantesNotInForm, adquirientes, enajenantes);
            newEnajenatesOfEnajenacion = ParceNegativePercentage(newEnajenatesOfEnajenacion);
            float totalSumPercentege = TotalSumFormPercentage(newEnajenatesOfEnajenacion);

            if (!isSumEqualTo100(totalSumPercentege))
            {
                newEnajenatesOfEnajenacion.ForEach(a => a.PorcentajeAdquiriente = RatioPercentage((float)a.PorcentajeAdquiriente, totalSumPercentege)); 
            }

            newEnajenatesOfEnajenacion = DeleteEnajenanteWithoutPercentage(newEnajenatesOfEnajenacion);

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
        
        public List<DateTime> GetDistinctEnajenacionDates(List<Historial> historiales) 
        {
            var distinctDates = historiales
                .Select(e => e.FechaInscripcion)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            return distinctDates;
        }

        #endregion
    }
}
