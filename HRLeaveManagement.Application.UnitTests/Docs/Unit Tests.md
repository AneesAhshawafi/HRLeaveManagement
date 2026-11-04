# Unit Tests Documentation

This document outlines the possible unit tests for every Command and Query across the features in `HRLeaveManagement.Application`.

---

## 1. LeaveType Feature

| Command/Query name | Unit Test name | Description |
| :--- | :--- | :--- |
| **GetLeaveTypesQuery** | `Handle_ValidRequest_ReturnsLeaveTypeDtoList` | Verifies that querying all leave types retrieves the list from the repository, maps them to `List<LeaveTypeDto>`, and logs an information message. |
| **GetLeaveTypesQuery** | `Handle_WhenNoLeaveTypesExist_ReturnsEmptyList` | Verifies that an empty `List<LeaveTypeDto>` is returned when the repository contains no leave types. |
| **GetLeaveTypeDetailsQuery** | `Handle_ExistingId_ReturnsLeaveTypeDetailsDto` | Verifies that requesting details for a valid `id` returns the mapped `LeaveTypeDetailsDto` and logs an information message. |
| **GetLeaveTypeDetailsQuery** | `Handle_NonExistingId_ReturnsNull` | Verifies that requesting details for a non-existent `id` returns `null` when the repository returns `null`. |
| **CreateLeaveTypeCommand** | `Handle_ValidCommand_CreatesLeaveTypeAndReturnsId` | Verifies that a valid command passes validation, maps to the domain entity, calls `CreateAsync` on the repository, and returns the new record's `Id`. |
| **CreateLeaveTypeCommand** | `Handle_EmptyOrNullName_ThrowsBadRequestException` | Verifies that an empty or `null` `Name` fails validation, logs an error, and throws `BadRequestException`. |
| **CreateLeaveTypeCommand** | `Handle_NameExceedsMaxLength_ThrowsBadRequestException` | Verifies that a `Name` longer than 70 characters fails validation and throws `BadRequestException`. |
| **CreateLeaveTypeCommand** | `Handle_DefaultDaysOutOfRange_ThrowsBadRequestException` | Verifies that `DefaultDays` less than or equal to 1, or greater than or equal to 100, fails validation and throws `BadRequestException`. |
| **CreateLeaveTypeCommand** | `Handle_DuplicateLeaveTypeName_ThrowsBadRequestException` | Verifies that when `IsLeaveTypeUnique` returns `false`, validation fails and throws `BadRequestException` without calling `CreateAsync`. |
| **UpdateLeaveTypeCommand** | `Handle_ValidCommand_UpdatesLeaveTypeAndReturnsUnit` | Verifies that a valid update command maps onto the existing entity, calls `UpdateAsync` on the repository, and returns `Unit.Value`. |
| **UpdateLeaveTypeCommand** | `Handle_LeaveTypeNotFound_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null` for the given `Id`, an error is logged and `NotFoundException` is thrown. |
| **UpdateLeaveTypeCommand** | `Handle_InvalidIdOrNonExistingLeaveType_ThrowsCustomValidationException` | Verifies that an `Id <= 0` or `IsLeaveTypeExist` returning `false` fails validation, logs a warning, and throws `CustomValidationException`. |
| **UpdateLeaveTypeCommand** | `Handle_InvalidName_ThrowsCustomValidationException` | Verifies that an empty `Name` or a `Name` exceeding 70 characters fails validation and throws `CustomValidationException`. |
| **UpdateLeaveTypeCommand** | `Handle_DefaultDaysOutOfRange_ThrowsCustomValidationException` | Verifies that `DefaultDays <= 1` or `DefaultDays >= 100` fails validation and throws `CustomValidationException`. |
| **UpdateLeaveTypeCommand** | `Handle_DuplicateLeaveTypeName_ThrowsCustomValidationException` | Verifies that a non-unique `Name` (`IsLeaveTypeUnique` returns `false`) fails validation and throws `CustomValidationException`. |
| **DeleteLeaveTypeCommand** | `Handle_ExistingId_DeletesLeaveTypeAndReturnsUnit` | Verifies that when the leave type exists, `DeleteAsync` is called on the repository and `Unit.Value` is returned. |
| **DeleteLeaveTypeCommand** | `Handle_NonExistingId_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, `NotFoundException` is thrown and `DeleteAsync` is never called. |

---

## 2. LeaveAllocation Feature

| Command/Query name | Unit Test name | Description |
| :--- | :--- | :--- |
| **GetLeaveAllocationsQuery** | `Handle_ValidRequest_ReturnsLeaveAllocationDtoList` | Verifies that `GetLeaveAllocationsWithDetails` is called, the results are mapped to `List<LeaveAllocationDto>`, and an info log is recorded. |
| **GetLeaveAllocationsQuery** | `Handle_WhenNoAllocationsExist_ReturnsEmptyList` | Verifies that an empty `List<LeaveAllocationDto>` is returned when no leave allocations exist in the repository. |
| **GetLeaveAllocationDetailsQuery** | `Handle_ExistingId_ReturnsLeaveAllocationDetailsDto` | Verifies that `GetLeaveAllocationWithDetails` is called with the requested `Id`, mapped to `LeaveAllocationDetailsDto`, and returned. |
| **GetLeaveAllocationDetailsQuery** | `Handle_NonExistingId_ReturnsNull` | Verifies that requesting details for a non-existent `Id` returns `null` when the repository returns `null`. |
| **CreateLeaveAllocationCommand** | `Handle_ValidCommand_CreatesAllocationAndReturnsId` | Verifies that a valid `LeaveTypeId` passes validation, maps the command to a `LeaveAllocation` entity, calls `CreateAsync`, logs info, and returns the created `Id`. |
| **CreateLeaveAllocationCommand** | `Handle_InvalidLeaveTypeId_ThrowsBadRequestException` | Verifies that `LeaveTypeId <= 0` fails validation, logs an error, and throws `BadRequestException`. |
| **CreateLeaveAllocationCommand** | `Handle_LeaveTypeDoesNotExist_ThrowsBadRequestException` | Verifies that when `IsLeaveTypeExist` returns `false`, validation fails and throws `BadRequestException` without creating an allocation. |
| **UpdateLeaveAllocationCommand** | `Handle_ValidCommand_UpdatesAllocationAndReturnsUnit` | Verifies that an existing allocation with valid `LeaveTypeId`, `NumberOfDays`, and `Period` is mapped, updated via `UpdateAsync`, and returns `Unit.Value`. |
| **UpdateLeaveAllocationCommand** | `Handle_AllocationNotFound_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, an error is logged and `NotFoundException` is thrown before validation. |
| **UpdateLeaveAllocationCommand** | `Handle_InvalidOrNonExistingLeaveTypeId_ThrowsBadRequestException` | Verifies that `LeaveTypeId <= 0` or a non-existing leave type fails validation and throws `BadRequestException`. |
| **UpdateLeaveAllocationCommand** | `Handle_NumberOfDaysZeroOrNegative_ThrowsBadRequestException` | Verifies that `NumberOfDays <= 0` fails validation, logs an error, and throws `BadRequestException`. |
| **UpdateLeaveAllocationCommand** | `Handle_PeriodBeforeCurrentYear_ThrowsBadRequestException` | Verifies that a `Period` earlier than `DateTime.Now.Year` fails validation and throws `BadRequestException`. |
| **UpdateLeaveAllocationCommand** | `Handle_AllocationIdDoesNotExistInValidator_ThrowsBadRequestException` | Verifies that when `IsLeaveAllocationExist` returns `false` during validation, `BadRequestException` is thrown. |
| **DeleteLeaveAllocationCommand** | `Handle_ExistingId_DeletesAllocationAndReturnsUnit` | Verifies that an existing leave allocation is retrieved, deleted via `DeleteAsync`, logged, and returns `Unit.Value`. |
| **DeleteLeaveAllocationCommand** | `Handle_NonExistingId_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, an error is logged and `NotFoundException` is thrown without calling `DeleteAsync`. |

---

## 3. LeaveRequest Feature

| Command/Query name | Unit Test name | Description |
| :--- | :--- | :--- |
| **GetLeaveRequestsQuery** | `Handle_ValidRequest_ReturnsLeaveRequestDtoList` | Verifies that `GetLeaveRequestsWithDetails` is called, mapped to `List<LeaveRequestDto>`, logged, and returned. |
| **GetLeaveRequestsQuery** | `Handle_WhenNoRequestsExist_ReturnsEmptyList` | Verifies that an empty `List<LeaveRequestDto>` is returned when the repository has no leave requests. |
| **GetLeaveRequestDetailsQuery** | `Handle_ExistingId_ReturnsLeaveRequestDetailsDto` | Verifies that `GetByIdAsync` retrieves the leave request for the given `Id`, maps it to `LeaveRequestDetailsDto`, logs info, and returns it. |
| **GetLeaveRequestDetailsQuery** | `Handle_NonExistingId_ReturnsNull` | Verifies that requesting details for a non-existent `Id` returns `null` when `GetByIdAsync` returns `null`. |
| **CreateLeaveRequestCommand** | `Handle_ValidCommand_CreatesRequestAndSendsEmail` | Verifies that a valid command sets `DateRequested`, calls `CreateAsync`, sends a confirmation email via `IEmailSender`, and returns `Unit.Value`. |
| **CreateLeaveRequestCommand** | `Handle_StartDateAfterOrEqualEndDate_ThrowsBadRequestException` | Verifies that `StartDate >= EndDate` fails `BaseLeaveRequestValidator` validation, logs an error, and throws `BadRequestException`. |
| **CreateLeaveRequestCommand** | `Handle_InvalidOrNonExistingLeaveTypeId_ThrowsBadRequestException` | Verifies that `LeaveTypeId <= 0` or `IsLeaveTypeExist` returning `false` fails validation and throws `BadRequestException`. |
| **CreateLeaveRequestCommand** | `Handle_WhenEmailSenderThrowsException_StillReturnsUnit` | Verifies that if `SendEmailAsync` throws an exception after creating the leave request, the exception is caught and `Unit.Value` is still returned. |
| **UpdateLeaveRequestCommand** | `Handle_ValidCommand_UpdatesRequestAndSendsEmail` | Verifies that an existing leave request with valid data is mapped, updated via `UpdateAsync`, sends a confirmation email, and returns `Unit.Value`. |
| **UpdateLeaveRequestCommand** | `Handle_LeaveRequestNotFound_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, an error is logged and `NotFoundException` is thrown. |
| **UpdateLeaveRequestCommand** | `Handle_StartDateAfterOrEqualEndDate_ThrowsBadRequestException` | Verifies that invalid date ranges (`StartDate >= EndDate`) fail validation, log an error, and throw `BadRequestException`. |
| **UpdateLeaveRequestCommand** | `Handle_InvalidOrNonExistingLeaveTypeId_ThrowsBadRequestException` | Verifies that `LeaveTypeId <= 0` or a non-existing leave type fails validation and throws `BadRequestException`. |
| **UpdateLeaveRequestCommand** | `Handle_WhenEmailSenderThrowsException_LogsWarningAndReturnsUnit` | Verifies that if `SendEmailAsync` throws an exception during update, a warning is logged via `_logger.LogWarning` and `Unit.Value` is returned. |
| **DeleteLeaveRequestCommand** | `Handle_ExistingId_DeletesRequestAndReturnsUnit` | Verifies that an existing leave request is deleted via `DeleteAsync`, logs an info message, and returns `Unit.Value`. |
| **DeleteLeaveRequestCommand** | `Handle_NonExistingId_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, an error is logged and `NotFoundException` is thrown without calling `DeleteAsync`. |
| **CancelLeaveRequestCommand** | `Handle_ExistingId_SetsCancelledTrueAndSendsEmail` | Verifies that an existing leave request has `Cancelled` set to `true`, sends a cancellation email via `IEmailSender`, and returns `Unit.Value`. |
| **CancelLeaveRequestCommand** | `Handle_NonExistingId_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, an info message is logged and `NotFoundException` is thrown. |
| **CancelLeaveRequestCommand** | `Handle_WhenEmailSenderThrowsException_LogsErrorAndReturnsUnit` | Verifies that if `SendEmailAsync` fails, the exception is caught, `_logger.LogError` is called, and `Unit.Value` is returned. |
| **ChangeLeaveRequestApprovalCommand** | `Handle_ValidCommand_UpdatesApprovalStatusAndSendsEmail` | Verifies that an existing leave request's `Approved` status is updated, `UpdateAsync` is called, an email is sent, and `Unit.Value` is returned. |
| **ChangeLeaveRequestApprovalCommand** | `Handle_NonExistingId_ThrowsNotFoundException` | Verifies that when `GetByIdAsync` returns `null`, an error is logged and `NotFoundException` is thrown. |
| **ChangeLeaveRequestApprovalCommand** | `Handle_WhenEmailSenderThrowsException_LogsErrorAndReturnsUnit` | Verifies that if `SendEmailAsync` throws an exception, `_logger.LogError` is called and `Unit.Value` is still returned. |
| **ChangeLeaveRequestApprovalCommand** | `Validate_NullApprovalStatus_FailsValidation` | Verifies `ChangeLeaveRequestApprovalCommandValidator` ensures `Approved` cannot be `null`. |

