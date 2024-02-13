using Newtonsoft.Json;
using System.Collections.Generic;
using TaskManagementTool.BusinessLogic.ViewModels;

namespace TaskManagementTool.BusinessLogic.Commands.Admin.GetUsers.Models;

public class GetUsersResponse
{
    [JsonProperty("users")]
    public IEnumerable<UserDto> Users { get; set; }
}