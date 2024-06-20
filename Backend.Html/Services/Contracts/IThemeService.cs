namespace Backend.Html.Services.Contracts;

public interface IThemeService
{
    void ChangeTheme();
    bool Theme { get; }
    string ThemeString { get; }
}