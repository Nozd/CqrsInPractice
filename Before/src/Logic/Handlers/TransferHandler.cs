using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class TransferHandler : ICommandHandler<TransferCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public TransferHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(TransferCommand command)
        {
            if (!Enum.TryParse(command.Grade, out Grade grade))
                return Result.Failure($"Grade is incorrect: '{command.Grade}'");

            var unitOfWork = new UnitOfWork(_sessionFactory);

            var student = new StudentRepository(unitOfWork).GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found with Id '{command.StudentId}'");

            var course = new CourseRepository(unitOfWork).GetByName(command.Course);
            if (course == null)
                return Result.Failure($"Course is incorrect: '{command.Course}'");

            var enrollment = student.GetEnrollment(command.EnrollmentNumber - 1);
            if (enrollment == null)
                return Result.Failure($"No enrollment found with number '{command.EnrollmentNumber}'");

            enrollment.Update(course, grade);

            unitOfWork.Commit();

            return Result.Success();
        }
    }
}
