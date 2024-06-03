using TaskManagementTool.Common.Enums;

namespace TaskManagementTool.BusinessLogic.Dto.Errors;

public record ErrorDto(ApiErrorCode ErrorCode, string ErrorMessage);