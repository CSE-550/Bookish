using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    public interface IAuthenticationService
    {
        AuthUserModel Login(UserLoginModel loginModel);
        void Logout();
    }
}
