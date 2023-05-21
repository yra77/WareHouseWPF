

namespace WareHouseWPF.Models
{
    public class BankDetails : BaseModel
    {
        public string BankAccounts { get; set; }

        public int EmployeeId { get; set; }
        public int ShipperId { get; set; }
        public int ClientId { get; set; }
    }
}
