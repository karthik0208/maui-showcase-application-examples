using MAUIShowcaseSample.Services;
using MAUIShowcaseSample.ViewModel;

namespace MAUIShowcaseSample.View.Dashboard;

public partial class HelpAndSupportPage : ContentPage
{
	public HelpAndSupportPage(HelpAndSupportPageViewModel viewModel, UserDataService dataService, DataStore dataStore)
	{
        string pageTitle = "Help and Support";
        InitializeComponent();
		BindingContext = viewModel;
        var layoutViewModel = new DashboardLayoutPageViewModel(dataService, dataStore, pageTitle);
        this.contentcontainer.Content = new DashboardLayoutPage(layoutViewModel, dataService, dataStore);
    }
}