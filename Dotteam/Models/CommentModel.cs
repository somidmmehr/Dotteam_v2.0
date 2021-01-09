using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotteam.Models
{
    [Table("Comments")]
    public class CommentModel
    {
        public CommentModel()
        {
            Replies = new HashSet<CommentModel>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string State { get; set; } // Show / Deleted / WaitingForApprova


        public int PresentationId { get; set; }
        [ForeignKey("PresentationId")]
        public virtual PresentaionModel Presentaion { get; set; }

        public int? ReplyToCommentId { get; set; }
        [ForeignKey("ReplyToCommentId")]
        public virtual CommentModel ParentComment { get; set; }
        public virtual ICollection<CommentModel> Replies { get; set; }

    }
}
