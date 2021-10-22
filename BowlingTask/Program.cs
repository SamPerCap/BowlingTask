using System;

namespace BowlingTask
{
    class Program
    {
        //All console input/output interface and its logic
        static void Main(string[] args)
        {
            //Create an instance to access it from a static method.
            DomainLogic fileManager = new DomainLogic();

            Console.WriteLine("Please. Choose wisely which game would you like to review:");

            //Print an option for each file recorded in the folder.
            for (int i = 0; i < fileManager.GetArrayLength(); i++)
            {
                Console.WriteLine(i + 1 + "," + fileManager.GetFileNameFromArray(i));
            }

            //Read the number from a text format input. Print the choosen file
            int position = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("|f1 |f2 |f3 |f4 |f5 |f6 |f7 |f8 |f9 |f10 |");
            Console.WriteLine(fileManager.ReadFile(position));
            Console.WriteLine("Score:" + fileManager.CalculateScore());

            //Prevents console to close until user inputs something
            Console.ReadLine();
        }
    }
}
