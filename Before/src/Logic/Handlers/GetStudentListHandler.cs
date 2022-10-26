using System.Collections.Generic;
using System.Linq;

using CSharpFunctionalExtensions;

using Logic.Dtos;
using Logic.Queries;
using Logic.Students;
using Logic.Utils;

namespace Logic.Handlers
{
    public sealed class GetStudentListHandler : IQueryHandler<GetStudentListQuery, Result<List<StudentDto>>>
    {
        private readonly SessionFactory _sessionFactory;

        public GetStudentListHandler(SessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Result<List<StudentDto>> Handle(GetStudentListQuery query)
        {
            var unitOfWork = new UnitOfWork(_sessionFactory);

            var studentsDto = new StudentRepository(unitOfWork)
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
