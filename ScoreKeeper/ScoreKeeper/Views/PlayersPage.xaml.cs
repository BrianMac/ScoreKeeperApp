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
            collectionView.ItemsSource = await App.Database.GetAllPlayersAsync();
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the PlayerEntryPage, without passing any data.
            //await Shell.Current.GoToAsync(nameof(PlayerEntryPage));
            await Shell.Current.GoToAsync($"{nameof(PlayerEntryPage)}?{nameof(PlayerEntryPage.NewAdd)}={""}");
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

        void PlayerOptions(object sender, EventArgs e)
        {

        }

        void ManuallyUpdateScore(object sender, EventArgs e)
        {

        }

        async void UpdateScore(object sender, EventArgs e)
        {
            ImageButton btn = (sender as ImageButton);
            Player player = await App.Database.GetPlayerAsync(Convert.ToInt32(btn.ClassId));
            if (player != null)
            {
                int score = player.CurrentScore;

                switch (btn.CornerRadius.ToString())
                {
                    case "10":
                        score++;
                        break;
                    case "9":
                        score--;
                        break;
                    default:
                        break;
                }
                player.CurrentScore = score;
                await App.Database.SavePlayerAsync(player);
                collectionView.ItemsSource = await App.Database.GetAllPlayersAsync();
            }
        }
    }
}