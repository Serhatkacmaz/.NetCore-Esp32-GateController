using GateController.API.ViewModels;
using GateController.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GateController.API.Controllers
{
    public class LogController : Controller
    {
        private readonly GateLogRepository _gateLogRepository;
        public LogController(GateLogRepository gateLogRepo)
        {
            _gateLogRepository = gateLogRepo;
        }

        [HttpGet]
        [Route("/Log")]
        public async Task<IActionResult> Log()
        {
            var logList = await _gateLogRepository.GetAll();
            var modelList = new List<LogModel>();
            foreach (var item in logList)
            {
                modelList.Add(new LogModel
                {
                    CardId = item.Id,
                    LoginDate = item.LoginDate,
                    Name = item.OfficeMember.Name,
                    Surname = item.OfficeMember.Surname,
                });
            }

            return View(modelList);
        }
        
    }
}
