using System;
using System.Collections.Generic;
using Kursach.Tables;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Kursach;

public partial class CinemaContext : DbContext
{
    public CinemaContext()
    {
    }

    public CinemaContext(DbContextOptions<CinemaContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Hall> Halls { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Seance> Seances { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<AuthorizationData> AuthorizationData { get; set; }

    // делегат и событие для уведомления пользователя об изменении бд
    public delegate void BaseNotify(string str);
    public event BaseNotify? _notify;
    public void Register(BaseNotify Notify) {
        _notify += Notify;
    }
    public void unRegister(BaseNotify Notify) {
        _notify -= Notify;
    }
    public void RaiseNotify(string str) {
        _notify?.Invoke(str);
    }
    private static string connection= "Data Source=LADA\\LADA;Initial Catalog=Cinema;Trusted_Connection=True;TrustServerCertificate=True;";
    public void ConnectSqlServer() {
        ConsolePrinter printer = new ConsolePrinter();
        string server = printer.Read<string>("Введите название сервера: ");
        string database = printer.Read<string>("Введите название базы данных: ");
        connection = "Data Source=" + server + ";Initial Catalog=" + database + ";Trusted_Connection=True;TrustServerCertificate=True;";
        CinemaContext db = new CinemaContext();
        try {
            db.Database.OpenConnection();
            printer.Print("Подключение к базе данных успешно установлено.");
        }
        catch (Exception ex) {
            printer.Print("Ошибка подключения к базе данных: " + ex.Message);
        }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

        optionsBuilder.UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorizationData>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Authoriz__B9BF33070B08C11E");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_ID");
            entity.Property(e => e.Pass)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("pass");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_name");
        });
        modelBuilder.Entity<Hall>(entity =>
        {
            entity.HasKey(e => e.HallId).HasName("PK__Halls__A63ED487DB641EE7");

            entity.Property(e => e.HallId)
                .ValueGeneratedNever()
                .HasColumnName("hall_ID");
            entity.Property(e => e.HallCapacity).HasColumnName("hall_capacity");
            entity.Property(e => e.HallName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hall_name");
            entity.Property(e => e.HallState)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hall_state");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__83DCAA71916CE8A0");

            entity.Property(e => e.MovieId)
                .ValueGeneratedNever()
                .HasColumnName("movie_ID");
            entity.Property(e => e.MovieChrono)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("movie_chrono");
            entity.Property(e => e.MovieDirector)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("movie_director");
            entity.Property(e => e.MovieGenre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("movie_genre");
            entity.Property(e => e.MovieName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("movie_name");
            entity.Property(e => e.MovieProduction)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("movie_production");
            entity.Property(e => e.MovieYear).HasColumnName("movie_year");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Places__BF220D329221AA9C");

            entity.Property(e => e.PlaceId)
                .ValueGeneratedNever()
                .HasColumnName("place_ID");
            entity.Property(e => e.HallId).HasColumnName("hall_ID");
            entity.Property(e => e.PlaceCategory)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("place_category");
            entity.Property(e => e.PlaceNumber).HasColumnName("place_number");
            entity.Property(e => e.PlaceRow).HasColumnName("place_row");
        });

        modelBuilder.Entity<Seance>(entity =>
        {
            entity.HasKey(e => e.SeanceId).HasName("PK__Seances__4FD93C75A700E507");

            entity.Property(e => e.SeanceId)
                .ValueGeneratedNever()
                .HasColumnName("seance_ID");
            entity.Property(e => e.HallId).HasColumnName("hall_ID");
            entity.Property(e => e.MovieId).HasColumnName("movie_ID");
            entity.Property(e => e.SeanceDatetime)
                .HasColumnType("datetime")
                .HasColumnName("seance_datetime");
            entity.Property(e => e.SeancePlace).HasColumnName("seance_place");
        });

        modelBuilder.Entity<Ticket>(entity => {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__D597FD63A791C22E");

            entity.Property(e => e.TicketId)
                .ValueGeneratedNever()
                .HasColumnName("ticket_ID");
            entity.Property(e => e.HallId).HasColumnName("hall_ID");
            entity.Property(e => e.PlaceId).HasColumnName("place_ID");
            entity.Property(e => e.SeanceId).HasColumnName("seance_ID");
            entity.Property(e => e.TicketCost).HasColumnName("ticket_cost");
            entity.Property(e => e.TicketState).HasColumnName("ticket_state");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
