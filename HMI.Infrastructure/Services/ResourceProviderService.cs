// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.IO;

namespace HMI.Infrastructure.Services
{
    public class ResourceProviderService : IResourceProviderService
    {
        public Stream GetResourceStream(string resourcePath)
        {
            string resourcePath2 = MapPath(resourcePath);
            return OpenFile(resourcePath2);
        }
        public TextReader GetResourceReader(string resourcePath)
        {
            string resourcePath2 = MapPath(resourcePath);
            return OpenTextFile(resourcePath2);
        }
        public virtual string MapPath(string resourcePath)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), resourcePath);
        }
        public virtual Stream OpenFile(string resourcePath)
        {
            return File.OpenRead(resourcePath);
        }
        public virtual TextReader OpenTextFile(string resourcePath)
        {
            return File.OpenText(resourcePath);
        }
    }
}
