using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Shared;

public class COUNTRIES
{
    [Key]
    public string C_CODE { get; set; }

    public string C_DESC { get; set; }

    [ForeignKey(nameof(Shared.CITIES.CIT_COUNTRY_CODE))]
    public IEnumerable<CITIES> CITIES { get; set; }
}
