using FMASolutionsCore.DataServices.FileHelper;
using FMASolutionsCore.BusinessServices.BusinessCore.Generic;
using System;
using System.IO;
using System.Collections.Generic;

namespace FMASolutionsCore.BusinessServices.AppConfigExtension
{
    public class AppConfigExtension : IAppConfigExtension
    {
        public AppConfigExtension(string[] settingKeys)
        {
            _settingKeys = settingKeys;
            _fileHelper = _factory.CreateProduct(EnumFileHelperTypes.TextFile
                 , C.AppConfigFilesFolder + Helper.GetCurrentAppName() + C.AppConfigFileExtension
            );
        }

        public string[] CurrentSettings { get { return _settingKeys; } }
        private FileHelperFactory _factory = (FileHelperFactory)new FileHelperFactoryWorker();
        private IFileHelper _fileHelper;
        private readonly string[] _settingKeys;

        public void Register()
        {
            ValidateSettings(_settingKeys);
        }

        public string GetSetting(string settingKey)
        {
            string settingValue = string.Empty;
            foreach (string setting in AllSettings)
            {
                if (setting.Contains(settingKey))
                    settingValue = setting.Substring(settingKey.Length + 1);
            }
            return settingValue;
        }

        public void WriteSetting(string settingKey, string settingValue)
        {
            string[] allLinesInFile = _fileHelper.GetAllLinesInFile();
            bool settingExists = false;
            int lineNumber = 0;
            foreach (string str in allLinesInFile)
            {
                if (str.Contains(settingKey))
                {
                    UpdateSetting(settingKey, settingValue, allLinesInFile, lineNumber);
                    settingExists = true;
                    break;
                }
                ++lineNumber;
            }
            if (settingExists)
                return;
            else
                _fileHelper.AppendLineToFile(settingKey + C.SettingKeyValueSeperator + settingValue);
        }

        private bool ValidateSettings(string[] settingsToValidate)
        {
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>)settingsToValidate);
            try
            {
                if (AllSettings.Length > 0)
                {
                    foreach (string setting in AllSettings)
                    {
                        if (!stringList.Contains(setting.Substring(0, setting.IndexOf(C.SettingKeyValueSeperator))))
                        {
                            CaptureSettings(settingsToValidate);
                            return false;
                        }
                    }
                }
                else
                {
                    CaptureSettings(settingsToValidate);
                    return false;
                }
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                System.IO.Directory.CreateDirectory(C.AppConfigFilesFolder);
                using (System.IO.File.Create(C.AppConfigFilesFolder + Helper.GetCurrentAppName() + C.AppConfigFileExtension)) { }
                CaptureSettings(settingsToValidate);
            }
            catch (FileNotFoundException)
            {
                if (!System.IO.Directory.Exists(C.AppConfigFilesFolder))
                {
                    System.IO.Directory.CreateDirectory(C.AppConfigFilesFolder);
                }
                using (System.IO.File.Create(C.AppConfigFilesFolder + Helper.GetCurrentAppName() + C.AppConfigFileExtension)) { }
                CaptureSettings(settingsToValidate);
            }
            return false; ;
        }

        private void UpdateSetting(string settingName, string settingValue, string[] currentConfig, int lineNumber)
        {
            _fileHelper.DeleteFile();
            currentConfig[lineNumber] = settingName + C.SettingKeyValueSeperator + settingValue;
            foreach (string lineToWrite in currentConfig)
                _fileHelper.AppendLineToFile(lineToWrite);
        }
        private string[] AllSettings { get { return _fileHelper.GetAllLinesInFile(); } }

        private void CaptureSettings(string[] keysToCapture)
        {
            List<string> settingsFile = new List<string>();
            Console.WriteLine(C.CaptureSettingsMessage);
            foreach (string setting in keysToCapture)
            {
                Console.WriteLine(C.CaptureParameterMessage + setting);
                string paramInputValue = Console.ReadLine();
                if (paramInputValue.Length > 0)
                    settingsFile.Add(paramInputValue);
                else
                    settingsFile.Add(C.ParameterDefault);
            }
            for (int i = 0; i < keysToCapture.Length; ++i)
                WriteSetting(keysToCapture[i], settingsFile[i]);
        }
    }
}