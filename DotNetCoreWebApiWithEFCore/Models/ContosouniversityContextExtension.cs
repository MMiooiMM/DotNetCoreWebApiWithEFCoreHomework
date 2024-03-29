﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetCoreWebApiWithEFCore.Models
{
    public partial class ContosouniversityContext
    {
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = this.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}