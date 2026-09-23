using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Queries
{
    /// <summary>
    /// Класс для передачи параметров обновления комнаты
    /// </summary>
    public class UpdateRoomQuery
    {
        public string NewName { get; set; }
        public int? NewArea { get; set; }
        public bool? NewGasConnected { get; set; }
        public int? NewVoltage { get; set; }

        public UpdateRoomQuery(string newName = null, int? newArea = null, bool? newGasConnected = null, int? newVoltage = null)
        {
            NewName = newName;
            NewArea = newArea;
            NewGasConnected = newGasConnected;
            NewVoltage = newVoltage;
        }
    }
}
