using Doctor_Booking.Application.Features.Caching.Commands;
using Doctor_Booking.Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Caching.CommandsHandler
{
    public class SetWithKeyCommandHandler : IRequestHandler<SetWithKeyCommand, string>
    {
        private readonly ICacheRepository _cacheRepostiory;
        public SetWithKeyCommandHandler(ICacheRepository cacheRepostiory)
        {
            _cacheRepostiory = cacheRepostiory;
        }
        public Task<string> Handle(SetWithKeyCommand request, CancellationToken cancellationToken)
        {
            var result = JsonSerializer.Serialize(request.Value);
            _cacheRepostiory.SetAsync(request.Key, result, request.TimeTolive);
            return Task.FromResult(result);
        }
    }
}
