using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Просмотр списка всех существующих комнат
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetRooms()
        {
            // Получаем список комнат из базы через репозиторий
            var rooms = await _repository.GetRooms();

            // Создаем ответ с помощью существующего класса GetRoomsResponse
            var response = new GetRoomsResponse
            {
                RoomAmount = rooms.Length,
                Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
            };

            // Возвращаем 200 OK
            return StatusCode(200, response);
        }

        /// <summary>
        /// Обновление параметров существующей комнаты
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            // Проверяем, существует ли редактируемая комната
            var room = await _repository.GetRoomById(id);
            if (room == null)
                return StatusCode(404, $"Ошибка: Комната с идентификатором {id} не найдена.");

            // Если меняется имя, проверяем, нет ли уже комнаты с таким же именем
            if (!string.IsNullOrEmpty(request.NewName))
            {
                var existingRoomWithSameName = await _repository.GetRoomByName(request.NewName);
                if (existingRoomWithSameName != null && existingRoomWithSameName.Id != id)
                    return StatusCode(400, $"Ошибка: Комната с именем '{request.NewName}' уже существует.");
            }

            // Маппим DTO в Query и сохраняем изменения
            var query = _mapper.Map<EditRoomRequest, UpdateRoomQuery>(request);
            await _repository.UpdateRoom(room, query);

            // Возвращаем успешный ответ
            return StatusCode(200, $"Комната '{room.Name}' успешно обновлена!");
        }

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
    }
}