using Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using System.Xml.Linq;
using TanuloProgram.MVVM.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TanuloProgram.Services
{

    public class SQLiteConnectionSimple : IDbtypeConnection
    {
        public const string dbs = "Data Source = .\\LanguageDB.db;Version=3;";
        /// <summary>
        /// Create a table in the database
        /// </summary>
        /// <param name="tableName"> The name of the table</param>
        public void CreateTable(string tableName)
        {
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                cnn.Execute($"create table {tableName} (Id integer not null unique,IsWord integet, NativeWord text not null, ForeignWord text not null, CONSTRAINT PK_{tableName} PRIMARY KEY(Id AUTOINCREMENT));");
            }
        }
        /// <summary>
        /// Delete a table from the database
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        public void DeleteTable(string tableName)
        {
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                cnn.Execute($"DROP TABLE {tableName}");
            }
        }
        public void UpdateTable(string tableName, Word word)
        {
            if (string.IsNullOrEmpty(tableName)) return;
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                cnn.Execute($@"UPDATE {tableName} SET IsWord = @IsWord, NativeWord = @NativeWord, ForeignWord = @ForeignWord WHERE Id = @ItemId",new { ItemId = word.Id, IsWord = word.IsWord, NativeWord = word.NativeWord, ForeignWord = word.ForeignWord });

            }
        }

        public ObservableCollection<Word> LoadData(string tableName)
        {
            ObservableCollection<Word> _tableNames = new ObservableCollection<Word>();
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                var output = cnn.Query<Word>($"SELECT * FROM {tableName}");
                foreach (Word w in output)
                {
                    _tableNames.Add(w);
                }
            }
            return _tableNames;
        }
        public void SaveData(string tableName,Word word)
        {
            int _isWord = 0;
            if (Convert.ToBoolean(word.IsWord))
            {
                _isWord = 1;
            }
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            { 
                cnn.Execute($"insert into {tableName} (IsWord, NativeWord, ForeignWord) values (@IsWord, @NativeWord, @ForeignWord)", new { IsWord = _isWord, NativeWord = word.NativeWord, ForeignWord = word.ForeignWord });
            }
        }
        public void DeleteData(string tableName, int itemId)
        {
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                cnn.Execute($"DELETE FROM {tableName} WHERE Id = @ItemId", new { ItemId = itemId });
            }
        }

        public ObservableCollection<string> LoadTables()
        {
            ObservableCollection<string> tablenames = new ObservableCollection<string>();
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                var output = cnn.Query<string>("SELECT name FROM sqlite_master WHERE type='table'");
                foreach (var o in output)
                {
                    tablenames.Add(o);
                }
            }
            return tablenames;
        }

        public static bool IsTableExists(string tableName)
        {
            bool tableExists = false;
            using (IDbConnection cnn = new SQLiteConnection(dbs))
            {
                tableExists = cnn.QueryFirstOrDefault<bool>(
                "SELECT EXISTS (SELECT 1 FROM sqlite_master WHERE type='table' AND LOWER(name)=@TableName)",
                new { TableName = tableName });
            }
            return tableExists;
        }

        public static bool ElementPairExists(string tableName, string nativeWord, string foreignWord)
        {
            nativeWord = nativeWord.Trim().ToLower();
            foreignWord = foreignWord.Trim().ToLower();
            using (var connection = new SQLiteConnection(dbs))
            {
                string query = $"SELECT COUNT(1) FROM {tableName} WHERE LOWER(NativeWord) = @NativeWord AND LOWER(ForeignWord) = @ForeignWord";
                var parameters = new { NativeWord = nativeWord, ForeignWord = foreignWord };

                int count = connection.ExecuteScalar<int>(query, parameters);
                return count > 0;
            }
        }

        public static bool ElementPairExists(string tableName, string nativeWord, string foreignWord, int isWord)
        {
            nativeWord = nativeWord.Trim().ToLower();
            foreignWord = foreignWord.Trim().ToLower();
            using (var connection = new SQLiteConnection(dbs))
            {
                string query = $"SELECT COUNT(1) FROM {tableName} WHERE LOWER(NativeWord) = @NativeWord AND LOWER(ForeignWord) = @ForeignWord AND IsWord = @IsWord";
                var parameters = new { NativeWord = nativeWord, ForeignWord = foreignWord, IsWord = isWord };

                int count = connection.ExecuteScalar<int>(query, parameters);
                return count > 0;
            }
        }
    }
}
