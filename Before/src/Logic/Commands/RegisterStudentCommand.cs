﻿namespace Logic.Commands
{
    public sealed class RegisterStudentCommand : ICommand
    {
        public string Name { get; }
        public string Email { get; }

        public string Course1 { get; }
        public string Course1Grade { get; }

        public string Course2 { get; }
        public string Course2Grade { get; }

        public RegisterStudentCommand(string name, string email, string course1, string course1Grade, string course2, string course2Grade)
        {
            Name = name;
            Email = email;
            Course1 = course1;
            Course1Grade = course1Grade;
            Course2 = course2;
            Course2Grade = course2Grade;
        }
    }
}
