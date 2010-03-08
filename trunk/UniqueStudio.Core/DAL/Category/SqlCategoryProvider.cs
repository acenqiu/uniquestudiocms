using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Resource;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL.Category
{
    internal class SqlCategoryProvider : ICategory
    {
        private const string CREATE_CATEGORY = "CreateCategory";
        private const string DELETE_CATEGORY = "DeleteCategory";
        private const string GET_ALL_CATEGORIES = "GetAllCategories";
        private const string GET_CATEGORY_BY_ID = "GetCategoryById";
        private const string GET_CATEGORY_BY_NICENAME = "GetCategoryByNiceName";
        private const string GET_CHILD_CATEGORIES_BY_ID = "GetChildCategoriesById";
        private const string GET_CHILD_CATEGORIES_BY_NICENAME = "GetChildCategoriesByNiceName";
        private const string UPDATE_CATEGORY = "UpdateCategory";

        public SqlCategoryProvider()
        {
            //默认构造函数
        }

        public CategoryInfo CreateCategory(CategoryInfo category)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                        new SqlParameter("@CategoryName",category.CategoryName),
                                                        new  SqlParameter("@CategoryNiceName",category.CategoryNiceName),
                                                        new SqlParameter("@Description",category.Description),
                                                        new SqlParameter("@SubOf",category.SubOf)};
            object o = SqlHelper.ExecuteScalar(CommandType.StoredProcedure, CREATE_CATEGORY, parms);
            if (o != null && o != DBNull.Value)
            {
                category.CategoryId = Convert.ToInt32(o);
                return category;
            }
            else
            {
                throw new Exception();
            }
        }

        public bool DeleteCategory(int categoryId, bool isDeleteChildCategories)
        {
            //@IsDeleteChildCategories暂时无法使用，现处理方式为不删除
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@CategoryID",categoryId),
                                                    new SqlParameter("@IsDeleteChildCategories",isDeleteChildCategories)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_CATEGORY, parms) > 0;
        }

        public CategoryCollection GetAllCategories()
        {
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_CATEGORIES, null))
            {
                while (reader.Read())
                {
                    CategoryInfo category = FillCategoryInfo(reader);
                    category.ParentCategoryName = reader["ParentCategoryName"].ToString();
                    collection.Add(category);
                }
            }
            return collection;
        }

        public CategoryInfo GetCategory(int categoryId)
        {
            SqlParameter parm = new SqlParameter("@CategoryID", categoryId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_BY_ID, parm))
            {
                if (reader.Read())
                {
                    return FillCategoryInfo(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public CategoryInfo GetCategory(string catNiceName)
        {
            SqlParameter parm = new SqlParameter("@CategoryNiceName", catNiceName);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CATEGORY_BY_NICENAME, parm))
            {
                if (reader.Read())
                {
                    return FillCategoryInfo(reader);
                }
                else
                {
                    return null;
                }
            }
        }

        public CategoryInfo GetCategoryPath(int categoryId)
        {
            CategoryInfo category = null;
            using (SqlConnection conn = new SqlConnection(GlobalConfig.SqlConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(GET_CATEGORY_BY_ID,conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CategoryID",SqlDbType.Int);
                    conn.Open();

                    CategoryInfo temp = null;
                    cmd.Parameters[0].Value = categoryId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            temp = FillCategoryInfo(reader);
                            category = temp;
                        }

                        reader.Close();
                    }
                    while (temp !=null && temp.SubOf !=0)
                    {
                        cmd.Parameters[0].Value = temp.SubOf;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                temp = FillCategoryInfo(reader);
                            }
                            reader.Close();
                        }

                        temp.ChildCategory = category;
                        category = temp;
                    }
                    conn.Close();
                }
            }
            return category;
        }

        public CategoryCollection GetChildCategories(int categoryId)
        {
            SqlParameter parm = new SqlParameter("@CategoryID", categoryId);
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CHILD_CATEGORIES_BY_ID, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillCategoryInfo(reader));
                }
            }
            return collection;
        }

        public CategoryCollection GetChildCategories(string categoryNiceName)
        {
            SqlParameter parm = new SqlParameter("@CategoryNiceName", categoryNiceName);
            CategoryCollection collection = new CategoryCollection();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_CHILD_CATEGORIES_BY_NICENAME, parm))
            {
                while (reader.Read())
                {
                    collection.Add(FillCategoryInfo(reader));
                }
            }
            return collection;
        }

        public bool UpdateCategory(CategoryInfo category)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@CategoryID",category.CategoryId),
                                                    new SqlParameter("@CategoryName",category.CategoryName),
                                                    new SqlParameter("@CategoryNiceName",category.CategoryNiceName),
                                                    new SqlParameter("@Description",category.Description),
                                                    new SqlParameter("@SubOf",category.SubOf)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, UPDATE_CATEGORY, parms) > 0;
        }

        private CategoryInfo FillCategoryInfo(SqlDataReader reader)
        {
            CategoryInfo category = new CategoryInfo();
            category.CategoryId = (int)reader["CategoryID"];
            category.CategoryName = reader["CategoryName"].ToString();
            category.CategoryNiceName = reader["CategoryNiceName"].ToString();
            if (DBNull.Value != reader["Description"])
            {
                category.Description = reader["Description"].ToString();
            }
            category.SubOf = (int)reader["SubOf"];
            return category;
        }
    }
}
