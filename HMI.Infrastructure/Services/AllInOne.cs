// SPDX-License-Identifier: GPL-3.0-only
using System.IO;

namespace HMI.Infrastructure.Services
{


    public interface IResourceProviderService
    {
        Stream GetResourceStream(string resourcePath);
        TextReader GetResourceReader(string resourcePath);
    }

    public interface ISerializationService
    {
        T Deserialize<T>(string xml);
        string Serialize(object dto);
    }

    public interface ITaskSchedulerFactory
    {
        TaskScheduler GetAsyncScheduler();
        TaskScheduler GetSynchronizedScheduler();
    }
}
