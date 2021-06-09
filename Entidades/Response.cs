using System.Collections.Generic;

namespace Entidades
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string mensaje { get; set; }
        public object Object { get; set; }
    }
    public class ResponseError
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
