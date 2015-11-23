using System;

namespace Model
{
    public static class StaticCounterHelper
    {
        public static Int32 SuccessCounter { get; set; }

        public static Int32 ExceptionCounter { get; set; }

        public static void ResetCounter()
        {
            SuccessCounter = 0;
            ExceptionCounter = 0;
        }
    }
}
