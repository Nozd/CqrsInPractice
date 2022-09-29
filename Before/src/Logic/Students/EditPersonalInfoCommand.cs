using CSharpFunctionalExtensions;
using Logic.Utils;

namespace Logic.Students
{
    public interface ICommand
    {

    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
    
    public sealed class EditPersonalInfoCommand : ICommand
    {
        public long StudentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public sealed class EditPersonalInfoHandler : ICommandHandler<EditPersonalInfoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public EditPersonalInfoHandler(UnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public Result Handle(EditPersonalInfoCommand command)
        {
            var studentRepository = new StudentRepository(_unitOfWork);

            Student student = studentRepository.GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found for Id {command.StudentId}");

            student.Name = command.Name;
            student.Email = command.Email;

            _unitOfWork.Commit();
            return Result.Success();
        }
    }
}
