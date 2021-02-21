namespace Chess.WebUI.Extensions
{
    public static class IntegerExtensions
    {
        public static bool IsEven(this int number) => number % 2 == 0;

        public static bool IsOdd(this int number) => !IsEven(number);
    }
}
