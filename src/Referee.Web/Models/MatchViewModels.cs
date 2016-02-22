using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Referee.Resources;

namespace Referee.Web.Models
{
    public class MatchCreateViewModel
    {
        public SelectList Players;
        public SelectList Tables;

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        public int TableId { get; set; }

        public string SideOneColorName { get; set; }
        public string SideTwoColorName { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        public int RedOneId { get; set; }

        public int? RedTwoId { get; set; }

        [Required(ErrorMessageResourceType = typeof (Resource), ErrorMessageResourceName = "FieldRequiredError")]
        public int BlueOneId { get; set; }

        public int? BlueTwoId { get; set; }
    }

    public class MatchIndexViewModel
    {
        public int Id { get; set; }

        [Display(Name = "TeamOneName", ResourceType = typeof (Resource))]
        public string TeamOneName { get; set; }

        [Display(Name = "TeamTwoName", ResourceType = typeof (Resource))]
        public string TeamTwoName { get; set; }

        [Display(Name = "LocationName", ResourceType = typeof (Resource))]
        public string Location { get; set; }

        [Display(Name = "IsFinishedName", ResourceType = typeof (Resource))]
        public bool IsFinished { get; set; }

        [Display(Name = "StartTimeName", ResourceType = typeof (Resource))]
        public DateTime StartTime { get; set; }

        [Display(Name = "EndTimeName", ResourceType = typeof (Resource))]
        public DateTime? EndTime { get; set; }

        [Display(Name = "CreatorName", ResourceType = typeof (Resource))]
        public string CreatorName { get; set; }
    }
}