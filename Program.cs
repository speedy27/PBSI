using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace fractales
{
    internal class Program
    {

        static void Main(string[] args)
        {
            bool continuer = true;

            while (continuer)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. fractale de mandelbrot");
                Console.WriteLine("2. fractale de Julia ");
                Console.WriteLine("3. 2ème fractale de Julia ");
                Console.WriteLine("4. Quitter");

                Console.Write("Choisissez une option : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        frac fractale = new frac();
                        fractale.creationfrac();
                        fractale.From_Image_To_File("hey.bmp");
                        Process.Start("hey.bmp");
                        break;
                    case "2":
                        frac fractale2 = new frac();
                        fractale2.creationfrac2();
                        fractale2.From_Image_To_File("hey.bmp");
                        Process.Start("hey.bmp");
                        break;
                    case "3":
                        frac fractale3 = new frac();
                        fractale3.creationfrac3();
                        fractale3.From_Image_To_File("hey.bmp");
                        Process.Start("hey.bmp");
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez réessayer.");
                        break;
                }
            }
        }

       
    }
}
