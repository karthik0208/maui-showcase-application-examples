using MAUIShowcaseSample.Services;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace MAUIShowcaseSample.ViewModel
{
    public class GoalDetailPageViewModel : INotifyPropertyChanged
    {
        private List<ChartDateRange> dateRange;

        private ChartDateRange selectedChartDateRange;

        private ObservableCollection<AreaChartData> goalAreaChart;

        private SummarizedGoalData goalData;

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

        public ChartDateRange SelectedChartDateRange
        {
            get
            {
                return this.selectedChartDateRange;
            }
            set
            {
                this.selectedChartDateRange = value;
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
                    GoalAreaChart = new ObservableCollection<AreaChartData>();
                }
                OnPropertyChanged("SelectedChartDateRange");
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public GoalDetailPageViewModel(UserDataService userDataService, DataStore dataStore, SummarizedGoalData goalData)
        {
            GoalData = goalData;
            DateRange = new List<ChartDateRange>
            {
                new ChartDateRange() {RangeType = "This Week", StartDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday), EndDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday).AddDays(6)},
                new ChartDateRange() { RangeType = "This Month", StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)},
                new ChartDateRange() {RangeType = "This Year", StartDate = new DateTime(DateTime.Today.Year, 1, 1), EndDate = new DateTime(DateTime.Today.Year, 12, 31)}
            };
            SelectedChartDateRange = DateRange.ElementAt(1);
            
        }

        // Example fix for UpdateGoalAreaChartData:
        private void UpdateGoalAreaChartData(SummarizedGoalData data, string rangeType, DateTime? startDate, DateTime? endDate)
        {
            if (data == null || data.Transactions == null)
            {
                GoalAreaChart = new ObservableCollection<AreaChartData>();
                return;
            }

            var filteredData = data.Transactions
                .Where(s => s.TransactionDate >= startDate && s.TransactionDate <= endDate)
                .ToObservableCollection<GoalsTransaction>();

            switch (rangeType)
            {
                case "This Week":
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

                    foreach (var transaction in filteredData)
                    {
                        dailyTotals[transaction.TransactionDate.DayOfWeek] += transaction.ContributionAmount;
                    }

                    GoalAreaChart = new ObservableCollection<AreaChartData>(
                        dailyTotals.Select(entry => new AreaChartData
                        {
                            TimeInterval = entry.Key.ToString(),
                            Data = entry.Value
                        })
                    );
                    break;

                case "This Month":
                    Dictionary<int, double> dailyTotalsMonth = Enumerable.Range(1, DateTime.DaysInMonth(startDate.Value.Year, startDate.Value.Month))
                        .ToDictionary(day => day, day => 0.0);

                    foreach (var transaction in filteredData)
                    {
                        dailyTotalsMonth[transaction.TransactionDate.Day] += transaction.ContributionAmount;
                    }

                    GoalAreaChart = new ObservableCollection<AreaChartData>(
                        dailyTotalsMonth.Select(entry => new AreaChartData
                        {
                            TimeInterval = entry.Key.ToString(),
                            Data = entry.Value
                        })
                    );
                    break;

                case "This Year":
                    Dictionary<int, double> monthlyTotals = Enumerable.Range(1, 12).ToDictionary(month => month, month => 0.0);

                    foreach (var transaction in filteredData)
                    {
                        monthlyTotals[transaction.TransactionDate.Month] += transaction.ContributionAmount;
                    }

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
    }


}
