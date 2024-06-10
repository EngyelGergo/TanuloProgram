using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;

namespace TanuloProgram.Services
{
    public interface INavigationService
    {
        ViewModel CurrentView { get; }
        void NavigateTo<T>(string? s = "") where T : ViewModel;
    }

    public interface IDBConnectionMethods
    {
        public ObservableCollection<Word> LoadData(string tableName);
        public void SaveData(string tableName, Word w);
        public void DeleteData(string tableName, int id);
    }

    public interface IDbtypeConnection : IDBConnectionMethods
    {
        public void CreateTable(string tableName);
        public void DeleteTable(string tableName);
        public void UpdateTable(string tableName, Word word);
        public ObservableCollection<string> LoadTables();
    }

    public interface IInsiderView
    {
        public  string? ActuallViewLabel { get; set; }
    }
}
