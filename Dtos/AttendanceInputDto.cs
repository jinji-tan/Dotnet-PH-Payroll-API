namespace EmployeeApi.Dtos
{
    public class AttendanceInputDto
    {
        public int EmployeeId { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsPresent { get; set; }
        public string? Remarks { get; set; }
    }
}