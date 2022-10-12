using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class UnregisterStudentHandler : ICommandHandler<UnregisterStudentCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public UnregisterStudentHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(UnregisterStudentCommand command)
        {
            var studentRepository = new StudentRepository(_unitOfWork);

            var student = studentRepository.GetById(command.Id);
            if (student == null)
                return Result.Failure($"No student found for Id {command.Id}");

            studentRepository.Delete(student);
            _unitOfWork.Commit();

            return Result.Success();
        }
    }
}
