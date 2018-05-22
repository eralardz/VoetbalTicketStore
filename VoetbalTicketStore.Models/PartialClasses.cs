﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoetbalTicketStore.Models
{

    [MetadataType(typeof(BezoekerMetadata))]
    public partial class Bezoeker
    {
    }

    [MetadataType(typeof(WedstrijdMetadata))]
    public partial class Wedstrijd
    {
    }

    [MetadataType(typeof(BestellingMetadata))]
    public partial class Bestelling
    {
    }

    [MetadataType(typeof(StadionMetadata))]
    public partial class Stadion
    {
    }

    [MetadataType(typeof(ClubMetadata))]
    public partial class Club
    {
    }

    [MetadataType(typeof(TicketMetadata))]
    public partial class Ticket
    {
    }

    [MetadataType(typeof(AbonnementMetadata))]
    public partial class Abonnement
    {
    }

    [MetadataType(typeof(ShoppingCartDataMetadata))]
    public partial class ShoppingCartData
    {
    }

}
