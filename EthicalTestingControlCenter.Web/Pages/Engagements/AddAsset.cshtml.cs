using System.ComponentModel.DataAnnotations;
using EthicalTestingControlCenter.Web.Data;
using EthicalTestingControlCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EthicalTestingControlCenter.Web.Pages.Engagements;

public sealed class AddAssetModel(AppDbContext context) : PageModel
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
        Input.Criticality = "Medium";
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

        context.Assets.Add(new AssessmentAsset
        {
            EngagementId = id,
            AssetName = Input.AssetName.Trim(),
            AssetType = Input.AssetType.Trim(),
            IpAddress = string.IsNullOrWhiteSpace(Input.IpAddress) ? null : Input.IpAddress.Trim(),
            Criticality = Input.Criticality,
            OwnerName = string.IsNullOrWhiteSpace(Input.OwnerName) ? null : Input.OwnerName.Trim(),
            Notes = string.IsNullOrWhiteSpace(Input.Notes) ? null : Input.Notes.Trim()
        });

        await context.SaveChangesAsync();

        TempData["FlashMessage"] = "Актив добавлен.";
        TempData["FlashType"] = "success";

        return RedirectToPage("/Engagements/Details", new { id });
    }

    public sealed class InputModel
    {
        [Required, StringLength(160)]
        public string AssetName { get; set; } = string.Empty;

        [Required, StringLength(120)]
        public string AssetType { get; set; } = string.Empty;

        [StringLength(120)]
        public string? IpAddress { get; set; }

        [Required, StringLength(40)]
        public string Criticality { get; set; } = "Medium";

        [StringLength(120)]
        public string? OwnerName { get; set; }

        [StringLength(1500)]
        public string? Notes { get; set; }
    }
}

