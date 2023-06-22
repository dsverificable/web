using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UAndes.ICC5103._202301.Controllers;
using UAndes.ICC5103._202301.Models;
using static UAndes.ICC5103._202301.Controllers.EnajenacionController;

namespace UAndes.ICC5103._202301Test.ControllersTest
{
    [TestClass]
    public class EnajenacionControllerTests
    {
        private EnajenacionController controller; 

        [TestInitialize]
        public void TestInitialize()
        {
            controller = new EnajenacionController(); 
        }

        [TestMethod]
        public void TestIsRdp_WithRdpCne_ShouldReturnTrue()
        {
            // Arrange
            int cne = 1;
     
            // Act
            bool result = controller.isRdp(cne); 

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsId_WithNonNullId_ShouldReturnTrue()
        {
            // Arrange
            int? id = 10;

            // Act
            bool result = controller.isId(id); 

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsEnajenacion_WithNonNullEnajenacion_ShouldReturnTrue()
        {
            // Arrange
            var enajenacion = new Enajenacion(); 

            // Act
            bool result = controller.isEnajenacion(enajenacion); 

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsEnajenateFantasma_WithNonEmptyList_ShouldReturnTrue()
        {
            // Arrange
            var enajenantesFantasmas = new List<Adquiriente> { new Adquiriente(), new Adquiriente() }; 

            // Act
            bool result = controller.isEnajenateFantasma(enajenantesFantasmas); 

            // Assert
            Assert.IsTrue(result); 
        }

        [TestMethod]
        public void TestIsEnajenateFantasmaEqualToEnajenantes_WithEqualLists_ShouldReturnTrue()
        {
            // Arrange
            var enajenantesFantasmas = new List<Adquiriente> { new Adquiriente(), new Adquiriente() }; 
            var enajenantes = new List<Adquiriente> { new Adquiriente(), new Adquiriente() }; 

            // Act
            bool result = controller.isEnajenateFantasmaEqualToEnajenantes(enajenantesFantasmas, enajenantes); 

            // Assert
            Assert.IsTrue(result); 
        }

        [TestMethod]
        public void TestIsSumAdquirienteEqual100_WithSum100_ShouldReturnTrue()
        {
            // Arrange
            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { PorcentajeAdquiriente = 50 },
                new Adquiriente { PorcentajeAdquiriente = 25 },
                new Adquiriente { PorcentajeAdquiriente = 25 }
            };

            // Act
            bool result = controller.isSumAdquirienteEqual100(adquirientes);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsOnlyOneAdquirienteAndOneEnajenante_WithOneAdquirienteOneEnajenante_ShouldReturnTrue()
        {
            // Arrange
            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente()
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente()
            };

            // Act
            bool result = controller.isOnlyOneAdquirienteAndOneEnajenante(adquirientes, enajenantes);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsSumEqualTo100_AndSumWithTotalSum100_ShouldReturnTrue()
        {
            // Arrange
            float totalSumPercenteges = 100;

            // Act
            bool result = controller.isSumEqualTo100(totalSumPercenteges);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsSumEqualTo100_WithTotalSum100_ShouldReturnTrue()
        {
            // Arrange
            float totalSumPercenteges = 100;

            // Act
            bool result = controller.isSumEqualTo100(totalSumPercenteges);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestIsLastEnajenacionEqualToCurrentEnajenacion_WithEqualEnajenaciones_ShouldReturnFalse()
        {
            // Arrange
            Enajenacion currentEnajenacion = new Enajenacion();
            Enajenacion lastEnajenacion = new Enajenacion();

            // Act
            bool result = controller.isLastEnajenacionEqualToCurrentEnajenacion(currentEnajenacion, lastEnajenacion);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestCheckValue_WithZeroPercentage_ShouldReturnTrue()
        {
            // Arrange
            float percentage = 0;

            // Act
            bool result = controller.CheckValue(percentage);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestTotalSumFormPercentage_CalculatesTotalSumCorrectly()
        {
            // Arrange
            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "1-1", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "2-2", PorcentajeAdquiriente = 40 }
            };

            float expectedTotalSum = 70;

            // Act
            float actualTotalSum = controller.TotalSumFormPercentage(enajenantes);

            // Assert
            Assert.AreEqual(expectedTotalSum, actualTotalSum);
        }

        [TestMethod]
        public void TestTotalPercentageEnajenantes_CalculatesTotalPercentageCorrectly()
        {
            // Arrange
            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "1234-5", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "6789-0", PorcentajeAdquiriente = 40 }
            };

            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "1234-5", PorcentajeAdquiriente = 20 },
                new Adquiriente { RutAdquiriente = "6789-0", PorcentajeAdquiriente = 35 }
            };

            float expectedTotalPercentage = 55;

            // Act
            float actualTotalPercentage = controller.TotalPercentageEnajenantes(enajenantes, currentEnajenantes);

            // Assert
            Assert.AreEqual(expectedTotalPercentage, actualTotalPercentage);
        }

        [TestMethod]
        public void TestDifferencePercentage_CalculatesDifferenceCorrectly()
        {
            // Arrange
            string[] percentages = { "30", "20", "0", "10" };
            float expectedDifference = 40;

            // Act
            float actualDifference = controller.DifferencePercentage(percentages);

            // Assert
            Assert.AreEqual(expectedDifference, actualDifference);
        }

        [TestMethod]
        public void TestPercentageValue_ReturnsPercentageWhenGreaterThanZero()
        {
            // Arrange
            float percentage = 30;
            float differencePercentage = 10;
            float expectedValue = 30;

            // Act
            float actualValue = controller.PercentageValue(percentage, differencePercentage);

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestRatioPercentage_WhenTotalPercentageGreaterThan100()
        {
            // Arrange
            float userPercentage = 30;
            float totalPercentage = 120;
            float expectedRatio = 25;

            // Act
            float actualRatio = controller.RatioPercentage(userPercentage, totalPercentage);

            // Assert
            Assert.AreEqual(expectedRatio, actualRatio);
        }

        [TestMethod]
        public void TestPercentagesToListAdquiriente_WithPositivePercentages()
        {
            // Arrange
            string[] percentages = { "20", "30", "50" };
            float[] expectedNewPercentages = { 20, 30, 50 };

            // Act
            List<float> actualNewPercentages = controller.PercentagesToListAdquiriente(percentages);

            // Assert
            CollectionAssert.AreEqual(expectedNewPercentages, actualNewPercentages);
        }

        [TestMethod]
        public void TestPercentagesToListEnajenante_WithPositivePercentages()
        {
            // Arrange
            string[] percentages = { "20", "30", "50" };
            float[] expectedFloatPercentages = { 20, 30, 50 };

            // Act
            List<float> actualFloatPercentages = controller.PercentagesToListEnajenante(percentages);

            // Assert
            CollectionAssert.AreEqual(expectedFloatPercentages, actualFloatPercentages);
        }

        [TestMethod]
        public void TestUpdateEnajenatePercentageTotalTransfer_WithValidData()
        {
            // Arrange
            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { PorcentajeAdquiriente = 50 },
                new Adquiriente { PorcentajeAdquiriente = 30 },
                new Adquiriente { PorcentajeAdquiriente = 20 }
            };

            // Act
            List<Adquiriente> updatedEnajenantes = controller.UpdateEnajenatePercentageTotalTransfer(enajenantes);

            // Assert
            Assert.AreEqual(3, updatedEnajenantes.Count);
            Assert.AreEqual(0, updatedEnajenantes[0].PorcentajeAdquiriente);
            Assert.AreEqual(0, updatedEnajenantes[1].PorcentajeAdquiriente);
            Assert.AreEqual(0, updatedEnajenantes[2].PorcentajeAdquiriente);
        }

        [TestMethod]
        public void TestUpdateEnajenatePercentageByRights_WithValidData()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 50 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 60 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 70 }
            };

