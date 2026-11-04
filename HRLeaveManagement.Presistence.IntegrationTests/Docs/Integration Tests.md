# Persistence Integration Tests Documentation

This document outlines the integration tests for `HRLeaveManagement.Presistence` in `HRLeaveManagement.Presistence.IntegrationTests`.

---

## 1. HRDatabaseContext Tests

| Component | Integration Test name | Description |
| :--- | :--- | :--- |
| **HRDatabaseContext** | `Save_SetDateCreatedValue` | Verifies that adding a new entity and calling `SaveChangesAsync` automatically sets `DateCreated`. |
| **HRDatabaseContext** | `Save_SetDateModifiedValue` | Verifies that adding a new entity and calling `SaveChangesAsync` automatically sets `DateModified`. |
| **HRDatabaseContext** | `Update_UpdatesDateModifiedAndPreservesDateCreated` | Verifies that modifying an existing entity and calling `SaveChangesAsync` updates `DateModified` while keeping the original `DateCreated` unchanged. |
| **HRDatabaseContext** | `EnsureCreated_SeedsDefaultVacationLeaveType` | Verifies that `LeaveTypeConfiguration` seeds the default `"Vacation"` `LeaveType` (`Id = 1`, `DefaultDays = 10`) when the database is initialized. |

---

## 2. GenericRepository Tests

| Method | Integration Test name | Description |
| :--- | :--- | :--- |
| **CreateAsync** | `CreateAsync_ValidEntity_PersistsToDatabase` | Verifies that `CreateAsync` adds the entity to the context, saves changes, and populates audit fields. |
| **GetAllAsync** | `GetAllAsync_WhenEntitiesExist_ReturnsAllEntities` | Verifies that `GetAllAsync` retrieves all persisted entities from the database. |
| **GetAllAsync** | `GetAllAsync_WhenNoEntitiesExist_ReturnsEmptyList` | Verifies that `GetAllAsync` returns an empty list when the table is empty. |
| **GetByIdAsync** | `GetByIdAsync_ExistingId_ReturnsEntity` | Verifies that `GetByIdAsync` returns the matching entity for an existing `Id`. |
| **GetByIdAsync** | `GetByIdAsync_NonExistingId_ThrowsException` | Verifies that `GetByIdAsync` throws an `Exception` when no entity with the specified `Id` exists. |
| **UpdateAsync** | `UpdateAsync_ExistingEntity_UpdatesEntityInDatabase` | Verifies that `UpdateAsync` marks the entity as modified and saves updated values to the database. |
| **DeleteAsync** | `DeleteAsync_ExistingEntity_RemovesEntityFromDatabase` | Verifies that `DeleteAsync` deletes the entity from the database and saves changes. |

---

## 3. LeaveTypeRepository Tests

| Method | Integration Test name | Description |
| :--- | :--- | :--- |
| **IsLeaveTypeExist** | `IsLeaveTypeExist_ExistingId_ReturnsTrue` | Verifies that `IsLeaveTypeExist` returns `true` when a `LeaveType` with the given `Id` exists in the database. |
| **IsLeaveTypeExist** | `IsLeaveTypeExist_NonExistingId_ReturnsFalse` | Verifies that `IsLeaveTypeExist` returns `false` when no `LeaveType` with the given `Id` exists. |
| **IsLeaveTypeUnique** | `IsLeaveTypeUnique_UniqueName_ReturnsTrue` | Verifies that `IsLeaveTypeUnique` returns `true` when no `LeaveType` in the database has the specified `Name`. |
| **IsLeaveTypeUnique** | `IsLeaveTypeUnique_DuplicateName_ReturnsFalse` | Verifies that `IsLeaveTypeUnique` returns `false` when a `LeaveType` with the specified `Name` already exists. |

---

## 4. LeaveAllocationRepository Tests

| Method | Integration Test name | Description |
| :--- | :--- | :--- |
| **GetLeaveAllocationsWithDetails()** | `GetLeaveAllocationsWithDetails_ReturnsAllocationsWithLeaveType` | Verifies that all leave allocations are returned with their `LeaveType` navigation property included. |
| **GetLeaveAllocationsWithDetails(userId)** | `GetLeaveAllocationsWithDetailsByUserId_ReturnsFilteredAllocationsWithLeaveType` | Verifies that only allocations matching the specified `EmployeeId` are returned with `LeaveType` included. |
| **GetLeaveAllocationWithDetails(id)** | `GetLeaveAllocationWithDetails_ExistingId_ReturnsAllocationWithLeaveType` | Verifies that a single allocation is returned with its `LeaveType` navigation property for an existing `Id`. |
| **GetLeaveAllocationWithDetails(id)** | `GetLeaveAllocationWithDetails_NonExistingId_ThrowsNotFoundException` | Verifies that `NotFoundException` is thrown when no allocation exists for the specified `Id`. |
| **AllocationExists** | `AllocationExists_WhenMatchingAllocationExists_ReturnsTrue` | Verifies that `AllocationExists` returns `true` when an allocation matches `userId`, `leaveTypeId`, and `period`. |
| **AllocationExists** | `AllocationExists_WhenNoMatchingAllocation_ReturnsFalse` | Verifies that `AllocationExists` returns `false` when no allocation matches the given criteria. |
| **GetUserAllocation** | `GetUserAllocation_ExistingUserAndLeaveType_ReturnsAllocation` | Verifies that `GetUserAllocation` returns the allocation matching `userId` and `leaveTypeId`. |
| **GetUserAllocation** | `GetUserAllocation_NonExistingUserOrLeaveType_ThrowsNotFoundException` | Verifies that `GetUserAllocation` throws `NotFoundException` when no matching user allocation is found. |
| **IsLeaveAllocationExist** | `IsLeaveAllocationExist_ExistingId_ReturnsTrue` | Verifies that `IsLeaveAllocationExist` returns `true` when an allocation with the given `Id` exists. |
| **IsLeaveAllocationExist** | `IsLeaveAllocationExist_NonExistingId_ReturnsFalse` | Verifies that `IsLeaveAllocationExist` returns `false` when no allocation with the given `Id` exists. |
| **AddAllocations** | `AddAllocations_ValidList_PersistsMultipleAllocations` | Verifies that `AddAllocations` adds a list of allocations and persists them to the database. |

---

## 5. LeaveRequestRepository Tests

| Method | Integration Test name | Description |
| :--- | :--- | :--- |
| **GetLeaveRequestsWithDetails()** | `GetLeaveRequestsWithDetails_ReturnsAllRequestsWithLeaveType` | Verifies that all leave requests are retrieved with their `LeaveType` navigation property included. |
| **GetLeaveRequestsWithDetails(userId)** | `GetLeaveRequestsWithDetailsByUserId_ReturnsUserRequestsWithLeaveType` | Verifies that only leave requests matching `RequestingEmployeeId == userId` are returned with `LeaveType` included. |
| **GetLeaveRequestWithDetails(id)** | `GetLeaveRequestWithDetails_ExistingId_ReturnsRequestWithLeaveType` | Verifies that a single leave request is retrieved with its `LeaveType` navigation property for an existing `Id`. |
| **GetLeaveRequestWithDetails(id)** | `GetLeaveRequestWithDetails_NonExistingId_ThrowsException` | Verifies that `GetLeaveRequestWithDetails` throws an `Exception` when no leave request with the given `Id` exists. |

