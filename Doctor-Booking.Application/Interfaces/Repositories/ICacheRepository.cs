using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces.Repositories
{
    public interface ICacheRepository
    {
        //Get
        Task<string?> GetAsync(string CacheKey);

        //Set
        Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive);
    }
}
