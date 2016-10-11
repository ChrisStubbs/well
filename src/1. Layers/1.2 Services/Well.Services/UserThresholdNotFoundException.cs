namespace PH.Well.Services
{
    using System;

    public class UserThresholdNotFoundException : Exception
    {
        public UserThresholdNotFoundException(string message) : base(message)
        {
        }
    }
}