using System.ComponentModel.DataAnnotations;
using EthicalTestingControlCenter.Web.Data;
using EthicalTestingControlCenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EthicalTestingControlCenter.Web.Pages.Engagements;

public sealed class CreateModel(AppDbContext context) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public void OnGet()
    {
        Input.Status = "Planned";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var engagement = new Engagement
        {
            Name = Input.Name.Trim(),
            ClientName = Input.ClientName.Trim(),
            Objective = Input.Objective.Trim(),
            AuthorizationCode = string.IsNullOrWhiteSpace(Input.AuthorizationCode) ? null : Input.AuthorizationCode.Trim(),
            ScopeDescription = Input.ScopeDescription.Trim(),
            StartDate = Input.StartDate,
            EndDate = Input.EndDate,
            Status = Input.Status,
            CreatedAt = DateTime.UtcNow
        };

        context.Engagements.Add(engagement);
        await context.SaveChangesAsync();

        TempData["FlashMessage"] = "Контур проверки создан.";
        TempData["FlashType"] = "success";

        return RedirectToPage("/Engagements/Details", new { id = engagement.Id });
    }

    public sealed class InputModel
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string ClientName { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Objective { get; set; } = string.Empty;

        [StringLength(100)]
        public string? AuthorizationCode { get; set; }

        [Required, StringLength(3000)]
        public string ScopeDescription { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required, StringLength(40)]
        public string Status { get; set; } = "Planned";
    }
}

