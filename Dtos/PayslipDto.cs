namespace EmployeeApi.Dtos
{
    public class PayslipDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal GrossPay { get; set; }
        public decimal SSS_Deduction { get; set; }
        public decimal PhilHealth_Deduction { get; set; }
        public decimal PagIBIG_Deduction { get; set; }
        public decimal Tax_Withheld { get; set; }
        public decimal NetPay { get; set; }
    }
}