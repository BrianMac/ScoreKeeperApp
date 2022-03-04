using ScoreKeeper.Views;
using Xamarin.Forms;

namespace ScoreKeeper
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PlayerEntryPage), typeof(PlayerEntryPage));
        }
    }
}