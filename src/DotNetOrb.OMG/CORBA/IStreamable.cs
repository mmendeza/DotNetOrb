// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public interface IStreamable
    {
        void _Read(IInputStream inputStream);
        void _Write(IOutputStream outputStream);
        TypeCode _Type { get; }
    }

}
