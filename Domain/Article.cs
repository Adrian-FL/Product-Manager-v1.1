using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Product_Manager_v1._1.Domain
{
    class Article
    {
        string articlenumber;
        string name;
        string description;
        int price;

        public Article(string articlenumber1, string name1, string description1, int price1)
        {
            Articlenumber = articlenumber1;
            Name = name1;
            Description = description1;
            Price = price1;
        }

        public string Articlenumber
        {
            get
            {
                return articlenumber;
            }
            set
            {
                articlenumber = value;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        public int Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        public static void DisplayArticle(Article myArticle)
        {
            WriteLine("Article number: " + myArticle.articlenumber);
            WriteLine("Name: " + myArticle.name);
            WriteLine("Description: " + myArticle.description);
            WriteLine("Price: " + myArticle.price);
        }
    }
}
