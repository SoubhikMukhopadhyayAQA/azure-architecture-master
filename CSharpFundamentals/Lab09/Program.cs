namespace Lab09
{
	public class Program
	{
        public static void Main(string[] args)
        {
            var number = new [] { 3,7,9,2,14,6 };

            // -------------------------------------------------Length
            Console.WriteLine("Length : " + number.Length);

            //number.IndexOf  // This is not a method of array class

            // -------------------------------------------------IndexOf()
            var index = Array.IndexOf(number, 9);
            Console.WriteLine("Index of 9 : " + index);

            // -------------------------------------------------Clear()
            Array.Clear(number, 0, 2);
            Console.WriteLine("Effect of Clear()");
            foreach (var item in number)
            {
                Console.WriteLine(item);
            }
            // -------------------------------------------------Copy()
            var another = new int[3];
            Array.Copy(number, another, 3);
            Console.WriteLine("Effect of Copy()");
            foreach (var item in another)
            {
                Console.WriteLine(item);
            }
            // -------------------------------------------------Sort()
            Array.Sort(number);
            Console.WriteLine("Effect of Sort()");
            foreach (var item in number)
            {
                Console.WriteLine(item);
            }
            // -------------------------------------------------Reverse()
            Array.Reverse(number);
            Console.WriteLine("Effect of Reverse()");
            foreach (var item in number)
            {
                Console.WriteLine(item);
            }
        }
    }
}