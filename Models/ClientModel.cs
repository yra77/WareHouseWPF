

using System.Collections.Generic;


namespace WareHouseWPF.Models
{
    public class ClientModel : HumanBaseModel
    {
        public string Organization { get; set; }
        public List<BankDetails> BanksAccount { get; set; }
    }
}
