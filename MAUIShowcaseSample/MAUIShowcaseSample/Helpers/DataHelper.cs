using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Transactions;
using Transaction = MAUIShowcaseSample.Services.Transaction;

namespace MAUIShowcaseSample
{
    public static class DataHelper
    {
        private static List<string> legendColors;

        private static readonly Random _random = new Random();

        public static List<Brush> LegendColors { get; set; } = new();

        public static ObservableCollection<TransactionChartData> GetChartData(UserDataService _userService, ObservableCollection<Transaction> transactions, string transactionType, DateTime? startDate = null, DateTime? endDate = null)
        {
            var currencySymbol = _userService.GetUserCurrencySymbol(_userService.LoggedInAccount);
            var _transactions = transactions.Where(t => t.TransactionType == transactionType);

            if (startDate != null && endDate != null)
            {
                _transactions = _transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);
            }

            var _chartData = new ObservableCollection<TransactionChartData>();

            var sumAmount = _transactions.Sum(t => double.Parse(t.TransactionAmount));

            var groupedTransaction = _transactions.GroupBy(t => t.TransactionName);

            InitializeLegendColors(); // Initializing legend colors 

            foreach (var _transaction in groupedTransaction)
            {
                var totalAmount = _transaction.Sum(t => double.Parse(t.TransactionAmount));
                var totalPercent = (totalAmount / sumAmount) * 100;
                Brush categoryColor = GetRandomLegendColor();
                LegendColors.Add(categoryColor);
                _chartData.Add(new TransactionChartData() { TransactionCategory = _transaction.Key, TransactionAmount = currencySymbol + totalAmount.ToString(), TransactionPercent = totalPercent, CategoryColor = categoryColor });
            }
            return _chartData;
        }

        public static ObservableCollection<AreaChartData> GetAreaChartData(ObservableCollection<Transaction> transactionChartData, string interval, DateTime? startDate, DateTime? endDate)
        {
            ObservableCollection<AreaChartData> data = new ObservableCollection<AreaChartData>();
            // Get today's date
            DateTime today = DateTime.Today;
            var tmpdata = transactionChartData.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToObservableCollection<Transaction>();
            switch (interval)
            {
                case "This Week":
                    //// Find last week's Saturday (the end of the last full week)
                    //DateTime lastSaturday = today.AddDays(-(int)today.DayOfWeek - 1);
                    //DateTime lastSunday = lastSaturday.AddDays(-6);

                    // Initialize dictionary for day-wise totals (Sunday to Saturday)
                    Dictionary<DayOfWeek, double> dailyTotals = new Dictionary<DayOfWeek, double>
                    {
                        { DayOfWeek.Sunday, 0 },
                        { DayOfWeek.Monday, 0 },
                        { DayOfWeek.Tuesday, 0 },
                        { DayOfWeek.Wednesday, 0 },
                        { DayOfWeek.Thursday, 0 },
                        { DayOfWeek.Friday, 0 },
                        { DayOfWeek.Saturday, 0 }
                    };

                    // Sum up transaction amounts per day
                    foreach (var transaction in tmpdata)
                    {
                        if (double.TryParse(transaction.TransactionAmount, out double amount))
                        {
                            dailyTotals[transaction.TransactionDate.DayOfWeek] += amount;
                        }
                    }

                    // Convert to AreaChartData format
                    data = dailyTotals
                        .Select(entry => new AreaChartData
                        {
                            TimeInterval = entry.Key.ToString(), // "Sunday", "Monday", etc.
                            Data = entry.Value
                        })
                        .ToObservableCollection<AreaChartData>();
                    break;

                case "This Month":
                    Dictionary<int, double> dailyTotalsMonth = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Value.Year, startDate.Value.Month)).ToDictionary(day => day, day => 0.0);

                    foreach (var transaction in tmpdata)
                    {
                        if (double.TryParse(transaction.TransactionAmount, out double amount))
                        {
                            dailyTotalsMonth[transaction.TransactionDate.Day] += amount;
                        }
                    }

