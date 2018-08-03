using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Models
{
    public class Note
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public List<Checklist> Checklist { get; set; }
        public List<Label> Labels { get; set; }
        public bool Pinned { get; set; }
    }
}
