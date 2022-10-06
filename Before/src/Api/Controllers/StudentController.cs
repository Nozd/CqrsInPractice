using Api.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Messages _messages;
        private readonly UnitOfWork _unitOfWork;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentController(UnitOfWork unitOfWork, Messages messages)
        {
            _messages = messages;
            _unitOfWork = unitOfWork;
            _studentRepository = new StudentRepository(unitOfWork);
            _courseRepository = new CourseRepository(unitOfWork);
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            IReadOnlyList<Student> students = _studentRepository.GetList(enrolled, number);
            List<StudentDto> dtos = students.Select(x => ConvertToDto(x)).ToList();
            _unitOfWork.Commit();
            return Ok(dtos);
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
        /// Регистрирует студента
        /// </summary>
        [HttpPost]
        public IActionResult RegisterStudent([FromBody] StudentRegisterDto dto)
        {
            var student = new Student(dto.Name, dto.Email);

            if (dto.Course1 != null && dto.Course1Grade != null)
            {
                Course course = _courseRepository.GetByName(dto.Course1);
                student.Enroll(course, Enum.Parse<Grade>(dto.Course1Grade));
            }

            if (dto.Course2 != null && dto.Course2Grade != null)
            {
                Course course = _courseRepository.GetByName(dto.Course2);
                student.Enroll(course, Enum.Parse<Grade>(dto.Course2Grade));
            }

            _studentRepository.Save(student);
            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// Отменяет регистрацию студента
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult UnregisterStudent(long id)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found for Id {id}");

            _studentRepository.Delete(student);
            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// Отменяет курс
        /// </summary>
        [HttpPost("{id:long}/enrollments/{enrollmentNumber:int}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Comment))
                return Error("Disenrollment comment is required");

            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found with Id '{id}'");

            var enrollment = student.GetEnrollment(enrollmentNumber);
            if (enrollment == null)
                return Error($"No enrollment found with number '{enrollmentNumber}'");
            
            student.RemoveEnrollment(enrollment, dto.Comment);

            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// Регистрирует студента на переданный курс
        /// </summary>
        [HttpPost("{id:long}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentTransferDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found with Id '{id}'");

            Course course = _courseRepository.GetByName(dto.Course);
            if (course == null)
                return Error($"Course is incorrect: '{dto.Course}'");

            bool success = Enum.TryParse(dto.Grade, out Grade grade);
            if (!success)
                return Error($"Grade is incorrect: '{dto.Grade}'");

            student.Enroll(course, grade);

            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// Переносит курс студента
        /// </summary>
        [HttpPut("{id:long}/enrollments/{enrollmentNumber:int}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentEnrollDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error($"No student found with Id '{id}'");

            Course course = _courseRepository.GetByName(dto.Course);
            if (course == null)
                return Error($"Course is incorrect: '{dto.Course}'");

            bool success = Enum.TryParse(dto.Grade, out Grade grade);
            if (!success)
                return Error($"Grade is incorrect: '{dto.Grade}'");

            var enrollment = student.GetEnrollment(enrollmentNumber - 1);
            if (enrollment == null)
                return Error($"No enrollment found with number '{enrollmentNumber}'");

            enrollment.Update(course, grade);

            _unitOfWork.Commit();

            return Ok();
        }

        /// <summary>
        /// Изменяет информацию о студенте
        /// </summary>
        [HttpPut("{id:long}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand()
            {
                StudentId = id,
                Email = dto.Email,
                Name = dto.Name
            };
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }
    }
}
