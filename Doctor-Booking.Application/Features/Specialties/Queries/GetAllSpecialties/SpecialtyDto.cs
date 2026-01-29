namespace Doctor_Booking.Application.Features.Specialties.Queries.GetAllSpecialties
{
    public record SpecialtyDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}
