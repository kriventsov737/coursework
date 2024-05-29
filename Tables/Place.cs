using System;
using System.Collections.Generic;

namespace Kursach;

public partial class Place
{
    public Place() { }
    public Place(int placeId, int? hallId, bool? placeCategory, int? placeNumber, int? placeRow) {
        PlaceId = placeId;
        HallId = hallId;
        PlaceCategory = placeCategory;
        PlaceNumber = placeNumber;
        PlaceRow = placeRow;
    }

    public int PlaceId { get; set; }

    public int? HallId { get; set; }

    public bool? PlaceCategory { get; set; }

    public int? PlaceNumber { get; set; }

    public int? PlaceRow { get; set; }
}
