﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apex7000_BillValidator
{
    /// <summary>
    /// Helper entry for describing serial communication transactions
    /// </summary>
    public class DebugBufferEntry
    {
        /// <summary>
        /// Creates a new entry and marks it as being sent master->slave
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <returns>DebugBufferEntry</returns>
        public static DebugBufferEntry AsMaster(byte[] data)
        {
            return new DebugBufferEntry(data, Flows.Master);
        }

        /// <summary>
        /// Creates a new entry and marks it as being sent slave->master
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <returns>DebugBufferEntry</returns>
        public static DebugBufferEntry AsSlave(byte[] data)
        {
            return new DebugBufferEntry(data, Flows.Slave);
        }

        private DebugBufferEntry(byte[] data, Flows flow)
        {
            var dt = DateTime.Now;
            Timestamp = String.Format("{0}:{1}:{2}", dt.Minute, dt.Second, dt.Millisecond);
            Data = data;
            Flow = flow;
        }

        /// <summary>
        /// Byte[] data that was transmitted
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Hex formatted byte[] data as 0xHH format
        /// </summary>
        public string PrintableData
        {
            get
            {
                return ByteArrayToString(Data);
            }
        }

        /// <summary>
        /// Returns Master or Slave
        /// </summary>
        public Flows Flow { get; private set; }

        /// <summary>
        /// Retrurns minutes:seconds:milliseconds timestamp
        /// </summary>
        public String Timestamp { get; private set; }

        /// <summary>
        /// Returns Flow :: Data :: Timestamp
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} :: {1} :: {2}", Flow, PrintableData, Timestamp);
        }

        /// <summary>
        /// Convert byte[] to a single-byte hex formatted string
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2} ", b);
            return hex.ToString();
        }        
    }

    public enum Flows {
        Master,
        Slave
    }
}