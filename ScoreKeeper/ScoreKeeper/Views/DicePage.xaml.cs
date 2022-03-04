using System;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace ScoreKeeper.Views
{
    public partial class DicePage : ContentPage
    {
        DetectShake shake = new DetectShake();

        public DicePage()
        {
            InitializeComponent();
        }

        private async void HyperLinkTapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync("http://return-true.net", BrowserLaunchMode.SystemPreferred);
        }

        public void ShakeTest(object sender, EventArgs e)
        {
            shake.ToggleAccelerometer();
            //ShowPopup("Message");
        }

        public async void ShowPopup(string msg)
        {
            await App.Current.MainPage.DisplayAlert("Alert", msg, "OK");
        }


        async void OnButtonClicked(object sender, EventArgs e)
        {
            if ((sender as Button).Text.ToUpper() == "LINKEDIN")
            {
                // Launch the specified URL in the system browser.
                await Launcher.OpenAsync("https://www.linkedin.com/in/bjmacdonald/");
            }
            else
            {
                // Launch the specified URL in the system browser.
                await Launcher.OpenAsync("https://github.com/BrianMac/");
            }
        }
    }
}