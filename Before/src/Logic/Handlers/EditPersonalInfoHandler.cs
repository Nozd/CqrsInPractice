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
        private readonly SessionFactory _sessionFactory;

        public EditPersonalInfoHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(EditPersonalInfoCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);

            var studentRepository = new StudentRepository(unitOfWork);

            Student student = studentRepository.GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found for Id {command.StudentId}");

            student.Name = command.Name;
            student.Email = command.Email;

            unitOfWork.Commit();
            return Result.Success();
        }
    }
}
