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
using RecipesBookDomain.Services;

namespace RecipesBookBllTests
{
    public class RecipeTests
    {
        [Test]
        public async Task UpdateRecipe_ShouldUpdateModel()
        {
            //Arrange
            var (recipeRepository, ingridientService, dataBase) = GetMocks();
            var recipeService = new RecipeService(recipeRepository.Object, ingridientService.Object);
            var idOfRecipeToBeUpdated = 1;
            var recipeUpdateModel = new RecipeUpdateModel() { Name = "Recipe1", Time = 1.5f, TotalCost = 1000, IngridientIds = new List<int> { 1 } };

            //Act
            var updatedRecipeModel = await recipeService.UpdateRecipe(idOfRecipeToBeUpdated, recipeUpdateModel);

            //Assert
            Assert.AreEqual(updatedRecipeModel.Name, dataBase[idOfRecipeToBeUpdated].Name);
            Assert.AreEqual(updatedRecipeModel.TotalCost, dataBase[idOfRecipeToBeUpdated].TotalCost);
            Assert.AreEqual(updatedRecipeModel.Time, dataBase[idOfRecipeToBeUpdated].Time);
            Assert.AreEqual(updatedRecipeModel.IngridientsIds, dataBase[idOfRecipeToBeUpdated].IngridientsIds);
        }

