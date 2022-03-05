using System;

namespace TaskManagementTool.Common.Exceptions
{
    public class TaskManagementToolException : Exception
    {
        public TaskManagementToolException(string message) : base(message)
        {

        }
    }
}
