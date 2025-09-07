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


            if (string.IsNullOrEmpty(" ".Trim()))
                Console.WriteLine("Invalid");

            if (string.IsNullOrWhiteSpace(" "))
                Console.WriteLine("Invalid");

            var str = "25";
            var age = Convert.ToByte(str);
            Console.WriteLine(age);

            float price = 29.95f;
            Console.WriteLine(price.ToString("C"));



            var sentence = "This is going to be a really really really long text";
            const int maxLength = 25;








            var summary = StringUtility.SummarizeText(sentence, maxLength);
            Console.WriteLine(summary);
        }
    }
}