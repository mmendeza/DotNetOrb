// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DotNetOrb.Core.CDR
{
    internal class EncapsulationInfo
    {
        public bool IsLittleEndian { get; set; }
        public int Index { get; set; }
        public int Start { get; set; }
        public int Size { get; set; }
        public Dictionary<object, int> ValueMap { get; set; }
        public Dictionary<string, int> RepIdMap { get; set; }
        public Dictionary<string, int> CodebaseMap { get; set; }


        /// <summary>constructor used by CDRInputStream</summary>
        public EncapsulationInfo(bool le, int index, int start, int size)
        {
            IsLittleEndian = le;
            Index = index;
            Start = start;
            Size = size;
            ValueMap = new Dictionary<object, int>();
            RepIdMap = new Dictionary<string, int>();
            CodebaseMap = new Dictionary<string, int>();
        }

        /// <summary>
        /// constructor used by CDROutputStream:
        /// record the index a new encapsulation starts with
        /// and the start position in the buffer.CORBA specifies that "indirections
        /// may not cross encapsulation boundaries", so the new encapsulation must
        /// set up its own indirection maps for values, repository ids and codebase
        /// strings.The maps currently in use are also recorded, to be restored at
        /// the end of the encapsulation.
        /// </summary>
        public EncapsulationInfo(int index, int start, Dictionary<object, int> vMap, Dictionary<string, int> rMap, Dictionary<string, int> cMap)
        {
            Index = index;
            Start = start;
            ValueMap = vMap;
            RepIdMap = rMap;
            CodebaseMap = cMap;

            if (ValueMap == null)
            {
                ValueMap = new Dictionary<object, int>();
            }
            if (RepIdMap == null)
            {
                RepIdMap = new Dictionary<string, int>();
            }
            if (CodebaseMap == null)
            {
                CodebaseMap = new Dictionary<string, int>();
            }
        }
    }
}
