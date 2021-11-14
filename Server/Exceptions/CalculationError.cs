using System;

namespace Server.Exceptions
{
    public class CalculationError : Exception
    {
        public override string Message => "Calculation Error";
    }
}