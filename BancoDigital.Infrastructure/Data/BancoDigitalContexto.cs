using BancoDigital.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BancoDigital.Infrastructure.Data
{
    public class BancoDigitalContexto : DbContext
    {
        public BancoDigitalContexto(DbContextOptions<BancoDigitalContexto> options)
         : base(options)
        {
        }

        public DbSet<Account> Account { get; set; }
       
    }
}
