﻿namespace Clinic.API.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Detail { get; set; } // opcional
    }
}