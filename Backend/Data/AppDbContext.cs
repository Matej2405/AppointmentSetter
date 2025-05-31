using Backend.Entities;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CodingProblem> CodingProblems { get; set; }
        public DbSet<TestCase> TestCases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CodingProblem>(e =>
            {
                e.HasKey(cp => cp.Id);

                e.HasMany(cp => cp.TestCases)
                 .WithOne()
                 .HasForeignKey(tc => tc.CodingProblemId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CodingProblem>().HasData(
                new CodingProblem
                {
                    Id = 1,
                    Title = "Dragon's Lair: Square Grid Treasure Hunt",
                    Description = "In the heart of the Firepeak Mountains lies the lair of an ancient dragon, guardian of a legendary treasure. To claim the hoard, adventurers must first solve the dragon’s riddle:\r\n\r\n\"A grid of magic seals, square and bright,\r\nGuards the path to treasures of light.\r\nSum the seals where row and column align,\r\nAnd the vault shall open—prove your mind!\"\r\n\r\nThe dragon conjures an n x n grid of glowing magical seals. Each seal holds an integer value. Your task is to compute the sum of the seals along the main diagonal (from the top-left to the bottom-right corner). Only then will the dragon’s barrier fall!",
                    DifficultyMultiplier = 0.25,
                }
            );

            modelBuilder.Entity<TestCase>().HasData(
                new TestCase
                {
                    Id = 1,
                    CodingProblemId = 1, // Link to CodingProblem 1
                    InputArguments = "2\n1 2\n3 4",
                    ExpectedOutput = "5",
                    isHidden = false
                },
                new TestCase
                {
                    Id = 2,
                    CodingProblemId = 1,
                    InputArguments = "3\n10 20 30\n40 50 60\n70 80 90",
                    ExpectedOutput = "150",
                    isHidden = false
                },
                new TestCase
                {
                    Id = 3,
                    CodingProblemId = 1,
                    InputArguments = "2\n5 6\n8 9",
                    ExpectedOutput = "14",
                    isHidden = true
                },
                new TestCase
                {
                    Id = 4,
                    CodingProblemId = 1,
                    InputArguments = "1\n100",
                    ExpectedOutput = "100",
                    isHidden = true
                },
                new TestCase
                {
                    Id = 5,
                    CodingProblemId = 1,
                    InputArguments = "4\n1 2 3 4\n5 6 7 8\n9 10 11 12\n13 14 15 16",
                    ExpectedOutput = "34",
                    isHidden = true
                }
            );
        }
    }
}
