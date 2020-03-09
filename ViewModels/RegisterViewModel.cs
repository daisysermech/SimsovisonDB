using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Электронная почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Подтвержение пароля")]
        [Compare("Password",ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
