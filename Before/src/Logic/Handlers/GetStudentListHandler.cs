using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using CSharpFunctionalExtensions;

using Dapper;

using Logic.Dtos;
using Logic.Options;
using Logic.Queries;
using Logic.Students;

using Microsoft.Extensions.Options;

namespace Logic.Handlers
{
    public sealed class GetStudentListHandler : IQueryHandler<GetStudentListQuery, Result<List<StudentDto>>>
    {
        private readonly DbOptions _dbOptions;

        public GetStudentListHandler(IOptions<DbOptions> dbOptions)
        {
            _dbOptions = dbOptions.Value;
        }

        public Result<List<StudentDto>> Handle(GetStudentListQuery query)
        {
            var sql = @"
                select s.*, e.Grade, c.Name as CourseName, c.Credits
                from dbo.Student s
                left join (
                    select e.StudentID, count(*) as Number
                    from dbo.Enrollment as e
                    group by e.StudentID) as t on t.StudentID=s.StudentID
                left join dbo.Enrollment as e on e.StudentID=s.StudentID
                left join dbo.Course as c on c.CourseID=e.CourseID
                where (c.Name=@Course or @Course is null)
                    and (isnull(t.Number, 0) = @Number or @Number is null)
                order by s.StudentID";

            using (var connection = new SqlConnection(_dbOptions.ConnectionString))
            {
                var students = connection
                    .Query<StudentInDB>(sql, new { Course = query.Enrolled, Number = query.Number })
                    .ToList();

                var ids = students
                    .GroupBy(s => s.StudentId)
                    .Select(s => s.Key)
                    .ToList();

                var result = new List<StudentDto>();
                foreach (var id in ids)
                {
                    var studentData = students
                        .Where(s => s.StudentId == id)
                        .ToList();
                    var studentDto = new StudentDto
                    {
                        Id = studentData[0].StudentId,
                        Name = studentData[0].Name,
                        Email = studentData[0].Email,
                        Course1 = studentData[0].CourseName,
                        Course1Credits = studentData[0].Credits,
                        Course1Grade = studentData[0].Grade?.ToString()
                    };
                    if (studentData.Count > 1)
                    {
                        studentDto.Course2 = studentData[1].CourseName;
                        studentDto.Course2Credits = studentData[1].Credits;
                        studentDto.Course2Grade = studentData[1].Grade?.ToString();
                    }

                    result.Add(studentDto);
                }

                return result;
            }
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

        /// <summary>
        /// Модель данных студента, получаемая из БД
        /// </summary>
        private class StudentInDB
        {
            public readonly long StudentId;
            public readonly string Name;
            public readonly string Email;
            public readonly Grade? Grade;
            public readonly string CourseName;
            public readonly int? Credits;

            public StudentInDB(long studentId, string name, string email, Grade? grade, string courseName, int? credits)
            {
                StudentId = studentId;
                Name = name;
                Email = email;
                Grade = grade;
                CourseName = courseName;
                Credits = credits;
            }
        }
    }
}
