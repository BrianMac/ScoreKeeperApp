using System;
using System.IO;
using ScoreKeeper.Models;
using Xamarin.Forms;

namespace ScoreKeeper.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class PlayerEntryPage : ContentPage
    {
        public string ItemId
        {
            set
            {
                LoadPlayer(value);
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
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load player.");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var player = (Player)BindingContext;
            player.Date = DateTime.UtcNow;
            if (!string.IsNullOrWhiteSpace(player.Text))
            {
                await App.Database.SaveNoteAsync(player);
            }

            // Navigate backwards
            await Shell.Current.GoToAsync("..");
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