using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain.ValueObjects
{
    public class AssignJobResult
    {
        public AssignJobResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public AssignJobResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
        public string Message { get; }
    }
}
