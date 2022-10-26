using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class EnrollHandler : ICommandHandler<EnrollCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public EnrollHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(EnrollCommand command)
        {
            if (!Enum.TryParse(command.Grade, out Grade grade))
                return Result.Failure($"Grade is incorrect: '{command.Grade}'");

            var unitOfWork = new UnitOfWork(_sessionFactory);

            Student student = new StudentRepository(unitOfWork).GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found with Id '{command.StudentId}'");

            Course course = new CourseRepository(unitOfWork).GetByName(command.Course);
            if (course == null)
                return Result.Failure($"Course is incorrect: '{command.Course}'");

            student.Enroll(course, grade);

            unitOfWork.Commit();

            return Result.Success();
        }
    }
}
