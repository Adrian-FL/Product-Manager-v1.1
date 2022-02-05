using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Product_Manager_v1._1.Domain
{
    class SqlUtils
    {

        static string conexionString = "Data Source=localhost;Initial Catalog =Product Manager v1.1; Integrated Security = True";

        



        #region Category ###############################################################################################################

        public static void InsertInArticleCategory(int idCategory, int idArticle)
        {
            var sql = $@"INSERT INTO ArticleCategory(idCategory, idArticle) VALUES (@idCategory, @idArticle)";

            SqlConnection connection = new SqlConnection(conexionString);
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@idCategory", idCategory);
            command.Parameters.AddWithValue("@idArticle", idArticle);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void AddCategoryToCategorySQL(int parentCategorytId, int childCategorytId) 
       {
            var sql = @"UPDATE Category SET categoryId = @ParentCategorytId   WHERE id_Category = @ChildCategorytId";
            

            SqlConnection connection = new SqlConnection(conexionString);
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@ParentCategorytId", parentCategorytId);
            command.Parameters.AddWithValue("@ChildCategorytId", childCategorytId);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }


        public static Dictionary<int, string> GetArticlesByName(string articleName) 
        {

            Dictionary<int, string> ArticlesWithId = new Dictionary<int, string>();
            var sql = @"SELECT id_Article, name FROM Article WHERE name LIKE CONCAT ('%',@ArticleName, '%');";

            using (SqlConnection connection = new SqlConnection(conexionString))
            using (SqlCommand command = new SqlCommand(sql, connection))

            {
                connection.Open();

                command.Parameters.AddWithValue("@ArticleName", articleName);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var idArticle = (int)reader["id_Article"];
                    var name = (string)reader["name"];

                    ArticlesWithId.Add(idArticle, name);
                }
                connection.Close();
            }
            return ArticlesWithId;
        }

        public static List<CategoryWithArticles> CategoryWithArticlesAndId()
        {
            List<CategoryWithArticles> categoryWithArticles = new List<CategoryWithArticles>();

            var sql = "SELECT c.id_Category, c.name, count(ac.idArticle) as 'numberOfArticles' " +
                "FROM Category c " +
                "LEFT JOIN ArticleCategory ac " +
                "ON c.id_Category = ac.idCategory " +
                "GROUP BY c.id_Category, c.name " +
                "UNION " +
                "SELECT c.id_Category, c.name, 0 " +
                "FROM Category c " +
                "WHERE c.id_Category NOT IN (SELECT idCategory FROM ArticleCategory) " +
                "ORDER BY 1 ASC";

            using (SqlConnection connection = new SqlConnection(conexionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var categoryId = (int)reader["id_Category"];
                    var categoryName = (string)reader["name"];
                    var categorynumberOfArticles = (int)reader["numberOfArticles"];
                    CategoryWithArticles categoyWithArticlesAndId = new CategoryWithArticles(categoryId, categoryName, categorynumberOfArticles);

                    categoryWithArticles.Add(categoyWithArticlesAndId);
                }
                connection.Close();
            }
                return categoryWithArticles;
        }


        public static Dictionary<string, int> GetListOfCategories() 
        {
            // Folosim dictionar pentru ca instructiunea SQL intoarce 2 coloane (2 valori ce trebuie luate simultan) 
            // Dintre care prima coloana nume are valori unice (nu exista 2 categorii cu aceleasi nume in DB) si a -2-a coloana 
            // care ne da numarul de produse pentru acea categorie!

            Dictionary<string, int> listOfCategories = new Dictionary<string, int>();

            var sql = "SELECT c.name, count(ac.idArticle) as 'numberOfArticles' " +
                "FROM Category c " +
                "JOIN ArticleCategory ac " +
                "ON c.id_Category = ac.idCategory " +
                "GROUP BY c.name " +
                "UNION " +
                "SELECT c.name, 0 " +
                "FROM Category c " +
                "WHERE c.id_Category NOT IN (SELECT idCategory FROM ArticleCategory) " +
                "ORDER BY 2 DESC";

            using (SqlConnection connection = new SqlConnection(conexionString))
            using (SqlCommand command = new SqlCommand(sql, connection))

            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var categoryName = (string)reader["name"];
                    var nrOfArticles = (int)reader["numberOfArticles"];

                    listOfCategories.Add(categoryName, nrOfArticles);
                }
                connection.Close();
            }
            return listOfCategories;
        }

       public static void InsertMyCategory(Category myCategory) 
       {                                      //coloane                           //string - apostroafe- int fara 
            var sql = $@"INSERT INTO Category(name) VALUES (@Name)";

            SqlConnection connection = new SqlConnection(conexionString);
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Name", myCategory.Name);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
       }

        public static Category CheckCategoryByNameInDb(string Name) 
        {
            string sql = $@"SELECT name,categoryId " +
                    "FROM Category WHERE name = @Name;";

            Category myCategory = null;
            using (SqlConnection connection = new SqlConnection(conexionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", Name);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var name = (string)reader["name"];
                    var categotyID = (string)reader["categotyID"];

                    myCategory = new Category(name, categotyID);
                }
                connection.Close();
            }
                return myCategory;
        }

        #endregion ####################################################################################################################




        #region Article ################################################################################################################

        public static Article CheckArticleByArticleNumberinDb(string ArticleNumber)
        { 
            string sql = $@"SELECT articleNumber, name, description, price " +
                "FROM Article WHERE articleNumber = @Articlenumber;";

            Article myarticle = null;

            using (SqlConnection connection = new SqlConnection(conexionString))
            //   Conexiune noua
            using (SqlCommand command = new SqlCommand(sql, connection))
           
            {
                command.Parameters.AddWithValue("@Articlenumber", ArticleNumber);
                connection.Open(); 

                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read()) 
                {
                    var articleNumber = (string)reader["articleNumber"];
                    var name = (string)reader["name"];
                    var description = (string)reader["description"];
                    var price = (int)reader["price"];

                    myarticle = new Article(articleNumber, name, description, price);

                }
                connection.Close(); 
            }
            return myarticle;
        }

        public static void InsertMyArticle(Article myArticle)
        {
            var sql = $@"
                INSERT INTO Article (articlenumber,name, description, price)
                VALUES (@ArticleNumber, @Name, @Description, @Price)";

            SqlConnection connection = new SqlConnection(conexionString);
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Articlenumber", myArticle.Articlenumber);
            command.Parameters.AddWithValue("@Name", myArticle.Name);
            command.Parameters.AddWithValue("@Description", myArticle.Description);
            command.Parameters.AddWithValue("@Price", myArticle.Price);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        static public void UpdateArticle(Article article)
        {
            var sql = @"UPDATE Article SET name = @Name, description = @Description, price = @Price " +
                     "WHERE articlenumber = @Articlenumber; ";
          
            SqlConnection connection = new SqlConnection(conexionString);
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Articlenumber", article.Articlenumber);
            command.Parameters.AddWithValue("@Name", article.Name);
            command.Parameters.AddWithValue("@Description",article.Description);
            command.Parameters.AddWithValue("@Price", article.Price);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void DeleteArticle(Article article)
        {
            var sql = @"DELETE FROM Article WHERE articlenumber = @Articlenumber; ";

            SqlConnection connection = new SqlConnection(conexionString);
            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Articlenumber", article.Articlenumber);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        #endregion ####################################################################################################################
    }
}
