
using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class DisenrollHandler : ICommandHandler<DisenrollCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public DisenrollHandler(UnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public Result Handle(DisenrollCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Comment))
                return Result.Failure("Disenrollment comment is required");

            var studentRepository = new StudentRepository(_unitOfWork);
            Student student = studentRepository.GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found with Id '{command.StudentId}'");

            var enrollment = student.GetEnrollment(command.EnrollmentNumber);
            if (enrollment == null)
                return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

            student.RemoveEnrollment(enrollment, command.Comment);

            _unitOfWork.Commit();

            return Result.Success();
        }
    }
}
