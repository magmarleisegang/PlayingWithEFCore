using Microsoft.EntityFrameworkCore;
using PlayingWithEFCore.PawtionData;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace PlayingWithEFCore
{
    class PawtionContext : DbContext
    {
        private SqliteOptions sqliteOptions;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (sqliteOptions != null)
                optionsBuilder.UseSqlite(sqliteOptions.connectionString);

            optionsBuilder.UseLoggerFactory(LoggerFactoryHelper.MyLoggerFactory);

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DogFood>().ToTable("DogFoods");
            modelBuilder.Entity<DogFood>()
                .HasAlternateKey(x => new { x.Name, x.BagSize });


            modelBuilder.Entity<Pawtion>()
                .HasOne(p => p.Food)
                .WithMany()
                .HasForeignKey("DogFoodId");

            modelBuilder.Entity<Pawtion>()
                .Property<DateTime>("_addedDate")
                .HasColumnName("AddedDate")
                .HasConversion<long>();

            var stringLIstValueComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());


            modelBuilder.Entity<DogFood>()
                .Property(x => x.Ingredients)
                .HasConversion(
                    listToDb => JsonSerializer.Serialize(listToDb, null),
                    jsonFromDb => JsonSerializer.Deserialize<List<string>>(jsonFromDb, null))
                .Metadata
                .SetValueComparer(stringLIstValueComparer);

            modelBuilder.Entity<PawtionResult>()
                .ToTable("PawtionResults")
                .HasNoKey();
            modelBuilder.Entity<PawtionResult>()
                .Property(x => x.AddedDate).HasConversion<long>(); ;

            base.OnModelCreating(modelBuilder);
        }

        public static PawtionContext GetSQLiteContext(string filename)
        {
            PawtionContext pc = new PawtionContext(new SqliteOptions(filename));
            return pc;
        }

        public PawtionContext(SqliteOptions sqliteOptions)
        {
            this.sqliteOptions = sqliteOptions;
        }

        public DbSet<DogFood> Foods { get; set; }
        public DbSet<Pawtion> Pawtions { get; set; }
        public DbSet<PawtionResult> PawtionResults { get; set; }
    }

    internal class SqliteOptions
    {
        internal string filename;
        internal string connectionString => $"Data Source={filename}";
        public SqliteOptions(string filename)
        {
            this.filename = filename;
        }
    }
}
