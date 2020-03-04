using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase
{
    public partial class Participants
    {
        public Participants()
        {
            Participations = new HashSet<Participations>();
        }

        public DateTime CurrentDate = DateTime.Now;
        public DateTime NullDate = DateTime.MinValue;
        public int IdParticipant { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string ParticipantName { get; set; }

        [Display(Name = "Город")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int IdRepresentedCity { get; set; }
        [Display(Name = "Тип участника")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int IdParticipantType { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения // Дата основания группы")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public DateTime? ParticipantDate{ get; set; }

        [Display(Name = "Биография")]
        public string Biography { get; set; }

        [Display(Name = "Тип участника")]
        public virtual ParticipantTypes IdParticipantTypeNavigation { get; set; }

        [Display(Name = "Город, который представляют")]
        public virtual Cities IdRepresentedCityNavigation { get; set; }

        public virtual ICollection<Participations> Participations { get; set; }
    }
}
