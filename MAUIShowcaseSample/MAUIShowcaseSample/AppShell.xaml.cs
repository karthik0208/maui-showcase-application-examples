using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MAUIShowcaseSample
{
    public partial class AppShell : Shell
    {
        public ICommand NavigateCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            NavigateCommand = new Command<string>(async (route) =>
            {
                await Shell.Current.GoToAsync("///" + route);
            });

            BindingContext = this; // Or use a proper ViewModel
           // NavigateCommand = new AsyncRelayCommand<string>(Navigate);
        }

        //public async Task Navigate(string route)
        //{
        //    await Shell.Current.GoToAsync("///" + route);
        //}
    }
}
