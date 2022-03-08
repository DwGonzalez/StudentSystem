namespace BackEnd.Models.DTO
{
    public class SubjectStudentDTO
    {
        public SubjectStudentDTO(SubjectDTO subject, StudentDTO student)
        {
            Subject = subject;
            Student = student;
            //ProfessorName = professorName;
        }

        public SubjectDTO Subject { get; set; }
        public StudentDTO Student { get; set; }
        //public string ProfessorName { get; set; }

    }
}
