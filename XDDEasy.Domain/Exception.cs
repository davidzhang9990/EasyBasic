using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Common.Exception;

namespace XDDEasy.Domain
{
    public enum EasyStatusCode
    {
        //Cannot repeat send code in 60 second
        CannotRepeatSendCode = 10001,
        //Wrong Captcha
        WrongCaptcha = 10002,
        //User already registered
        AlreadyRegistered = 10003,
        //the mobile number is not registed
        TheMobileNumberNotReg = 10004,
        //the mobile number is inactive
        TheMobileNumberInactive = 10005,
        //the user is not in any school
        TheUserNotInAnySchool = 10006,
        //the school not enable sms code
        NotEnableSendSmsCode = 10007,
        //the User is Invalid
        InvalidAccount = 10008,
        //the User is Locked
        UserLocked = 10009,
        //the UserName or Password is not correct
        IncorrectUserNamePassword = 10010,
        //the User is not Exist
        TheUserNotExist = 10011
    }
}
