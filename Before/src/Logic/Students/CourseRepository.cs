using System.Linq;

using Logic.Utils;

namespace Logic.Students
{
    public sealed class CourseRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public CourseRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Course GetByName(string name)
        {
            return _unitOfWork.Query<Course>()
                .SingleOrDefault(x => x.Name == name);
        }
    }
}
