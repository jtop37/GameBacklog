using System.Windows.Input;
using GameBacklog.Common;
using GameBacklog.Models;
using GameBacklog.Services;

namespace GameBacklog.ViewModels;

public class AddGameViewModel : ObservableObject
{
    private readonly GameRepository repository;
    private string title = string.Empty;
    private string? platform;
    private string yearText = DateTime.Now.Year.ToString();
    private string? imagePath;

    public AddGameViewModel(GameRepository repository)
    {
        this.repository = repository;
        SaveCommand = new Command(async () => await SaveAsync());
        PickImageCommand = new Command(async () => await PickImageAsync());
    }

    public IReadOnlyList<string> Platforms { get; } = new[]
    {
        "PC",
        "PlayStation 5",
        "PlayStation 4",
        "Xbox Series X|S",
        "Xbox One",
        "Nintendo Switch",
        "Nintendo Switch 2",
        "Steam Deck",
        "Other"
    };

    public ICommand SaveCommand { get; }

    public ICommand PickImageCommand { get; }

    public string? ImagePath
    {
        get => imagePath;
        set => SetProperty(ref imagePath, value);
    }

    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    public string? Platform
    {
        get => platform;
        set => SetProperty(ref platform, value);
    }

    public string YearText
    {
        get => yearText;
        set => SetProperty(ref yearText, value);
    }

    private async Task SaveAsync()
    {
        int.TryParse(YearText, out var year);

        await repository.AddAsync(new Game
        {
            Title = Title.Trim(),
            Platform = Platform ?? "Other",
            Year = year,
            ImagePath = ImagePath
        });

        await Shell.Current.GoToAsync("..");
    }

    private async Task PickImageAsync()
    {
        var file = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Select cover image",
            FileTypes = FilePickerFileType.Images
        });

        if (file is null)
            return;

        var coversDirectory = Path.Combine(FileSystem.AppDataDirectory, "covers");
        Directory.CreateDirectory(coversDirectory);

        var targetPath = Path.Combine(coversDirectory, $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");

        await using (var source = await file.OpenReadAsync())
        await using (var target = File.Create(targetPath))
        {
            await source.CopyToAsync(target);
        }

        ImagePath = targetPath;
    }
}
