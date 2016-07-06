using System;
using System.ComponentModel.DataAnnotations;

namespace Tennis.ViewModels
{
    public class ReservationVM
    {
        [Required]
        public string Reserver { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
    }
}
