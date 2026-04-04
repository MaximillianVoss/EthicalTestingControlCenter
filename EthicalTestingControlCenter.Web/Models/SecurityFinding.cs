using System.ComponentModel.DataAnnotations;

namespace EthicalTestingControlCenter.Web.Models;

public sealed class SecurityFinding
{
    public int Id { get; set; }

    public int EngagementId { get; set; }

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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Engagement Engagement { get; set; } = default!;
}

