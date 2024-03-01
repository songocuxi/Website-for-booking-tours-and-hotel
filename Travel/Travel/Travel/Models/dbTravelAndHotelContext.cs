using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Travel.Models
{
    public partial class dbTravelAndHotelContext : DbContext
    {
        public dbTravelAndHotelContext()
        {
        }

        public dbTravelAndHotelContext(DbContextOptions<dbTravelAndHotelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<GiamGia> GiamGia { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelBooking> HotelBookings { get; set; }
        public virtual DbSet<ImgHotel> ImgHotels { get; set; }
        public virtual DbSet<ImgTour> ImgTours { get; set; }
        public virtual DbSet<Location1> Location1s { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<TypeRoom> TypeRooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-4GCS7EQ4\\SQLEXPRESS;Initial Catalog=dbTravelAndHotel;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Fullname).IsRequired();

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Salt).IsRequired();

                entity.Property(e => e.Username).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Account");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Booking_Tour");
            });

            modelBuilder.Entity<GiamGia>(entity =>
            {
                entity.HasKey(e => e.Kmid);

                entity.Property(e => e.Kmid).HasColumnName("KMID");

                entity.Property(e => e.Ngay).HasColumnType("date");

                entity.Property(e => e.Thang).HasColumnType("date");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Phone).IsRequired();
            });

            modelBuilder.Entity<HotelBooking>(entity =>
            {
                entity.HasKey(e => e.Hbid);

                entity.ToTable("HotelBooking");

                entity.Property(e => e.Hbid).HasColumnName("HBID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.HotelBookings)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelBooking_Hotel");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.HotelBookings)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HotelBooking_Tour");
            });

            modelBuilder.Entity<ImgHotel>(entity =>
            {
                entity.ToTable("ImgHotel");

                entity.Property(e => e.Description).IsRequired();

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.ImgHotels)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImgHotel_Hotel");
            });

            modelBuilder.Entity<ImgTour>(entity =>
            {
                entity.ToTable("ImgTour");

                entity.Property(e => e.ImgTourId).HasColumnName("ImgTourID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.HasOne(d => d.Tour)
                    .WithMany(p => p.ImgTours)
                    .HasForeignKey(d => d.TourId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ImgTour_Tour");
            });

            modelBuilder.Entity<Location1>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.ToTable("Location1");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Parent).IsRequired();

                entity.Property(e => e.Type).IsRequired();
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.ToTable("Place");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Place__LocationI__73852659");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.ToTable("Tour");

                entity.Property(e => e.TourId).HasColumnName("TourID");

                entity.Property(e => e.Cost).IsRequired();

                entity.Property(e => e.EndTime).HasColumnType("date");

                entity.Property(e => e.PlaceId).HasColumnName("PlaceID");

                entity.Property(e => e.StartTime).HasColumnType("date");

                entity.HasOne(d => d.Place)
                    .WithMany(p => p.Tours)
                    .HasForeignKey(d => d.PlaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tour_Place");
            });

            modelBuilder.Entity<TypeRoom>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("TypeRoom");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.TypeRooms)
                    .HasForeignKey(d => d.HotelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TypeRoom_Hotel");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
