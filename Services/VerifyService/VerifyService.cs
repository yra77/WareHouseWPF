

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace WareHouseWPF.Services.VerifyService
{
    internal class VerifyService : IVerifyService
    {

        public bool IsValidEmail(string email)
        {
            Regex regex =
         new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
         RegexOptions.CultureInvariant | RegexOptions.Singleline);

            return regex.IsMatch(email);
        }

        public bool EmailVerify(ref string str)
        {
            bool flag = true;
            string temp = str;

            for (int i = 0; i < temp.Length; i++)
            {

                if (char.IsDigit(temp[i]) || (temp[i] >= 'A' && temp[i] <= 'Z') || (temp[i] >= 'a' && temp[i] <= 'z')
                    || temp[i] == '.' || temp[i] == '@' || temp[i] == '_' || temp[i] == '-')
                {
                    continue;
                }
                else
                {
                    temp = temp.Remove(i, 1);
                    flag = false;
                }

            }
            str = temp;

            return flag;
        }

        public bool TelVerify(string number)
        {
            string reg = @"^(\+)\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

            if (number != null)
            {
                return Regex.IsMatch(number, reg);
            }
            else
            {
                return false;
            }
        }

        public bool NameVerify(ref string str)
        {
            bool flag = true;
            string temp = str;

            for (int i = 0; i < temp.Length; i++)
            {

                if ((temp[i] >= 'A' && temp[i] <= 'Z') || (temp[i] >= 'a' && temp[i] <= 'z') || temp[i] == ' ')
                {
                    continue;
                }
                else
                {
                    temp = temp.Remove(i, 1);
                    flag = false;
                }

            }
            str = temp;
            return flag;
        }

        public bool PasswordCheckin(ref string str)
        {
            bool flag = true;
            string temp = str;

            for (int i = 0; i < temp.Length; i++)
            {

                if (char.IsDigit(temp[i]) || (temp[i] >= 'A' && temp[i] <= 'Z') || (temp[i] >= 'a' && temp[i] <= 'z'))
                {
                    continue;
                }
                else
                {
                    temp = temp.Remove(i, 1);
                    flag = false;
                }

            }
            str = temp;
            return flag;
        }

        public bool PasswordVerify(string str)
        {

            Regex regex = new Regex(@"^([A-Z]{1}[A-Za-z0-9]{0,}[0-9]{1,})",
             RegexOptions.CultureInvariant | RegexOptions.Singleline);

            return regex.IsMatch(str);
        }

        public bool PositionVerify(ref string str)
        {
            bool flag = true;
            string temp = str;

            for (int i = 0; i < temp.Length; i++)
            {

                if (char.IsDigit(temp[i]) || temp[i] == ',' || temp[i] == '-')
                {
                    continue;
                }
                else
                {
                    temp = temp.Remove(i, 1);
                    flag = false;
                }

            }
            str = temp;
            return flag;
        }
    }
}
