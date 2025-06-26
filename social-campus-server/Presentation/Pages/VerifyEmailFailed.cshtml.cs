using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Pages;

public class VerifyEmailFailed : PageModel
{
    public string? Error { get; set; }

    public void OnGet(string? error = null)
    {
        Error = error;
    }
}