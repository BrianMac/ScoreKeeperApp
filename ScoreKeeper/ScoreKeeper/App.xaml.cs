using System;
using System.IO;
using ScoreKeeper.Data;
using Xamarin.Forms;

namespace ScoreKeeper
{
    public partial class App : Application
    {
        static PlayerDatabase database;

        // Create the database connection as a singleton.
        public static PlayerDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new PlayerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                }
                return database;
            }
        }


        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}