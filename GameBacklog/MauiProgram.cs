using GameBacklog.Services;
using GameBacklog.ViewModels;
using GameBacklog.Views;
using Microsoft.Extensions.Logging;

namespace GameBacklog
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<GameRepository>();

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<AddGameViewModel>();
            builder.Services.AddTransient<AddGamePage>();
            builder.Services.AddTransient<GameDetailViewModel>();
            builder.Services.AddTransient<GameDetailPage>();
            builder.Services.AddTransient<StatisticsViewModel>();
            builder.Services.AddTransient<StatisticsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
