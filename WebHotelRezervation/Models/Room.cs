using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHotelRezervation.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public int Beds { get; set; }
        [Required]
        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }
        [Required]
        public decimal Price { get; set; }
        //Navigation
        public RoomType? RoomType { get; set; }
    }
}
