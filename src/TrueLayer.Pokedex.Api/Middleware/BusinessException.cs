using System;

namespace TrueLayer.Pokedex.Api.Middleware
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {

        }
    }
}