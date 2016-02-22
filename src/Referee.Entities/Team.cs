using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Referee.Entities
{
    public class Team
    {
        [SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Team()
        {
        }

        [Key, Required]
        public int Id { get; set; }

        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match> MatchesAsRed { get; set; }

        [SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Match> MatchesAsBlue { get; set; }

        [Required, InverseProperty("TeamsAsFirst"), Display(Name = "First Player")]
        public virtual Player FirstPlayer { get; set; }

        [InverseProperty("TeamsAsSecond"), Display(Name = "Second Player")]
        public virtual Player SecondPlayer { get; set; }

        public List<Player> GetPlayers()
        {
            var result = new List<Player>();
            if (FirstPlayer == null)
            {
                throw new InvalidOperationException("A team must have a FirstPlayer.");
            }
            result.Add(FirstPlayer);

            if (SecondPlayer != null)
            {
                result.Add(SecondPlayer);
            }

            return result;
        }

        public string GetNameOrDefault()
        {
            return string.IsNullOrEmpty(Name) ? "Unnamed Team" : Name;
        }
    }
}