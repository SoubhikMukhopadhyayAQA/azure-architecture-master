namespace Lab16
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //-------------------------------------------------------------------------------------------- Files & FileInfo
            var path1 = @"C:\Users\s.p.mukhopadhyay\Downloads\Soubhik_Resume_Sept2025.pdf";
            var path2 = @"C:\Users\s.p.mukhopadhyay\Downloads\Loops\Soubhik_Resume_Sept2025.pdf";
            File.Copy(@"C:\Users\s.p.mukhopadhyay\Downloads\Soubhik_Resume_Sept2025.pdf", @"C:\Users\s.p.mukhopadhyay\Downloads\Loops\Soubhik_Resume_Sept2025.pdf", true);
            File.Delete(path2);

            if(File.Exists(path2))
                Console.WriteLine("File exists");

            //Console.WriteLine("Content: " + File.ReadAllText(path1));
            



            var fileInfo = new FileInfo(path1);                               //FileInfo object creation
            Console.WriteLine("File Info: " + fileInfo.CreationTime);

            //fileInfo.CopyTo("...");
            //fileInfo.Delete();
            //if(fileInfo.Exists)
            //    Console.WriteLine("File exists");

            //-------------------------------------------------------------------------------------------- Directories & DirectoryInfo
            var dirPath1 = @"C:\Users\s.p.mukhopadhyay\Downloads\Loops2";
            var dirPath2 = @"C:\Users\s.p.mukhopadhyay\Downloads\Loops";
            Directory.CreateDirectory(dirPath1);

            var files = Directory.GetFiles(dirPath2, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
                Console.WriteLine(file);

            var directories = Directory.GetDirectories(dirPath2, "*.*", SearchOption.AllDirectories);
            foreach (var directory in directories)
                Console.WriteLine(directory);



            //Directory.Exists("...");
            var dirInfo = new DirectoryInfo("...");                     //DirectoryInfo object creation
            //dirInfo.GetFiles();
            //dirInfo.GetDirectories();

            //-------------------------------------------------------------------------------------------- Path

            var path = @"C:\Users\s.p.mukhopadhyay\Downloads\Soubhik_Resume_Sept2025.pdf";
            Console.WriteLine("Directory: " + Path.GetDirectoryName(path));
            Console.WriteLine("File Name: " + Path.GetFileName(path));
            Console.WriteLine("Extension: " + Path.GetExtension(path));


        }
    }
}