using System;
using System.IO;
using ScoreKeeper.Models;
using Xamarin.Forms;

namespace ScoreKeeper.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    [QueryProperty(nameof(NewAdd), nameof(NewAdd))]
    public partial class PlayerEntryPage : ContentPage
    {
        int selectedAvatar = -1;
        readonly string[] avatars = new string[] { 
            "avatar_01.png", 
            "avatar_02.png", 
            "avatar_03.png", 
            "avatar_04.png", 
            "avatar_05.png", 
            "avatar_06.png", 
            "avatar_07.png", 
            "avatar_08.png", 
            "default_avatar.png" };

        public string ItemId
        {
            set
            {
                LoadPlayer(value);
            }
        }
        public string NewAdd
        {
            set
            {
                ChangeAvatarPreview(value);
            }
        }
        public PlayerEntryPage()
        {
            InitializeComponent();
            // Set the BindingContext of the page to a new Player.
            BindingContext = new Player();
        }

        async void LoadPlayer(string itemId)
        {
            try
            {
                int id = Convert.ToInt32(itemId);
                // Retrieve the player and set it as the BindingContext of the page.
                Player player = await App.Database.GetPlayerAsync(id);
                BindingContext = player;
                // Show the default avatar if player hasn't already chosen one
                if (player.AvatarFileName == "")
                {
                    AvatarPreview.Source = "default_avatar.png";
                }
                else
                {
                    SetSelectedAvatar(player.AvatarFileName);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load player.");
            }
        }

        private void SetSelectedAvatar(string avatarFileName)
        {
            selectedAvatar = Array.IndexOf(avatars, avatarFileName);
            AvatarPreview.Source = avatars[selectedAvatar];
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var player = (Player)BindingContext;
            player.Date = DateTime.UtcNow;
            player.AvatarFileName = GetAvatarText(selectedAvatar);
            if (!string.IsNullOrWhiteSpace(player.Name))
            {                
                if ((await App.Database.SavePlayerAsync(player)) > 0)
                {
                    // Navigate backwards upon successful save
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    string msg = "Sorry, there was a problem saving.";
                    await App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {   
                // Prompt player to set a name.
                string msg = "Player Name invalid. Field must not empty.";
                await App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
                PlayerName.Focus();
            }
        }

        async void CancelAndReturn(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private string GetAvatarText(int selectedAvatar)
        {
            if (!(selectedAvatar == -1))
            {
                return avatars[selectedAvatar];
            }
            else
            {
                return avatars[8];
            }
        }

        //private void UpdateScore(object sender, EventArgs e){}

        private void ChangeAvatar(object sender, EventArgs e)
        {
            AvatarPreview.Source = (sender as ImageButton).Source;
            selectedAvatar = Convert.ToInt32((sender as ImageButton).ClassId);
        }

        private void ChangeAvatarPreview(string newAdd)
        {
            AvatarPreview.Source = "default_avatar.png";
            DeleteButton.IsEnabled = false;
            DeleteButton.IsVisible = false;
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Delete Player?", "Are you sure you want to DELETE player?", "Continue", "Cancel");
            if (response)
            {
                var playa = (Player)BindingContext;
                await App.Database.DeletePlayerAsync(playa);
                // Navigate backwards
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}