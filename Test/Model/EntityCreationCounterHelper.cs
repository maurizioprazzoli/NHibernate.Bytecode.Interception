using System;

namespace Model
{
    public static class EntityCreationCounterHelper
    {
        public static Int32 NewItemCall { get; set; }

        public static Int32 NewBidCall { get; set; }

        public static void ResetCounter()
        {
            NewItemCall = 0;
            NewBidCall = 0;
        }
    }
}
