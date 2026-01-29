using Doctor_Booking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface IFAQRepository
    {
        public Task<List<FAQ?>> GetAllAsync();
        public Task<FAQ> GetAsync(int id);
        public Task<int> DeleteAsync(int id);
        public Task<int> AddAsync(FAQ item);
        public Task<int> UpdateAsync(FAQ item);
    }
}
