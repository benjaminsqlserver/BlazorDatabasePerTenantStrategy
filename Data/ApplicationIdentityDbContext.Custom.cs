﻿using Microsoft.EntityFrameworkCore;

namespace BikeStores.Data
{
    public partial class ApplicationIdentityDbContext
    {
        private readonly HttpContext context;
        private readonly Multitenancy multitenancy;

        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options, IHttpContextAccessor httpContextAccessor, Multitenancy mt) : base(options)
        {
            context = httpContextAccessor.HttpContext;
            multitenancy = mt;
        }

        public ApplicationIdentityDbContext(IHttpContextAccessor httpContextAccessor, Multitenancy mt)
        {
            context = httpContextAccessor.HttpContext;
            multitenancy = mt;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (multitenancy != null && context != null)
            {
                var tenant = multitenancy.Tenants
                        .Where(t => t.Hostnames.Contains(context.Request.Host.Value)).FirstOrDefault();

                if (tenant != null)
                {
                    optionsBuilder.UseSqlServer(tenant.ConnectionString);
                }
            }
        }
    }
}
