using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Transactions;
using Transaction = MAUIShowcaseSample.Services.Transaction;

namespace MAUIShowcaseSample
{
    /// <summary>
    /// Static helper class providing data processing and utility methods for charts, themes, and UI elements
    /// </summary>
    public static class DataHelper
    {
        #region Private Fields

        /// <summary>
        /// List of legend color keys for chart visualization
        /// </summary>
        private static List<string> legendColors;

        /// <summary>
        /// Random number generator for color selection
        /// </summary>
        private static readonly Random _random = new Random();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the collection of legend colors used in charts
        /// </summary>
        /// <value>List of Brush objects representing legend colors</value>
        public static List<Brush> LegendColors { get; set; } = new();

        #endregion

        #region Chart Data Methods

        /// <summary>
        /// Generates chart data for transaction visualization with category grouping and percentage calculations
        /// </summary>
        /// <param name="_userService">User data service for currency symbol retrieval</param>
        /// <param name="transactions">Collection of transactions to process</param>
        /// <param name="transactionType">Type of transactions to filter (Income/Expense)</param>
        /// <param name="startDate">Optional start date for filtering transactions</param>
        /// <param name="endDate">Optional end date for filtering transactions</param>
        /// <returns>Observable collection of transaction chart data with categories, amounts, and percentages</returns>
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

            // Initialize legend colors for chart visualization
            InitializeLegendColors();

            foreach (var _transaction in groupedTransaction)
            {
                var totalAmount = _transaction.Sum(t => double.Parse(t.TransactionAmount));
                var totalPercent = (totalAmount / sumAmount) * 100;
                Brush categoryColor = GetRandomLegendColor();
                LegendColors.Add(categoryColor);
                _chartData.Add(new TransactionChartData() 
                { 
                    TransactionCategory = _transaction.Key, 
                    TransactionAmount = currencySymbol + totalAmount.ToString(), 
                    TransactionPercent = totalPercent, 
                    CategoryColor = categoryColor 
                });
            }
            return _chartData;
        }

        /// <summary>
        /// Generates area chart data for different time intervals (week, month, year)
        /// </summary>
        /// <param name="transactionChartData">Collection of transactions to process</param>
        /// <param name="interval">Time interval for grouping ("This Week", "This Month", "This Year")</param>
        /// <param name="startDate">Start date for data filtering</param>
        /// <param name="endDate">End date for data filtering</param>
        /// <returns>Observable collection of area chart data grouped by specified interval</returns>
        public static ObservableCollection<AreaChartData> GetAreaChartData(ObservableCollection<Transaction> transactionChartData, string interval, DateTime? startDate, DateTime? endDate)
        {
            ObservableCollection<AreaChartData> data = new ObservableCollection<AreaChartData>();
            // Get today's date
            DateTime today = DateTime.Today;
            var tmpdata = transactionChartData.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToObservableCollection<Transaction>();
            
            switch (interval)
            {
                case "This Week":
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
                    // Initialize dictionary for daily totals within the month
                    Dictionary<int, double> dailyTotalsMonth = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Value.Year, startDate.Value.Month)).ToDictionary(day => day, day => 0.0);

                    // Sum up transaction amounts per day of the month
                    foreach (var transaction in tmpdata)
                    {
                        if (double.TryParse(transaction.TransactionAmount, out double amount))
                        {
                            dailyTotalsMonth[transaction.TransactionDate.Day] += amount;
                        }
                    }

                    // Convert to AreaChartData format with day numbers
                    data = dailyTotalsMonth.Select(entry => new AreaChartData
                    {
                        TimeInterval = entry.Key.ToString(),
                        Data = entry.Value
                    }).ToObservableCollection<AreaChartData>();
                    break;

                case "This Year":
                    // Initialize dictionary for monthly totals (12 months)
                    Dictionary<int, double> monthlyTotals = Enumerable.Range(1, 12).ToDictionary(month => month, month => 0.0);

                    // Sum up transaction amounts per month
                    foreach (var transaction in tmpdata)
                    {
                        if (double.TryParse(transaction.TransactionAmount, out double amount))
                        {
                            monthlyTotals[transaction.TransactionDate.Month] += amount;
                        }
                    }

