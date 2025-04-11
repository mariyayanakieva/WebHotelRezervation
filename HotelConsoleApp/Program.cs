using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelConsoleApp;
using Microsoft.EntityFrameworkCore;
using System;
using WebHotelRezervation.Models;
namespace ConsoleHotelApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Конфигурационен код!
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            var services = new ServiceCollection()
                .AddDbContext<HotelDbContext>(options =>
                 options.UseMySQL(config.GetConnectionString("MyDbConst")))
                .AddTransient<ReservationService>()
                .BuildServiceProvider();
            //Важно!!!
            var reservationService = services.GetRequiredService<ReservationService>();
            //Program
            Console.Write("Име на клиента: ");
            string name = Console.ReadLine();
            Console.Write("Лична карта:");
            string idCard = Console.ReadLine();
            Console.Write("Адрес: ");
            string address = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();
            Console.Write("Националност: ");
            string nationality = Console.ReadLine();
            var client = reservationService.AddClient(name, idCard, address, phone, nationality);

            //Показване на стаи
            Console.WriteLine("--Налични стаи--");
            var rooms = reservationService.GetAllRoom();
            foreach (var room in rooms)
            {
                Console.WriteLine($"ID: {room.Id}, Номер: {room.Number}, Тип: {room.RoomType.Name}, Цена: {room.Price}");
            }

            // Избор на стая
            int roomId = 0;
            while (true)
            {
                Console.Write("Избери ID на стая: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out roomId))
                    break;
                Console.WriteLine("Моля, въведи валидно число.");
            }

            // Въвеждане на дата
            DateTime date;
            while (true)
            {
                Console.Write("Дата на резервация (гггг-мм-дд): ");
                var input = Console.ReadLine();
                if (DateTime.TryParse(input, out date))
                    break;
                Console.WriteLine("Невалиден формат. Пример: 2025-04-01");
            }

            // Продължителност
            int duration;
            while (true)
            {
                Console.Write("Продължителност на престоя (в дни): ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out duration))
                    break;
                Console.WriteLine("Моля, въведи валидно число.");
            }

            // Записване
            var reservation = reservationService.MakeReservation(client.Id, roomId, date, duration);
            Console.WriteLine("Резервацията е създадена успешно!");
        }
    }
}
