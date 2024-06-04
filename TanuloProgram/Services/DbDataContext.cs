using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TanuloProgram.MVVM.Model;
using System.Data.SQLite;
using System.Data;
using Dapper;

namespace TanuloProgram.Services
{
    public class DbDataContext : DbContext 
    {
        public const string dbs = "Data Source = .\\LanguageDB.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(dbs);
        }

        public static void CreateDBTable(string tableName)
        {
            using (IDbConnection cnn = new SQLiteConnection(dbs + ";Version=3;"))
            {
                cnn.Execute($"create table {tableName} (Id integer not null unique, NativeWord text not null, ForeignWord text not null, CONSTRAINT PK_{tableName} PRIMARY KEY(Id AUTOINCREMENT));");
               // cnn.Execute("insert into Words (TableName,NativeWord,ForeignWord) values (@TableName,@NativeWord,@ForeignWord)", word);
            }
        }

    }
}
