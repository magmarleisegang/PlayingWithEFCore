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
    }
}
