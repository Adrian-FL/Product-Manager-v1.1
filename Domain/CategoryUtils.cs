using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static System.Console;

namespace Product_Manager_v1._1.Domain
{
    class CategoryUtils
    {
        public static void CategoryMenu()
        {
            CursorVisible = false;
            bool appliationRunning = true;

            do
            {
                CursorVisible = false;
                WriteLine("1. Add Category");
                WriteLine("2. List Category");
                WriteLine("3. Add article to Category");
                WriteLine("4. Add Category to Category");
                ConsoleKeyInfo input = ReadKey(true);
                Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        AddCategory();
                        break;

                    case ConsoleKey.D2:
                        ListCategories();
                        break;

                    case ConsoleKey.D3:
                        AddProductToCategory();
                        break;

                    case ConsoleKey.D4:
                        AddCategoryToCategory();
                       
                        appliationRunning = false;
                        break;
                }
            }
            while (appliationRunning);
        }

        private static void AddCategoryToCategory()
        {
            CursorVisible = false;
            List<CategoryWithArticles> categoryWithArticles = SqlUtils.CategoryWithArticlesAndId();
            Clear();
            WriteLine($"{"ID",-10 }{"Category",-40}");
            WriteLine("================================================================");

            foreach (CategoryWithArticles category in categoryWithArticles)
            {
                WriteLine($"{category.IdCategory,-10 }{category.Name,-40} ");
            }
            CursorVisible = true;

            WriteLine(" ");
            WriteLine("Parent Category ID: ");
            WriteLine("Child Category ID: ");
            int nrOfRows = 2 + categoryWithArticles.Count + 2;
            SetCursorPosition(20, nrOfRows -1);
            var parentCategorytId = Int32.Parse(ReadLine());
            SetCursorPosition(19, nrOfRows);
            var childCategoryId = Int32.Parse(ReadLine());

            bool parentCategoryIdExists = false;
            bool childCategoryIdExists = false;

            foreach (CategoryWithArticles category1 in categoryWithArticles)
            {
                if (category1.IdCategory == parentCategorytId)
                {
                    parentCategoryIdExists = true;
                }
                if (category1.IdCategory == childCategoryId)
                {
                    childCategoryIdExists = true;
                }
            }

            if (parentCategoryIdExists == true && childCategoryIdExists == true)
            {
                if (parentCategorytId != childCategoryId)
                {
                   SqlUtils.AddCategoryToCategorySQL(parentCategorytId, childCategoryId);
                    Clear();
                    WriteLine("Category added to category");
                    Thread.Sleep(2000);
                    Clear();
                    Program.DisplayMenu();
                }
                else
                {
                    Clear();
                    WriteLine("Child Id & Parent Id cannot be the same");
                    Thread.Sleep(2000);
                    Clear();
                    AddCategoryToCategory();
                }
            }
            else
            {
                Clear();
                if (parentCategoryIdExists == false)
                {
                    WriteLine("You have entered a incorrect ID for parrent");
                }
                if (childCategoryIdExists == false)
                {
                    WriteLine("You have entered a incorrect ID for child!");
                }
                Thread.Sleep(2000);
                Clear();
                AddCategoryToCategory();
            }
        }

