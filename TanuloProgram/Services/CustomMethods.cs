using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media;
using System.Windows.Shapes;
using TanuloProgram.Core;
using TanuloProgram.MVVM.Model;
using TanuloProgram.MVVM.ViewModel.LanguageLobbyViewModels;

namespace TanuloProgram.Services
{
    public static class CustomMethods
    {
        public static SolidColorBrush MakeColor(string hexColor)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            return brush;
        }

        public const string SQL_MAIN_TABLE = "sqlite_sequence";

        public static string SelectNextLanguageType(LanguageType lang, string currentLang)
        {
            if (currentLang == lang.NativeLanguage)
            {
                currentLang = lang.ForeignLanguage;
            }
            else if (currentLang == lang.ForeignLanguage)
            {
                currentLang = lang.Variable;
            }
            else if (currentLang == lang.Variable)
            {
                currentLang = lang.NativeLanguage;
            }
            return currentLang;
        }

        public static string SelectPreviousLanguageType(LanguageType lang, string currentLang)
        {
            if (currentLang == lang.NativeLanguage)
            {
                currentLang = lang.Variable;
            }
            else if (currentLang == lang.ForeignLanguage)
            {
                currentLang = lang.NativeLanguage;
            }
            else if (currentLang == lang.Variable)
            {
                currentLang = lang.ForeignLanguage;
            }
            return currentLang;
        }

        public static LanguageType SetCurrentLanguages(string tableName, string defaultLanguage)
        {
            LanguageType languages = new LanguageType
            {
                NativeLanguage = defaultLanguage,
                ForeignLanguage = tableName
            };
            return languages;
        }

        public static void ResizeApplicationMainWindow(double height, double width)
        {
            Application.Current.MainWindow.Width = width;
            Application.Current.MainWindow.Height = height;
            ResizeWindow(Application.Current.MainWindow as MainWindow);
        }
        public static void ResizeApplicationMainWindow(WindowState state, WindowStyle style)
        {
            Application.Current.MainWindow.WindowState = state;
            Application.Current.MainWindow.WindowStyle = style;
        }
        public static void ResizeApplicationMainWindow(double height, double width, WindowState state, WindowStyle style)
        {
            Application.Current.MainWindow.Width = width;
            Application.Current.MainWindow.Height = height;
            Application.Current.MainWindow.WindowState = state;
            Application.Current.MainWindow.WindowStyle = style;
            ResizeWindow(Application.Current.MainWindow as MainWindow);
        }

        /// <summary>
        /// Resize to the center of the computer
        /// </summary>
        /// <param name="_mainWindow"> The given View</param>
        public static void ResizeWindow(MainWindow _mainWindow)
        {
            _mainWindow.WindowState = WindowState.Normal;
            _mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = _mainWindow.Width;
            double windowHeight = _mainWindow.Height;

            double left = (screenWidth - windowWidth) / 2;
            double top = (screenHeight - windowHeight) / 2;

            _mainWindow.Left = left;
            _mainWindow.Top = top;
        }

        /// <summary>
        /// Checks whether the given text meets the requirements
        /// It should not contain or cannot be: Number, Empty string, Special character, Space
        /// </summary>
        /// <param name="w">The given word</param>
        /// <returns></returns>
        public static bool IsCorrectTxTBoxForm(string w)
        {
            if (!string.IsNullOrEmpty(w)) 
            {
                return false;
            }
            else if (w.All(c => char.IsLetter(c) || c != ' '))
            {
                return false;
            }
            return true;

        }

