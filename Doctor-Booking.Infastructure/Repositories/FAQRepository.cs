using Doctor_Booking.Application.Interfaces.Repositories;
using Doctor_Booking.Domain.Entities;
using Doctor_Booking.Infastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Infastructure.Repositories
{
    public class FAQRepository : IFAQRepository
    {

        private readonly DoctorBookingDbContext _context;
        public FAQRepository(DoctorBookingDbContext context)
        {
            _context = context;
        }
        public async Task<int> AddAsync(FAQ item)
        {
           _context.FAQs.Add(item);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var faq = _context.FAQs.Where(x=>x.Id == id).First();
            _context.FAQs.Remove(faq);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<FAQ?>> GetAllAsync()
        {
            var faqs = await _context.FAQs.ToListAsync();
            return faqs;
        }

        public async Task<FAQ> GetAsync(int id)
        {
            var faq = await _context.FAQs.Where(x=>x.Id==id).FirstAsync();
            return faq;
        }

        public async Task<int> UpdateAsync(FAQ item)
        {
            _context.FAQs.Update(item);
            return await _context.SaveChangesAsync();
        }
    }
}
