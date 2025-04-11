using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHotelRezervation.Models;

namespace HotelConsoleApp
{
    public class ReservationService
    {
        private readonly HotelDbContext context;
        public ReservationService(HotelDbContext context)
        {
            this.context = context;
        }
        public Client AddClient(string name, string idcard, string address, string phone, string nationality)
        {
            var client = new Client
            {
                Name = name,
                IdCard = idcard,
                Address = address,
                Phone = phone,
                Nationality = nationality
            };
            //Добавяме в БД
            context.Clients.Add(client);
            context.SaveChanges();
            return client;
        }//end add
        public List<Room> GetAllRoom()
        {
            return context.Rooms.Include(r => r.RoomType).ToList();
        }//end2
        public Reservation MakeReservation(int clientId, int roomId, DateTime data, int stayduration)
        {
            var reservation = new Reservation
            {
                ClientId = clientId,
                RoomId = roomId,
                Date = data,
                StayDuration = stayduration
            };
            context.Reservations.Add(reservation);
            context.SaveChanges();
            return reservation;
        }//end3
        public bool DeleteReservation(int reservId)
        {
            var searchReserv = context.Reservations.Find(reservId);
            if (searchReserv == null) { return false; }
            context.Reservations.Remove(searchReserv);
            context.SaveChanges();
            return true;
        }//end4
        public List<Reservation> GetReservations()
        {
            return context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Room)
                .ThenInclude(t => t.RoomType)
                .ToList();
        }//end5
    }
}
