using Logic.Utils;

namespace Logic.Students
{
    public sealed class StudentRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public StudentRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Student GetById(long id)
        {
            return _unitOfWork.Get<Student>(id);
        }

        public void Save(Student student)
        {
            _unitOfWork.SaveOrUpdate(student);
        }

        public void Delete(Student student)
        {
            _unitOfWork.Delete(student);
        }
    }
}
