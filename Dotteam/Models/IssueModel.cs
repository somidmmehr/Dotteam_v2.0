using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotteam.Models
{
    [Table("Issues")]
    public class IssueModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }


        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual ProjectModel Project { get; set; }
    }
}
