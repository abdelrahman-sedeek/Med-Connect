using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Interfaces
{
    public interface IDataSeeding
    {
        Task IdentityDataSeedAsync();
        Task SeedUsersAsync();
    }
}
