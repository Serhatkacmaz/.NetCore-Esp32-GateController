using GateController.Repository.Contexts;
using GateController.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateController.Repository
{
    public class SystemDevicesRepository
    {

        public SystemDevice GetDevice(string deviceId)
        {
            using (var context = new ApplicationDbContext())
            {
                var device = context.SystemDevices.FirstOrDefault(x => x.DeviceId == deviceId);

                return device;
            }
        }
    }
}
