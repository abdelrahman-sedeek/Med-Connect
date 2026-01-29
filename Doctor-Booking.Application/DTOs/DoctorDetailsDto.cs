using Doctor_Booking.Domain.Enum;

namespace Doctor_Booking.Application.DTOs;

public class DoctorDetailsDto
{
	
		public int DoctorId { get; set; }
		public string Full_Name { get; set; } = null!;
	    public DoctorStatus doctor_Status { get; set; }
		public decimal SessionPrice { get; set; }
		public decimal AverageRating { get; set; }
		public string About { get; set; }
	public List<ReviewDto> Reviews { get; set; } = new();
	

}
