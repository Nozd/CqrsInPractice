using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Decorators;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    [AuditLog]
    public sealed class RegisterStudentHandler : ICommandHandler<RegisterStudentCommand>
    {
        private readonly SessionFactory _sessionFactory;

        public RegisterStudentHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result Handle(RegisterStudentCommand command)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);

            var courseRepository = new CourseRepository(unitOfWork);
            var studentRepository = new StudentRepository(unitOfWork);

            var student = new Student(command.Name, command.Email);

            if (command.Course1 != null && command.Course1Grade != null)
            {
                Course course = courseRepository.GetByName(command.Course1);
                student.Enroll(course, Enum.Parse<Grade>(command.Course1Grade));
            }

            if (command.Course2 != null && command.Course2Grade != null)
            {
                Course course = courseRepository.GetByName(command.Course2);
                student.Enroll(course, Enum.Parse<Grade>(command.Course2Grade));
            }

            studentRepository.Save(student);

            unitOfWork.Commit();

            return Result.Success();
        }
    }
}
