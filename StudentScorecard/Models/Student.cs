using System;
using System.Collections.Generic;

namespace StudentScorecard.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Class { get; set; }

    public int? NumberofSubjects { get; set; }

    public double? Percentage { get; set; }

    public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
}
