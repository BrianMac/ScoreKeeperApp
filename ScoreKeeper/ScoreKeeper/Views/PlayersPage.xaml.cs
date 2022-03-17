using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScoreKeeper.Models;
using Xamarin.Forms;

namespace ScoreKeeper.Views
{
    public partial class PlayersPage : ContentPage
    {
        public PlayersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Retrieve all the players from the database, and set them as the
            // data source for the CollectionView.
            collectionView.ItemsSource = await App.Database.GetPlayersAsync();
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the PlayerEntryPage, without passing any data.
            await Shell.Current.GoToAsync(nameof(PlayerEntryPage));
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the PlayerEntryPage, passing the ID as a query parameter.
                Player player = (Player)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(PlayerEntryPage)}?{nameof(PlayerEntryPage.ItemId)}={player.ID.ToString()}");
            }
        }

        private void UpdateScore(object sender, EventArgs e)
        {

                App.Current.MainPage.DisplayAlert("Alert:", "You Clicked a " + sender.GetType().ToString(), "Dismiss");

        }
    }
}