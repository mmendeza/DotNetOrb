// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public interface IBoxedValueHelper
    {
        string GetId();
        object ReadValue(IInputStream inputStream);
        void WriteValue(IOutputStream outputStream, object value);
        
    }

}
