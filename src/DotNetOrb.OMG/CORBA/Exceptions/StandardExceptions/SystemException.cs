// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace CORBA
{
    public enum CompletionStatus
    {
        Yes,
        No,
        Maybe
    }

    public class SystemException : Exception
    {
        public int Minor { get; set; }
        public CompletionStatus Completed { get; set; }

        public SystemException()
        {
            Minor = 0;
            Completed = CompletionStatus.No;
        }

        public SystemException(int minor, CompletionStatus completed)
        {
            Minor = minor;
            Completed = completed;
        }

        public SystemException(string reason): base(reason)
        {
            Completed = CompletionStatus.No;
        }

        public SystemException(string? reason, int minor, CompletionStatus completed): base(reason)
        {
            Minor = minor;
            Completed = completed;
        }
        public SystemException(string? reason, int minor, CompletionStatus completed, Exception? innerException) : base(reason, innerException)
        {
            Minor = minor;
            Completed = completed;
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(base.ToString());
            buffer.Append(" Minor: ");
            buffer.Append(Minor);
            buffer.Append(" CompletionStatus: ");
            switch (Completed)
            {
                case CompletionStatus.No:
                    buffer.Append("COMPLETED_NO.");
                    break;
                case CompletionStatus.Yes:
                    buffer.Append("COMPLETED_YES.");
                    break;
                case CompletionStatus.Maybe:
                    buffer.Append("COMPLETED_MAYBE.");
                    break;
            }
            return buffer.ToString();
        }
    }
}
