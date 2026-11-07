// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.Serialization;
namespace HMI.Common.Modularity
{
    [Serializable]
    public class MappingException : Exception
    {
        public MappingException()
        {
        }
        public MappingException(string message)
            : base(message)
        {
        }
        public MappingException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected MappingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
