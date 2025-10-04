namespace StudentScorecard.Models
{
    public class StudentViewModels
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Class { get; set; }
        

        public List<int> SelectedSubjects { get; set; } = new List<int>();
        public List<Subject> AvailableSubjects { get; set; } = new List<Subject>();
    }

    public class MarksItem
    {
        public int StudentSubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public int? Marks { get; set; }
    }

    public class MarksEntryViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public List<MarksItem> Subjects { get; set; } = new List<MarksItem>();
    }
}
