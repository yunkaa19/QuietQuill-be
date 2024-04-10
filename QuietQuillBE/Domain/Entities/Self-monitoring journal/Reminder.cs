namespace Domain.Entities;

public class Reminder
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ReminderTime { get; private set; }
    public string Message { get; private set; }
    public bool IsRecurring { get; private set; }
    // Recurrence pattern (daily, weekly, etc.) could be added

    private Reminder() { }

    public Reminder(Guid userId, DateTime reminderTime, string message, bool isRecurring)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ReminderTime = reminderTime;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        IsRecurring = isRecurring;
    }
}