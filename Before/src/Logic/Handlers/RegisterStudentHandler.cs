﻿using System;

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
        private readonly UnitOfWork _unitOfWork;

        public RegisterStudentHandler(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Handle(RegisterStudentCommand command)
        {
            var courseRepository = new CourseRepository(_unitOfWork);
            var studentRepository = new StudentRepository(_unitOfWork);

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

            _unitOfWork.Commit();

            return Result.Success();
        }
    }
}