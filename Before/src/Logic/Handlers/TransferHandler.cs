using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class TransferHandler : ICommandHandler<TransferCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public TransferHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(TransferCommand command)
        {
            if (!Enum.TryParse(command.Grade, out Grade grade))
                return Result.Failure($"Grade is incorrect: '{command.Grade}'");

            var student = new StudentRepository(_unitOfWork).GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found with Id '{command.StudentId}'");

            var course = new CourseRepository(_unitOfWork).GetByName(command.Course);
            if (course == null)
                return Result.Failure($"Course is incorrect: '{command.Course}'");

            var enrollment = student.GetEnrollment(command.EnrollmentNumber - 1);
            if (enrollment == null)
                return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

            enrollment.Update(course, grade);

            _unitOfWork.Commit();

            return Result.Success();
        }
    }
}
