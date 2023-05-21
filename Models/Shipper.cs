

using System.Collections.Generic;


namespace WareHouseWPF.Models
{
    internal class Shipper : BaseModel
    {
        public string Phone { get; set; }
        public List<BankDetails> BankAccount { get; set; }
    }
}
