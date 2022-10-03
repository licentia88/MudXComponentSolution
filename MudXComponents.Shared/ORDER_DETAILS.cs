using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Shared;

public class ORDER_DETAILS
{
    [Key, DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int ORD_ROWID { get; set; }

    public int ORD_M_REFNO { get; set; }

    public string ORD_ADDRESS { get; set; }
}