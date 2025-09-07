namespace Lab03
{
    public class Person
    {
        public int Age;
    }
    class Part1
    {
        static void Main(string[] args)
        {
            // -------------------------------Value Type-------------------------------
            var a = 10;
            var b = a;

            b++;

            Console.WriteLine(string.Format("a:{0}, b: {1}", a, b));

            //----------------------------Reference Type-------------------------------

            var array1 = new int[3] { 1, 2, 3 };
            var array2 = array1;

            array2[0] = 0;

            Console.WriteLine(string.Format("array1[0]:{0}, array2[0]: {1}", array1[0], array2[0]));

            //----------------------------Class Type-------------------------------

            //-------Value type-------
            var number = 1;
            Increment(number);
            Console.WriteLine(string.Format("number: {0}", number));


            //-------Reference type-------
            var person = new Person() { Age = 20 };
            MakeOld(person);

            Console.WriteLine(string.Format("person.Age: {0}", person.Age));

        }
        public static void Increment(int number)
        {
            number +=10;
        }
        public static void MakeOld(Person person)
        {
            person.Age += 10;
        }
    }
}