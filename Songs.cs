using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimsovisionDataBase
{
    public partial class Songs
    {
        public Songs()
        {
            Participations = new HashSet<Participations>();
        }

        public int IdSong { get; set; }

        [Display(Name = "Название песни")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public string SongName { get; set; }

        [Display(Name = "Длительность")]
        [Required(ErrorMessage = "Поле не может быть пустым.")]
        public TimeSpan Duration { get; set; }

        public virtual ICollection<Participations> Participations { get; set; }
    }
}
