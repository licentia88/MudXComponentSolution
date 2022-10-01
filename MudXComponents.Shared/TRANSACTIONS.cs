using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Shared;

public class TRANSACTIONS
{
    [Key, DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int T_ROWID { get; set; }

    public int T_USER_REFNO { get; set; }

    public string T_DETAIL { get; set; }

    //[DataType(DataType.DateTime)]
    public DateTime? T_DATE { get; set; }

     
}