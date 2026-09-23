using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все комнаты
        /// </summary>
        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms.ToArrayAsync();
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Найти комнату по идентификатору
        /// </summary>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Обновить существующую комнату
        /// </summary>
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            // Обновляем только те поля, которые были переданы
            if (!string.IsNullOrEmpty(query.NewName))
                room.Name = query.NewName;
            if (query.NewArea.HasValue)
                room.Area = query.NewArea.Value;
            if (query.NewGasConnected.HasValue)
                room.GasConnected = query.NewGasConnected.Value;
            if (query.NewVoltage.HasValue)
                room.Voltage = query.NewVoltage.Value;

            // Сохраняем изменения
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            await _context.SaveChangesAsync();
        }
    }
}