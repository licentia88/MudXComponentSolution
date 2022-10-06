using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Shared;

public class USERS
{
    [Required]
    [Key,DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int U_ROWID { get; set; }

    public string U_NAME { get; set; }

    [Required(ErrorMessage ="surname field is required")]
    public string U_SURNAME { get; set; }

    public string U_DESCRIPTION { get; set; }

    public string U_COUNTRY_CODE { get; set; }

    public bool U_IS_CHECKED { get; set; }

    [ForeignKey(nameof(U_COUNTRY_CODE))]
    public COUNTRIES COUNTRIES { get; set; }

    [ForeignKey(nameof(Shared.ORDERS.OR_USER_REFNO))]
    public ICollection<ORDERS> ORDERS { get; set; } = new HashSet<ORDERS>();

    [ForeignKey(nameof(Shared.TRANSACTIONS.T_USER_REFNO))]
    public ICollection<TRANSACTIONS> TRANSACTIONS { get; set; } = new HashSet<TRANSACTIONS>();
}
