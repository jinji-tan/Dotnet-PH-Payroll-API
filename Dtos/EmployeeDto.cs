namespace EmployeeApi.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public decimal DailyRate { get; set; }
        public string WorkingDays { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}