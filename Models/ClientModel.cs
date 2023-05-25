

using System.Collections.Generic;


namespace WareHouseWPF.Models
{
    public class ClientModel : HumanBaseModel
    {
        public string Organization { get; set; } = "организація";
        public List<BankDetails> BanksAccount { get; set; } = new List<BankDetails>(3);
    }
}
