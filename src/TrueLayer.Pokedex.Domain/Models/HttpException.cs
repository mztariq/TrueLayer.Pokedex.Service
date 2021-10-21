using System;

namespace TrueLayer.Pokedex.Domain.Models
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 402;

        public object Value { get; set; }
    }
}