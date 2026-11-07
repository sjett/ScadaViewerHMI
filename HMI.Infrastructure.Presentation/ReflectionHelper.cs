// SPDX-License-Identifier: GPL-3.0-only
using NLog;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace HMI.Infrastructure.Presentation
{
    public static class ReflectionHelper
    {
        private static readonly Logger logger = LogManager.GetLogger("Reflection");
        private static readonly Regex TypeNamePattern = new Regex("(?<TypeName>[^\\s,]+)(?:\\s*,\\s*(?<AssemblyName>[^,]+)(?:\\s*,\\s*(Version\\s*=\\s*(?<Version>[^,]+)|Culture\\s*=\\s*(?<Culture>[^,]+)|PublicKeyToken\\s*=\\s*(?<PublicKeyToken>[^,]+)|ProcessorArchitecture\\s*=\\s*(?<ProcessorArchitecture>)[^,]+)){0,4})?", RegexOptions.Compiled);

        public static Type GetType(string assembly, string typeName)
        {
            Type result;
            try
            {
                logger.Debug("Getting type for {0}. Default assembly is: {1}", typeName, assembly);
                assembly = MatchAssemblyName(typeName) ?? assembly;
                typeName = MatchTypeName(typeName);
                Type type = Type.GetType(typeName);
                if (type == null)
                {
                    logger.Info("Could not get type {0}, trying with explicit assembly name", typeName);
                    string location = Assembly.GetExecutingAssembly().Location;
                    string directoryName = Path.GetDirectoryName(location);
                    string text = Path.Combine(directoryName, assembly + ".dll");
                    logger.Info("Getting assembly info for {0}", text);
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(text);
                    logger.Trace<string, string>("Creating type {0} from assembly {1}", typeName, assemblyName.FullName);
                    type = GetTypeFromAssembly(typeName, assemblyName.FullName);
                }
                result = type;
            }
            catch (Exception exception)
            {
                logger.Fatal("Failed to get type", exception);
                throw;
            }
            return result;
        }
        private static string MatchAssemblyName(string assemblyQualifiedTypeName)
        {
            Match match = TypeNamePattern.Match(assemblyQualifiedTypeName);
            Group group = match.Groups["AssemblyName"];
            if (!group.Success)
            {
                return null;
            }
            return group.Value;
        }
        private static string MatchTypeName(string assemblyQualifiedTypeName)
        {
            Match match = TypeNamePattern.Match(assemblyQualifiedTypeName);
            return match.Groups["TypeName"].Value;
        }
        private static Type GetTypeFromAssembly(string typeName, string assemblyName)
        {
            string typeFullName = Assembly.CreateQualifiedName(assemblyName, typeName);
            return Type.GetType(typeFullName);
        }
    }
}
