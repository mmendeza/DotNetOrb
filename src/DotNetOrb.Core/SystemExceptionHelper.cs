// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.Util;
using IOP;
using System;

namespace DotNetOrb.Core
{
    public class SystemExceptionHelper
    {
        private SystemExceptionHelper()
        {
            // utility class
        }

        //"IDL:omg.org/CORBA/Bounds:1.0";
        private static string ClassName(string repId)
        {
            // cut "IDL:" prefix and version
            var index = repId.IndexOf('/');
            string idBase = repId.Substring(index + 1, repId.LastIndexOf(':') - (index + 1));
            return idBase.Replace('/', '.');
        }

        private static string RepId(Type type)
        {
            string className = type.FullName;
            string body = className.Replace('.', '/');
            return "IDL:omg.org/" + body + ":1.0";
        }

        public static void Insert(CORBA.Any any, CORBA.SystemException exception)
        {
            any.Type = GetTypeCode(exception);
            Write(any.CreateOutputStream(), exception);
        }

        public static CORBA.TypeCode GetTypeCode(CORBA.SystemException exception)
        {
            string fullName = exception.GetType().FullName;
            string name = fullName.Substring(fullName.LastIndexOf('.') + 1);
            fullName = fullName.Replace('.', '/');
            CORBA.ORB orb = CORBA.ORB.Init();
            CORBA.TypeCode _type =
            orb.CreateStructTc("IDL:omg.org/" + fullName + ":1.0", name,
            new StructMember[]{
                            new StructMember("minor", orb.GetPrimitiveTc(TCKind.TkLong), null),
                            new StructMember("completed",orb.CreateEnumTc("IDL:omg.org/CORBA/CompletionStatus:1.0",
                                                                                "CompletionStatus",
                                                                                new string[]{"COMPLETED_YES","COMPLETED_NO","COMPLETED_MAYBE"}),
                            null)
                        });
            return _type;
        }

        public static CORBA.SystemException Read(IInputStream inputStream)
        {
            string className = ClassName(inputStream.ReadString());
            int minor = inputStream.ReadLong();
            CompletionStatus completed = (CompletionStatus)inputStream.ReadLong();
            string message = null;

            if (inputStream is ReplyInputStream input)
            {
                try
                {
                    foreach (var context in input.ServiceContextList)
                    {
                        if (context.ContextId == ExceptionDetailMessage.Value)
                        {
                            CDRInputStream data = new CDRInputStream(context.ContextData);
                            try
                            {
                                data.OpenEncapsulatedArray();
                                message = data.ReadWString();
                            }
                            finally
                            {
                                data.Close();
                            }
                        }
                    }
                }
                finally
                {
                    input.Close();
                }
            }

            try
            {
                CORBA.SystemException sysEx = (CORBA.SystemException)ObjectUtil.GetInstance(className, "Server side exception: " + message, minor, completed);
                return sysEx;
            }
            catch (Exception e)
            {
                return new Unknown(className);
            }
        }

        public static void Write(IOutputStream outputStream, CORBA.SystemException exception)
        {
            outputStream.WriteString(RepId(exception.GetType()));
            outputStream.WriteLong(exception.Minor);
            outputStream.WriteLong((int)exception.Completed);
        }
    }
}
