

namespace WareHouseWPF.Services.VerifyService
{
    internal interface IVerifyService
    {
        bool IsValidEmail(string email);
        bool EmailVerify(ref string str);
        bool TelVerify(string number);
        bool NameVerify(ref string str);
        bool PasswordCheckin(ref string str);
        bool PasswordVerify(string str);
        bool PositionVerify(ref string str);
    }
}
