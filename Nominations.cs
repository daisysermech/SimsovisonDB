using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase
{
    public partial class Nominations
    {
        public Nominations()
        {
            Participations = new HashSet<Participations>();
        }

        public int IdNomination { get; set; }

        [Display(Name = "Номинация")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string NominationName { get; set; }

        public virtual ICollection<Participations> Participations { get; set; }
    }
}
