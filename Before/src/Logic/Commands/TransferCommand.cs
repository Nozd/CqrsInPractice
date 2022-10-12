namespace Logic.Commands
{
    public sealed class TransferCommand : ICommand
    {
        public long StudentId { get; }
        public int EnrollmentNumber { get; }
        public string Course { get; }
        public string Grade { get; }

        public TransferCommand(long studentId, int enrollmentNumber, string course, string grade)
        {
            StudentId = studentId;
            EnrollmentNumber = enrollmentNumber;
            Course = course;
            Grade = grade;
        }
    }
}
