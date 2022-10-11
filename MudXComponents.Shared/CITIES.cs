using System.ComponentModel.DataAnnotations;

namespace MudXComponents.Shared;

public class CITIES
{
    [Key]
    public string CIT_CODE { get; set; }

    public string CIT_COUNTRY_CODE { get; set; }

    public string CIT_DESC { get; set; }
}