// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    /// <summary>
    ///  ValueOutputStream is used for implementing RMI-IIOP stream format version 2.
    /// </summary>
    public interface IValueOutputStream
    {

        /// <summary>
        /// Ends any currently open chunk, writes a valuetype header for a nested custom valuetype
        /// (with a null codebase and the specified repository ID), and increments the valuetype nesting depth.        
        /// </summary>
        void StartValue(string repId);

        /// <summary>
        /// Ends any currently open chunk, writes the end tag for the nested custom valuetype,
        /// and decrements the valuetype nesting depth.
        /// </summary>
        void EndValue();
    }
}
