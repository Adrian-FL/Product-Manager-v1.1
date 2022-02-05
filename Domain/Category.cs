using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Product_Manager_v1._1.Domain
{
    class Category
    {
        string name;
        string categoryId;

        public Category(string name1, string categoryId1)
        {
            Name = name1;
            
            CategoryId = categoryId1;
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
        public string CategoryId
        {
            get 
            {
                return categoryId;
            }
            set
            {
                categoryId = value; 
            }
        }
    }
}
