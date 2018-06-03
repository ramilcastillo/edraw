using eDraw.api.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eDraw.api.Persistance
{
    public class EDrawDbContext : IdentityDbContext<ApplicationUser>
    {
        public EDrawDbContext(DbContextOptions<EDrawDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Invoices> Invoices { get; set; }
        public virtual DbSet<InvoiceTransactionHistory> InvoiceTransactionHistory { get; set; }
        public virtual DbSet<InvoiceTypes> InvoiceTypes { get; set; }
        public virtual DbSet<JobBudgets> JobBudgets { get; set; }
        public virtual DbSet<JobCategories> JobCategories { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<Loans> Loans { get; set; }
        public virtual DbSet<LoanStatus> LoanStatus { get; set; }
        public virtual DbSet<LoanTypes> LoanTypes { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
