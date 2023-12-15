using Implementacija.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Implementacija.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ObicniKorisnik> ObicniKorisnici { get; set; }
        public DbSet<Iznajmljivac> Iznajmljivaci { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Izvodjac> Izvodjaci { get; set; }
        public DbSet<Recenzija> Recenzije { get; set; }
        public DbSet<RezervacijaDvorane> RezervacijaDvorana { get; set; }
        public DbSet<RezervacijaKarte> RezervacijaKarata { get; set; }
        public DbSet<Dvorana> Dvorane { get; set; }
        public DbSet<Koncert> Koncerti { get; set; }
        public DbSet<Poruka> Poruke { get; set; }
        public DbSet<Implementacija.Models.Rezervacija> Rezervacija { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ObicniKorisnik>().ToTable("ObicniKorisnici");
            builder.Entity<Iznajmljivac>().ToTable("Iznajmljivaci");
            builder.Entity<Korisnik>().ToTable("Korisnici");
            builder.Entity<Izvodjac>().ToTable("Izvodjaci");
            builder.Entity<Recenzija>().ToTable("Recenzije");
            builder.Entity<RezervacijaDvorane>().ToTable("RezervacijaDvorana");
            builder.Entity<RezervacijaKarte>().ToTable("RezervacijaKarata");
            builder.Entity<Dvorana>().ToTable("Dvorane");
            builder.Entity<Koncert>().ToTable("Koncerti");
            builder.Entity<Poruka>().ToTable("Poruke");
            base.OnModelCreating(builder);       
        }

    }
}
