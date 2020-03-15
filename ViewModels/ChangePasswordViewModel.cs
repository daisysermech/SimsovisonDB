using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Новый пароль")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
