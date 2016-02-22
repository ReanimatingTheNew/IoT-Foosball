using System.ComponentModel.DataAnnotations;
using Referee.Resources;

namespace Referee.Web.Models
{
    public class TeamViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof (Resource),
            ErrorMessageResourceName = "LengthMustBeBelow255")]
        public string Name { get; set; }

        public int PlayerOneId { get; set; }
        public int PlayerTwoId { get; set; }

        [Display(Name = "PlayerOneName", ResourceType = typeof (Resource))]
        public string PlayerOneName { get; set; }

        [Display(Name = "PlayerTwoName", ResourceType = typeof (Resource))]
        public string PlayerTwoName { get; set; }
    }
}