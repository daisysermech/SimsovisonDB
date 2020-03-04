using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase
{
    public partial class Years
    {
        public Years()
        {
            Participations = new HashSet<Participations>();
        }

        public int IdYearOfContest { get; set; }

        [Display(Name = "Год проведения")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int YearOfContest { get; set; }

        [Display(Name = "Город-хозяин")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int IdHostCity { get; set; }

        [Display(Name = "Слоган")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string Slogan { get; set; }

        [Display(Name = "Сцена")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string Stage { get; set; }

        [Display(Name = "Город-хозяин")]
        public virtual Cities IdHostCityNavigation { get; set; }
        public virtual ICollection<Participations> Participations { get; set; }
    }
}
