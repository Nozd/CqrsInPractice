namespace Logic.Commands
{
    public sealed class DisenrollCommand : ICommand
    {
        public long StudentId { get; }
        public int EnrollmentNumber { get; }
        public string Comment { get; }

        public DisenrollCommand(long studentId, int enrollmentNumber, string comment)
        {
            Comment = comment;
            StudentId = studentId;
            EnrollmentNumber = enrollmentNumber;
        }
    }
}
