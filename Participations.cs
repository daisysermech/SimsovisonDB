using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase
{
    public partial class Participations
    {
        public int IdParticipation { get; set; }

        [Display(Name = "Год проведения")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int IdYearOfContest { get; set; }

        [Display(Name = "Участник")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int IdParticipant { get; set; }

        [Display(Name = "Песня")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int IdSong { get; set; }

        [Display(Name = "Номинация")]
        public int? IdNomination { get; set; }

        [Display(Name = "Место")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public int Place { get; set; }


        [Display(Name = "Номинация")] 
        public virtual Nominations IdNominationNavigation { get; set; }

        [Display(Name = "Участник")]
        public virtual Participants IdParticipantNavigation { get; set; }

        [Display(Name = "Песня")]
        public virtual Songs IdSongNavigation { get; set; }

        [Display(Name = "Год участия")]
        public virtual Years IdYearOfContestNavigation { get; set; }
    }
}
