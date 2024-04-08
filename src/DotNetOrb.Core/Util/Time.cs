// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using System;
using System.Threading;
using TimeBase;

namespace DotNetOrb.Core.Util
{
    public class Time
    {
        private Time()
        {
        }

        /// <summary>
        /// Difference between the CORBA Epoch and the Unix Epoch: the time 
        /// from 1582/10/15 00:00 until 1970/01/01 00:00 in 100 ns units.
        /// </summary>
        public static long UnixOffset = 122192928000000000L;

        /// <summary>
        /// Returns the current time as a CORBA UtcT.
        /// </summary>
        public static UtcT CorbaTime()
        {
            return CorbaTime(DateTimeOffset.Now.ToUnixTimeMilliseconds());
        }

        /// <summary>
        /// Converts the given unixTime into a CORBA UtcT.         
        /// </summary>
        /// <param name="unixTime">unixTime the number of milliseconds since 1970/01/01 00:00 UTC.</param>
        /// <returns>CORBA UtcT</returns>
        public static UtcT CorbaTime(long unixTime)
        {
            UtcT result = new UtcT();

            result.Time = (ulong)(unixTime * 10000 + UnixOffset);

            // unixTime is always UTC.
            // Therefore, no time zone offset.
            result.Tdf = 0;

            // nothing reasonable to put here
            result.Inacchi = 0;
            result.Inacclo = 0;

            return result;
        }


        /// <summary>
        /// Converts the given DateTime to a CORBA UtcT.
        /// </summary>
        public static UtcT CorbaTime(DateTime date)
        {
            return CorbaTime(date.Ticks / TimeSpan.TicksPerMillisecond);
        }

        public static long NanoTime()
        {
            return DateTime.Now.Ticks * 100;
        }

        /// <summary>
        /// Returns a CORBA UtcT that represents an instant that lies a given number of CORBA time units (100 ns) in the future. 
        /// If the argument is negative, returns null.
        /// </summary>        
        public static UtcT CorbaFuture(ulong corbaUnits)
        {
            if (corbaUnits < 0)
            {
                return null;
            }

            UtcT result = CorbaTime();
            result.Time = result.Time + corbaUnits;
            return result;
        }


        /// <summary>
        ///  Returns the number of milliseconds between now and the given CORBA time.
        ///  The value is positive if that time is in the future, and negative otherwise.
        /// </summary>        
        public static long MillisTo(UtcT time)
        {
            long unixTime = ((long)time.Time - UnixOffset) / 10000;

            // if the time is not UTC, correct time zone
            if (time.Tdf != 0)
            {
                unixTime = unixTime - time.Tdf * 60000;
            }

            return unixTime - DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }


        /// <summary>
        /// Returns true if the instant represented by the given UtcT is already in the past, false otherwise.
        /// As a special convenience, this method also returns false if the argument is null.
        /// </summary>
        public static bool HasPassed(UtcT time)
        {
            if (time != null)
            {
                return MillisTo(time) < 0;
            }

            return false;
        }


        /// <summary>
        /// Compares two UtcT time values and returns that which is earlier.  Either argument can be null; 
        /// this is considered as a time that lies indefinitely in the future.If both arguments are null,
        /// this method returns null itself.
        /// </summary>        
        public static UtcT Earliest(UtcT timeA, UtcT timeB)
        {
            if (timeA == null)
            {
                return timeB;
            }

            if (timeB == null || timeA.Time <= timeB.Time)
            {
                return timeA;
            }
            return timeB;
        }

        /// <summary>
        /// Returns a CDR encapsulation of the given UtcT.
        /// </summary>        
        public static byte[] ToCDR(UtcT time)
        {
            using (var outputStream = new CDROutputStream())
            {
                outputStream.BeginEncapsulatedArray();
                TimeBase.UtcTHelper.Write(outputStream, time);
                return outputStream.GetBufferCopy();
            }
        }

        /// <summary>
        /// Decodes a CDR encapsulation of a UtcT.
        /// </summary>        
        public static UtcT FromCDR(byte[] buffer)
        {
            using (var inputStream = new CDRInputStream(buffer))
            {
                inputStream.OpenEncapsulatedArray();
                return TimeBase.UtcTHelper.Read(inputStream);
            }
        }

        /// <summary>
        /// This method blocks until the given time has been reached. 
        /// If the time is null, or it has already passed, then this method returns immediately.
        /// </summary>
        public static void WaitFor(UtcT time)
        {
            if (time != null)
            {
                long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                long delta = MillisTo(time);
                long then = now + delta;

                while (delta > 0)
                {
                    try
                    {
                        Thread.Sleep((int)delta);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        // ignored
                    }

                    delta = then - DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }
            }
        }
    }
}
