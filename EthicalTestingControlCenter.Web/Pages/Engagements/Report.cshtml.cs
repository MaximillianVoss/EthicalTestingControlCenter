using EthicalTestingControlCenter.Web.Data;
using EthicalTestingControlCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EthicalTestingControlCenter.Web.Pages.Engagements;

public sealed class ReportModel(AppDbContext context) : PageModel
{
    public Engagement Engagement { get; private set; } = default!;

    public IReadOnlyList<AssessmentAsset> Assets { get; private set; } = [];

    public IReadOnlyList<SecurityFinding> Findings { get; private set; } = [];

    public IReadOnlyList<SecurityFinding> OpenFindings { get; private set; } = [];

    public IReadOnlyList<SeveritySummary> SeveritySummaries { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var engagement = await context.Engagements
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (engagement is null)
        {
            TempData["FlashMessage"] = "Контур проверки не найден.";
            TempData["FlashType"] = "error";
            return RedirectToPage("/Engagements/Index");
        }

        Engagement = engagement;
        Assets = await context.Assets
            .AsNoTracking()
            .Where(item => item.EngagementId == id)
            .OrderBy(item => item.AssetName)
            .ToListAsync();

        Findings = await context.Findings
            .AsNoTracking()
            .Where(item => item.EngagementId == id)
            .OrderByDescending(item => item.CreatedAt)
            .ToListAsync();

        OpenFindings = Findings.Where(item => item.Status != "Resolved").ToList();

        SeveritySummaries =
        [
            new SeveritySummary("Critical", Findings.Count(item => item.Severity == "Critical")),
            new SeveritySummary("High", Findings.Count(item => item.Severity == "High")),
            new SeveritySummary("Medium", Findings.Count(item => item.Severity == "Medium")),
            new SeveritySummary("Low", Findings.Count(item => item.Severity == "Low"))
        ];

        return Page();
    }

    public sealed record SeveritySummary(string Name, int Count);
}

