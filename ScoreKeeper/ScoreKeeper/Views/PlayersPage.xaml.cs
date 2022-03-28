using AudioPlayEx;
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
        bool rollSoundEnabled = true;
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

        //void PlayerOptions(object sender, EventArgs e){}

        //void ManuallyUpdateScore(object sender, EventArgs e){}

        async void UpdateScore(object sender, EventArgs e)
        {
            ImageButton btn = (sender as ImageButton);
            Player player = await App.Database.GetPlayerAsync(Convert.ToInt32(btn.ClassId));
            if (player != null)
            {
                int score = player.CurrentScore;

                if (btn.CornerRadius.ToString() == "10")
                {
                    score++;
                    if (rollSoundEnabled)
                        DependencyService.Get<IAudio>().PlayAudioFile("plus.wav");
                }
                else
                {
                    score--;
                    if (rollSoundEnabled)
                        DependencyService.Get<IAudio>().PlayAudioFile("minus.wav");
                }

                player.CurrentScore = score;
                await App.Database.SavePlayerAsync(player);
                collectionView.ItemsSource = await App.Database.GetAllPlayersAsync();
            }
        }

        public void ToggleSound(object sender, EventArgs e)
        {
            rollSoundEnabled = !rollSoundEnabled;
            if (rollSoundEnabled)
            {
                MuteButton.IconImageSource = "unmute_icon.png";
            }
            else
            {
                MuteButton.IconImageSource = "mute_icon.png";
            }
        }
    }
}