using System.ComponentModel.DataAnnotations;

namespace MudXComponents.Shared;

public class COUNTRIES
{
    [Key]
    public string C_CODE { get; set; }

    public string C_DESC { get; set; }
}