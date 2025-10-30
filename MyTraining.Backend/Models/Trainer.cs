namespace MyTraining.Backend.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Expertise { get; set; } = "";
        public string Email { get; set; } = "";
        public string Bio { get; set; } = "";
    }
}
