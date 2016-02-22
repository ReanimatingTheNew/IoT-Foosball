using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Referee.Entities;
using Referee.Resources;

namespace Referee.Web.Models
{
    public class TableIndexViewModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public bool MatchOngoing { get; set; }
        public int SideOneScore { get; set; }
        public int SideTwoScore { get; set; }
        public Table.Color SideOneColor { get; set; }
        public Table.Color SideTwoColor { get; set; }
        public string SideOneTeamName { get; set; }
        public string SideTwoTeamName { get; set; }
        public int? OngoingMatchId { get; set; }
    }

    public class TableCreateViewModel
    {
        public SelectList Colors;

        [Key, Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof (Resource),
            ErrorMessageResourceName = "LengthMustBeBelow255")]
        public string Location { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof (Resource),
            ErrorMessageResourceName = "LengthMustBeBelow255")]
        public string Name { get; set; }

        [Required, Editable(false)]
        public Guid Guid { get; set; }

        [Required]
        [Display(Name = "SideOneColorName", ResourceType = typeof (Resource))]
        public Table.Color SideOneColor { get; set; }

        [Required]
        [Display(Name = "SideTwoColorName", ResourceType = typeof (Resource))]
        public Table.Color SideTwoColor { get; set; }

        public void Validate(ModelStateDictionary modelState)
        {
            if (SideOneColor == SideTwoColor)
            {
                modelState.AddModelError("SideOneColor", Resource.ColorsMustBeDifferentError);
            }
        }
    }

    public class TableApiViewModel
    {
        public Guid Guid { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public Match Match { get; set; }
    }

    public class Match
    {
        public int SideOneScore { get; set; }
        public int SideTwoScore { get; set; }
        public Table.Color SideOneColor { get; set; }
        public Table.Color SideTwoColor { get; set; }
        public string SideOneTeamName { get; set; }
        public string SideTwoTeamName { get; set; }
        public int? OngoingMatchId { get; set; }
    }
}