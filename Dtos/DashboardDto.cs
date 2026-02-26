namespace EmployeeApi.Dtos
{
    public class DashboardDto
    {
        public int TotalEmployees { get; set; }
        public decimal TotalPayrollMonthly { get; set; }
        public int PresentToday { get; set; }
        public double AttendanceRate { get; set; }
    }
}