using System;
using System.Collections.Generic;

namespace Kursach;

public partial class Hall
{
    public Hall() { }
    public Hall(int hallId, string? hallName, bool hallState, int? hallCapacity) {
        HallId = hallId;
        HallName = hallName;
        HallState = hallState;
        HallCapacity = hallCapacity;
    }

    public int HallId { get; set; }

    public string? HallName { get; set; }

    public bool HallState { get; set; }

    public int? HallCapacity { get; set; }
}
