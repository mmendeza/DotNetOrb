// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Test;

namespace DotNetOrb.Test
{
    public class BasicTypesIfzImpl : BasicTypesIfzPOA
    {
        public override sbyte ASbyte { get; set; }
        public override sbyte[] ASbyteSeq { get; set; }
        public override byte AByte { get; set; }
        public override byte[] AByteSeq { get; set; }
        public override short AShort { get; set; }
        public override short[] AShortSeq { get; set; }
        public override ushort AUshort { get; set; }
        public override ushort[] AUshortSeq { get; set; }
        public override int ALong { get; set; }
        public override int[] ALongSeq { get; set; }
        public override uint AUlong { get; set; }
        public override uint[] AUlongSeq { get; set; }
        public override long ALongLong { get; set; }
        public override long[] ALongLongSeq { get; set; }
        public override ulong AUlongLong { get; set; }
        public override ulong[] AUlongLongSeq { get; set; }
        public override float AFloat { get; set; }
        public override float[] AFloatSeq { get; set; }
        public override double ADouble { get; set; }
        public override double[] ADoubleSeq { get; set; }
        public override char AChar { get; set; }
        public override char[] ACharSeq { get; set; }
        public override char AWchar { get; set; }
        public override char[] AWcharSeq { get; set; }
        public override string AString { get; set; }
        public override string[] AStringSeq { get; set; }
        public override string AWstring { get; set; }
        public override string[] AWstringSeq { get; set; }
        public override bool ABoolean { get; set; }
        public override bool[] ABooleanSeq { get; set; }
        public override byte AOctet { get; set; }
        public override byte[] AOctetSeq { get; set; }
        public override decimal AFixed { get; set; }
        public override decimal[] AFixedSeq { get; set; }
    }
}
