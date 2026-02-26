namespace EmployeeApi.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsPresent { get; set; }
        public string? Remarks { get; set; }
    }
}