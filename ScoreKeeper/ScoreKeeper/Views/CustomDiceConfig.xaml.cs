using System;
using System.IO;
using System.Text.RegularExpressions;
using ScoreKeeper.Models;
using Xamarin.Forms;

namespace ScoreKeeper.Views
{
    [QueryProperty(nameof(DiceId), nameof(DiceId))]

    public partial class CustomDiceConfig : ContentPage
    {
        public string DiceId
        {
            set
            {
                LoadDice(value);
            }
        }

        public CustomDiceConfig()
        {
            InitializeComponent();
            // Set the BindingContext of the page
            BindingContext = new CustomDice();
        }

        async void LoadDice(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(1);
                // Retrieve the player and set it as the BindingContext of the page.
                CustomDice customDice = await App.Database.GetDieAsync(id);
                BindingContext = customDice;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load custom dice.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (ValidateInput(lowEnd.Text, highEnd.Text))
            {
                var customDice = (CustomDice)BindingContext;
                BindingContext = customDice;
                customDice.LowEnd = Convert.ToInt32(lowEnd.Text);
                customDice.HighEnd = Convert.ToInt32(highEnd.Text);
                DicePage.Dxlow = Convert.ToInt32(lowEnd.Text);
                DicePage.Dxhigh = Convert.ToInt32(highEnd.Text);
                DicePage.Range = customDice.HighEnd - (customDice.LowEnd - 1);

                await App.Database.SaveDiceAsync(customDice);
                // Navigate backwards
                await Shell.Current.GoToAsync("..");
            }
        }

        bool ValidateInput(string lowEnd, string highEnd)
        {
            string lowNum = Regex.Replace(lowEnd, @"[^\d]", "");
            string highNum = Regex.Replace(highEnd, @"[^\d]", "");

            if (int.TryParse(lowNum, out _))
            {
                if (int.TryParse(highNum, out _))
                {
                    if (Convert.ToInt32(highNum) <= Convert.ToInt32(lowNum))
                    {
                        ShowPopup($"High end value is less than or equal to Low end value. Enter a positive whole number greater than {lowNum}.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    ShowPopup($"High End value is invalid. Enter a positive whole number greater than {lowNum}.");
                    return false;
                }
            }
            else
            {
                ShowPopup("Low end value is invalid. Enter a positive non-zero whole number.");
                return false;
            }
        }

        public void ShowPopup(string msg)
        {
            App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }
    }
}