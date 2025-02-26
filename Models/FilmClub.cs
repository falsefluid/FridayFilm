using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FridayFilmClub
{
    public class Movies
    {
        [Key]
        public int MovieID { get; set; }
        [Required]
        public string? MovieTitle { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string? Rated { get; set; }
        public int RunTime { get; set; }
        public string? Plot { get; set; }
        [Required]
        public string? Poster { get; set; }
        public int NumberOfReviews { get; set; } = 0;
        public float AverageRating { get; set; } = 0;
        [Required]
        public DateOnly ReleaseDate { get; set; }
        [Required]
        public string? Trailer { get; set; }
        

        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<CelebritiesInMovies> CelebritiesInMovies { get; set; } = new List<CelebritiesInMovies>();
        public ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
        public ICollection<Trivia> Trivia { get; set; } = new List<Trivia>();
    }

    public class Genres 
    {
        [Key]
        public int GenreID { get; set; }
        [Required]
        public string? GenreName { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }

    public class MovieGenre
    {
        public int MovieID { get; set; }
        public int GenreID { get; set; }

        public Movies? Movie { get; set; }
        public Genres? Genre { get; set; }
    }

    public class Trivia 
    {
        [Key]
        public int TriviaID { get; set; }
        public int MovieID { get; set; }
        [Required]
        public string? TriviaText { get; set; }

        public Movies? Movie { get; set; }
    }

    public class Credits
    {
        [Key]
        public int CreditID { get; set; }
        [Required]
        public string? Role { get; set; }

        public ICollection<CelebritiesInMovies> CelebritiesInMovies { get; set; } = new List<CelebritiesInMovies>();
    }

    public class Celebrities
    {
        [Key]
        public int CelebrityID { get; set; }
        [Required]
        public string? Forename { get; set; }
        [Required]
        public string? Surname { get; set; }
        public string? Bio { get; set; }
        public string? Birthday { get; set; }
        public string? Birthplace { get; set; }
        [Required]
        public string? Picture { get; set; }

        public ICollection<CelebritiesInMovies> CelebritiesInMovies { get; set; } = new List<CelebritiesInMovies>();
    }

    public class CelebritiesInMovies
    {
        public int MovieID { get; set; }
        public int CelebrityID { get; set; }
        public int CreditID { get; set; }
        public string? Character { get; set; }

        public Movies? Movie { get; set; }
        public Celebrities? Celebrity { get; set; }
        public Credits? Credit { get; set; }
    }

    public class Users
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        [Required]
        public string? Salt { get; set; }
        [Required]
        public string? Bio { get; set; }
        public string? Picture { get; set; }
        public ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
    }

    public enum ReportCategory
    {
        Spam,
        InappropriateContent,
        Harassment,
        Hate,
        Privacy,
        Spoilers,
        Impersonation,
        Other
    }
    public class Reports
    {
        [Key]
        public int ReportID { get; set; }
        // Foreign key for the logged-in user (the one reporting)
        public int LoggedInUserId { get; set; }
        public Users? LoggedInUser { get; set; }

        // Foreign key for the user being reported
        public int ReportedUserId { get; set; }
        public Users? ReportedUser { get; set; }

        // Foreign key for the review being reported
        public int ReviewId { get; set; }
        public Reviews? Review { get; set; }

        [Required]
        public string? ReportCategory { get; set; }
        [Required]
        public string? ReportText { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }

    public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }
        public int MovieID { get; set; }
        public int UserID { get; set; }
        [Required]
        public float Rating { get; set; }
        public string? ReviewText { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool isSpoiler { get; set; }

        public Movies? Movie { get; set; }
        public Users? User { get; set; }
    }

    public class FilmClubContext : DbContext
    {
        public FilmClubContext(DbContextOptions<FilmClubContext> options) : base(options) 
        {    
        }

        public required DbSet<Movies> Movies { get; set; }
        public required DbSet<Genres> Genres { get; set; }
        public required DbSet<MovieGenre> MovieGenres { get; set; }
        public required DbSet<Credits> Credits { get; set; }
        public required DbSet<Celebrities> Celebrities { get; set; }
        public required DbSet<CelebritiesInMovies> CelebritiesInMovies { get; set; }
        public required DbSet<Users> Users { get; set; }
        public required DbSet<Reviews> Reviews { get; set;}
        public required DbSet<Reports> Reports { get; set; }
        public required DbSet<Trivia> Trivia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships

            modelBuilder.Entity<Users>() 
                .HasIndex(u => u.Username)
                .IsUnique(); // Makes sure that the username is unique

            modelBuilder.Entity<CelebritiesInMovies>()
                .HasOne(c => c.Movie)
                .WithMany(m => m.CelebritiesInMovies)
                .HasForeignKey(c => c.MovieID); // One-to-many relationship between Movies and CelebritiesInMovies
            
            modelBuilder.Entity<CelebritiesInMovies>()
                .HasOne(c => c.Celebrity)
                .WithMany(c => c.CelebritiesInMovies)
                .HasForeignKey(c => c.CelebrityID) // One-to-many relationship between Celebrities and CelebritiesInMovies
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CelebritiesInMovies>()
                .HasOne(c => c.Credit)
                .WithMany(c => c.CelebritiesInMovies)
                .HasForeignKey(c => c.CreditID) // One-to-many relationship between Credits and CelebritiesInMovies
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.Movie)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MovieID) // One-to-many relationship between Movies and Reviews
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID) // One-to-many relationship between Users and Reviews
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieID, mg.GenreID });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieID);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreID);

            modelBuilder.Entity<Reports>()
                .HasOne(r => r.LoggedInUser)
                .WithMany() 
                .HasForeignKey(r => r.LoggedInUserId) // One-to-many relationship between Users and Reports
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Reports>()
                .HasOne(r => r.ReportedUser)
                .WithMany() 
                .HasForeignKey(r => r.ReportedUserId) // One-to-many relationship between Users and Reports
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Reports>()
                .HasOne(r => r.Review)
                .WithMany() 
                .HasForeignKey(r => r.ReviewId) // One-to-many relationship between Reviews and Reports
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            modelBuilder.Entity<Trivia>()
                .HasOne(t => t.Movie)
                .WithMany(m => m.Trivia)
                .HasForeignKey(t => t.MovieID) // One-to-many relationship between Movies and Trivia
                .OnDelete(DeleteBehavior.Cascade); 
            
            modelBuilder.Entity<CelebritiesInMovies>()
                .HasKey(cm => new { cm.MovieID, cm.CelebrityID, cm.CreditID }); // Composite key for CelebritiesInMovies

            modelBuilder.Entity<Movies>()
                .Property(m => m.NumberOfReviews)
                .HasDefaultValue(0);

            modelBuilder.Entity<Movies>()
                .Property(m => m.AverageRating)
                .HasDefaultValue(0);

            // Seed data
            modelBuilder.Entity<Genres>().HasData(
                new Genres { GenreID = 1, GenreName = "Action" },
                new Genres { GenreID = 2, GenreName = "Animation" },
                new Genres { GenreID = 3, GenreName = "Adventure" },
                new Genres { GenreID = 4, GenreName = "Comedy" },
                new Genres { GenreID = 5, GenreName = "Crime" },
                new Genres { GenreID = 6, GenreName = "Drama" },
                new Genres { GenreID = 7, GenreName = "Documentary" },
                new Genres { GenreID = 8, GenreName = "Fantasy" },
                new Genres { GenreID = 9, GenreName = "Horror" },
                new Genres { GenreID = 10, GenreName = "Romance" },
                new Genres { GenreID = 11, GenreName = "Sci-Fi" },
                new Genres { GenreID = 12, GenreName = "Thriller" },
                new Genres { GenreID = 13, GenreName = "Western" },
                new Genres { GenreID = 14, GenreName = "Musical" },
                new Genres { GenreID = 15, GenreName = "War" }
            );

            modelBuilder.Entity<Credits>().HasData(
                new Credits { CreditID = 1, Role = "Director" },
                new Credits { CreditID = 2, Role = "Actor" },
                new Credits { CreditID = 3, Role = "Producer" },
                new Credits { CreditID = 4, Role = "Writer" },
                new Credits { CreditID = 5, Role = "Cinematographer" },
                new Credits { CreditID = 6, Role = "Self" },
                new Credits { CreditID = 7, Role = "Narrator" },
                new Credits { CreditID = 8, Role = "Writer" },
                new Credits { CreditID = 9, Role = "Voice" },
                new Credits { CreditID = 10, Role = "Editor" }
            );
        }         
    }
}