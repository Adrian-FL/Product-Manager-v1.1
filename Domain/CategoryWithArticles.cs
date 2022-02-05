using System;
using System.Collections.Generic;
using System.Text;

namespace Product_Manager_v1._1.Domain
{
    class CategoryWithArticles
    {
        int idCategory;
        string name;
        int numberOfArticles;

        public CategoryWithArticles(int idCategory1, string name1, int numberOfArticles1)
        {
            IdCategory = idCategory1;
            Name = name1;
            NumberOfArticles = numberOfArticles1;
        }

        public int IdCategory
        {
            get
            {
                return idCategory;
            }
            set
            {
                idCategory = value;
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
        public int NumberOfArticles
        {
            get
            {
                return numberOfArticles;
            }
            set
            {
                numberOfArticles = value;
            }
        }
    }
}
