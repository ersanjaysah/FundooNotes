using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ReposatoryLayer.Entities
{
    public class Labels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public int? Userid { get; set; }
        public virtual User user{ get; set; }

        public int? NoteID { get; set; }
        public virtual Note note { get; set; }

    }
}
