﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FasTagApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FasTagEntities1 : DbContext
    {
        public FasTagEntities1()
            : base("name=FasTagEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<VehicleRequest> VehicleRequests { get; set; }
        public virtual DbSet<MobileChange> MobileChanges { get; set; }
        public virtual DbSet<Billing> Billings { get; set; }
        public virtual DbSet<DMMASTER> DMMASTERs { get; set; }
        public virtual DbSet<PMMASTER> PMMASTERs { get; set; }
    }
}
