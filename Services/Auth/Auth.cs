

using WareHouseWPF.Models;
using WareHouseWPF.Properties;
using WareHouseWPF.Services.Repository;
using WareHouseWPF.Services.VerifyService;

using System.Threading.Tasks;
using WareHouseWPF.Services.Localisation;

namespace WareHouseWPF.Services.Auth
{
    internal class Auth : IAuth
    {

        protected IVerifyService _verifyService;
        protected IRepository _repository;


        public Auth(IRepository repository,
                    IVerifyService verifyService)
        {
            _verifyService = verifyService;
            _repository = repository;
        }


        public async Task<(bool, (string, Employee))> AuthAsync(string password, string email)
        {

            string str = null;


            if (_verifyService.IsValidEmail(email))
            {

                if (_verifyService.PasswordVerify(password))
                {

                    Employee res = new Employee();
                    try
                    {
                        res = await _repository.GetDataAsync<Employee>("Email", email);

                        if (res != null)
                        {

                            if (res.Password == password)
                            {
                                return (true, (str, res));
                            }
                            else
                            {                             
                                str = TranslationSource.Instance["Alert_Password1"];//Resources.Alert_Password1;
                            }
                        }
                        else
                        {
                            str = TranslationSource.Instance["Alert_Email2"];//Resources.Alert_Email2;
                        }
                    }
                    catch
                    {
                        str = TranslationSource.Instance["NotServer"];//Resources.NotServer;
                    }
                }
                else
                {
                    str = TranslationSource.Instance["Alert_Password2"];//Resources.Alert_Password2;
                }
            }
            else
            {
                str = TranslationSource.Instance["Alert_Email3"];//Resources.Alert_Email3;
            }

            return (false, (str, null));
        }


        //public async Task<(bool, string)> RegistrAsync(Employee profile)
        //{

        //    string str = null;


        //    if (_verifyService.IsValidEmail(profile.Email))
        //    {

        //        if (profile.Name.Length > 2 && _verifyService.PasswordVerify(profile.Password))
        //        {

        //            try
        //            {

        //                var res = await _repository.GetDataAsync<Employee>("Email", profile.Email);

        //                if (res == null && await _repository.InsertAsync<Employee>(profile))
        //                {
        //                    _settingsManager.Name = profile.Name;
        //                    _settingsManager.Email = profile.Email;
        //                    _settingsManager.Password = profile.Password;
        //                    _settingsManager.Tel = profile.Tel;
        //                    _settingsManager.Address = profile.Address;
        //                    _settingsManager.IsCircleCart = (profile.CartStatus > 0);
        //                    return (true, str);
        //                }
        //                else
        //                {
        //                    str = Resources.Strings.Resource.Alert_Email1;
        //                }
        //            }
        //            catch
        //            {
        //                str = Resources.Strings.Resource.NotServer;
        //            }
        //        }
        //        else
        //        {
        //            str = Resources.Strings.Resource.Alert_Password2;
        //        }
        //    }
        //    else
        //    {
        //        str = Resources.Strings.Resource.Alert_Email3;
        //    }

        //    return (false, str);
        //}

        //public async Task<bool> UpdateAsync(Employee log)
        //{
        //    try
        //    {
        //        var res = await _repository.GetDataAsync<Employee>("Email", _settingsManager.Email);

        //        if (res != null)
        //        {
        //            res = log;

        //            return await _repository.UpdateDataAsync(res.Id, res);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Update Error " + e.Message);
        //    }
        //    return false;
        //}

        //private async Task<bool> InsertAsync(Login profile)
        //{
        //    try
        //    {
        //        var res = await _restService.GetDataAsync<Login>("Email", profile.Email);

        //        if (res == null && await _restService.InsertAsync<Login>(profile))
        //        {
        //            return true;
        //        }
        //    }
        //    catch
        //    {

        //    }
        //        return false;
        //}
    }
}
