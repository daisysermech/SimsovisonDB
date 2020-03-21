using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimsovisionDataBase
{
    public partial class ParticipantTypes
    {
        public ParticipantTypes()
        {
            Participants = new HashSet<Participants>();
        }

        public int IdParticipantType { get; set; }

        [Display(Name = "Тип участника")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string ParticipantType { get; set; }

        public virtual ICollection<Participants> Participants { get; set; }

    }
}
