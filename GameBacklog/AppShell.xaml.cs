using GameBacklog.Views;

namespace GameBacklog
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("add", typeof(AddGamePage));
            Routing.RegisterRoute("game", typeof(GameDetailPage));
        }
    }
}
