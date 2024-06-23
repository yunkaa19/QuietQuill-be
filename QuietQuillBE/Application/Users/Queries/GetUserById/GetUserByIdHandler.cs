using Application.Abstraction.Data;
using Application.Abstractions.Messaging;
using Domain.Exceptions.Users;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Application.Users.DTOs;

[assembly: InternalsVisibleTo("ArchitectureTests")]

namespace Application.Users.Queries.GetUserById
{
    internal sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, FullUserDTO>
    {
        private readonly IDbQueryExecutor _dbQueryExecutor;

        public GetUserByIdHandler(IDbQueryExecutor dbQueryExecutor)
        {
            _dbQueryExecutor = dbQueryExecutor;
        }

        public async Task<FullUserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            // Fetch user details
            const string userSql = @"SELECT UserId, Username, Email FROM `Users` WHERE `UserId` = @UserId";
            var user = await _dbQueryExecutor.QueryFirstOrDefaultAsync<FullUserDTO>(userSql, new { UserId = request.UserId });

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            // Fetch journal entries
            const string journalSql = @"SELECT Id, UserId, Content, EntryDate, Mood, Tags FROM `JournalEntries` WHERE `UserId` = @UserId";
            var journalEntries = await _dbQueryExecutor.QueryAsync<JournalEntryDto>(journalSql, new { UserId = request.UserId });
            user.JournalEntries = journalEntries.ToList();

            // Fetch reminders
            const string reminderSql = @"SELECT Id, UserId, ReminderTime, Message, IsRecurring FROM `Reminders` WHERE `UserId` = @UserId";
            var reminders = await _dbQueryExecutor.QueryAsync<ReminderDto>(reminderSql, new { UserId = request.UserId });
            user.Reminders = reminders.ToList();

            // Fetch habits
            const string habitsSql = @"SELECT Id, UserId, Name, Description, StartDate, TargetFrequency, CurrentStreak, LongestStreak, IsActive FROM `Habits` WHERE `UserId` = @UserId";
            var habits = await _dbQueryExecutor.QueryAsync<HabitDto>(habitsSql, new { UserId = request.UserId });
            user.Habits = habits.ToList();

            // Fetch meditation sessions
            const string meditationSql = @"SELECT Id, Title, Duration, Description, Type FROM `MeditationSessions` WHERE `UserId` = @UserId";
            var meditationSessions = await _dbQueryExecutor.QueryAsync<MeditationSessionDto>(meditationSql, new { UserId = request.UserId });
            user.MeditationSessions = meditationSessions.ToList();

            // Fetch quiz records
            const string quizRecordSql = @"SELECT Id, UserId, QuizId, CompletedOn, Score FROM `UserQuizRecords` WHERE `UserId` = @UserId";
            var quizRecords = await _dbQueryExecutor.QueryAsync<UserQuizRecordDto>(quizRecordSql, new { UserId = request.UserId });
            user.UserQuizRecords = quizRecords.ToList();

            return user;
        }
    }
}
