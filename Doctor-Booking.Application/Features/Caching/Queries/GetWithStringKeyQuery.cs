using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Caching.Queries
{
    public class GetWithStringKeyQuery : IRequest<string>
    {
        private readonly string _key;
        public string Key => _key;
        public GetWithStringKeyQuery(string key)
        {
            _key = key;
        }
    }
}
