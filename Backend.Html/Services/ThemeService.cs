using Backend.Html.Services.Contracts;

namespace Backend.Html.Services;

public class ThemeService: IThemeService
{
    public bool Theme { get; private set; }
    public string ThemeString => Theme ? "light" : "dark";
    public void ChangeTheme()
    {
        Theme = !Theme;
    }
}