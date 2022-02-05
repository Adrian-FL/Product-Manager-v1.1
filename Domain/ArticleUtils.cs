using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;


namespace Product_Manager_v1._1.Domain
{
    class ArticleUtils
    {
        public static void ArticlesMenu()
        {
            CursorVisible = false;
            bool appliationRunning = true;

            do
            {
                CursorVisible = false;
                WriteLine("1. Add article");
                WriteLine("2. Search article");
                WriteLine("3. Exit");
                ConsoleKeyInfo input = ReadKey(true);
                Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        AddArticle();
                        break;

                    case ConsoleKey.D2:
                        SearchArticle();
                        break;

                    case ConsoleKey.D3:
                        appliationRunning = false;
                        break;
                }
            }
            while (appliationRunning);
        }

        static void AddArticle()
        {
            while (true)
            {
                Clear();
                CursorVisible = true;
                WriteLine("Article number: ");
                WriteLine("Name:  ");
                WriteLine("Description: ");
                WriteLine("Price: ");

                SetCursorPosition(16, 0);
                var articleNumber = ReadLine().Trim();
                SetCursorPosition(6, 1);
                var name = ReadLine().Trim();
                SetCursorPosition(13, 2);
                var description = ReadLine().Trim();
                SetCursorPosition(7, 3);
                var price = ReadLine();

                CursorVisible = false;
                WriteLine("Is this correct? (Y)es (N)o ");
                var answer = Console.ReadKey(true).Key;
                while (answer != ConsoleKey.Y && answer != ConsoleKey.N)
                {
                    answer = Console.ReadKey(true).Key;
                }
                if (answer == ConsoleKey.Y)
                {
                    if (String.IsNullOrEmpty(articleNumber))
                    {
                        Clear();
                        WriteLine("Article number cannot be empty");
                        Thread.Sleep(2000);
                        Clear();
                        AddArticle();
                    }
                    else
                    {
                        Article myArticle = SqlUtils.CheckArticleByArticleNumberinDb(articleNumber);
                        if (myArticle == null)
                        {
                            try
                            {
                                int articlePrice = Int32.Parse(price);
                                Article article = new Article(articleNumber, name, description, articlePrice);
                                SqlUtils.InsertMyArticle(article);
                                Clear();
                                WriteLine("Article registered.");
                                Thread.Sleep(2000);
                                Clear();
                                Program.DisplayMenu();
                            }
                            catch (Exception)
                            {
                                Clear();
                                WriteLine("Price must be a number!");
                                Thread.Sleep(2000);
                                Clear();
                                AddArticle();
                            }
                        }
                        else
                        {
                            Clear();
                            WriteLine("Article already exists!");
                            Thread.Sleep(2000);
                            Clear();
                            AddArticle();
                        }
                    }
                }
                else
                {
                    continue;
                }
                break;
            }
            Console.Clear();
        }

        static void EditArticle(Article article)
        {
            while (true)
            {
                CursorVisible = true;
                Clear();
                WriteLine("Article Number: " + article.Articlenumber);
                WriteLine("Name: ");
                WriteLine("Description: ");
                WriteLine("Price: ");

                SetCursorPosition(6, 1);
                var name = ReadLine();
                SetCursorPosition(13, 2);
                var description = ReadLine();
                SetCursorPosition(7, 3);
                var price = ReadLine();

                WriteLine("Is this correct? (Y)es (N)o ");
                var answer = Console.ReadKey(true).Key;
                while (answer != ConsoleKey.Y && answer != ConsoleKey.N)
                {
                    answer = Console.ReadKey(true).Key;
                }
                if (answer == ConsoleKey.Y)
                {
                    article.Name = name;
                    article.Description = description;
                    article.Price = Int32.Parse(price);
                    SqlUtils.UpdateArticle(article);
                }
                else
                {
                    EditArticle(article);
                }
                break;
            }
        }

        private static void SearchArticle()
        {
            Clear();
            CursorVisible = true;
            WriteLine("Article number: ");
            SetCursorPosition(16, 0);
            string articleNumber = ReadLine();
            CursorVisible = false;

            if (String.IsNullOrWhiteSpace(articleNumber))
            {
                Clear();
                WriteLine("You haven't provided an article number!");
                Thread.Sleep(2000);
                Clear();
                SearchArticle();
            }
            else
            {
                Article article = SqlUtils.CheckArticleByArticleNumberinDb(articleNumber);
                if (article == null)
                {
                    Clear();
                    WriteLine("Article not found");
                    Thread.Sleep(2000);
                    Clear();
                    Program.DisplayMenu();
                }
                else
                {
                    Clear();
                    Article.DisplayArticle(article);

                    WriteLine(" ");
                    WriteLine("[E] Edit | [D] Delete | [Esc] Main Menu");

                    ConsoleKeyInfo keyPressed = ReadKey(true);
                    Clear();

                    switch (keyPressed.Key)
                    {
                        case ConsoleKey.E:
                            EditArticle(article);
                            break;

                        case ConsoleKey.D:
                            Clear();
                            WriteLine("Delete this article? (Y)es (N)o");
                            var answer = Console.ReadKey(true).Key;
                            switch (answer)
                            {
                                case ConsoleKey.Y:
                                    SqlUtils.DeleteArticle(article);
                                    Clear();
                                    WriteLine("Article deleted");
                                    Thread.Sleep(2000);
                                    Clear();
                                    Program.DisplayMenu();
                                    break;
                            }
                            break;

                        case ConsoleKey.Escape:
                            Program.DisplayMenu();
                            break;
                    }
                    Console.Clear();
                }
            }
        }
    }
}
