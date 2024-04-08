// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotNetOrb.Core.Config
{
    public interface IConfiguration
    {
        ORB ORB { get; }

        bool ContainsKey(string key);

        string GetValue(string key);

        string GetValue(string key, string defaultValue);

        bool GetAsBoolean(string key);

        bool GetAsBoolean(string key, bool defaultValue);
        double GetAsFloat(string key);

        double GetAsFloat(string key, double defaultValue);

        int GetAsInteger(string key);

        int GetAsInteger(string key, int defaultValue);

        int GetAsInteger(string key, int defaultValue, int radix);

        long GetAsLong(string key);

        long GetAsLong(string key, long defaultValue);

        object? GetAsObject(string key, params object[] paramArray);

        object? GetAsObject(string key, string defaultClass, params object[] paramArray);

        ILogger GetLogger(string name);

        ILogger GetLogger(Type type);

        string[] GetAsStringArray(string key);

        string[] GetAsStringArray(string key, string defaultValue);

        List<string> GetAsList(string key);

        List<string> GetAsList(string key, string defaultValue);

        List<string> GetKeysWithPrefix(string prefix);

        void SetValue(string key, int value);

        void SetValue(string key, string value);

        void SetValues(IDictionary<string, object> properties);
    }
}
