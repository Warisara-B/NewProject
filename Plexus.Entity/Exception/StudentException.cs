using System;
using System.Net;
using Plexus.Utility.Exception;

namespace Plexus.Entity.Exception
{
    public class StudentException : BaseCustomException
    {
        public StudentException(string message, HttpStatusCode statusCode) : base(message, "STU", statusCode) { }

        public class NotFound : StudentException
        {
            public NotFound(Guid studentId) : base($"Student with given id ({studentId}) was not found", HttpStatusCode.NotFound) { }
        }

        public class CodeNotFound : StudentException
        {
            public CodeNotFound(string code) : base($"Student with given code ({code}) was not found", HttpStatusCode.NotFound) { }
        }

        public class GuardianNotFound : StudentException
        {
            public GuardianNotFound(Guid guardianId) : base($"Guardian with given id ({guardianId}) was not found", HttpStatusCode.NotFound) { }
        }

        public class AddressNotFound : StudentException
        {
            public AddressNotFound(Guid addressId) : base($"Contact address with given id ({addressId}) was not found", HttpStatusCode.NotFound) { }
        }

        public class StudentTermNotFound : StudentException
        {
            public StudentTermNotFound(Guid termid) : base($"Student term with given term id ({termid}) was not found", HttpStatusCode.NotFound) { }
        }

        public class DuplicateStudentTerm : StudentException
        {
            public DuplicateStudentTerm(Guid termId) : base($"Student term with given term id is already exists. (term id : {termId})", HttpStatusCode.Conflict) { }
        }

        public class InvalidStudentCode : StudentException
        {
            public InvalidStudentCode() : base("Invalid student code.", HttpStatusCode.Forbidden) { }
        }
    }
}

