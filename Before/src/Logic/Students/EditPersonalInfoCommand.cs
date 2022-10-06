using CSharpFunctionalExtensions;
using Logic.Dtos;
using Logic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Students
{
    public interface ICommand
    {

    }

    public interface IQuery<TResult>
    {

    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }

    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
    
    public sealed class EditPersonalInfoCommand : ICommand
    {
        public long StudentId { get; }
        public string Name { get; }
        public string Email { get; }

        public EditPersonalInfoCommand(long studentId, string name, string email)
        {
            StudentId = studentId;
            Name = name;
            Email = email;
        }
    }

    public sealed class GetStudentListQuery : IQuery<Result<List<StudentDto>>>
    {
        public string Enrolled { get; }

        public int? Number { get; }

        public GetStudentListQuery(string enrolled, int? number)
        {
            Enrolled = enrolled;
            Number = number;
        }
    }

    public sealed class EditPersonalInfoHandler : ICommandHandler<EditPersonalInfoCommand>
    {
        private readonly UnitOfWork _unitOfWork;

        public EditPersonalInfoHandler(UnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public Result Handle(EditPersonalInfoCommand command)
        {
            var studentRepository = new StudentRepository(_unitOfWork);

            Student student = studentRepository.GetById(command.StudentId);
            if (student == null)
                return Result.Failure($"No student found for Id {command.StudentId}");

            student.Name = command.Name;
            student.Email = command.Email;

            _unitOfWork.Commit();
            return Result.Success();
        }
    }

    public sealed class GetStudentListHandler : IQueryHandler<GetStudentListQuery, Result<List<StudentDto>>>
    {
        private readonly UnitOfWork _unitOfWork;

        public GetStudentListHandler(UnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public Result<List<StudentDto>> Handle(GetStudentListQuery query)
        {
            var studentsDto = new StudentRepository(_unitOfWork)
                .GetList(query.Enrolled, query.Number)
                .Select(x => ConvertToDto(x))
                .ToList();

            return Result.Success(studentsDto);
        }

        private StudentDto ConvertToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Course1 = student.FirstEnrollment?.Course?.Name,
                Course1Grade = student.FirstEnrollment?.Grade.ToString(),
                Course1Credits = student.FirstEnrollment?.Course?.Credits,
                Course2 = student.SecondEnrollment?.Course?.Name,
                Course2Grade = student.SecondEnrollment?.Grade.ToString(),
                Course2Credits = student.SecondEnrollment?.Course?.Credits,
            };
        }
    }
}
