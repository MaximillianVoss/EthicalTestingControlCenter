using EthicalTestingControlCenter.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EthicalTestingControlCenter.Web.Pages.Engagements;

public sealed class IndexModel(AppDbContext context) : PageModel
{
    public IReadOnlyList<EngagementOverview> Items { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Items = await context.Engagements
            .AsNoTracking()
            .OrderByDescending(item => item.CreatedAt)
            .ThenByDescending(item => item.Id)
            .Select(item => new EngagementOverview(
                item.Id,
                item.Name,
                item.ClientName,
                item.Status,
                item.Assets.Count,
                item.Findings.Count))
            .ToListAsync();
    }

    public sealed record EngagementOverview(
        int Id,
        string Name,
        string ClientName,
        string Status,
        int AssetsCount,
        int FindingsCount);
}

