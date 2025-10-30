
using System.Text.Json.Serialization;

namespace goftapp.Entity;


public class Company
{
  
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Address { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public bool IsPartnerActive { get; set; } = true;

    // ผู้แทนบริษัท (1 บริษัทมี 1 user เป็นตัวแทน)
    public Guid? CompanyRepUserId { get; set; }
    [JsonIgnore]
    public Users? CompanyRepUser { get; set; }

    public ICollection<InternshipApplication> Applications { get; set; } = new List<InternshipApplication>();
}