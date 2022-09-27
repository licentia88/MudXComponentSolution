using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Shared;

public class ORDERS
{
    [Key,DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int OR_ROWID { get; set; }

    public int OR_USER_REFNO { get; set; }
    
    public string OR_NAME { get; set; }

    public int OR_QUANTITY { get; set; }
}