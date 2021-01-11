using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotteam.Models
{
    [Table("Projects")]
    public class ProjectModel
    {   
        [Key()]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string DescriptionShort { get; set; }
        public string DescriptionLong { get; set; }
        public string Image { get; set; }

        public virtual ICollection<PresentaionModel> Presentations { get; set; }
        public virtual ICollection<IssueModel> Issues { get; set; }
        public virtual ICollection<ProjectTechModel> ProjectTeches { get; set; }
    }
}
