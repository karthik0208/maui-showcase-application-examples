using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.ViewModel;
using MAUIShowcaseSample;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class GoalDetailMobilePage : ContentPage
{

    public GoalDetailMobilePage(UserDataService dataService, DataStore dataStore)
	{
        string pageTitle = "Goal";
        InitializeComponent();
		BindingContext = NavigationDataStore.GoalDetailPageViewModel;
    }

	private void OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
	{
		// ((GoalDetailPageViewModel)BindingContext).SelectAllRowsInGrid(e.Value);
	}
}