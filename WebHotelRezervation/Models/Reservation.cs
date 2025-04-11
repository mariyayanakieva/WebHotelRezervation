using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHotelRezervation.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Client")]
        [Required]
        public int ClientId { get; set; }
        [ForeignKey("Room")]
        [Required]
        public int RoomId { get; set; }
        [Required]
        public DateTime Date { get; set; }= DateTime.Now;
        [Required]
        public int StayDuration { get; set; }
        //Navigation
        public Client? Client { get; set; }
        public Room? Room { get; set; }
    }
}
