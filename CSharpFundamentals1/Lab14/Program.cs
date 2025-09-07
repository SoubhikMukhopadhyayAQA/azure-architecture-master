using System.Text;

namespace Lab14
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new StringBuilder("Hello World");

            builder.Append('-', 10)
                   .AppendLine()
                   .Append("Header")
                   .AppendLine()
                   .Append('-', 10);


            builder.Replace('-', '*');
            builder.Remove(0, 5);
            builder.Insert(0, new string('-', 10));


            Console.WriteLine(builder);

            Console.WriteLine("First Char: " + builder[0]); 
        }
    }
}