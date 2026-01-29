using Doctor_Booking.Domain.Common;
using NetTopologySuite.Geometries;

namespace Doctor_Booking.Domain.Entities
{
    public class Patient : BaseEntity
    {
        public Point? Location { get; set; } = null! ;

        public int? UserId { get; set; }

        // Navigations
        public ApplicationUser? User { get; set; }
        public ICollection<SearchHistory> SearchHistories { get; set; } = new HashSet<SearchHistory>();
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    }
}
