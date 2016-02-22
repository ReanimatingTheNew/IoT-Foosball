using System;
using System.ComponentModel.DataAnnotations;

namespace Referee.Entities
{
    public enum TableSide
    {
        Blue = 0,
        Red = 1
    }

    public class Goal
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public TableSide Side { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public virtual Match Match { get; set; }
    }
}