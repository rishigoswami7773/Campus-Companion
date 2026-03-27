namespace Campus_Companion.Models
{
    public class DashboardViewModel
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double CompletedPercentage { get; set; }
        public int UrgentTaskCount { get; set; }
        public int PendingHighCount { get; set; }
        public int PendingMediumCount { get; set; }
        public int PendingLowCount { get; set; }
        public int DueTomorrowCount { get; set; }
        public int UpcomingEvents { get; set; }
        public int TotalNotices { get; set; }
        public IEnumerable<string> SmartSuggestions { get; set; } = [];
        public IEnumerable<TaskItem> PriorityTasks { get; set; } = [];
        public IEnumerable<TaskItem> UpcomingDeadlines { get; set; } = [];
        public IEnumerable<ScheduleItem> TodaySchedule { get; set; } = [];
        public IEnumerable<ScheduleItem> UpcomingSchedule { get; set; } = [];
        public IEnumerable<NoticeItem> LatestNotices { get; set; } = [];
    }
}
