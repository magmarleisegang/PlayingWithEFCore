﻿using Microsoft.EntityFrameworkCore;
using PlayingWithEFCore.PawtionData;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayingWithEFCore
{
    class PawtionContext : DbContext
    {
        private SqliteOptions sqliteOptions;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (sqliteOptions != null)
                optionsBuilder.UseSqlite(sqliteOptions.connectionString);

            base.OnConfiguring(optionsBuilder);
           
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