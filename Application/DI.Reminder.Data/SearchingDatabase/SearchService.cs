﻿using DI.Reminder.Common.CategoryModel;
﻿using DI.Reminder.Common.Logger;
using DI.Reminder.Common.PromptModel;
using DI.Reminder.Data.CategoryDataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DI.Reminder.Data.SearchingDatabase
{
    public class SearchService : ISearchService
    {
        private ICategoryRepository _categoryRepository;
        private ILogger _logger;
        private string connection = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private string GetConnection
        {
            get { return connection; }
            set { }
        }
        public SearchService(ICategoryRepository categoryRepository, ILogger logger)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IList<Prompt> GetSearchItems(int userID, int id, string value)
        {
            IList<Prompt> SearchingItems = null;
            switch (id)
            {
                case 1: SearchingItems = GetByCategory(value, userID); break;
                case 2: SearchingItems = GetByPrompt(value, userID); break;
                case 3: SearchingItems = GetByDate(value, userID); break;
            }
            return SearchingItems;
        }

        private IList<Prompt> GetByCategory(string value, int userID)
        {
            List<Prompt> _list = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnection))
                {
                    connection.Open();
                    string sqlExpression = "SearchByCategory";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter sqlparam = new SqlParameter()
                    {
                        ParameterName = "@id",
                        Value = int.Parse(value)
                    };
                    command.Parameters.Add(sqlparam);
                    SqlParameter sqlparam1 = new SqlParameter()
                    {
                        ParameterName = "@userId",
                        Value = userID
                    };
                    command.Parameters.Add(sqlparam1);
                    SqlDataReader reader = command.ExecuteReader();
                    _list = new List<Prompt>();
                    Category category = null;
                    while (reader.Read())
                    {
                        int IDCategory = int.Parse(reader["CategoryID"].ToString());
                        category = _categoryRepository.GetCategory(IDCategory);
                        int Id = int.Parse(reader["ID"].ToString());
                        _list.Add(new Prompt()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Name = reader["Name"].ToString(),
                            Category = category,
                            Date = Convert.ToDateTime(reader["DateOfCreating"].ToString()),
                        });
                    }
                    connection.Close();

                }
            }
            catch (SqlException sqlExc)
            {
                _logger.Error("SqlException: " + sqlExc.Source + "\t" + sqlExc.Message);
            }
            catch (Exception ex)
            {
                _logger.Error("SqlException: " + ex + "\t" + ex.Message);
            }
            return _list;
        }
        private IList<Prompt> GetByPrompt(string value, int userID)
        {
            List<Prompt> _list = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnection))
                {
                    connection.Open();
                    string sqlExpression = "SearchByPrompt";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter sqlparam = new SqlParameter()
                    {
                        ParameterName = "@name",
                        Value = value
                    };
                    command.Parameters.Add(sqlparam);
                    SqlParameter sqlparam1 = new SqlParameter()
                    {
                        ParameterName = "@userId",
                        Value = userID
                    };
                    command.Parameters.Add(sqlparam1);
                    SqlDataReader reader = command.ExecuteReader();
                    _list = new List<Prompt>();
                    Category category = null;
                    while (reader.Read())
                    {
                        int IDCategory = int.Parse(reader["CategoryID"].ToString());
                        category = _categoryRepository.GetCategory(IDCategory);
                        int Id = int.Parse(reader["ID"].ToString());
                        _list.Add(new Prompt()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Name = reader["Name"].ToString(),
                            Category = category,
                            Date = Convert.ToDateTime(reader["DateOfCreating"].ToString()),
                        });
                    }
                    connection.Close();

                }
            }
            catch (SqlException sqlExc)
            {
                _logger.Error("SqlException: " + sqlExc.Source + "\t" + sqlExc.Message);
            }
            catch (Exception ex)
            {
                _logger.Error("SqlException: " + ex + "\t" + ex.Message);
            }
            return _list;
        }
        private IList<Prompt> GetByDate(string value, int userID)
        {
            List<Prompt> _list = null;
            DateTime dateTime;
            try
            {
                dateTime = Convert.ToDateTime(value);
            }
            catch
            {
                return null;
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnection))
                {
                    connection.Open();
                    string sqlExpression = "SearchByDate";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter sqlparam = new SqlParameter()
                    {
                        ParameterName = "@date",
                        Value = dateTime
                    };
                    command.Parameters.Add(sqlparam);
                    SqlParameter sqlparam1 = new SqlParameter()
                    {
                        ParameterName = "@userId",
                        Value = userID
                    };
                    command.Parameters.Add(sqlparam1);
                    SqlDataReader reader = command.ExecuteReader();
                    _list = new List<Prompt>();
                    Category category = null;
                    while (reader.Read())
                    {
                        int IDCategory = int.Parse(reader["CategoryID"].ToString());
                        category = _categoryRepository.GetCategory(IDCategory);
                        int Id = int.Parse(reader["ID"].ToString());
                        _list.Add(new Prompt()
                        {
                            ID = int.Parse(reader["ID"].ToString()),
                            Name = reader["Name"].ToString(),
                            Category = category,
                            Date = Convert.ToDateTime(reader["DateOfCreating"].ToString()),
                        });
                    }
                    connection.Close();

                }
            }
            catch (SqlException sqlExc)
            {
                _logger.Error("SqlException: " + sqlExc.Source + "\t" + sqlExc.Message);
            }
            catch (Exception ex)
            {
                _logger.Error("SqlException: " + ex + "\t" + ex.Message);
            }
            return _list;
        }
    }
}
