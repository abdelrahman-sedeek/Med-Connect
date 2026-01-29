namespace Doctor_Booking.Application.DTOs;

public record NotificationDto(
int Id,
string Title,
string Body,
bool IsRead,
DateTime CreatedAt
);
