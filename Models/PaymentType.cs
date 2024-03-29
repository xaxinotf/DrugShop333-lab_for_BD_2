﻿using System;
using System.Collections.Generic;

namespace DrugShop333.Models;

public partial class PaymentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Desription { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
