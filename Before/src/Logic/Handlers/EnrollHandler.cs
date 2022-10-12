using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class EnrollHandler : ICommandHandler<EnrollCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public EnrollHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(EnrollCommand command)
        {
            if (!Enum.TryParse(command.Grade, out Grade grade))
                return Result.Failure($"Grade is incorrect: '{command.Grade}'");

            Student student = new StudentRepository(_unitOfWork).GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found with Id '{command.StudentId}'");

            Course course = new CourseRepository(_unitOfWork).GetByName(command.Course);
            if (course == null)
                return Result.Failure($"Course is incorrect: '{command.Course}'");

            student.Enroll(course, grade);

            _unitOfWork.Commit();

            return Result.Success();
        }
    }
}
