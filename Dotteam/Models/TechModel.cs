using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotteam.Models
{
    [Table("Techs")]
    public class TechModel
    {
        public TechModel()
        {
            this.Projects = new HashSet<ProjectModel>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public ICollection<ProjectModel> Projects { get; set; }
    }
}
