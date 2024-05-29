using System;
using System.Collections.Generic;

namespace Kursach;

public partial class Ticket
{
    public Ticket() { }
    public Ticket(int ticketId, int? ticketCost, bool? ticketState, int? seanceId, int? hallId, int? placeId) {
        TicketId = ticketId;
        TicketCost = ticketCost;
        TicketState = ticketState;
        SeanceId = seanceId;
        HallId = hallId;
        PlaceId = placeId;
    }

    public int TicketId { get; set; }

    public int? TicketCost { get; set; }

    public bool? TicketState { get; set; }

    public int? SeanceId { get; set; }

    public int? HallId { get; set; }
    public int? PlaceId { get; set; }
}
