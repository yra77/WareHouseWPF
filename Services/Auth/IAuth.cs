

using WareHouseWPF.Models;
using System.Threading.Tasks;


namespace WareHouseWPF.Services.Auth
{
    internal interface IAuth
    {
        Task<(bool, (string, Employee))> AuthAsync(string password, string email);
      //  Task<(bool, string)> RegistrAsync(Employee profile);
     //   Task<bool> UpdateAsync(Employee log);
    }
}
