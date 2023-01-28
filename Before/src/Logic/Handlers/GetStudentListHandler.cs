using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using CSharpFunctionalExtensions;

using Dapper;

using Logic.Dtos;
using Logic.Options;
using Logic.Queries;

using Microsoft.Extensions.Options;

namespace Logic.Handlers
{
    public sealed class GetStudentListHandler : IQueryHandler<GetStudentListQuery, Result<List<StudentDto>>>
    {
        private readonly QueryDbOptions _dbOptions;

        public GetStudentListHandler(IOptions<QueryDbOptions> dbOptions)
        {
            _dbOptions = dbOptions.Value;
        }

        public Result<List<StudentDto>> Handle(GetStudentListQuery query)
        {
            string sql = @"
                    SELECT s.StudentID Id, s.Name, s.Email,
	                    s.FirstCourseName Course1, s.FirstCourseCredits Course1Credits, s.FirstCourseGrade Course1Grade,
	                    s.SecondCourseName Course2, s.SecondCourseCredits Course2Credits, s.SecondCourseGrade Course2Grade
                    FROM dbo.Student s
                    WHERE (s.FirstCourseName = @Course
		                    OR s.SecondCourseName = @Course
		                    OR @Course IS NULL)
                        AND (s.NumberOfEnrollments = @Number
                            OR @Number IS NULL)
                    ORDER BY s.StudentID ASC";

            using (SqlConnection connection = new SqlConnection(_dbOptions.ConnectionString))
            {
                List<StudentDto> students = connection
                    .Query<StudentDto>(sql, new
                    {
                        Course = query.Enrolled,
                        Number = query.Number
                    })
                    .ToList();

                return students;
            }
        }
    }
}