        public static List<int> GenerateUniqueIndex(int numberElements, bool isWord, bool isSentence, ObservableCollection<Word> words)
        {
            Random random = new();
            List<int> uniqueNumbers = new();          
            List<int> tempList = new();
            int amount = words.Count;
            tempList.FillWithNumber(amount);

            int randomIndex = tempList[random.Next(0,tempList.Count)];

            while (uniqueNumbers.Count < numberElements)
            {
                if (!isWord && !isSentence)
                {
                    uniqueNumbers.Add(randomIndex);
                }
                else if (isWord && words[randomIndex].IsWord == 1)
                {
                    uniqueNumbers.Add(randomIndex);
                }
                else if (isSentence && words[randomIndex].IsWord == 0)
                {
                    uniqueNumbers.Add(randomIndex);
                }

                tempList.Remove(randomIndex);

                if (tempList.Count != 0) 
                {
                    randomIndex = tempList[random.Next(0, tempList.Count)];
                }
            }
            return uniqueNumbers;
        }
    }
    public enum LogDataType
    {
        ProgramStart = 0,
        MostChoosenLanguageAmount = 1,
        MostChoosenLanguage = 2,
        NewAdatbase = 3,
        DeletedAdatbase = 4,
        NewElementCreating = 5,
        ChangedElement = 6,
        DeletedElement = 7,
        PairRuns = 9,
        PairGoodAnswer = 10,
        PairBadAnswer = 11,
        PairResult = 12,
        SubstitutionRuns = 14,
        SubstitutionGoodAnswer = 15,
        SubstitutionBadAnswer = 16,
        SubstitutionResult = 17,
        CompletionRuns = 19,
        CompletionGoodAnswer = 20,
        CompletionBadAnswer = 21,
        CompletionResult = 22,
    }

    public class LogFile
    {
        private static Dictionary<string, int> choosenLanguages = new Dictionary<string, int>();
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string logFileName = "log.txt";
        private static string logFilePath = System.IO.Path.Combine(currentDirectory, logFileName);
        private static bool FileExists()
        {
            return File.Exists(logFilePath);
        }

        private static void FileCreate()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"Program elindítva: 0");
                    writer.WriteLine($"Legtöbbet válaszott nyelv mennyisége: 0");
                    writer.WriteLine($"Legtöbbet válaszott nyelv: None");
                    writer.WriteLine($"Adatbázisok létrehozásainak számai: 0");
                    writer.WriteLine($"Adatbázisok törlésének számai: 0");
                    writer.WriteLine($"Összes új elem létrehzoása: 0");
                    writer.WriteLine($"Megváltoztatott elemek mennyisége: 0");
                    writer.WriteLine($"Törölt elemek mennyisége: 0");
                    writer.WriteLine("-----");
                    writer.WriteLine($"Párosítás futattásainak száma: 0");
                    writer.WriteLine($"Párosítás helyes megoldások száma: 0");
                    writer.WriteLine($"Párosítás helytelen megoldások száma: 0");
                    writer.WriteLine($"Párosítás össz eredménye: 0/0");
                    writer.WriteLine("-----");
                    writer.WriteLine($"Helyesírás futattásainak száma: 0");
                    writer.WriteLine($"Helyesírás helyes megoldások száma: 0");
                    writer.WriteLine($"Helyesírás helytelen megoldások száma: 0");
                    writer.WriteLine($"Helyesírás össz eredménye: 0/0");
                    writer.WriteLine("-----");
                    writer.WriteLine($"Kiegésztítés futattásainak száma: 0");
                    writer.WriteLine($"Kiegésztítés helyes megoldások száma: 0");
                    writer.WriteLine($"Kiegésztítés helytelen megoldások száma: 0");
                    writer.WriteLine($"Kiegésztítés össz eredménye: 0/0");
                    writer.WriteLine("-----");
                }
            }
            catch (Exception) { }
        }

        public static void LoggingLoadAllLanguage()
        {
            string[] lines = File.ReadAllLines(logFilePath);
            int size = lines.Length - 1;
            if (size > 23)
            {
                for (int i = 24; i < size + 1; i++)
                {
                    string[] new_lines = lines[i].Split(" ");
                    string lang = new_lines[0].Trim(':');
                    int value = Convert.ToInt32(new_lines[1]);
                    choosenLanguages.Add(lang, value);
                }
            }
        }

        public static void LogLanguageSet(string lang)
        {
            string[] lines = File.ReadAllLines(logFilePath);
            int size = lines.Length - 1;
            if (size == 23)
            {
                choosenLanguages.Add(lang, 1);
                Array.Resize(ref lines, lines.Length + 1);
                lines[lines.Length - 1] = $"{lang}: 1";
                File.WriteAllLines(logFilePath, lines);
                return;
            }
            else
            {
                for (int i = 24; i < lines.Length; i++)
                {
                    string[] new_lines = lines[i].Split(" ");
                    string lang_line = new_lines[0].Trim(':');
                    int value = Convert.ToInt32(new_lines[1]);
                    if (lang_line != lang && !choosenLanguages.ContainsKey(lang))
                    {
                        choosenLanguages.Add(lang, 1);
                        Array.Resize(ref lines, lines.Length + 1);
                        lines[lines.Length - 1] = $"{lang}: 1";
                        File.WriteAllLines(logFilePath, lines);
                        return;
                    }
                    else if (lang_line == lang)
                    {
                         choosenLanguages[lang]++;
                        string new_line = $"{lang}: {value + 1}";
                        lines[i] = new_line ;
                        File.WriteAllLines(logFilePath, lines);
                        return;
                    }
                }
            }
        }

        public static void LogData(LogDataType type)
        {
            if (!FileExists()) FileCreate();

            try
            {
                // Fájl tartalmának beolvasása soronként
                string[] lines = File.ReadAllLines(logFilePath);

                string new_line = ChangeSpecificLineValue(lines[(int)type], type);
                lines[(int)type] = new_line;
                File.WriteAllLines(logFilePath, lines);
            }
            catch (Exception) { }
        }
        public static void LogData(LogDataType type, int value)
        {
            if (!FileExists()) FileCreate();

            try
            {
                // Fájl tartalmának beolvasása soronként
                string[] lines = File.ReadAllLines(logFilePath);

                string new_line = ChangeSpecificLineValue(lines[(int)type], type, value);
                lines[(int)type] = new_line;
                File.WriteAllLines(logFilePath, lines);
            }
            catch (Exception) { }
        }
        public static void LogData(LogDataType type, string all_value)
        {
            if (!FileExists()) FileCreate();

            try
            {
                // Fájl tartalmának beolvasása soronként
                string[] lines = File.ReadAllLines(logFilePath);

                string new_line = ChangeSpecificLineValue(lines[(int)type], type,0, all_value);
                lines[(int)type] = new_line;
                File.WriteAllLines(logFilePath, lines);
            }
            catch (Exception) { }
        }
        private static string ChangeSpecificLineValue (string line, LogDataType type, int withValue = 0, string withAll = "")
        {
            string[] line_words = line.Split(" ");
            KeyValuePair<string, int> maxEntry;
            int actuall_amount;

            switch (type)
            {
                case LogDataType.ProgramStart:
                    return IncreaseElementNumber(line_words, 2, 1);
                case LogDataType.MostChoosenLanguageAmount:
                    maxEntry = choosenLanguages.OrderByDescending(x => x.Value).FirstOrDefault();
                    actuall_amount = maxEntry.Value;
                    line_words[4] = actuall_amount.ToString();
                    return string.Join(" ", line_words);
                case LogDataType.MostChoosenLanguage:
                    maxEntry = choosenLanguages.OrderByDescending(x => x.Value).FirstOrDefault();
                    line_words[3] = maxEntry.Key;
                    return string.Join(" ", line_words);
                case LogDataType.NewAdatbase:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.DeletedAdatbase:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.NewElementCreating:
                    return IncreaseElementNumber(line_words, 4, 1);
                case LogDataType.ChangedElement:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.DeletedElement:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.PairRuns:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.PairGoodAnswer:
                    return IncreaseElementNumber(line_words, 4, withValue);
                case LogDataType.PairBadAnswer:
                    return IncreaseElementNumber(line_words, 4, withValue);
                case LogDataType.PairResult:
                    return IncreaseResult(line_words, 3, withAll);
                case LogDataType.SubstitutionRuns:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.SubstitutionGoodAnswer:
                    return IncreaseElementNumber(line_words, 4, withValue);
                case LogDataType.SubstitutionBadAnswer:
                    return IncreaseElementNumber(line_words, 4, withValue);
                case LogDataType.SubstitutionResult:
                    return IncreaseResult(line_words, 3, withAll);
                case LogDataType.CompletionRuns:
                    return IncreaseElementNumber(line_words, 3, 1);
                case LogDataType.CompletionGoodAnswer:
                    return IncreaseElementNumber(line_words, 4, withValue);
                case LogDataType.CompletionBadAnswer:
                    return IncreaseElementNumber(line_words, 4, withValue);
                case LogDataType.CompletionResult:
                    return IncreaseResult(line_words, 3, withAll);
                default:
                    return "Hiba";
            }
        }
        private static string IncreaseResult(string[] line_words, int idx, string all_value)
        {
            string[] reuslt = line_words[idx].Split("/");
            int all_amount = Convert.ToInt32(reuslt[0]);
            int good_amount = Convert.ToInt32(reuslt[1]);

            string[] new_reuslt = all_value.Split("/");
            int new_all_amount = Convert.ToInt32(new_reuslt[0]);
            int new_good_amount = Convert.ToInt32(new_reuslt[1]);

            string back_result = $"{all_amount + new_all_amount}/{good_amount + new_good_amount}";
            line_words[idx] = back_result;
            return string.Join(" ", line_words);
        }
        private static string IncreaseElementNumber(string[] line_words,int idx, int value)
        {
            int actuall_amount;
            actuall_amount = Convert.ToInt32(line_words[idx]) + value;
            line_words[idx] = actuall_amount.ToString();
            return string.Join(" ", line_words);
        }
    }

    public static class Paging
    {
        public static IInsiderView NextView(IInsiderView insiderView, ObservableCollection<IInsiderView> VMs)
        {
            if (VMs?.Any() != true) return insiderView;

            int c = VMs.Count;
            for (int i = 0; i < c; i++)
            {
                if (i == VMs.Count - 1)
                {
                    return insiderView;
                }
                if (VMs[i].ActuallViewLabel == insiderView.ActuallViewLabel)
                {
                    return insiderView = VMs[i + 1];
                }

            }
            return insiderView;
        }

        public static IInsiderView PreviousView(IInsiderView insiderView, ObservableCollection<IInsiderView> VMs)
        {
            if (VMs?.Any() != true) return insiderView;

            int c = VMs.Count;
            for (int i = 0; i < c; i++)
            {
                if (VMs[0].ActuallViewLabel == insiderView.ActuallViewLabel)
                {
                    break;
                }
                if (VMs[i].ActuallViewLabel == insiderView.ActuallViewLabel)
                {
                    insiderView = VMs[i - 1];
                    break;
                }
            }
            return insiderView;
        }
    }

    public static class ListExtensions
    {
        public static void FillWithNumber(this  List<int> list, int value, int offset = 0)
        {
            list.Clear();
            for (int i = 0 + offset; i < value; i++)
            {
                list.Add(i);
            }
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class DictionaryExtensions
    {
        public static void LowercaseAndTrimPairs<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) where TKey : class where TValue : class
        {
            var newDictionary = new Dictionary<TKey, TValue>();

            foreach (var pair in dictionary)
            {
                TKey originalKey = pair.Key;
                TValue originalValue = pair.Value;

                // Trim és kisbetűsre alakítás a kulcsra
                string modifiedKey = (originalKey as string)?.LowcaseOneLineText();

                // Trim és kisbetűsre alakítás az értékre
                string modifiedValue = (originalValue as string)?.LowcaseOneLineText();

                newDictionary.Add((TKey)(object)modifiedKey, (TValue)(object)modifiedValue);
            }

            dictionary.Clear();
            foreach (var pair in newDictionary)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
        }
    }

    public static class StringExtentsions
    {
        public static string LowcaseOneLineText(this string input)
        {
            return input.ToLower().Trim();
        }
        public static string FirstToUpper(this string input) 
        {
            return char.ToUpper(input[0]) + input.Substring(1);
        }
        public static string RemoveSentence(this string input)
        {
            return input.Replace(".","");
        }
        public static string InsertSentence(this string input)
        {
            return (input.RemoveSentence() + ".");
        }


    }

}
