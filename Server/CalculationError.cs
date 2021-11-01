using System;

namespace Server
{
    public class CalculationError : Exception
    {
        public override string Message => "Calculation Error";
    }
}