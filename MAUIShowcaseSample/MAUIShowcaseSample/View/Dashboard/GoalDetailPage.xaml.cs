using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.ViewModel;
using MAUIShowcaseSample;

namespace MAUIShowcaseSample.View.Dashboard;

[QueryProperty(nameof(GoalId), "GoalId")]
public partial class GoalDetailPage : ContentPage
{
    public int GoalId { get; set; }

    public GoalDetailPage(UserDataService dataService, DataStore dataStore)
	{
        string pageTitle = "Goal";
        InitializeComponent();
		BindingContext = NavigationDataStore.GoalDetailPageViewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }

	private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
	{
		// ((GoalDetailPageViewModel)BindingContext).SelectAllRowsInGrid(e.Value);
	}
}