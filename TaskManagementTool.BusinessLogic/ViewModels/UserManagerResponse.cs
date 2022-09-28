using System;
using System.Collections.Generic;

namespace TaskManagementTool.BusinessLogic.ViewModels
{
    public class UserManagerResponse
    {
        public string Message { get; init; }

        public bool IsSuccess { get; init; }

        public IEnumerable<string> Errors { get; set; }

        public DateTime? ExpiredDate { get; set; }
    }
}
