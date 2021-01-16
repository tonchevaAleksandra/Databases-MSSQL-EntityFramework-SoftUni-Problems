
namespace MyCoolCarSystem.Data
{
   public static class DataValidations
    {
        public static class Make
        {
            public const int MaxName = 20;
        }

        public static class Model
        {
            public const int MaxName = 20;
            public const int MaxModification = 30;
        }

        public static class Car
        {
            public const int VinLength = 17;
            public const int ColorLength = 15;
        }

        public static class Customer
        {
            public const int MaxLengthName = 30;
        }

        public static class Address
        {
            public const int MaxTextLength = 100;
            public const int MaxTownLength = 30;
        }
    }
}
