using System.ComponentModel.DataAnnotations;

namespace EthicalTestingControlCenter.Web.Models;

public sealed class AssessmentAsset
{
    public int Id { get; set; }

    public int EngagementId { get; set; }

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

    public Engagement Engagement { get; set; } = default!;
}

