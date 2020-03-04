using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase
{
    public partial class Cities
    {
        public Cities()
        {
            Participants = new HashSet<Participants>();
            Years = new HashSet<Years>();
        }

        public int IdCity { get; set; }

        [Display(Name = "Город")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]

        public string CityName { get; set; }

        [Display(Name = "Описание города")]

        public string Description { get; set; }

        public virtual ICollection<Participants> Participants { get; set; }
        public virtual ICollection<Years> Years { get; set; }
    }
}
