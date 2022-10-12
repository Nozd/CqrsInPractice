namespace Logic.Commands
{
    public sealed class EditPersonalInfoCommand : ICommand
    {
        public long StudentId { get; }
        public string Name { get; }
        public string Email { get; }

        public EditPersonalInfoCommand(long studentId, string name, string email)
        {
            StudentId = studentId;
            Name = name;
            Email = email;
        }
    }
}
