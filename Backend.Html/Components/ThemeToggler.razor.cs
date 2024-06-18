using Backend.Html.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Backend.Html.Components;

public partial class ThemeToggler : ComponentBase
{
    [Inject] private IThemeService _themeService { get; set; }
    private IJSObjectReference? _jsModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule ??= await jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./Components/ThemeToggler.razor.js");
            if (_jsModule != null) await _jsModule.InvokeVoidAsync("setTheme", _themeService.Theme);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnInitializedAsync()
    {
        if (_jsModule != null) await _jsModule.InvokeVoidAsync("setTheme", _themeService.Theme);
        await base.OnInitializedAsync();
    }
    
    private async Task OnCheckBoxChanged()
    {
        _themeService.ChangeTheme();
        if (_jsModule != null) await _jsModule.InvokeVoidAsync("setTheme", _themeService.Theme);
    }
}