using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample.ViewModel
{
    /// <summary>
    /// ViewModel for managing goal detail page operations including chart data visualization and date range filtering
    /// </summary>
    public class GoalDetailPageViewModel : INotifyPropertyChanged
    {
        #region Private Fields

        /// <summary>
        /// Available date ranges for chart filtering
        /// </summary>
        private List<ChartDateRange> dateRange;

        /// <summary>
        /// Currently selected date range for chart filtering
        /// </summary>
        private ChartDateRange selectedChartDateRange;

        /// <summary>
        /// Chart data for goal area chart visualization
        /// </summary>
        private ObservableCollection<AreaChartData> goalAreaChart;

        /// <summary>
        /// Summarized goal data for display
        /// </summary>
        private SummarizedGoalData goalData;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the available date ranges for chart filtering
        /// </summary>
        /// <value>List of ChartDateRange objects representing available time periods</value>
        public List<ChartDateRange> DateRange
        {
            get
            {
                return this.dateRange;
            }
            set
            {
                this.dateRange = value;
                OnPropertyChanged("DateRange");
            }
        }

        /// <summary>
        /// Gets or sets the selected date range for chart data filtering
        /// </summary>
        /// <value>ChartDateRange object representing the selected time period</value>
        public ChartDateRange SelectedChartDateRange
        {
            get
            {
                return this.selectedChartDateRange;
            }
            set
            {
                this.selectedChartDateRange = value;
                
                // Update chart data when date range changes
                if (this.selectedChartDateRange != null && GoalData != null && GoalData.Transactions != null)
                {
                    UpdateGoalAreaChartData(
                        GoalData,
                        this.selectedChartDateRange.RangeType ?? string.Empty,
                        this.selectedChartDateRange.StartDate,
                        this.selectedChartDateRange.EndDate
                    );
                }
                else
                {
                    // Clear chart data if no valid selection
                    GoalAreaChart = new ObservableCollection<AreaChartData>();
                }
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

        /// <summary>
        /// Gets or sets the goal area chart data for visualization
        /// </summary>
        /// <value>Observable collection of AreaChartData objects</value>
        public ObservableCollection<AreaChartData> GoalAreaChart
        {
            get
            {
                return this.goalAreaChart;
            }
            set
            {
                this.goalAreaChart = value;
                OnPropertyChanged(nameof(GoalAreaChart));
            }
        }

        /// <summary>
        /// Gets or sets the summarized goal data
        /// </summary>
        /// <value>SummarizedGoalData object containing goal information</value>
        public SummarizedGoalData GoalData
        {
            get
            {
                return this.goalData;
            }
            set
            {
                this.goalData = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the PropertyChanged event for the specified property
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the GoalDetailPageViewModel class
        /// </summary>
        /// <param name="userDataService">User data service for user operations</param>
        /// <param name="dataStore">Data store service for data operations</param>
        /// <param name="goalData">Summarized goal data to display</param>
        public GoalDetailPageViewModel(UserDataService userDataService, DataStore dataStore, SummarizedGoalData goalData)
        {
            GoalData = goalData;

            // Initialize date range options
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };

            // Set default date range to "This Month"
            SelectedChartDateRange = DateRange.ElementAt(1);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the goal area chart data based on the selected date range and goal transactions
        /// </summary>
        /// <param name="data">Summarized goal data containing transactions</param>
        /// <param name="rangeType">Type of date range (This Week, This Month, This Year)</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        private void UpdateGoalAreaChartData(SummarizedGoalData data, string rangeType, DateTime? startDate, DateTime? endDate)
        {
            // Validate input data
            if (data == null || data.Transactions == null)
            {
                GoalAreaChart = new ObservableCollection<AreaChartData>();
                return;
            }

            // Filter transactions by date range
            var filteredData = data.Transactions
                .Where(s => s.TransactionDate >= startDate && s.TransactionDate <= endDate)
                .ToObservableCollection<GoalsTransaction>();

            // Generate chart data based on range type
            switch (rangeType)
            {
                case "This Week":
                    // Initialize daily totals for all days of the week
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

                    // Aggregate transactions by day of week
                    foreach (var transaction in filteredData)
                    {
                        dailyTotals[transaction.TransactionDate.DayOfWeek] += transaction.ContributionAmount;
                    }

                    // Create chart data for weekly view
                    GoalAreaChart = new ObservableCollection<AreaChartData>(
                        dailyTotals.Select(entry => new AreaChartData
                        {
                            TimeInterval = entry.Key.ToString(),
                            Data = entry.Value
                        })
                    );
                    break;

                case "This Month":
                    // Initialize daily totals for all days in the month
                    Dictionary<int, double> dailyTotalsMonth = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Value.Year, startDate.Value.Month))
                        .ToDictionary(day => day, day => 0.0);

                    // Aggregate transactions by day of month
                    foreach (var transaction in filteredData)
                    {
                        dailyTotalsMonth[transaction.TransactionDate.Day] += transaction.ContributionAmount;
                    }

                    // Create chart data for monthly view
                    GoalAreaChart = new ObservableCollection<AreaChartData>(
                        dailyTotalsMonth.Select(entry => new AreaChartData
                        {
                            TimeInterval = entry.Key.ToString(),
                            Data = entry.Value
                        })
                    );
                    break;

                case "This Year":
                    // Initialize monthly totals for all months
                    Dictionary<int, double> monthlyTotals = Enumerable.Range(1, 12).ToDictionary(month => month, month => 0.0);

                    // Aggregate transactions by month
                    foreach (var transaction in filteredData)
                    {
                        monthlyTotals[transaction.TransactionDate.Month] += transaction.ContributionAmount;
                    }

                    // Create chart data for yearly view
                    GoalAreaChart = new ObservableCollection<AreaChartData>(
                        monthlyTotals.Select(entry => new AreaChartData
                        {
                            TimeInterval = new DateTime(1, entry.Key, 1).ToString("MMM"),
                            Data = entry.Value
                        })
                    );
                    break;
            }
        }

        #endregion
    }
}