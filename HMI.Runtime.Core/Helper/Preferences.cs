// SPDX-License-Identifier: GPL-3.0-only

using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Windows;

namespace HMI.Runtime.Core.Helper
{
    public static class Preferences
    {
        static readonly IsolatedStorageFile isolatedStorageFile;
        static readonly string fileName;
        static JsonNode root;
        //static readonly Logger logger;

        static Preferences()
        {
            isolatedStorageFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            //logger = LogManager.GetLogger("Preferences");
            try
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                fileName = executingAssembly.GetName().Name + "." + FileVersionInfo.GetVersionInfo(executingAssembly.Location).ProductVersion + ".Preferences";
                Application.Current.Exit += onExit;
                load();
                if (root == null)
                {
                    root = new JsonObject();
                }
            }
            catch (Exception exception)
            {
                throw;
                //logger.WarnException(ErrorTexts.CouldNotInitializePreferences, exception);
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
                        root = JsonNode.Parse(json)!;
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
                //logger.WarnException(ErrorTexts.CouldNotLoadPreferences, exception);
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
                throw;
                //logger.WarnException(ErrorTexts.CouldNotSavePreferences, exception);
            }
        }

        public static void Reset()
        {
            root = new JsonObject();
            save();
        }
        public static JsonNode? GetPreferences(object caller)
        {
            if (caller != null && root != null)
            {
                Type type = caller.GetType();
                return GetPreferences(type);
            }
            return null;
        }
        public static JsonNode? GetPreferences(Type callerType)
        {
            string? fullName = callerType.FullName;

            return root[fullName!];
        }
        public static void SetPreferences(object caller, JsonNode preferences)
        {
            if (caller == null)
            {
                return;
            }
            Type type = caller.GetType();
            SetPreferences(type, preferences);
        }
        public static void SetPreferences(Type callerType, JsonNode preferences)
        {
            string fullName = callerType.FullName;
            //var xElement = root[fullName] as JObject;

             root[fullName] = preferences; //全部替换

        }
    }
}