        [Test, TestCaseSource("UpdateRecipe_ThrowsException_Source")]
        public void UpdateRecipe_ShouldThrow_EntityException(RecipeUpdateModel recipe, string expectedMessage)
        {
            //Arrange
            var (recipeRepository, ingridientService, dataBase) = GetMocks();
            var recipeService = new RecipeService(recipeRepository.Object, ingridientService.Object);

            //Act
            var exception = Assert.ThrowsAsync<EntityException>(() => recipeService.UpdateRecipe(1, recipe));

            //Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        private static object[] UpdateRecipe_ThrowsException_Source = new object[]
        {
            new object[] { new RecipeUpdateModel(){Name = "", Time = 1f, TotalCost = 100, IngridientIds = new List<int>{ 1 }}, "Recipe's name can't be empty"},
            new object[] { new RecipeUpdateModel(){Name = "GoodName", Time = -1f, TotalCost = 100, IngridientIds = new List<int>{ 1 }}, "Time can't be negative"},
            new object[] { new RecipeUpdateModel(){Name = "GoodName", Time = 1f, TotalCost = -100, IngridientIds = new List<int>{ 1 }}, "TotalCost can't be negative"},
        };

        [Test]
        public async Task CreateRecipe_ShouldCreateIngridient()
        {
            //Arrange
            var (recipeRepository, ingridientService, dataBase) = GetMocks();
            var recipeService = new RecipeService(recipeRepository.Object, ingridientService.Object);
            var idOfRecipeToBeCreated = dataBase.Count + 1;
            var recipeModelToBeCreated = new Recipe() { Name = "CreatedRecipe1", Time = 300, TotalCost = 500, IngridientsIds = new List<int>{ 1 }};

            //Act
            var createdRecipetModel = await recipeService.CreateRecipe(recipeModelToBeCreated);

            //Assert
            Assert.AreEqual(createdRecipetModel.Name, dataBase[idOfRecipeToBeCreated].Name);
            Assert.AreEqual(createdRecipetModel.Time, dataBase[idOfRecipeToBeCreated].Time);
            Assert.AreEqual(createdRecipetModel.TotalCost, dataBase[idOfRecipeToBeCreated].TotalCost);
            Assert.AreEqual(createdRecipetModel.IngridientsIds, dataBase[idOfRecipeToBeCreated].IngridientsIds);
        }

        [Test, TestCaseSource("CreateRecipe_ThrowsException_Source")]
        public void CreateRecipe_ShouldThrow_EntityException(Recipe recipe, string expectedMessage)
        {
            //Arrange
            var (recipeRepository, ingridientService, dataBase) = GetMocks();
            var recipeService = new RecipeService(recipeRepository.Object, ingridientService.Object);

            //Act
            var exception = Assert.ThrowsAsync<EntityException>(() => recipeService.CreateRecipe(recipe));

            //Assert
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        private static object[] CreateRecipe_ThrowsException_Source = new object[]
        {
            new object[] { new Recipe(){Name = "", Time = 1f, TotalCost = 100, IngridientsIds = new List<int>{ 1 }}, "Recipe's name can't be null or empty"},
            new object[] { new Recipe(){Name = "GoodName", Time = -1f, TotalCost = 100, IngridientsIds = new List<int>{ 1 }}, "Time can't be null or negative"},
            new object[] { new Recipe(){Name = "GoodName", Time = 1f, TotalCost = -100, IngridientsIds = new List<int>{ 1 }}, "Total cost can't be null or negative"},
        };

        [Test]
        public async Task GetRecipe_ShouldReturnIngridient()
        {
            //Arrange
            var (recipeRepository, ingridientService, dataBase) = GetMocks();
            var recipeService = new RecipeService(recipeRepository.Object, ingridientService.Object);
            var idOfRecipe = 1;

            //Act
            var recipe = await recipeService.GetRecipe(idOfRecipe);

            //Assert
            Assert.AreEqual("Recipe1", recipe.Name);
            Assert.AreEqual(1, recipe.Time);
            Assert.AreEqual(500, recipe.TotalCost);
            Assert.AreEqual(new List<int>(){ 1 }, recipe.IngridientsIds);
        }

        [Test]
        public async Task DeleteIngridient_ShouldDeleteIngridient()
        {
            //Arrange
            var (recipeRepository, ingridientService, dataBase) = GetMocks();
            var recipeService = new RecipeService(recipeRepository.Object, ingridientService.Object);
            var idOfRecipe = 1;

            //Act
            await recipeService.DeleteRecipe(idOfRecipe);

            //Assert
            Assert.IsFalse(dataBase.ContainsKey(idOfRecipe));
        }

        private (Mock<IRecipeRepository> recipeRepository, Mock<IIngridientService> ingridientService, Dictionary<int, Recipe> dataBase) GetMocks()
        {
            var dataBase = new Dictionary<int, Recipe>()
            {
                [1] = new Recipe() { Name = "Recipe1", Time = 1, TotalCost = 500, IngridientsIds = new List<int> { 1 } }
            };

            var recipeRepository = new Mock<IRecipeRepository>(MockBehavior.Strict);
            recipeRepository.Setup(r => r.UpdateRecipe(It.IsAny<int>(), It.IsAny<RecipeUpdateModel>()))
                                .ReturnsAsync((int id, RecipeUpdateModel recipeUpdateModel) =>
                                {
                                    dataBase[id] = UpdateModelToDomainModel(recipeUpdateModel);
                                    return dataBase[id];
                                });
            recipeRepository.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync((int id) => dataBase.ContainsKey(id));
            recipeRepository.Setup(r => r.CreateRecipe(It.IsAny<Recipe>()))
                                .ReturnsAsync((Recipe recipe) => dataBase[dataBase.Count + 1] = recipe);
            recipeRepository.Setup(r => r.GetRecipe(It.IsAny<int>())).ReturnsAsync((int id) => dataBase[id]);
            recipeRepository.Setup(r => r.DeleteRecipe(It.IsAny<int>())).Returns((int id) =>
            {
                dataBase.Remove(id);
                return Task.CompletedTask;
            });

            var ingridientService = new Mock<IIngridientService>(MockBehavior.Strict);
            ingridientService.Setup(s => s.GetIngridients(It.IsAny<IEnumerable<int>>())).ReturnsAsync
            (
                (IEnumerable<int> ids) => 
                {
                    return new List<Ingridient> { new Ingridient() { Name = "Ingridient1", Kcal = 100 } };
                }
            );

            return (recipeRepository, ingridientService, dataBase);
        }

        private Recipe UpdateModelToDomainModel(RecipeUpdateModel updateModel)
        {
            return new Recipe()
            {
                Name = updateModel.Name,
                Time = updateModel.Time.Value,
                TotalCost = updateModel.TotalCost.Value,
                IngridientsIds = updateModel.IngridientIds
            };
        }
    }
}