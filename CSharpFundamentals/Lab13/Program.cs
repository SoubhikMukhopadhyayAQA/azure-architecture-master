namespace Lab13
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fullName = "   Soubhik Mukhopadhyay ";
            Console.WriteLine("Trim: '{0}'", fullName.Trim());
            Console.WriteLine("ToUpper: '{0}',", fullName.Trim().ToUpper());
            Console.WriteLine("ToLower: '{0}'", fullName.ToLower());
            Console.WriteLine("Length: {0}", fullName.Length);

            var index = fullName.IndexOf('M');
            Console.WriteLine(index);
            var firstName = fullName.Substring(0, index).Trim();
            var lastName = fullName.Substring((index)).Trim();
            Console.WriteLine("First Name: '{0}'", firstName);
            Console.WriteLine("Last Name: '{0}'", lastName);


            var name = fullName.Trim().Split(' ');
            Console.WriteLine("First Name: '{0}'", name[0]);
            Console.WriteLine("Last Name: '{0}'", name[1]);


            lastName = lastName.Replace("opadhyay", "erjee");
            Console.WriteLine("Last Name: '{0}'", lastName);
        }
    }
}