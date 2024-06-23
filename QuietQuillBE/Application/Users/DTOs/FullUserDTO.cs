namespace Application.Users.DTOs;

public class FullUserDTO
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<JournalEntryDto> JournalEntries { get; set; }
    public List<ReminderDto> Reminders { get; set; }
    public List<HabitDto> Habits { get; set; }
    public List<MeditationSessionDto> MeditationSessions { get; set; }
    public List<UserQuizRecordDto> UserQuizRecords { get; set; }
}

public class UserQuizRecordDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public string CompletedOn { get; set; } // Assuming string for datetime formatting, can be DateTime if supported
    public int Score { get; set; }
}

public class MeditationSessionDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Duration { get; set; } // Assuming string for timespan formatting, can be TimeSpan if supported
    public string Description { get; set; }
    public string Type { get; set; } // Assuming MeditationType is an enum or class with a proper ToString implementation
}


public class HabitDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartDate { get; set; } // Assuming string for date formatting, can be DateTime if supported
    public int TargetFrequency { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public bool IsActive { get; set; }
}

public class ReminderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ReminderTime { get; set; } // Assuming string for datetime formatting, can be DateTime if supported
    public string Message { get; set; }
    public bool IsRecurring { get; set; }
}

public class JournalEntryDto
{
    public string Id { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public string EntryDate { get; set; } // Assuming string for date formatting, can be DateOnly if supported
    public string Mood { get; set; } // Assuming Mood is an enum or class with a proper ToString implementation
    public string Tags { get; set; }
}
