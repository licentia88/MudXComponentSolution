﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MudXComponents.Shared;

public class USERS
{
    [Key,DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int U_ROWID { get; set; }

    public string U_NAME { get; set; }

    public string U_SURNAME { get; set; }

    public string U_DESCRIPTION { get; set; }

    public string U_COUNTRY_CODE { get; set; }

    [ForeignKey(nameof(U_COUNTRY_CODE))]
    public COUNTRIES COUNTRIES { get; set; }

    [ForeignKey(nameof(Shared.ORDERS.OR_USER_REFNO))]
    public ICollection<ORDERS> ORDERS { get; set; } = new HashSet<ORDERS>();
}