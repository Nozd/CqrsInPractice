namespace Logic.Commands
{
    public sealed class UnregisterStudentCommand : ICommand
    {
        public long Id { get; }

        public UnregisterStudentCommand(long id)
        {
            Id = id;
        }
    }
}
