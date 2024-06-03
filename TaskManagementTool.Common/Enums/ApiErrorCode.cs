using System.Text.Json.Serialization;

namespace TaskManagementTool.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApiErrorCode
{
    UserNotFound,
    TodoNotFound,
    Unautorized,
    InvalidInput,
    Forbidden,
}