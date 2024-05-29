using System;
using System.Collections.Generic;

namespace Kursach;

public partial class Movie
{
    public Movie() { }
    public Movie(int movieId, string? movieName, int? movieYear, string? movieDirector, string? movieGenre, string? movieChrono, string? movieProduction) {
        MovieId = movieId;
        MovieName = movieName;
        MovieYear = movieYear;
        MovieDirector = movieDirector;
        MovieGenre = movieGenre;
        MovieChrono = movieChrono;
        MovieProduction = movieProduction;
    }

    public int MovieId { get; set; }

    public string? MovieName { get; set; }

    public int? MovieYear { get; set; }

    public string? MovieDirector { get; set; }

    public string? MovieGenre { get; set; }

    public string? MovieChrono { get; set; }

    public string? MovieProduction { get; set; }
}
