using System.ComponentModel.DataAnnotations;
using EthicalTestingControlCenter.Web.Data;
using EthicalTestingControlCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EthicalTestingControlCenter.Web.Pages.Engagements;

public sealed class AddFindingModel(AppDbContext context) : PageModel
{
    public Engagement Engagement { get; private set; } = default!;

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var engagement = await context.Engagements.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (engagement is null)
        {
            TempData["FlashMessage"] = "Контур проверки не найден.";
            TempData["FlashType"] = "error";
            return RedirectToPage("/Engagements/Index");
        }

        Engagement = engagement;
        Input.Severity = "Medium";
        Input.Status = "Open";
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var engagement = await context.Engagements.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id);
        if (engagement is null)
        {
            TempData["FlashMessage"] = "Контур проверки не найден.";
            TempData["FlashType"] = "error";
            return RedirectToPage("/Engagements/Index");
        }

        Engagement = engagement;

        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.Findings.Add(new SecurityFinding
        {
            EngagementId = id,
            Title = Input.Title.Trim(),
            Category = Input.Category.Trim(),
            Severity = Input.Severity,
            Status = Input.Status,
            AssetName = string.IsNullOrWhiteSpace(Input.AssetName) ? null : Input.AssetName.Trim(),
            Evidence = string.IsNullOrWhiteSpace(Input.Evidence) ? null : Input.Evidence.Trim(),
            Recommendation = Input.Recommendation.Trim(),
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        TempData["FlashMessage"] = "Находка зарегистрирована.";
        TempData["FlashType"] = "success";

        return RedirectToPage("/Engagements/Details", new { id });
    }

    public sealed class InputModel
    {
        [Required, StringLength(180)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string Category { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string Severity { get; set; } = "Medium";

        [Required, StringLength(40)]
        public string Status { get; set; } = "Open";

        [StringLength(160)]
        public string? AssetName { get; set; }

        [StringLength(2000)]
        public string? Evidence { get; set; }

        [Required, StringLength(3000)]
        public string Recommendation { get; set; } = string.Empty;
    }
}

