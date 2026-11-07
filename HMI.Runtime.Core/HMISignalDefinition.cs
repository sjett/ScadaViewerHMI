// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core
{
    /// <summary>

    /// </summary>
    public class HMISignalDefinition
	{
		public Guid SignalID
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string AliasName
		{
			get;
			set;
		}
		public bool Active
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public string DescriptionSymbolicText
		{
			get;
			set;
		}
		public bool LogUserActivity
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double FactorX1
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double FactorX2
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double FactorY1
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double FactorY2
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double Hysterese
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double HystereseLog
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double HystereseAlarm
		{
			get;
			set;
		}
		public double Maximum
		{
			get;
			set;
		}
		public double Minimum
		{
			get;
			set;
		}
		public string Unit
		{
			get;
			set;
		}
		public bool VChannel
		{
			get;
			set;
		}
		public int VChannelTypeID
		{
			get;
			set;
		}
		public string VChannelInitValue
		{
			get;
			set;
		}
		public bool AltValue
		{
			get;
			set;
		}
		public double? AltValue_1
		{
			get;
			set;
		}
		public double? AltValue_2
		{
			get;
			set;
		}
		public double? AltValue_3
		{
			get;
			set;
		}
		public double? AltValue_4
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double Hysterese_2
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double HystereseLog_2
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public double HystereseAlarm_2
		{
			get;
			set;
		}
		public int DataTypeID
		{
			get;
			set;
		}
		public int Status
		{
			get;
			set;
		}
		public string OfflineValue
		{
			get;
			set;
		}
		public bool OPCEnabled
		{
			get;
			set;
		}
		public int OPCQuality
		{
			get;
			set;
		}
        /// <summary>

        /// </summary>
		public Guid? DiscreteValueTypeID
		{
			get;
			set;
		}
		public HMISignalGroup Group
		{
			get;
			set;
		}
		public HMIWriteGroup WriteGroup
		{
			get;
			set;
		}
		public HMIServer Server
		{
			get;
			set;
		}
		public HMIConnectorEntity Connector
		{
			get;
			set;
		}
		public IEnumerable<HMIDiscreteValue> DiscreteValues
		{
			get;
			set;
		}
		public IEnumerable<HMILog> Logs
		{
			get;
			set;
		}
	}
}
