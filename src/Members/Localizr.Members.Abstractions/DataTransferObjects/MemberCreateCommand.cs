﻿using Localizr.Core.Cqrs;

namespace Localizr.Members.Abstractions.DataTransferObjects;

public record MemberCreateCommand(string? SubjectId, string DisplayName, string EmailAddress, string? ProfilePicture) : CommandBase;