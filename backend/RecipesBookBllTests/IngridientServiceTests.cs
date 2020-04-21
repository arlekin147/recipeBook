using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RecipesBookDomain.Repositories;
using RecipesBookDomain.Models;
using RecipesBookBll;
using System.Threading.Tasks;
using RecipesBookBll.Exceptions;
using System;
using System.Linq;

namespace RecipesBookBllTests
{
    public class IngridientServiceTests
    {
        [Test]
        public async Task UpdateIngridient_ShouldUpdateModel()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var idOfIngridientToBeUpdated = 1;
            var ingridientUpdateModel = new IngridientUpdateModel() { Name = "UpdatedIngridient1", Kcal = 300 };

            //Act
            var updatedIngridientModel = await ingridientService.UpdateIngridient(idOfIngridientToBeUpdated, ingridientUpdateModel);

            //Assert
            Assert.AreEqual(updatedIngridientModel.Name, dataBase[idOfIngridientToBeUpdated].Name);
            Assert.AreEqual(updatedIngridientModel.Kcal, dataBase[idOfIngridientToBeUpdated].Kcal);
        }

        [Test, TestCaseSource("UpdateIngridient_ThrowsException_Source")]
        public void UpdateIngridient_ShouldThrow_EntityException(IngridientUpdateModel ingridient, string expectedMessage)
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);

            //Act
            var exception = Assert.ThrowsAsync<EntityException>(() => ingridientService.UpdateIngridient(1, ingridient));

            //Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        private static object[] UpdateIngridient_ThrowsException_Source = new object[]
        {
            new object[] { new IngridientUpdateModel(){Name = "", Kcal = 100}, "Field 'name' of ingridient can't be empty"},
            new object[] { new IngridientUpdateModel(){Name = "AGoodName", Kcal = -100}, "Field 'kcal' of ingridient can't be less than 0"},
        };

        [Test]
        public void UpdateIngridient_ShouldThrow_EntityDoesntExistException()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var ingridientUpdateModel = new IngridientUpdateModel() { Name = "Ingridient1", Kcal = 200 };
            var idOfIngridientToBeUpdated = 100;

            //Act
            var exception = Assert.ThrowsAsync<EntityDoesNotExistException>(() => ingridientService.UpdateIngridient(idOfIngridientToBeUpdated, ingridientUpdateModel));

            //Assert
            Assert.AreEqual("Ingridient with id = 100 doesn't exist", exception.Message);
        }

        [Test]
        public async Task CreateIngridient_ShouldCreateIngridient()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var idOfIngridientToBeCreated = dataBase.Count + 1;
            var ingridient = new Ingridient() { Name = "UpdatedIngridient1", Kcal = 300 };

            //Act
            var createdIngridientModel = await ingridientService.CreateIngridient(ingridient);

            //Assert
            Assert.AreEqual(createdIngridientModel.Name, dataBase[idOfIngridientToBeCreated].Name);
            Assert.AreEqual(createdIngridientModel.Kcal, dataBase[idOfIngridientToBeCreated].Kcal);
        }

        [Test, TestCaseSource("CreateIngridient_ThrowsException_Source")]
        public void CreateIngridient_ShouldThrow_EntityException(Ingridient ingridient, string expectedMessage)
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);

            //Act
            var exception = Assert.ThrowsAsync<EntityException>(() => ingridientService.CreateIngridient(ingridient));

            //Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        private static object[] CreateIngridient_ThrowsException_Source = new object[]
        {
            new object[] { new Ingridient(){Name = null, Kcal = 100}, "Field 'name' of ingridient can't be null or empty"},
            new object[] { new Ingridient(){Name = "", Kcal = 100}, "Field 'name' of ingridient can't be null or empty"},
            new object[] { new Ingridient(){Name = "AGoodName", Kcal = -100}, "Field 'kcal' of ingridient can't be less than 0"},
        };

        [Test]
        public async Task GetIngridient_ShouldReturnIngridient()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var idOfIngridient = 1;

            //Act
            var ingridient = await ingridientService.GetIngridient(idOfIngridient);

            //Assert
            Assert.AreEqual("Ingridient1", ingridient.Name);
            Assert.AreEqual(200, ingridient.Kcal);
        }

        [Test]
        public async Task DeleteIngridient_ShouldDeleteIngridient()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var idOfIngridient = 1;

            //Act
            await ingridientService.DeleteIngridient(idOfIngridient);

            //Assert
            Assert.IsFalse(dataBase.ContainsKey(idOfIngridient));
        }

        [Test]
        public async Task GetIngridients_ShouldReturnIngridients()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var idOfIngridient = 1;

            //Act
            var ingridient = (await ingridientService.GetIngridients(new int[] { idOfIngridient })).First();

            //Assert
            Assert.AreEqual("Ingridient1", ingridient.Name);
            Assert.AreEqual(200, ingridient.Kcal);
        }

        [Test]
        public async Task GetIngridients_ShouldReturn_EntityDoesntExistException()
        {
            //Arrange
            var (ingridientRepository, dataBase) = GetMocks();
            var ingridientService = new IngridientService(ingridientRepository.Object);
            var idOfIngridient = 100;

            //Act
            var exception = Assert.ThrowsAsync<EntityDoesNotExistException>(() => ingridientService.GetIngridients(new int[]{idOfIngridient}));

            //Assert
            Assert.AreEqual("One or more ingridients don't exist", exception.Message);
        }

        private (Mock<IIngridientRepository> ingridientRepository, Dictionary<int, Ingridient> dataBase) GetMocks()
        {
            var dataBase = new Dictionary<int, Ingridient>()
            {
                [1] = new Ingridient() { Name = "Ingridient1", Kcal = 200 }
            };

            var ingridientRepository = new Mock<IIngridientRepository>(MockBehavior.Strict);
            ingridientRepository.Setup(r => r.UpdateIngridient(It.IsAny<int>(), It.IsAny<IngridientUpdateModel>()))
                                .ReturnsAsync((int id, IngridientUpdateModel ingridientUpdateModel) =>
                                {
                                    dataBase[id] = UpdateModelToDomainModel(ingridientUpdateModel);
                                    return dataBase[id];
                                });
            ingridientRepository.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync((int id) => dataBase.ContainsKey(id));
            ingridientRepository.Setup(r => r.CreateIngridient(It.IsAny<Ingridient>()))
                                .ReturnsAsync((Ingridient ingridient) => dataBase[dataBase.Count + 1] = ingridient);
            ingridientRepository.Setup(r => r.GetIngridient(It.IsAny<int>())).ReturnsAsync((int id) => dataBase[id]);
            ingridientRepository.Setup(r => r.DeleteIngridient(It.IsAny<int>())).Returns((int id) =>
            {
                dataBase.Remove(id);
                return Task.CompletedTask;
            });
            ingridientRepository.Setup(r => r.GetIngridients(It.IsAny<IEnumerable<int>>())).ReturnsAsync((IEnumerable<int> ids) => 
            {
                return dataBase.Where(p => ids.Contains(p.Key)).Select(p => p.Value).ToList();
            });

            return (ingridientRepository, dataBase);
        }

        private Ingridient UpdateModelToDomainModel(IngridientUpdateModel updateModel)
        {
            return new Ingridient() { Name = updateModel.Name, Kcal = updateModel.Kcal.Value };
        }
    }
}