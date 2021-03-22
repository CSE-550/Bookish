using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    public interface IAuthenticationService
    {
        void SignUp(UserSignUpModel signUpModel);
        AuthUserModel Login(UserLoginModel loginModel);
        List<IListItem> History(AuthUserModel authUser, int page);
        void Logout();
    }
}
