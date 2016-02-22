using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Referee.Resources;

namespace Referee.Entities
{
    public class Table
    {
        public enum Color
        {
            // Only use CSS colors
            [Display(Name = "Blue", ResourceType = typeof (Resource))] Blue,
            [Display(Name = "Orange", ResourceType = typeof (Resource))] Orange,
            [Display(Name = "Red", ResourceType = typeof (Resource))] Red,
            [Display(Name = "White", ResourceType = typeof (Resource))] White
        }

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Table()
        {
            Guid = Guid.NewGuid();
        }

        [Key, Required]
        public int Id { get; set; }

        [Required, StringLength(255, MinimumLength = 1)]
        public string Location { get; set; }

        [Required, StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        [Required, Editable(false)]
        public Guid Guid { get; set; }

        [Required]
        public Color SideOneColor { get; set; }

        [Required]
        public Color SideTwoColor { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match> Matches { get; set; }

        public Match ActiveMatch()
        {
            return Matches.SingleOrDefault(m => !m.IsFinished);
        }
    }
}