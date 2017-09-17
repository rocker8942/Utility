using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Web.Hosting;

namespace Utility
{
    public class Setting
    {
        /// <summary>
        /// Get value in AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exePath">full exePath</param>
        /// <returns></returns>
        public static string GetAppSetting(string key, string exePath = "")
        {
            //Load the appsettings
            Configuration config = ConfigurationManager.OpenExeConfiguration(string.IsNullOrWhiteSpace(exePath) ? System.Reflection.Assembly.GetCallingAssembly().Location : exePath);

            //Return the value which matches the key
            string value = config.AppSettings.Settings[key].Value;
            
            return value;
        }

        /// <summary>
        /// set value in AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exePath">full exePath</param>
        public static void SetAppSetting(string key, string value, string exePath = "")
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(string.IsNullOrWhiteSpace(exePath) ? System.Reflection.Assembly.GetCallingAssembly().Location : exePath);

            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;

            //Save the changed settings
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Get web settings from web.config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        public static string GetWebAppSetting(string key, string physicalPath = "")
        {
            Configuration configuration = GetConfig(physicalPath);
            AppSettingsSection appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
            if (appSettingsSection != null)
            {
                return appSettingsSection.Settings[key].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Set web.config setting. AppSettings can be changed too, even it's in a separate file.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="physicalPath">(Optional)Full physical path to web.config</param>
        /// <param name="AppSettingsFileName">(Optional)If AppSettings is in a separate file, specify it.</param>
        public static void SetWebAppSetting(string key, string value, string physicalPath = "", string AppSettingsFileName = "")
        {
            Configuration configuration = GetConfig(physicalPath);

            // if appSettings are stored in a separate file, e.g. AppSettings.config
            if (!string.IsNullOrWhiteSpace(AppSettingsFileName))
                configuration.AppSettings.SectionInformation.ConfigSource = AppSettingsFileName;

            AppSettingsSection appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
            if (appSettingsSection != null)
            {
                if (appSettingsSection.Settings[key] == null)
                    appSettingsSection.Settings.Add(key, value);
                else
                {
                    appSettingsSection.Settings.Remove(key);
                    appSettingsSection.Settings.Add(key, value);
                }

                configuration.Save(ConfigurationSaveMode.Modified);
            }
        }

        public static void DeleteWebAppSetting(string key, string physicalPath = "")
        {
            Configuration configuration = GetConfig(physicalPath);
            configuration.AppSettings.Settings.Remove(key);
            configuration.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Get web configuration
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns>configuration object</returns>
        public static Configuration GetConfig(string physicalPath)
        {
            Configuration configuration;
            if (string.IsNullOrWhiteSpace(physicalPath))
                configuration = WebConfigurationManager.OpenWebConfiguration("~");
            else
            {
                var configFile = new FileInfo(physicalPath);
                var vdm = new VirtualDirectoryMapping(configFile.DirectoryName, true, configFile.Name);
                var wcfm = new WebConfigurationFileMap();
                wcfm.VirtualDirectories.Add("/", vdm);
                var websiteName = HostingEnvironment.SiteName;
                configuration = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/", websiteName);
            }

            return configuration;
        }
    }
}
