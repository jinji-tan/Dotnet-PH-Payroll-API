namespace EmployeeApi.Dtos
{
    public class PayslipResponseDto
    {
        public decimal GrossPay { get; set; }
        public decimal SssContribution { get; set; } 
        public decimal PhilHealthContribution { get; set; } 
        public decimal PagIbigContribution { get; set; } 
        public decimal WithholdingTax { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
    }
}