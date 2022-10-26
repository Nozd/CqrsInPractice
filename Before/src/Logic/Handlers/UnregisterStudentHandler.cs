using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class UnregisterStudentHandler : ICommandHandler<UnregisterStudentCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public UnregisterStudentHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(UnregisterStudentCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);

            var studentRepository = new StudentRepository(unitOfWork);

            var student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            studentRepository.Delete(student);
            unitOfWork.Commit();

            return Result.Success();
        }
    }
}
