using EthicalTestingControlCenter.Web.Data;
using EthicalTestingControlCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EthicalTestingControlCenter.Web.Pages.Engagements;

public sealed class DetailsModel(AppDbContext context) : PageModel
{
    public Engagement Engagement { get; private set; } = default!;

    public IReadOnlyList<AssessmentAsset> Assets { get; private set; } = [];

    public IReadOnlyList<SecurityFinding> Findings { get; private set; } = [];

    public IReadOnlyList<ComplianceCheck> ComplianceChecks { get; private set; } = [];

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
            .OrderBy(item => item.Criticality == "Critical" ? 0 :
                item.Criticality == "High" ? 1 :
                item.Criticality == "Medium" ? 2 : 3)
            .ThenByDescending(item => item.Id)
            .ToListAsync();

        Findings = await context.Findings
            .AsNoTracking()
            .Where(item => item.EngagementId == id)
            .OrderBy(item => item.Severity == "Critical" ? 0 :
                item.Severity == "High" ? 1 :
                item.Severity == "Medium" ? 2 : 3)
            .ThenByDescending(item => item.Id)
            .ToListAsync();

        ComplianceChecks =
        [
            new ComplianceCheck(
                "Формальное разрешение",
                !string.IsNullOrWhiteSpace(Engagement.AuthorizationCode),
                !string.IsNullOrWhiteSpace(Engagement.AuthorizationCode)
                    ? "Код авторизации указан."
                    : "Нужно зафиксировать разрешение на проведение работ."),
            new ComplianceCheck(
                "Согласованный scope",
                !string.IsNullOrWhiteSpace(Engagement.ScopeDescription),
                !string.IsNullOrWhiteSpace(Engagement.ScopeDescription)
                    ? "Границы проверки описаны."
                    : "Нет описания охвата тестирования."),
            new ComplianceCheck(
                "Контур активов",
                Assets.Count > 0,
                Assets.Count > 0
                    ? $"В scope добавлено {Assets.Count} актив(ов)."
                    : "Активы пока не заведены."),
            new ComplianceCheck(
                "Регистр находок",
                Findings.Count > 0,
                Findings.Count > 0
                    ? $"Зафиксировано {Findings.Count} находк(и)."
                    : "Находки пока не оформлены.")
        ];

        return Page();
    }

    public sealed record ComplianceCheck(string Title, bool IsOk, string Text);
}

