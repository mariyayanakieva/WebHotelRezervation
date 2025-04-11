using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebHotelRezervation.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string IdCard { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Nationality { get; set; }
        //Navigation
        public List<Reservation> Reservations { get; set; }=new List<Reservation>(){ }; 
    }
}
