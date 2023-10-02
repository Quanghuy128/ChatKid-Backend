using ChatKid.DataLayer.Entities;
using ChatKid.DataLayer.EntityInterfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatKid.DataLayer
{
    public class ApplicationDBContext : DbContext, IDBContext
    {
        public DbSet<AccountMockData> AccountMock { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            AccountMock.AddRange(new AccountMockData[]
            {
                new AccountMockData()
                {
                    Id = new Guid(),
                    UserName = "chatkid",
                    Password = "1",
                }
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
