using System;

namespace Campus_Companion.Models
{
    public class NoticeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
        public string TargetAudience { get; set; }
    }
}
