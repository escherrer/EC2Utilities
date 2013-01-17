using System;
using System.Web.Security;
using System.Runtime.InteropServices;

namespace EC2Utilities.Common.Utility
{
    public class WinNtMembershipProvider : MembershipProvider
    {
        [DllImport("ADVAPI32.dll", EntryPoint = "LogonUserW", SetLastError = true,
        CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain,
               string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        private String _strName;
        private String _strApplicationName;
        private String _userDomain;
        private int _logonType;

        private readonly Boolean _boolEnablePasswordReset;
        private readonly Boolean _boolEnablePasswordRetrieval;
        private readonly int _intMaxInvalidPasswordAttempts;
        private readonly int _intMinRequiredAlphanumericCharacters;
        private readonly int _intMinRequiredPasswordLength;
        private readonly MembershipPasswordFormat _oPasswordFormat;
        private readonly string _strPasswordStrengthRegularExpression;
        private readonly Boolean _boolRequiresQuestionAndAnswer;
        private readonly Boolean _boolRequiresUniqueEMail;
            
        public WinNtMembershipProvider()
        {
            _strName = "WinNTMembershipProvider";
            _strApplicationName = "DefaultApp";
            _userDomain = "";
            _logonType = 2; // Interactive by default

            _boolEnablePasswordReset = false;
            _boolEnablePasswordRetrieval = false;
            _boolRequiresQuestionAndAnswer = false;
            _boolRequiresUniqueEMail = false;

            _intMaxInvalidPasswordAttempts = 3;
            _intMinRequiredAlphanumericCharacters = 1;
            _intMinRequiredPasswordLength = 5;
            _strPasswordStrengthRegularExpression = @"[\w| !§$%&amp;/()=\-?\*]*";

            _oPasswordFormat = MembershipPasswordFormat.Clear;

        }

        //DFB: reads entries from web.config and initializes this class from those values
        //  Once the provider is loaded, the
        //  runtime calls Initialize and passes the settings as name-value
        //  pairs in an instance of the NameValueCollection class.
        //
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "WinNTMembershipProvider";
            }

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "WinNT Membership Provider");
            }

            base.Initialize(name, config);

            foreach (string key in config.Keys)
            {
                switch (key.ToLower())
                {
                    case "name": _strName = config[key]; break;
                    case "applicationname": _strApplicationName = config[key]; break;
                    case "userdomain": _userDomain = config[key]; break;
                    case "logontype": _logonType = int.Parse(config[key]); break;
                }
            }
        }        

        public override bool ValidateUser(string strName, string strPassword)
        {
            bool bSuccessfulLogin = false;

            IntPtr token = IntPtr.Zero;

            //userName, domainName and Password parameters are very obvious.
            /* dwLogonType (3rd parameter): I used LOGON32_LOGON_INTERACTIVE, This logon type is
            intended for users who will be interactively using the computer, such as a user being
            logged on by a terminal server, remote shell, or similar process. This logon type has
            the additional expense of caching logon information for disconnected operations. For
            more details about this parameter please see http://msdn.microsoft.com/en-
            us/library/aa378184(VS.85).aspx */
            /* dwLogonProvider (4th parameter) : I used LOGON32_PROVIDER_DEFAUL, This provider
            uses the standard logon provider for the system. The default security provider is
            negotiate, unless you pass NULL for the domain name and the user name is not in UPN
            format. In this case, the default provider is NTLM. For more details about this
            parameter please see http://msdn.microsoft.com/en-us/library/aa378184(VS.85).aspx */
            /* phToken (5th parameter): A pointer to a handle variable that receives a handle to
            a token that represents the specified user. We can use this handler for impersonation
            purpose. */
            bSuccessfulLogin = LogonUser(strName, _userDomain, strPassword, _logonType, 0, ref token);

            return bSuccessfulLogin;
        }

        /**
         * Properties
         */

        public override string ApplicationName
        {
            get
            {
                return _strApplicationName;
            }
            set
            {
                _strApplicationName = value;
            }
        }
        public override string Name
        {
            get
            {
                return _strName;
            }
        }
        public override bool EnablePasswordReset
        {
            get
            {
                return _boolEnablePasswordReset;
            }
        }
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return _boolEnablePasswordRetrieval;
            }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return _intMaxInvalidPasswordAttempts;
            }

        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return _intMinRequiredAlphanumericCharacters;
            }

        }
        public override int MinRequiredPasswordLength
        {
            get
            {
                return _intMinRequiredPasswordLength;
            }

        }
        public override int PasswordAttemptWindow
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return _oPasswordFormat;
            }
        }
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return _strPasswordStrengthRegularExpression;
            }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return _boolRequiresQuestionAndAnswer;
            }
        }
        public override bool RequiresUniqueEmail
        {
            get
            {
                return _boolRequiresUniqueEMail;
            }
        }

        /*
         * API Functions
         */

        public override string GetPassword(string strName, string strAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
                                                  string passwordAnswer, bool isApproved, object userId, out MembershipCreateStatus status)
        {

            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string strEmail)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string strName, string strAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string strName, string strOldPwd, string strNewPwd)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string strName, string strPassword, string strNewPwdQuestion, string strNewPwdAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string strName, bool boolUserIsOnline)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string strName, bool boolDeleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string strEmailToMatch, int iPageIndex, int iPageSize, out int iTotalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string strUsernameToMatch, int iPageIndex, int iPageSize, out int iTotalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int iPageIndex, int iPageSize, out int iTotalRecords)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string strUserName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
    }
}