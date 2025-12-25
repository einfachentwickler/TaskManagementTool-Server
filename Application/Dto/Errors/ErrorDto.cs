using TaskManagementTool.Common.Enums;

namespace Application.Dto.Errors;

public record ErrorDto(ApiErrorCode ErrorCode, string ErrorMessage);