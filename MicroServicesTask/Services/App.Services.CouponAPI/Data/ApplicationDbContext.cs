using App.Services.CouponApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Services.CouponApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){}

        public DbSet<Coupon> Coupons {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Adding seed data to the coupons class
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon(){
                    CouponId = 1,
                    CouponCode = "10OFF",
                    MinAmount = 100,
                    DiscountAmount = 10
                },
                new Coupon()
                {
                    CouponId = 2,
                    CouponCode = "20OFF",
                    MinAmount = 100,
                    DiscountAmount = 20
                },
                new Coupon()
                {
                    CouponId = 3,
                    CouponCode = "30OFF",
                    MinAmount = 150,
                    DiscountAmount = 30
                }
            );
        }
    }
}