using Doctor_Booking.Application.Features.Caching.Queries;
using Doctor_Booking.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Caching.QueriesHandler
{
    public class GetWithStringKeyQueryHandler : IRequestHandler<GetWithStringKeyQuery, string>
    {
        private readonly ICacheRepository _cacheRepostiory;
        public GetWithStringKeyQueryHandler(ICacheRepository cacheRepostiory)
        {
            _cacheRepostiory = cacheRepostiory;
        }
        public async Task<string> Handle(GetWithStringKeyQuery request, CancellationToken cancellationToken)
        {
            var result = await _cacheRepostiory.GetAsync(request.Key);
            if (result is null)
            {
                return null;
            }
            return result;
        }
    }
}
