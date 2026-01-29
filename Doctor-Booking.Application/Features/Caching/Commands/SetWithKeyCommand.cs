using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.Caching.Commands
{
    public class SetWithKeyCommand : IRequest<string>
    {
        public object Value { get; set; }
        public string Key { get; set; }
        public TimeSpan TimeTolive { get; set; } = TimeSpan.FromHours(1);

        public SetWithKeyCommand(string key, string value, TimeSpan? time)
        {
            Value = value;
            Key = key;
            TimeTolive = time ?? TimeSpan.FromHours(1);
        }
    }
}
