

using WareHouseWPF.Services.Auth;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Commands;
using Prism.Regions;

using System.ComponentModel;


namespace WareHouseWPF.ViewsModel
{
    internal class AuthViewModel : BaseViewModel
    {


        public AuthViewModel(IRegionManager regionManager,
                                   IVerifyService verifyService,
                                   IAuth auth,
                                   ITranslationSource translation)
            : base()
        {
            _regionManager = regionManager;
            _verifyService = verifyService;
            _auth = auth;
            _translation = translation;
           
            IsEnabled = false;
            IsLoaded = "Hidden";//Visible

            Email = "";
            Password = "";
            EmailBorderColor = "LightGray";
            PassBorderColor = "LightGray";

            Color_OkBtn = "LightGray";
        }



        #region Public property

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }


        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }


        private string _emailBorderColor;
        public string EmailBorderColor
        {
            get => _emailBorderColor;
            set => SetProperty(ref _emailBorderColor, value);
        }


        private string _passBorderColor;
        public string PassBorderColor
        {
            get => _passBorderColor;
            set => SetProperty(ref _passBorderColor, value);
        }


        private string _errorEmailText;
        public string ErrorEmailText
        {
            get => _errorEmailText;
            set => SetProperty(ref _errorEmailText, value);
        }


        private string _errorPassText;
        public string ErrorPassText
        {
            get => _errorPassText;
            set => SetProperty(ref _errorPassText, value);
        }


        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }


        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }


        private string _color_OkBtn;
        public string Color_OkBtn
        {
            get => _color_OkBtn;
            set => SetProperty(ref _color_OkBtn, value);
        }


        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }


        public DelegateCommand SignInCommand =>
       new DelegateCommand(SignInAsync, IsSignInEnable).ObservesProperty(() => IsEnabled);

        #endregion



        private async void SignInAsync()
        {
            _isPressed = true;
            IsSignInEnable();
            IsLoaded = "Visible";

            var result = await _auth.AuthAsync(Password, Email);

            IsLoaded = "Hidden";

            if (result.Item1)
            {
                _employeeAuth = result.Item2.Item2;
                _regionManager.RequestNavigate("MainRegion", "Home");
            }
            else
            {
                ErrorText = result.Item2.Item1;
                _isPressed = false;
            }
        }

        private void CheckEmail()
        {

            if (_email.Length > 0)
            {
                EmailBorderColor = "Red";
                ErrorEmailText = TranslationSource.Instance["ErrorText_Email"];

                string email = Email;

                if (!_verifyService.EmailVerify(ref email))
                {
                    Email = email;
                }
                else
                {
                    if (_verifyService.IsValidEmail(Email))
                    {
                        ErrorEmailText = "Ok!";
                        EmailBorderColor = "Green";
                    }
                }

                IsSignInEnable();
            }
        }

        private void CheckPassword()
        {

            if (_password.Length > 0)
            {
                PassBorderColor = "Red";
                ErrorPassText = TranslationSource.Instance["ErrorText_Password"];

                string password = Password;

                if (!_verifyService.PasswordCheckin(ref password))
                {
                    Password = password;
                }
                else
                {
                    if (_verifyService.PasswordVerify(Password) && (Password.Length > 7 && Password.Length < 17))
                    {
                        ErrorPassText = "Ok!";
                        PassBorderColor = "Green";
                    }
                }

                IsSignInEnable();
            }
        }

        private bool IsSignInEnable()//Enable disable "Sign in" Button
        {
            IsEnabled = (!_isPressed && _stateNethwork
                && PassBorderColor == "Green"
                && EmailBorderColor == "Green"
                    && _email.Length > 0
                    && _password.Length > 0)
                ? IsEnabled = true : IsEnabled = false;

            if (IsEnabled)
            {
                Color_OkBtn = "#36A3C0";
            }
            else
            {
                Color_OkBtn = "LightGray";
            }

            return IsEnabled;
        }


        #region implement Interface

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "Email":
                    CheckEmail();
                    break;
                case "Password":
                    CheckPassword();
                    break;

                default:
                    break;
            }
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _isPressed = false;
            IsSignInEnable();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsSignInEnable();
        }

        #endregion
    }
}
