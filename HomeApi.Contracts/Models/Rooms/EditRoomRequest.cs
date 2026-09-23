using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Models.Rooms
{
    /// <summary>
    /// Модель запроса для редактирования комнаты
    /// </summary>
    public class EditRoomRequest
    {
        public string NewName { get; set; }
        public int? NewArea { get; set; }
        public bool? NewGasConnected { get; set; }
        public int? NewVoltage { get; set; }
    }
}
