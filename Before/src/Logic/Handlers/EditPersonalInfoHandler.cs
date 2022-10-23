using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    [AuditLog]
    [DatabaseRetry]
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
