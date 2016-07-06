using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Tennis.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<Reservation> Reservations { get; set; }

        public ModelContext(DbContextOptions<ModelContext> contextOptions)
            : base(contextOptions) { }
    }

    public class Reservation
    {
        [Required]
        [Key]
        public int ID { get; set; }

        [Required]
        public string Reserver { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
    }
}
