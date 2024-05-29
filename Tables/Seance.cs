using System;
using System.Collections.Generic;

namespace Kursach;

public partial class Seance
{
    public Seance() { }
    public Seance(int seanceId, int? hallId, int? movieId, DateTime? seanceDatetime, int? seancePlace) {
        SeanceId = seanceId;
        HallId = hallId;
        MovieId = movieId;
        SeanceDatetime = seanceDatetime;
        SeancePlace = seancePlace;
    }

    public int SeanceId { get; set; }

    public int? HallId { get; set; }

    public int? MovieId { get; set; }

    public DateTime? SeanceDatetime { get; set; }

    public int? SeancePlace { get; set; }

}
