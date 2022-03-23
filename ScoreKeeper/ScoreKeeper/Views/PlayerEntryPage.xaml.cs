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
                ChangeAvatar(value);
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
            int selection;
            switch (avatarFileName)
            {
                case ("avatar_01.png"):
                    selection = 0;
                    break;
                case ("avatar_02.png"):
                    selection = 1;
                    break;
                case ("avatar_03.png"):
                    selection = 2;
                    break;
                case ("avatar_04.png"):
                    selection = 3;
                    break;
                case ("avatar_05.png"):
                    selection = 4;
                    break;
                case ("avatar_06.png"):
                    selection = 5;
                    break;
                case ("avatar_07.png"):
                    selection = 6;
                    break;
                case ("avatar_08.png"):
                    selection = 7;
                    break;
                default:
                    selection = -1;
                    break;
            }
            selectedAvatar = selection;
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var player = (Player)BindingContext;
            player.Date = DateTime.UtcNow;
            player.AvatarFileName = GetAvatarText(selectedAvatar);
            if (!string.IsNullOrWhiteSpace(player.Name))
            {                
                await App.Database.SavePlayerAsync(player);
            }

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }

        private string GetAvatarText(int selectedAvatar)
        {
            var player = (Player)BindingContext;
            string avatar = player.AvatarFileName;

            switch (selectedAvatar)
            {
                case (0):
                    avatar = "avatar_01.png";
                    break;
                case (1):
                    avatar = "avatar_02.png";
                    break;
                case (2):
                    avatar = "avatar_03.png";
                    break;
                case (3):
                    avatar = "avatar_04.png";
                    break;
                case (4):
                    avatar = "avatar_05.png";
                    break;
                case (5):
                    avatar = "avatar_06.png";
                    break;
                case (6):
                    avatar = "avatar_07.png";
                    break;
                case (7):
                    avatar = "avatar_08.png";
                    break;
                default:
                    avatar = "default_avatar.png";
                    break;
            }
            return avatar;
        }

        private void UpdateScore(object sender, EventArgs e)
        {
            App.Current.MainPage.DisplayAlert("Alert:", "You Clicked a " + sender.GetType().ToString(), "Dismiss");
        }

        private void ChangeAvatar(object sender, EventArgs e)
        {
            AvatarPreview.Source = (sender as ImageButton).Source;
            selectedAvatar = Convert.ToInt32((sender as ImageButton).ClassId);
        }

        private void ChangeAvatar(string newAdd)
        {
            AvatarPreview.Source = "default_avatar.png";
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Player)BindingContext;
            await App.Database.DeletePlayerAsync(note);

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
        }

    }
}