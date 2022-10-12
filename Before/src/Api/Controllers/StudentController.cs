using Logic.Commands;
using Logic.Dtos;
using Logic.Queries;
using Logic.Utils;

using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Messages _messages;

        public StudentController(UnitOfWork unitOfWork, Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            var result = _messages.Dispatch(new GetStudentListQuery(enrolled, number));
            return FromResult(result);
        }

        /// <summary>
        /// Регистрирует студента
        /// </summary>
        [HttpPost]
        public IActionResult RegisterStudent([FromBody] StudentRegisterDto dto)
        {
            var command = new RegisterStudentCommand(
                dto.Name,
                dto.Email,
                dto.Course1,
                dto.Course1Grade,
                dto.Course2,
                dto.Course2Grade);
            var result = _messages.Dispatch(command);

            return FromResult(result);
        }

        /// <summary>
        /// Отменяет регистрацию студента
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult UnregisterStudent(long id)
        {
            var result = _messages.Dispatch(new UnregisterStudentCommand(id));

            return FromResult(result);
        }

        /// <summary>
        /// Отменяет курс
        /// </summary>
        [HttpPost("{id:long}/enrollments/{enrollmentNumber:int}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            var result = _messages.Dispatch(new DisenrollCommand(id, enrollmentNumber, dto.Comment));

            return FromResult(result);
        }

        /// <summary>
        /// Регистрирует студента на переданный курс
        /// </summary>
        [HttpPost("{id:long}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentTransferDto dto)
        {
            var result = _messages.Dispatch(new EnrollCommand(id, dto.Course, dto.Grade));

            return FromResult(result);
        }

        /// <summary>
        /// Переносит курс студента
        /// </summary>
        [HttpPut("{id:long}/enrollments/{enrollmentNumber:int}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentEnrollDto dto)
        {
            var command = new TransferCommand(id, enrollmentNumber, dto.Course, dto.Grade);
            var result = _messages.Dispatch(command);

            return FromResult(result);
        }

        /// <summary>
        /// Изменяет информацию о студенте
        /// </summary>
        [HttpPut("{id:long}")]
        public IActionResult EditPersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Email, dto.Name);
            var result = _messages.Dispatch(command);

            return result.IsSuccess ? Ok() : Error(result.Error);
        }
    }
}
