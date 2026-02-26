namespace EmployeeApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty; 
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }            
        public decimal DailyRate { get; set; }                
        public string WorkingDays { get; set; } = "MWF";   

        public string Name => $"{FirstName} {LastName}";
        public string Position { get; set; } = string.Empty;
    }
}
