using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotteam.Models
{
    [Table("Presentaions")]
    public class PresentaionModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public string DescriptionShort { get; set; }
        public string DescriptionLong { get; set; }
        public string EstimateTime { get; set; }
        [Required]
        public int EstimatePrice { get; set; }
        public DateTime LastChange { get; set; }


        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual ProjectModel Project { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; }
    }
}
