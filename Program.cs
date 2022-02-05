using Product_Manager_v1._1.Domain;
using System;
using static System.Console;

namespace Product_Manager_v1._1
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayMenu();
        }


        public static void  DisplayMenu()
        {
            CursorVisible = false;
            bool appliationRunning = true;

            do
            {
                CursorVisible = false;
                WriteLine("1. Categories");
                WriteLine("2. Articles");
                WriteLine("3. Exit");
                ConsoleKeyInfo input = ReadKey(true);
                Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        CategoryUtils.CategoryMenu();
                        break;

                    case ConsoleKey.D2:
                        ArticleUtils.ArticlesMenu();
                        break;

                    case ConsoleKey.D3:

                        appliationRunning = false;

                        break;
                }
            }
            while (appliationRunning);
        }
    }
}
