namespace MyTraining.Backend.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int TrainerId { get; set; }
        public Trainer? Trainer { get; set; }
        public int DurationMinutes { get; set; } = 60;
        public decimal Price { get; set; } = 0;

        // --- TAMBAHAN YANG MEMICU MIGRASI ---
        public string? CoverImageUrl { get; set; }
    }   
}
