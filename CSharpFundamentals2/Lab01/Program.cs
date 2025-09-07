namespace Lab1
{
    public class Person
    {
        public string Name;

        public void Introduce(string to)
        {
            Console.WriteLine("Hi {0}, I am {1}", to, Name);
        }
        //public Person Parse(string str)
        //{
        //    var person = new Person();
        //    person.Name = str;
        //    return person;
        //}
        public static Person Parse(string str)
        {
            var person = new Person();
            person.Name = str;
            return person;
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            //Person person = new Person();
            //var person = new Person();

            //var p = person.Parse("Kalle");

            //person.Name = "Kalle";
            //person.Introduce("Johanna");

            var person = Person.Parse("Kalle");
            person.Introduce("Johanna");

        }
    }
}