                    data = dailyTotalsMonth.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key.ToString(),
                        Data = entry.Value
                    }).ToObservableCollection<AreaChartData>();
                    break;

                case "This Year":
                    Dictionary<int, double> monthlyTotals = Enumerable.Range(1, 12).ToDictionary(month => month, month => 0.0);

                    foreach (var transaction in tmpdata)
                    {
                        if (double.TryParse(transaction.TransactionAmount, out double amount))
                        {
                            monthlyTotals[transaction.TransactionDate.Month] += amount;
                        }
                    }

                    data = monthlyTotals.Select(entry => new AreaChartData
                    {
                        TimeInterval = new DateTime(1, entry.Key, 1).ToString("MMM"),
                        Data = entry.Value
                    }).ToObservableCollection<AreaChartData>();
                    break;
            }
            return data;
        }

        // Method to group weekly transactions from Sunday to Saturday
        static List<AreaChartData> GetWeeklyChartData(List<Transaction> transactions, DateTime startDate)
        {
            Dictionary<DayOfWeek, double> dailyTotals = new Dictionary<DayOfWeek, double>
        {
            { DayOfWeek.Sunday, 0 },
            { DayOfWeek.Monday, 0 },
            { DayOfWeek.Tuesday, 0 },
            { DayOfWeek.Wednesday, 0 },
            { DayOfWeek.Thursday, 0 },
            { DayOfWeek.Friday, 0 },
            { DayOfWeek.Saturday, 0 }
        };

            foreach (var transaction in transactions)
            {
                if (double.TryParse(transaction.TransactionAmount, out double amount))
                {
                    dailyTotals[transaction.TransactionDate.DayOfWeek] += amount;
                }
            }

            return dailyTotals.Select(entry => new AreaChartData
            {
                TimeInterval = entry.Key.ToString(),
                Data = entry.Value
            }).ToList();
        }

        // Method to group transactions by week (for last and current month)
        static List<AreaChartData> GetMonthlyChartData(List<Transaction> transactions)
        {
            Dictionary<int, double> weekTotals = new Dictionary<int, double>();

            foreach (var transaction in transactions)
            {
                int weekNumber = (transaction.TransactionDate.Day - 1) / 7 + 1;
                if (!weekTotals.ContainsKey(weekNumber))
                    weekTotals[weekNumber] = 0;

                if (double.TryParse(transaction.TransactionAmount, out double amount))
                {
                    weekTotals[weekNumber] += amount;
                }
            }

            return weekTotals.Select(entry => new AreaChartData
            {
                TimeInterval = $"Week {entry.Key}",
                Data = entry.Value
            }).ToList();
        }

        private static Brush GetRandomLegendColor()
        {
            var randomKey = legendColors[_random.Next(legendColors.Count)];
            legendColors.Remove(randomKey); // Removed from list to ensure duplicate colors in chart

            if (Application.Current.Resources.TryGetValue(randomKey, out var colorValue) && colorValue is Color color)
            {
                return (new SolidColorBrush(Color.FromRgb(color.Red, color.Green, color.Blue)));
            }
            return (new SolidColorBrush(Color.FromRgb(Colors.Transparent.Red, Colors.Transparent.Green, Colors.Transparent.Blue)));
        }

        private static void InitializeLegendColors()
        {
            // Fetch colors from the resource dictionary
            legendColors = new List<string>
            {
            "LegendColor1", "LegendColor2", "LegendColor3", "LegendColor4", "LegendColor5", "LegendColor6", "LegendColor7", "LegendColor8", "LegendColor9", "LegendColor10",
            "LegendColor11", "LegendColor12", "LegendColor13", "LegendColor14", "LegendColor15", "LegendColor16", "LegendColor17", "LegendColor18", "LegendColor19", "LegendColor20"
            };
        }

        public static ObservableCollection<AppThemes> GetAppThemes()
        {
            return new ObservableCollection<AppThemes>()
            {
                new AppThemes() { Theme = "Light", IsSelected = true },
                new AppThemes() {Theme = "Dark", IsSelected = false},
                new AppThemes() {Theme = "System Default", IsSelected = false }
            };
        }

        public static string GetGoalIcon(string? title)
        {
            string iconCode = "\ue73d";

            if (title.Contains("Vacation", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue73b";
            }
            else if (title.Contains("Car", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue72a";
            }
            else if (title.Contains("Wedding", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue73c";
            }
            else if (title.Contains("Home", StringComparison.OrdinalIgnoreCase))
            {
                iconCode = "\ue716";
            }
            return iconCode;
        }

        public static Color GetColorCode(int elementCount)
        {
            List<string> colorValues = new List<string>() { "#315A74", "#2196F5", "#963C70", "#EC5C7B" };
            elementCount = elementCount > 3 ? elementCount % 3 : elementCount;
            return Color.FromArgb(colorValues[elementCount]);
        }
    }

    public class AreaChartData
    {
        private string timeInterval;

        private double data;

        public string TimeInterval
        {
            get
            {
                return this.timeInterval;
            }
            set
            {
                this.timeInterval = value;
            }
        }

        public double Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }

    public class AppThemes : INotifyPropertyChanged
    {
        private string theme;

        private bool isSelected;

        public string Theme
        {
            get
            {
                return this.theme;
            }
            set
            {
                this.theme = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
