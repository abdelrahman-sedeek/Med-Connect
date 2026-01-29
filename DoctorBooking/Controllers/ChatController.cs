using Doctor_Booking.Application.DTOs.ChatDtos;
using Doctor_Booking.Application.Features.Attachments.Commands;
using Doctor_Booking.Application.Features.CharCqrs.Commands;
using Doctor_Booking.Application.Features.CharCqrs.Queries;
using Doctor_Booking.Application.Features.CharCqrs.QueriesHandler;
using Doctor_Booking.Application.Features.ChatCqrs.Commands;
using Doctor_Booking.Application.ViewModels;
using Doctor_Booking.Domain.Entities;
using DoctorBooking.Extensions;
using DoctorBooking.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DoctorBooking.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    [Authorize(Roles ="Patient,Doctor")]
    public class ChatController : ControllerBase
    {
        string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp3", ".mp4" };
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(IMediator mediator,
                              IHubContext<ChatHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetUserChats()
        {
            var userId = User.GetCurrentUserId();
            var res = await _mediator.Send(new GetUserChatsQuery(userId));
            return Ok(res);
        }
        
        [HttpPost("start")]
        public async Task<IActionResult> CreateChat(int doctorId)
        {
            var userIds= new List<int>();
             var userId = User.GetCurrentUserId();
            userIds.Add(userId);
            userIds.Add(doctorId);
            var res = await _mediator.Send(
                new CreateChatCommand(userIds));
            if (res.IsSucsess != true)
                return BadRequest(res);
            else
                return Ok(res);
        }
        
        [HttpPost("{chatId}/messages")]
        public async Task<IActionResult> SendMessage(
            int chatId,
            [FromForm] ChatMessageDto message)
        {
            string cont = message.MessageCont is null ? "" : message.MessageCont;
            var userId = User.GetCurrentUserId();
            if (message.ContentType == "image" || message.ContentType == "voice")
            {
              cont =   await _mediator.Send(new UploadAttachmentCommand(message.File, (message.ContentType == "image" ? "images" : "voices")));
            }
            var chatUsers = await _mediator.Send(new GetChatUsersQuery(chatId));
            var badRes = ResponseViewModel<string>.FailureResponse("this user can not send message to this chat", 401, "Request Can not be completed", null);
            if (!chatUsers.Contains(userId))
                return BadRequest(badRes);

            var res = await _mediator.Send(
                new SendMessageCommand(chatId, userId, cont,message.ContentType));
             await _hubContext.Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message);
            return Ok(res);
        }
        
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetMessages(int chatId)
        {
            var userId = User.GetCurrentUserId();
            var chatUsers = await _mediator.Send(new GetChatUsersQuery(chatId));
            var badRes = ResponseViewModel<string>.FailureResponse("this user can not send message to this chat", 401, "Request Can not be completed", null);
            if (!chatUsers.Contains(userId))
                return BadRequest(badRes);

             await _mediator.Send(new MarkChatAsReadCommand(chatId, userId));

            var res = await _mediator.Send(
                new GetChatMessagesQuery(chatId));
            return Ok(res);
        }
        
        [HttpGet("{chatId}/unread-count")]
        public async Task<IActionResult> UnreadCount(int chatId)
        {
            var userId = User.GetCurrentUserId();
            
            var chatUsers = await _mediator.Send(new GetChatUsersQuery(chatId));
            var badRes = ResponseViewModel<string>.FailureResponse("this user can not deal with this chat", 401, "Request Can not be completed", null);
            if (!chatUsers.Contains(userId))
                return BadRequest(badRes);

            var res = await _mediator.Send(
                new CountUnreadMessagesQuery(userId, chatId));

            return Ok(res);
        }
         
        [HttpPost("{chatId}/mark-read")]
        public async Task<IActionResult> MarkRead(int chatId)
        {
            var userId = User.GetCurrentUserId();
           

            var chatUsers = await _mediator.Send(new GetChatUsersQuery(chatId));
            var badRes = ResponseViewModel<string>.FailureResponse("this user can not deal with this chat", 401, "Request Can not be completed", null);
            if (!chatUsers.Contains(userId))
                return BadRequest(badRes);

            var res=await _mediator.Send(new MarkChatAsReadCommand(chatId, userId));
            return Ok(res);
        }
        
        [HttpPatch("{chatId}/archive")]
        public async Task<IActionResult> ArchiveChat(int chatId)
        {
            var userId=User.GetCurrentUserId();
           
            var res = await _mediator.Send(new ChangeChatStatusCommand(userId, chatId, "archive"));
            return Ok(res);
        }
        
        [HttpPatch("{chatId}/favorite")]
        public async Task<IActionResult> FavoriteChat(int chatId)
        {
            var userId = User.GetCurrentUserId();
            
            var res = await _mediator.Send(new ChangeChatStatusCommand(userId, chatId, "favorite"));
            return Ok(res);
        }
        
        [HttpPatch("{chatId}/MakeItNormal")]
        public async Task<IActionResult> NormalChat(int chatId)
        {
            var userId = User.GetCurrentUserId();
            
           var res = await _mediator.Send(new ChangeChatStatusCommand(userId, chatId, "normal"));
            return Ok(res);
        }
        [HttpDelete("{chatID}/delete/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int chatId,int messageId)
        {
            var userId = User.GetCurrentUserId();


            var chatUsers = await _mediator.Send(new GetChatUsersQuery(chatId));
            var badRes = ResponseViewModel<string>.FailureResponse("this user can not deal with this chat", 401, "Request Can not be completed", null);
            if (!chatUsers.Contains(userId))
                return BadRequest(badRes);


            var res = await _mediator.Send(new DeleteMessageCommand(messageId));
            if(res.IsSucsess)return Ok(res);
            else return BadRequest(res);
        }
    }
}