                    // Convert to AreaChartData format with month abbreviations
                    data = monthlyTotals.Select(entry => new AreaChartData
                    {
                        TimeInterval = new DateTime(1, entry.Key, 1).ToString("MMM"),
                        Data = entry.Value
                    }).ToObservableCollection<AreaChartData>();
                    break;
            }
            return data;
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// Groups weekly transactions from Sunday to Saturday
        /// </summary>
        /// <param name="transactions">List of transactions to group</param>
        /// <param name="startDate">Start date for the week</param>
        /// <returns>List of area chart data grouped by day of week</returns>
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

        /// <summary>
        /// Groups transactions by week for monthly view
        /// </summary>
        /// <param name="transactions">List of transactions to group</param>
        /// <returns>List of area chart data grouped by week number</returns>
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

        /// <summary>
        /// Gets a random legend color from the available color palette
        /// </summary>
        /// <returns>Brush object representing a random legend color</returns>
        private static Brush GetRandomLegendColor()
        {
            var randomKey = legendColors[_random.Next(legendColors.Count)];

            if (Application.Current.Resources.TryGetValue(randomKey, out var colorValue) && colorValue is Color color)
            {
                return (new SolidColorBrush(Color.FromRgb(color.Red, color.Green, color.Blue)));
            }
            return (new SolidColorBrush(Color.FromRgb(Colors.Transparent.Red, Colors.Transparent.Green, Colors.Transparent.Blue)));
        }

        /// <summary>
        /// Initializes the legend colors collection with predefined color keys
        /// </summary>
        private static void InitializeLegendColors()
        {
            // Fetch colors from the resource dictionary
            legendColors = new List<string>
            {
                "LegendColor1", "LegendColor2", "LegendColor3", "LegendColor4", "LegendColor5", "LegendColor6", "LegendColor7", "LegendColor8", "LegendColor9", "LegendColor10",
                "LegendColor11", "LegendColor12", "LegendColor13", "LegendColor14", "LegendColor15", "LegendColor16", "LegendColor17", "LegendColor18", "LegendColor19", "LegendColor20"
            };
        }

        #endregion

        #region Theme and UI Helper Methods

        /// <summary>
        /// Gets the available application themes with default selection
        /// </summary>
        /// <returns>Observable collection of app themes with Light theme selected by default</returns>
        public static ObservableCollection<AppThemes> GetAppThemes()
        {
            return new ObservableCollection<AppThemes>()
            {
                new AppThemes() { Theme = "Light", IsSelected = true },
                new AppThemes() { Theme = "Dark", IsSelected = false },
                new AppThemes() { Theme = "System Default", IsSelected = false }
            };
        }

        /// <summary>
        /// Gets the appropriate icon code based on goal title
        /// </summary>
        /// <param name="title">Title of the goal to determine icon</param>
        /// <returns>Unicode string representing the icon for the goal type</returns>
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

        /// <summary>
        /// Gets a color code based on element count for UI styling
        /// </summary>
        /// <param name="elementCount">Count of elements to determine color</param>
        /// <returns>Color object from predefined color palette</returns>
        public static Color GetColorCode(int elementCount)
        {
            List<string> colorValues = new List<string>() { "#315A74", "#2196F5", "#963C70", "#EC5C7B" };
            elementCount = elementCount > 3 ? elementCount % 3 : elementCount;
            return Color.FromArgb(colorValues[elementCount]);
        }

        #endregion
    }

    #region Data Models

    /// <summary>
    /// Represents data structure for area chart visualization
    /// </summary>
    public class AreaChartData
    {
        #region Private Fields

        /// <summary>
        /// Backing field for TimeInterval property
        /// </summary>
        private string timeInterval;

        /// <summary>
        /// Backing field for Data property
        /// </summary>
        private double data;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the time interval label for the chart data point
        /// </summary>
        /// <value>String representing the time interval (day, week, month, etc.)</value>
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

        /// <summary>
        /// Gets or sets the numeric data value for the chart data point
        /// </summary>
        /// <value>Double value representing the data amount</value>
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

        #endregion
    }

    /// <summary>
    /// Represents application theme options with selection state
    /// </summary>
    public class AppThemes : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Backing field for Theme property
        /// </summary>
        private string theme;

        /// <summary>
        /// Backing field for IsSelected property
        /// </summary>
        private bool isSelected;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the theme name
        /// </summary>
        /// <value>String representing the theme name (Light, Dark, System Default)</value>
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

        /// <summary>
        /// Gets or sets whether this theme is currently selected
        /// </summary>
        /// <value>Boolean indicating if the theme is selected</value>
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

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion
}