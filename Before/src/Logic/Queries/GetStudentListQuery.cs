using System.Collections.Generic;

using CSharpFunctionalExtensions;

using Logic.Dtos;

namespace Logic.Queries
{
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
}
