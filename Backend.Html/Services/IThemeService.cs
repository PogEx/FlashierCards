namespace Backend.Html.Services;

public interface IThemeService
{
    void ChangeTheme();
    bool Theme { get; }
    
    string ThemeString { get; }
}