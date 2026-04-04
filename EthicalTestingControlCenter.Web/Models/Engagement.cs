using System.ComponentModel.DataAnnotations;

namespace EthicalTestingControlCenter.Web.Models;

public sealed class Engagement
{
    public int Id { get; set; }

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

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AssessmentAsset> Assets { get; set; } = new List<AssessmentAsset>();

    public ICollection<SecurityFinding> Findings { get; set; } = new List<SecurityFinding>();
}

