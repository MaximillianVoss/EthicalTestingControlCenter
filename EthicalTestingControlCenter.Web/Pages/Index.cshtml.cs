using EthicalTestingControlCenter.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EthicalTestingControlCenter.Web.Pages;

public sealed class IndexModel(AppDbContext context) : PageModel
{
    public DashboardMetrics Metrics { get; private set; } = new(0, 0, 0, 0);

    public IReadOnlyList<RecentEngagementItem> RecentEngagements { get; private set; } = [];

    public IReadOnlyList<RecentFindingItem> RecentFindings { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Metrics = new DashboardMetrics(
            await context.Engagements.CountAsync(),
            await context.Assets.CountAsync(),
            await context.Findings.CountAsync(item => item.Status != "Resolved"),
            await context.Findings.CountAsync(item => item.Severity == "High" || item.Severity == "Critical"));

        RecentEngagements = await context.Engagements
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .ThenByDescending(item => item.Id)
            .Take(5)
            .Select(item => new RecentEngagementItem(
                item.Id,
                item.Name,
                item.ClientName,
                item.Status,
                item.StartDate,
                item.EndDate))
            .ToListAsync();

        RecentFindings = await context.Findings
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .ThenByDescending(item => item.Id)
            .Take(5)
            .Select(item => new RecentFindingItem(
                item.Title,
                item.Severity,
                item.Status,
                item.AssetName,
                item.Engagement.Name))
            .ToListAsync();
    }

    public sealed record DashboardMetrics(
        int EngagementsTotal,
        int AssetsTotal,
        int ActiveFindings,
        int PriorityFindings);

    public sealed record RecentEngagementItem(
        int Id,
        string Name,
        string ClientName,
        string Status,
        DateTime? StartDate,
        DateTime? EndDate);

    public sealed record RecentFindingItem(
        string Title,
        string Severity,
        string Status,
        string? AssetName,
        string EngagementName);
}
