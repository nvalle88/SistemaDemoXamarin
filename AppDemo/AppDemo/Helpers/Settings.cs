// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AppDemo.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        const string SETTINGS_KEY = "settings_key";
        static readonly string SettingsDefault = string.Empty;

        const string IS_LOGGED_IN_TOKEN_KEY = "isloggedid_key";
        static readonly bool IsLoggedInTokenDefault = false;

        const string USER_KEY = "user_key";
        static readonly int UserDefault = 0;

        const string COMPANY_KEY = "company_key";
        static readonly int CompanyDefault = 0;

        const string NAME_KEY = "name_key";
        static readonly string nameDefault = string.Empty;

        const string LAST_NAME_KEY = "lastname_key";
        static readonly string lastnameDefault = string.Empty;

        // private const string PasswordKey = "password_key";
        // private static readonly string PasswordDefault = string.Empty;

        const string DEVICE_KEY = "device_key";
        static readonly int DeviceDefault = 0;

        #endregion

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SETTINGS_KEY, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SETTINGS_KEY, value);
            }
        }
        public static bool IsLoggedIn

        {

            get { return AppSettings.GetValueOrDefault(IS_LOGGED_IN_TOKEN_KEY, IsLoggedInTokenDefault); }

            set { AppSettings.AddOrUpdateValue(IS_LOGGED_IN_TOKEN_KEY, value); }

        }
        public static int userId
        {
            get
            {
                return AppSettings.GetValueOrDefault(USER_KEY, UserDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(USER_KEY, value);
            }
        }

        public static int companyId
        {
            get
            {
                return AppSettings.GetValueOrDefault(COMPANY_KEY, CompanyDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(COMPANY_KEY, value);
            }
        }

        public static int deviceId
        {
            get
            {
                return AppSettings.GetValueOrDefault(DEVICE_KEY, DeviceDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DEVICE_KEY, value);
            }
        }
        // public static string PasswordData
        // {
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault(PasswordKey, PasswordDefault);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue(PasswordKey, value);
        //    }
        // }
        public static string UserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(NAME_KEY, nameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NAME_KEY, value);
            }
        }
        public static string UserLastName
        {
            get
            {
                return AppSettings.GetValueOrDefault(LAST_NAME_KEY, lastnameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LAST_NAME_KEY, value);
            }
        }
    }
}