            // Act
            List<Adquiriente> updatedEnajenantes = controller.UpdateEnajenatePercentageByRights(currentEnajenantes, enajenantes);

            // Assert
            Assert.AreEqual(3, updatedEnajenantes.Count);
        }

        [TestMethod]
        public void TestUpdateEnajenatePercentageByDomain_WithValidData()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40},
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30},
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20}
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 50 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 60 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 70 }
            };

            // Act
            List<Adquiriente> updatedEnajenantes = controller.UpdateEnajenatePercentageByDomain(currentEnajenantes, enajenantes);

            // Assert
            Assert.AreEqual(3, updatedEnajenantes.Count);
        }

        [TestMethod]
        public void TestUpdateAdquirientesPercentage_WithValidData()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 10 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 20 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 30 }
            };

            // Act
            List<Adquiriente> updatedAdquirientes = controller.UpdateAdquirientesPercentage(currentEnajenantes, adquirientes);

            // Assert
            Assert.AreEqual(3, updatedAdquirientes.Count);
            Assert.AreEqual(50, updatedAdquirientes[0].PorcentajeAdquiriente);
            Assert.AreEqual(50, updatedAdquirientes[1].PorcentajeAdquiriente);
            Assert.AreEqual(50, updatedAdquirientes[2].PorcentajeAdquiriente);
        }

        [TestMethod]
        public void TestUpdatePercentageForEnajenantesFantasmas_WithMatchingEnajenanteFantasma()
        {
            // Arrange
            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> enajenantesFantasmas = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 0 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 0 }
            };

            // Act
            List<Adquiriente> updatedEnajenantes = controller.UpdatePercentageForEnajenantesFantasmas(enajenantes, enajenantesFantasmas);

            // Assert
            Assert.AreEqual(3, updatedEnajenantes.Count);
            Assert.AreEqual(40, updatedEnajenantes[0].PorcentajeAdquiriente);
            Assert.AreEqual(0, updatedEnajenantes[1].PorcentajeAdquiriente);
            Assert.AreEqual(0, updatedEnajenantes[2].PorcentajeAdquiriente);
        }

        [TestMethod]
        public void TestParceNegativePercentage_WithNegativePercentage()
        {
            // Arrange
            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = -30 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            // Act
            List<Adquiriente> updatedAdquirientes = controller.ParceNegativePercentage(adquirientes);

            // Assert
            Assert.AreEqual(3, updatedAdquirientes.Count);
            Assert.AreEqual(40, updatedAdquirientes[0].PorcentajeAdquiriente);
            Assert.AreEqual(0, updatedAdquirientes[1].PorcentajeAdquiriente);
            Assert.AreEqual(20, updatedAdquirientes[2].PorcentajeAdquiriente);
        }

        [TestMethod]
        public void TestEnjanenatesNotInTheForm_WithEnajenantesNotInTheForm()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            // Act
            List<Adquiriente> enajenantesNotInTheForm = controller.EnjanenatesNotInTheForm(currentEnajenantes, enajenantes);

            // Assert
            Assert.AreEqual(1, enajenantesNotInTheForm.Count);
            Assert.AreEqual("45-6", enajenantesNotInTheForm[0].RutAdquiriente);
        }

        [TestMethod]
        public void TestAdquirientesNotInTheForm_WithAdquirientesNotInTheForm()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            // Act
            List<Adquiriente> adquirientesNotInTheForm = controller.AdquirientesNotInTheForm(currentEnajenantes, adquirientes);

            // Assert
            Assert.AreEqual(1, adquirientesNotInTheForm.Count);
            Assert.AreEqual("45-6", adquirientesNotInTheForm[0].RutAdquiriente);
        }

        [TestMethod]
        public void TestCombineListsForNewData_WithValidLists()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "32-1", PorcentajeAdquiriente = 10 },
                new Adquiriente { RutAdquiriente = "65-4", PorcentajeAdquiriente = 5 }
            };

            // Act
            List<Adquiriente> combinedList = controller.CombineListsForNewData(currentEnajenantes, adquirientes, enajenantes);

            // Assert
            Assert.AreEqual(5, combinedList.Count);
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "12-3"));
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "45-6"));
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "78-9"));
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "32-1"));
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "65-4"));
        }

        [TestMethod]
        public void TestAddEnajenantesFantasmasToCurrentEnajenantes_WithValidLists()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 },
            };

            // Act
            List<Adquiriente> combinedList = controller.AddEnajenantesFantasmasToCurrentEnajenantes(currentEnajenantes, enajenantes);

            // Assert
            Assert.AreEqual(3, combinedList.Count);
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "12-3"));
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "45-6"));
            Assert.IsTrue(combinedList.Any(a => a.RutAdquiriente == "78-9"));
        }

        [TestMethod]
        public void TestCurrentEnajenteIsFantasmaChangePercentage_WithValidLists()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            List<Adquiriente> enajenantesFantasmas = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "45-6" }
            };

            // Act
            List<Adquiriente> updatedEnajenantes = controller.CurrentEnajenteIsFantasmaChangePercentage(currentEnajenantes, enajenantesFantasmas);

            // Assert
            Assert.AreEqual(2, updatedEnajenantes.Count);
            Assert.AreEqual(40, updatedEnajenantes.First(a => a.RutAdquiriente == "12-3").PorcentajeAdquiriente);
            Assert.AreEqual(100, updatedEnajenantes.First(a => a.RutAdquiriente == "45-6").PorcentajeAdquiriente);
        }

        [TestMethod]
        public void TestDeleteEnajenanteWithoutPercentage_WithValidList()
        {
            // Arrange
            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 0 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 60 }
            };

            // Act
            List<Adquiriente> updatedEnajenantes = controller.DeleteEnajenanteWithoutPercentage(enajenantes);

            // Assert
            Assert.AreEqual(2, updatedEnajenantes.Count);
            Assert.IsTrue(updatedEnajenantes.Any(a => a.RutAdquiriente == "12-3"));
            Assert.IsTrue(updatedEnajenantes.Any(a => a.RutAdquiriente == "78-9"));
            Assert.IsFalse(updatedEnajenantes.Any(a => a.RutAdquiriente == "45-6"));
        }

        [TestMethod]
        public void TestGetEnajenatesFantasmas_WhenCurrentEnajenatesExist_ReturnsEnajenatesFantasmas()
        {
            // Arrange
            List<Adquiriente> currentEnajenates = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 50 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 70 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
           
            };

            // Act
            List<Adquiriente> result = controller.GetEnajenatesFantasmas(currentEnajenates, enajenantes);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("78-9", result[0].RutAdquiriente);
            Assert.AreEqual(20, result[0].PorcentajeAdquiriente);
        }

        [TestMethod]
        public void TestCaseSumAdquirienteEqual100_WhenEnajenanteFantasmaNotEqualToEnajenantes_UpdatesAdquirientes()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 50 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 70 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> enajenantesFantasmas = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 10 }
            };

            // Act
            EnajenantesAndAdquirientes result = controller.CaseSumAdquirienteEqual100(currentEnajenantes, enajenantes, adquirientes, enajenantesFantasmas);

            // Assert
            Assert.AreEqual(2, result.Adquirientes.Count);
        }

        [TestMethod]
        public void TestCaseOnlyOneAdquirienteAndOneEnajenante_WhenEnajenateFantasmaExistsAndLastEnajenacionIsNull_UpdatesAdquirientesAndEnajenantes()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 50 },
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 70 },
            };

            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
            };

            List<Adquiriente> enajenantesFantasmas = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            Enajenacion lastEnajenacion = null;

            // Act
            EnajenantesAndAdquirientes result = controller.CaseOnlyOneAdquirienteAndOneEnajenante(currentEnajenantes, enajenantes, adquirientes, enajenantesFantasmas, lastEnajenacion);

            // Assert
            Assert.AreEqual(1, result.Adquirientes.Count);
            Assert.AreEqual(1, result.Enajenantes.Count);
        }

        [TestMethod]
        public void TestCaseDefault_UpdatesAdquirientesAndEnajenantes()
        {
            // Arrange
            List<Adquiriente> currentEnajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 50 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 30 }
            };

            List<Adquiriente> enajenantes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 70 },
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> adquirientes = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 40 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 20 }
            };

            List<Adquiriente> enajenantesFantasmas = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "78-9", PorcentajeAdquiriente = 10 }
            };

            List<Adquiriente> enajenantesNotInForm = new List<Adquiriente>
            {
                new Adquiriente { RutAdquiriente = "12-3", PorcentajeAdquiriente = 10 },
                new Adquiriente { RutAdquiriente = "45-6", PorcentajeAdquiriente = 20 }
            };

           
            // Act
            EnajenantesAndAdquirientes result = controller.CaseDefault(currentEnajenantes, enajenantes, adquirientes, enajenantesFantasmas, enajenantesNotInForm);

            // Assert
            Assert.AreEqual(2, result.Adquirientes.Count);
            Assert.AreEqual(2, result.Enajenantes.Count);
        }

        [TestMethod]
        public void TestGetLastUpdateOfAndSpecificEnajenacion_WhenEnajenacionesExist_ReturnsLastUpdate()
        {
            // Arrange
            List<Enajenacion> enajenaciones = new List<Enajenacion>
            {
                new Enajenacion { IdInscripcion = 1, FechaInscripcion = new DateTime(2022, 1, 1), Vigente = true },
                new Enajenacion { IdInscripcion = 2, FechaInscripcion = new DateTime(2022, 2, 1), Vigente = false },
                new Enajenacion { IdInscripcion = 3, FechaInscripcion = new DateTime(2023, 1, 1), Vigente = true },
                new Enajenacion { IdInscripcion = 4, FechaInscripcion = new DateTime(2023, 2, 1), Vigente = true },
                new Enajenacion { IdInscripcion = 5, FechaInscripcion = new DateTime(2023, 2, 1), Vigente = false }
            };

            // Act
            Enajenacion result = controller.GetLastUpdateOfAndSpecificEnajenacion(enajenaciones);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.IdInscripcion);
            Assert.AreEqual(new DateTime(2023, 2, 1), result.FechaInscripcion);
            Assert.IsTrue(result.Vigente);
        }

        [TestMethod]
        public void TestGetDistinctEnajenacionDates_WhenHistorialesExist_ReturnsDistinctDates()
        {
            // Arrange
            List<Historial> historiales = new List<Historial>
            {
                new Historial { FechaInscripcion = new DateTime(2022, 1, 1) },
                new Historial { FechaInscripcion = new DateTime(2022, 2, 1) },
                new Historial { FechaInscripcion = new DateTime(2022, 2, 1) },
                new Historial { FechaInscripcion = new DateTime(2023, 1, 1) },
                new Historial { FechaInscripcion = new DateTime(2023, 2, 1) }
            };

            // Act
            List<DateTime> result = controller.GetDistinctEnajenacionDates(historiales);

            // Assert
            Assert.AreEqual(4, result.Count);
                CollectionAssert.AreEqual(new List<DateTime>
            {
                new DateTime(2022, 1, 1),
                new DateTime(2022, 2, 1),
                new DateTime(2023, 1, 1),
                new DateTime(2023, 2, 1)
            }, result);
        }

        [TestMethod]
        public void TestFilterLogsOfEnajenacionByDate_WhenHistorialesExist_ReturnsFilteredHistoriales()
        {
            // Arrange
            DateTime fechaInscripcion = new DateTime(2022, 1, 1);

            List<Historial> historiales = new List<Historial>
            {
                new Historial { FechaInscripcion = new DateTime(2022, 1, 1), Id = 1 },
                new Historial { FechaInscripcion = new DateTime(2022, 1, 1), Id = 2 },
                new Historial { FechaInscripcion = new DateTime(2022, 2, 1), Id = 3 },
                new Historial { FechaInscripcion = new DateTime(2023, 1, 1), Id = 4 },
                new Historial { FechaInscripcion = new DateTime(2023, 2, 1), Id = 5 }
            };

            // Act
            List<Historial> result = controller.FilterLogsOfEnajenacionByDate(fechaInscripcion, historiales);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [TestMethod]
        public void TestGetOperation_WhenHistorialesNotEmpty_ReturnsOperation()
        {
            // Arrange
            List<Historial> historiales = new List<Historial>
            {
                new Historial { CNE = 123},
                new Historial { CNE = 456 },
                new Historial { CNE = 789, }
            };

            // Act
            int operation = controller.GetOperation(historiales);

            // Assert
            Assert.AreEqual(123, operation);
        }

        [TestMethod]
        public void TestPastLogsToEnajenanteModel_WhenHistorialesExist_ReturnsEnajenantes()
        {
            // Arrange
            List<Historial> historiales = new List<Historial>
            {
                new Historial { Participante = "enajenante", Rut = "12-3", IdEnajenacion = 1, Porcentaje = 50, Check = true },
                new Historial { Participante = "adquiriente", Rut = "45-6", IdEnajenacion = 1, Porcentaje = 30, Check = false },
                new Historial { Participante = "enajenante", Rut = "78-9", IdEnajenacion = 1, Porcentaje = 20, Check = true }
            };

            // Act
            List<Adquiriente> result = controller.PastLogsToEnajenanteModel(historiales);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("12-3", result[0].RutAdquiriente);
            Assert.AreEqual(1, result[0].IdEnajenacion);
            Assert.AreEqual(50, result[0].PorcentajeAdquiriente);
            Assert.AreEqual(true, result[0].CheckAdquiriente);
            Assert.AreEqual("78-9", result[1].RutAdquiriente);
            Assert.AreEqual(1, result[1].IdEnajenacion);
            Assert.AreEqual(20, result[1].PorcentajeAdquiriente);
            Assert.AreEqual(true, result[1].CheckAdquiriente);
        }

        [TestMethod]
        public void TestPastLogsToAdquirientesModel_WhenHistorialesExist_ReturnsAdquirientes()
        {
            // Arrange
            List<Historial> historiales = new List<Historial>
            {
                new Historial { Participante = "enajenante", Rut = "12-3", IdEnajenacion = 1, Porcentaje = 50, Check = true },
                new Historial { Participante = "adquiriente", Rut = "45-6", IdEnajenacion = 1, Porcentaje = 30, Check = false },
                new Historial { Participante = "adquiriente", Rut = "78-9", IdEnajenacion = 1, Porcentaje = 20, Check = true }
            };

            // Act
            List<Adquiriente> result = controller.PastLogsToAdquirientesModel(historiales);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("45-6", result[0].RutAdquiriente);
            Assert.AreEqual(1, result[0].IdEnajenacion);
            Assert.AreEqual(30, result[0].PorcentajeAdquiriente);
            Assert.AreEqual(false, result[0].CheckAdquiriente);
            Assert.AreEqual("78-9", result[1].RutAdquiriente);
            Assert.AreEqual(1, result[1].IdEnajenacion);
            Assert.AreEqual(20, result[1].PorcentajeAdquiriente);
            Assert.AreEqual(true, result[1].CheckAdquiriente);
        }
    }
}