        private static void AddProductToCategory()
        {
          
            List<CategoryWithArticles> categoryWithArticles = SqlUtils.CategoryWithArticlesAndId();
            Clear();
            WriteLine($"{"ID",-10 }{"Category",-40}Total Articles");
            WriteLine("================================================================");

            foreach (CategoryWithArticles category in categoryWithArticles)
            {
                WriteLine($"{category.IdCategory,-10 }{category.Name,-40} {category.NumberOfArticles}");
            }
            CursorVisible = true;
            WriteLine(" ");
            WriteLine("Selected ID> ");
            int nrOfRows = 2 + categoryWithArticles.Count + 2;
            SetCursorPosition(14, nrOfRows -1);

            var idCategory = Int32.Parse(ReadLine()); // id este id category
            Clear();
            bool categoryExists = false;
            foreach (CategoryWithArticles category1 in categoryWithArticles)
            {
                if (category1.IdCategory == idCategory)
                {
                    categoryExists = true;
                    CursorVisible = false;
                    WriteLine("Name: " + category1.Name);
                    WriteLine(" ");
                    WriteLine($"{"[A] Add article", -20}{"[Esc]", -10}");
                    var selectedOption = Console.ReadKey(true).Key;

                    switch (selectedOption)
                    {
                        case ConsoleKey.Escape:
                            Clear();
                            AddProductToCategory();
                            break;

                        case ConsoleKey.A:

                            CursorVisible = true;
                            Clear();
                            WriteLine("Search article: ");
                            SetCursorPosition(17, 0);
                            var articleName = ReadLine();
                            Dictionary<int, string> articles = SqlUtils.GetArticlesByName(articleName);
                            Clear();
                            if (articles.Count > 0)
                            {
                                WriteLine($"{"Id",-10} Name");
                                WriteLine("================================================================");

                                foreach (KeyValuePair<int, string> entry in articles)
                                {
                                    WriteLine($"{entry.Key,-10}" + entry.Value);
                                }
                                WriteLine(" ");
                                WriteLine("Selected ID> ");
                                int nrOfLines = 2 + articles.Count + 2;
                                SetCursorPosition(14, nrOfLines - 1);
                                var articleId = Int32.Parse(ReadLine());
                                if (articles.ContainsKey(articleId))
                                {
                                    SqlUtils.InsertInArticleCategory(idCategory, articleId);
                                    Clear();
                                    WriteLine("Article added to category");
                                    Thread.Sleep(2000);
                                    Clear();
                                    Program.DisplayMenu();
                                }
                                else
                                {
                                    Clear();
                                    WriteLine("Article id dosen't match an article!");
                                    Thread.Sleep(2000);
                                    Clear();
                                    AddProductToCategory();
                                }
                                break;
                            }
                            else
                            {
                                Clear();
                                WriteLine("No article with this name");
                                Thread.Sleep(2000);
                                Clear();
                                AddProductToCategory();
                            }
                            break;
                    }
                    break;
                }  
            }
            if (categoryExists == false)
            {
                Clear();
                WriteLine("Selected id dosen't mach a category!");
                Thread.Sleep(2000);
                Clear();
                AddProductToCategory();
            }
        Clear();
        }

        private static void ListCategories()
        {
            while (true)
            {
                Dictionary<string, int> Categories = SqlUtils.GetListOfCategories();
                Clear();
                WriteLine($"{"Category",-50}Total Articles");
                WriteLine("================================================================");
                foreach (KeyValuePair<string, int> entry in Categories)
                {
                    WriteLine($"{entry.Key,-50} {entry.Value}");
                }
                WriteLine("  ");
                WriteLine("Press Esc to return to menu");
                var answer = Console.ReadKey(true).Key;
                while (answer != ConsoleKey.Escape)
                {
                    answer = Console.ReadKey(true).Key;
                }
                if (answer == ConsoleKey.Escape)
                {
                    Clear();
                    Program.DisplayMenu();
                }
                break;
            }
            Console.Clear();
        }

        private static void AddCategory()
        {
            while (true)
            {
                Clear();
                CursorVisible = true;
                WriteLine("Name:  ");
                SetCursorPosition(6, 0);
                var name = ReadLine();
                CursorVisible = false;
                WriteLine("Is this correct? (Y)es (N)o ");
                var answer = Console.ReadKey(true).Key;
                while (answer != ConsoleKey.Y && answer != ConsoleKey.N)
                {
                    answer = Console.ReadKey(true).Key;
                }
                if (answer == ConsoleKey.Y)
                {
                    if (String.IsNullOrEmpty(name))
                    {
                        Clear();
                        WriteLine("Article name cannot be empty");
                        Thread.Sleep(2000);
                        Clear();
                        AddCategory();
                    }
                    else
                    {
                        Category myCategory = SqlUtils.CheckCategoryByNameInDb(name);
                        if (myCategory == null)
                        {          
                        Category category = new Category(name, null);
                        SqlUtils.InsertMyCategory(category);
                        Console.WriteLine("Category added");
                        }
                        else
                        {
                            Clear();
                            WriteLine("Category already exists!");
                            Thread.Sleep(2000);
                            Clear();
                            AddCategory();
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
    }
 }

