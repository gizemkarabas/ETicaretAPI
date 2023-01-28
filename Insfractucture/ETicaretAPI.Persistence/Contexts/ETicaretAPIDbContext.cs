using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Contexts
{
	public class ETicaretAPIDbContext : DbContext
	{
		public ETicaretAPIDbContext(DbContextOptions options) : base(options)
		{ //IoC konteynıra koyulacak orada doldurulacak *//
		}

		public DbSet<Product> Products { get; set; } //Entity e karşılık tablo oluşturulması.
		public DbSet<Order> Orders { get; set; } 
		public DbSet<Customer> customers{ get; set; }


		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			//Entityler üzerinden yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını sağlayan propertydir. Update operasyonlarında Track edilen verileri yakalayıp elde etmemizi sağlar.
			var datas = ChangeTracker
				.Entries<BaseEntity>();
			foreach (var data in datas)
			{
				_ = data.State switch
				{
					EntityState.Added=> data.Entity.CreatedDate=DateTime.UtcNow,
					EntityState.Modified=>data.Entity.UpdateDate=DateTime.UtcNow
				};
			}
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
