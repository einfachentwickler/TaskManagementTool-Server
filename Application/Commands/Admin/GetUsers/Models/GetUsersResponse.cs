using Application.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Commands.Admin.GetUsers.Models;

public class GetUsersResponse
{
    [JsonProperty("users")]
    public IEnumerable<UserDto> Users { get; set; }
}