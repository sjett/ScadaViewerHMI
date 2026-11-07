// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Resources;
using Newtonsoft.Json.Linq;
using NLog;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Windows;

namespace HMI.Infrastructure.Services
{
    public static class Preferences
    {
        static readonly IsolatedStorageFile isolatedStorageFile;
        static readonly string fileName;
        static JObject root;
        static readonly Logger logger;

        static Preferences()
        {
            isolatedStorageFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            logger = LogManager.GetLogger("Preferences");
            try
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                fileName = executingAssembly.GetName().Name + "." + FileVersionInfo.GetVersionInfo(executingAssembly.Location).ProductVersion + ".Preferences";
                Application.Current.Exit += onExit;
                load();
                if (root == null)
                {
                    root = new JObject();
                }
            }
            catch (Exception exception)
            {
                logger.WarnException(ErrorTexts.CouldNotInitializePreferences, exception);
            }
        }
        static void load()
        {
            if (isolatedStorageFile.GetFileNames(fileName).Length == 0)
            {
                return;
            }

            try
            {
                using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isolatedStorageFile))
                {
                    using (StreamReader streamReader = new StreamReader(isolatedStorageFileStream))
                    {
                        string json = streamReader.ReadToEnd();
                        root = JObject.Parse(json);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.WarnException(ErrorTexts.CouldNotLoadPreferences, exception);
            }
        }
        static void onExit(object sender, ExitEventArgs e)
        {
            save();
        }
        static void save()
        {
            try
            {
                using (IsolatedStorageFileStream isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isolatedStorageFile))
                {
                    using (StreamWriter streamWriter = new StreamWriter(isolatedStorageFileStream))
                    {
                        streamWriter.Write(root.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                logger.WarnException(ErrorTexts.CouldNotSavePreferences, exception);
            }
        }

        public static void Reset()
        {
            root = new JObject();
            save();
        }
        public static JObject? GetPreferences(object caller)
        {
            if (caller != null && root != null)
            {
                Type type = caller.GetType();
                return GetPreferences(type);
            }
            return null;
        }
        public static JObject? GetPreferences(Type callerType)
        {
            string? fullName = callerType.FullName;

            return root[fullName!] as JObject;
        }
        public static void SetPreferences(object caller, JObject preferences)
        {
            if (caller == null)
            {
                return;
            }
            Type type = caller.GetType();
            SetPreferences(type, preferences);
        }
        public static void SetPreferences(Type callerType, JObject preferences)
        {
            string fullName = callerType.FullName;
            //var xElement = root[fullName] as JObject;

             root[fullName] = preferences; //全部替换

        }
    }
}
