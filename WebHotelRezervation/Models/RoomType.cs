using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebHotelRezervation.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //Navigation
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
