namespace EmployeeApi.Dtos
{
    public class EmployeeInputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public decimal DailyRate { get; set; }
        public string WorkingDays { get; set; } = "MWF";
    }
}