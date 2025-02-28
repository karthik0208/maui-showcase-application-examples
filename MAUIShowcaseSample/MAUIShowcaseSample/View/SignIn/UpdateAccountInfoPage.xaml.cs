using Syncfusion.Maui.DataForm;

namespace MAUIShowcaseSample.View.SignIn
{
    public partial class UpdateAccountInfoPage : ContentPage
    {
        public UpdateAccountInfoPage(SignInPageViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
            AccountInfoDataForm.GenerateDataFormItem += this.OnAutoGeneratingDataFormItem;
        }

        private void OnAutoGeneratingDataFormItem(object sender, GenerateDataFormItemEventArgs e)
        {
            if (e.DataFormItem.FieldName == "TimeZone")
            {
                e.DataFormItem.ColumnSpan = 2; // Makes it span across two columns
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