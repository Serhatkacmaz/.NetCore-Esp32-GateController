using System.ComponentModel.DataAnnotations;

namespace GateController.API.ViewModels
{
    public class LoginModel
    {
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
