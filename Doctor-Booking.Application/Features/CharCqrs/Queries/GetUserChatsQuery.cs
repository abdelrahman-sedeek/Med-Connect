using Doctor_Booking.Application.DTOs.ChatDtos;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctor_Booking.Application.Features.CharCqrs.Queries
{
    public class GetUserChatsQuery : IRequest<ResponseViewModel<List<ChatDto?>>>
    {
        public int UserId { get; set; }
        public GetUserChatsQuery(int userId)
        {
            UserId = userId;
        }   
    }
}
