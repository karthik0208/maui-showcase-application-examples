using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample.View.SignIn
{
    public partial class UpdateAccountInfoPage : ContentPage
    {
        public UpdateAccountInfoPage(SignInPageViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
            AccountInfoDataForm.GenerateDataFormItem += OnAutoGeneratingDataFormItem;
        }

        private void OnAutoGeneratingDataFormItem(object sender, GenerateDataFormItemEventArgs e)
        {
            if (e.DataFormItem.FieldName == "TimeZone" && e.DataFormItem is DataFormComboBoxItem comboBoxItem)
            {
                comboBoxItem.ItemsSource = Enum.GetNames(typeof(TimeZoneEnum)).ToList();
                e.DataFormItem.ColumnSpan = 2; // Makes it span across two columns
            }
            else if(e.DataFormItem.FieldName == "Currency" && e.DataFormItem is DataFormComboBoxItem comboBoxItem1)
            {
                comboBoxItem1.ItemsSource = Enum.GetNames(typeof(CurrencyEnum)).ToList();
            }
            else if (e.DataFormItem.FieldName == "Gender" && e.DataFormItem is DataFormComboBoxItem comboBoxItem2)
            {
                comboBoxItem2.ItemsSource = Enum.GetNames(typeof(GenderEnum)).ToList();
            }
            else if (e.DataFormItem.FieldName == "Language" && e.DataFormItem is DataFormComboBoxItem comboBoxItem3)
            {
                comboBoxItem3.ItemsSource = Enum.GetNames(typeof(GenderEnum)).ToList();
            }
        }



        //private void CustomizeFormFields()
        //{
        //    //accountinfo.RegisterEditor("FirstName", new DataFormTextEditor { WidthRequest = 150 });
        //    //accountinfo.RegisterEditor("LastName", new DataFormTextEditor { WidthRequest = 150 });
        //    //dataForm.RegisterEditor("DateOfBirth", new DataFormDateEditor { WidthRequest = 120 });
        //    //dataForm.RegisterEditor("Gender", new DataFormDropDownEditor { WidthRequest = 100 });
        //    //dataForm.RegisterEditor("Language", new DataFormDropDownEditor { WidthRequest = 150 });
        //    //dataForm.RegisterEditor("Currency", new DataFormDropDownEditor { WidthRequest = 150 });
        //    //dataForm.RegisterEditor("TimeZone", new DataFormDropDownEditor { WidthRequest = 300 });
        //}
    }
}