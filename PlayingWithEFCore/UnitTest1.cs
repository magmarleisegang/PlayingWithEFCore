using PlayingWithEFCore.PawtionData;
using System;
using Xunit;
using System.Linq;
using NetTopologySuite.Geometries;

namespace PlayingWithEFCore
{
    public class UnitTest1
    {
        private readonly string sqlFilename = @"D:\Databases\Sqlite\pawtion.db";

        [Fact]
        public void CanCreateAPawtionContextUsingSqlite()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                Console.Write(dbc.ContextId);
            }
        }

        [Fact]
        public void CanReadOutADogFoodEntity()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var dogfood = dbc.Foods.Find(1);
                Assert.NotNull(dogfood);
            }
        }

        [Fact]
        public void WillNotAddDuplicateDogFoodBasedOnAlternateKey()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var dogfood1 = new DogFood() { Name = "DogFood 1", BagSize = 12 };
                var dogfood2 = new DogFood() { Name = "DogFood 1", BagSize = 12 };

                dbc.Foods.Add(dogfood1);
                Assert.ThrowsAny<System.InvalidOperationException>(() => dbc.Foods.Add(dogfood2));
                //()=>dbc.SaveChanges());

            }
        }

        [Fact]
        public void CanReadOutAPawtionEntity()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var pawtion = dbc.Pawtions.Find(3);
                Assert.NotNull(pawtion);
            }
        }

        [Fact]
        public void CanReadShadowProperty()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var pawtion = dbc.Pawtions.Find(1);
                Assert.Equal(1, dbc.Entry(pawtion).Property("DogFoodId").CurrentValue);
            }
        }

        [Fact]
        public void CanInsertAPawtionEntity()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var food = new DogFood() { BagSize = 12, Name = "Hills Healthy Adult" };
                var pawtion = new Pawtion(food, 765, 32);
                dbc.Pawtions.Add(pawtion);
                dbc.SaveChanges();
                Assert.NotEqual(0, pawtion.Id);
                Assert.Equal(DateTime.Today, pawtion.AddedDate().Date);
            }
        }

        [Fact]
        public void CanWriteAndReadDogFoodIngredients()
        {
            int foodId;
            var ingredientsList = new System.Collections.Generic.List<string> { "Chicken", "Bacon", "Peas", "Water", "Preservative" };
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var food = new DogFood() { BagSize = 12, Name = "Magi's pet mince" };
                food.Ingredients = ingredientsList;
                dbc.Foods.Add(food);
                dbc.SaveChanges();

                foodId = food.Id;
            }
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var dogfood = dbc.Foods.Find(foodId);
                Assert.NotNull(dogfood);
                Assert.NotEmpty(dogfood.Ingredients);
                Assert.Equal(ingredientsList.Count, dogfood.Ingredients.Count);
            }
        }

        [Fact]
        public void CanEditDogFoodIngredients()
        {
            int foodId;
            var ingredientsList = new System.Collections.Generic.List<string> { "Chicken", "Bacon", "Peas", "Water", "Preservative" };
            var ingredientsList2 = new System.Collections.Generic.List<string> { "Bacon", "Chicken", "Desicated coconut", "Peas", "Preservative", "Water", "ZeroMSG" };
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var food = new DogFood() { BagSize = 12, Name = "Magi's pet mince 2" };
                food.Ingredients = ingredientsList;
                dbc.Foods.Add(food);
                dbc.SaveChanges();

                foodId = food.Id;
            }
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var dogfood = dbc.Foods.Find(foodId);
                Assert.NotNull(dogfood);
                Assert.NotEmpty(dogfood.Ingredients);
                dogfood.Ingredients.Add("Desicated coconut");
                dogfood.Ingredients.Add("ZeroMSG");
                dogfood.Ingredients.Sort();
                dbc.SaveChanges();
            }
        }


        [Fact]
        public void CanReadFromPawtionResultsViewWithNoKey()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var allPawtions = dbc.PawtionResults;//.Where(x => x.AddedDate > System.DateTime.Today).ToList();
                Assert.NotEmpty(allPawtions);
                var todaysPawtions = allPawtions.Where(x => x.AddedDate > System.DateTime.Today).ToList();
                Assert.NotEmpty(todaysPawtions);
            }
        }

        [Fact]
        public void CanInsertAPawtionEntityWithShopDetails()
        {
            int pawtionId;
            var myShop = new Shop("PetMax Kyalami Corner", new Point(new Coordinate(28.074818, -25.985173)));
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var food = new DogFood() { BagSize = 12, Name = "Hills Active Adult" };
                var pawtion = new Pawtion(food, 851, 32);
                pawtion.PetShop = new Shop(myShop.Name,myShop.Location);
                dbc.Pawtions.Add(pawtion);
                
                dbc.SaveChanges();
                pawtionId = pawtion.Id;
            }

            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var pawtion = dbc.Pawtions.Find(pawtionId);
                Assert.Equal(myShop.Name, pawtion.PetShop.Name);
                Assert.Equal(myShop.Location, pawtion.PetShop.Location);
            }
        }
    }
}
