// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public interface IContext
    {
        string ContextName();
        IContext Parent();
        IContext CreateChild(string childContextName);
        void SetOneValue(string propName, Any value);
        void SetValues(NVList values);
        void DeleteValues(string propName);
        NVList GetValues(string startScope, int opFlags, string pattern);
    }
}
