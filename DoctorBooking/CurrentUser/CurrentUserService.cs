namespace DoctorBooking.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public int UserId
		=> int.Parse(
			_httpContextAccessor.HttpContext!
				.User
				.FindFirst("userId")!.Value);
}
