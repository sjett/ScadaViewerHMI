// SPDX-License-Identifier: GPL-3.0-only
//using NLog;
//using System;
//namespace HMI.Editor
//{
//	internal class NLogAdapter : ILoggerFacade
//	{
//        public void Log(string message, Category category, Priority priority)
//        {
//            LogLevel logLevel;
//            switch (category)
//            {
//                case Category.Debug:
//                    logLevel = LogLevel.Debug;
//                    break;
//                case Category.Exception:
//                    logLevel = LogLevel.Error;
//                    break;
//                case Category.Info:
//                    logLevel = LogLevel.Info;
//                    break;
//                case Category.Warn:
//                    logLevel = LogLevel.Warn;
//                    break;
//                default:
//                    logLevel = LogLevel.Trace;
//                    break;
//            }
//            logger.Log(logLevel, message);
//        }
//	}
//}
