
using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class DisenrollHandler : ICommandHandler<DisenrollCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public DisenrollHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(DisenrollCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Comment))
                return Result.Failure("Disenrollment comment is required");

            var unitOfWork = new UnitOfWork(_sessionFactory);

            var studentRepository = new StudentRepository(unitOfWork);
            Student student = studentRepository.GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found with Id '{command.StudentId}'");

            var enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

            student.RemoveEnrollment(enrollment, command.Comment);

            unitOfWork.Commit();

            return Result.Success();
        }
    }
}
