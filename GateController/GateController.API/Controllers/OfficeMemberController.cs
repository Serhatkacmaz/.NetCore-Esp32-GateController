using GateController.API.ViewModels;
using GateController.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GateController.API.Controllers
{
    public class OfficeMemberController : Controller
    {
        private readonly OfficeMemberRepository _officeMemberRepository;
        public OfficeMemberController(OfficeMemberRepository officeMemberRepo)
        {
            _officeMemberRepository = officeMemberRepo;
        }

        [HttpGet]
        [Route("/OfficeMember")]
        public async Task<IActionResult> OfficeMember()
        {
            var model = new List<OfficeMemberModel>();
            var list = await _officeMemberRepository.GetAll();

            list.ForEach(x => model.Add(new OfficeMemberModel
            {
                Name = x.Name,
                Phone = x.Phone,
                Surname = x.Surname
            }));

            return View(model);
        }
    }
}
