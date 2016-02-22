using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Referee.Resources;

namespace Referee.Entities
{
    public class Player
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
        }

        [Key, Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof (Resource),
            ErrorMessageResourceName = "LengthMustBeBelow255")]
        public string Name { get; set; }

        [Required]
        public string AccountEmail { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team> TeamsAsFirst { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team> TeamsAsSecond { get; set; }
    }
}