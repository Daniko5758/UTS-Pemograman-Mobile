using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining.Shared.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }   // navigation
        public int DurationMinutes { get; set; } = 60;
        public decimal Price { get; set; } = 0;
    }
}
