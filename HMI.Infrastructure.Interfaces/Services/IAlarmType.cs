// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Services
{
    public interface IAlarmType
    {
        Guid ID
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        string Translation
        {
            get;
            set;
        }
    }

}
