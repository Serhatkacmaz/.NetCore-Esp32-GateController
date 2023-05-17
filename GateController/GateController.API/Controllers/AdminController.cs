using GateController.API.Models.DTOs.Input;
using GateController.API.ViewModels;
using GateController.Repository;
using GateController.Repository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GateController.API.Controllers
{
    public class AdminController : Controller
    {
        private readonly SystemCardsRepository _systemCardsRepository;
        public AdminController(SystemCardsRepository systemCardRepo)
        {
            _systemCardsRepository = systemCardRepo;    
        }

        [HttpGet]
        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("/LoginClick")]
        public IActionResult LoginClick(LoginModel loginModel)
        {
            if (loginModel.UserName== "admin" && loginModel.Password=="1234")
            {
                return RedirectToAction("Log", "Log");
            }

            return RedirectToAction("Login","Admin");   
        }

        [HttpGet]
        [Route("/NewMember")]
        public IActionResult NewMember()
        {
            return View();
        }

        [HttpPost]
        [Route("/NewMemberSave")]
        public async Task<IActionResult> NewMemberSave(NewMember member)
        {
            var card = new SystemCard { CardHex = member.CardId, OfficeMember = new OfficeMember { Name = member.Name, Surname = member.Surname, Phone = member.Phone } };
            await _systemCardsRepository.NewCard(card);
            return Content("Başarılı");
        }
    }
}
