namespace Api.Dtos
{
    public sealed class StudentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Course1 { get; set; }
        public string Course1Grade { get; set; }
        public string Course1DisenrollmentComment { get; set; }
        public int? Course1Credits { get; set; }

        public string Course2 { get; set; }
        public string Course2Grade { get; set; }
        public string Course2DisenrollmentComment { get; set; }
        public int? Course2Credits { get; set; }
    }

    /// <summary>
    /// Персональная информация студента
    /// </summary>
    public sealed class StudentPersonalInfoDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }


    /// <summary>
    /// Модель данных регистрации на курс
    /// </summary>
    public sealed class StudentEnrollDto
    {
        public string Course { get; set; }
        public string Grade { get; set; }
    }

    /// <summary>
    /// Модель данных переноса курса
    /// </summary>
    public sealed class StudentTransferDto
    {
        public string Course { get; set; }
        public string Grade { get; set; }
    }

    /// <summary>
    /// Модель данных отмены курса
    /// </summary>
    public sealed class StudentDisenrollmentDto
    {
        public string Comment { get; set; }
    }
}
