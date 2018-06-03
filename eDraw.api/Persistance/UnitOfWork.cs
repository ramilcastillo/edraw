using eDraw.api.Core;
using System.Threading.Tasks;

namespace eDraw.api.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EDrawDbContext _context;
    
        public UnitOfWork(EDrawDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

    
    }
}
