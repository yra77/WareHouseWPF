

using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WareHouseWPF.Controls.Entrys
{
    internal class MyTextBox : TextBox
    {

        public MyTextBox()
        {
            TextChanged += MyTextBox_DataContextChanged_Async;
        }


        public static readonly DependencyProperty SelectedTypeProperty =
        DependencyProperty.RegisterAttached("SelectedType",
                                typeof(string),
                                typeof(MyTextBox));

        public string SelectedType
        {
            get { return GetValue(SelectedTypeProperty) as string; }
            set { SetValue(SelectedTypeProperty, value); }
        }


        private async void MyTextBox_DataContextChanged_Async(object sender, TextChangedEventArgs e)
        {
            //Debug.WriteLine(Text);

            var res = true;

            if (sender is TextBox box)
            {
                if (Text != null && Text.Length > 0)
                {
                    switch (this.SelectedType)
                    {
                        case "text":
                            res = NameVerify();
                            break;

                        case "email":
                            res = EmailVerify();
                            break;

                        case "digit":
                            res = DigitVerify();
                            break;

                        case "password":
                            res = PasswordCheckin();
                            break;
                    }
                    CaretIndex = Text.Length;

                    if (res)
                    {
                        box.Foreground = Brushes.Green;
                    }
                    else
                    {
                        box.Foreground = Brushes.Red;
                        await Task.Delay(1500);
                        box.Foreground = Brushes.Green;
                    }
                }
            }
        }

        private bool NameVerify()
        {
            bool flag = true;
            string temp = Text;

            for (int i = 0; i < temp.Length; i++)
            {

                if ((temp[i] >= 'A' && temp[i] <= 'Z') || (temp[i] >= 'a' && temp[i] <= 'z') || (temp[i] >= 'А' && temp[i] <= 'Щ') 
                    || (temp[i] >= 'Ю' && temp[i] <= 'Я') || (temp[i] >= 'а' && temp[i] <= 'п') || (temp[i] >= 'р' && temp[i] <= 'щ')
                    || (temp[i] >= 'ю' && temp[i] <= 'я') || temp[i] == 'Ь' || temp[i] == 'ь' || temp[i] == 'Ї' || temp[i] == 'ї'
                    || temp[i] == 'І' || temp[i] == 'і' || temp[i] == 'Є' || temp[i] == 'є' || temp[i] == '`' || temp[i] == 'ь')
                {
                    continue;
                }
                else
                {
                    temp = temp.Remove(i, 1);
                    flag = false;
                }
            }

            Text = temp;

            return flag;
        }

        private bool EmailVerify()
        {
            bool flag = true;
            string temp = Text;

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
            Text = temp;

            return flag;
        }

        private bool DigitVerify()
        {
            bool flag = true;
            string temp = Text;

            for (int i = 0; i < temp.Length; i++)
            {

                if (char.IsDigit(temp[i]))
                {
                    continue;
                }
                else
                {
                    temp = temp.Remove(i, 1);
                    flag = false;
                }
            }
            Text = temp;

            return flag;
        }

        private bool PasswordCheckin()
        {
            bool flag = true;
            string temp = Text;

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

            Text = temp;
            
            return flag;
        }
    }
}
