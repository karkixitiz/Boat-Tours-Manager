﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BoatToursManager.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BoatTourManagerEntities1 : DbContext
    {
        public BoatTourManagerEntities1()
            : base("name=BoatTourManagerEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Boat> Boat { get; set; }
        public virtual DbSet<BoatRental> BoatRental { get; set; }
        public virtual DbSet<BoatToursManager> BoatToursManager { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<GroupTrip> GroupTrip { get; set; }
        public virtual DbSet<GroupTripPriceCategory> GroupTripPriceCategory { get; set; }
        public virtual DbSet<LatLongCoordinate> LatLongCoordinate { get; set; }
        public virtual DbSet<LikeDislikeBoat> LikeDislikeBoat { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<PersonCategory> PersonCategory { get; set; }
        public virtual DbSet<Popularity> Popularity { get; set; }
        public virtual DbSet<PriceCategory> PriceCategory { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<SchedulePlan> SchedulePlan { get; set; }
        public virtual DbSet<ScheduleRoute> ScheduleRoute { get; set; }
        public virtual DbSet<Ship> Ship { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TripType> TripType { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
    }
}
