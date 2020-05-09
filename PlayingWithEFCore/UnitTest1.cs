using PlayingWithEFCore.PawtionData;
using System;
using Xunit;

namespace PlayingWithEFCore
{
    public class UnitTest1
    {
        private readonly string sqlFilename = @"D:\Databases\Sqlite\pawtion.db";

        [Fact]
        public void CanCreateAPawtionContextUsingSqlite()
        {
            using(var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
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
                var dogfood1 = new DogFood() { Name="DogFood 1", BagSize = 12 };
                var dogfood2= new DogFood() { Name="DogFood 1", BagSize = 12 };

                dbc.Foods.Add(dogfood1);
                Assert.ThrowsAny<System.InvalidOperationException>(()=> dbc.Foods.Add(dogfood2));
               //()=>dbc.SaveChanges());
               
            }
        }

        [Fact]
        public void CanReadOutAPawtionEntity()
        {
            using (var dbc = PawtionContext.GetSQLiteContext(sqlFilename))
            {
                var pawtion = dbc.Pawtions.Find(1);
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
            }
        }
    }
}
