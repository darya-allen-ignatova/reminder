﻿using System.Collections.Generic;
using System;
using DI.Reminder.Data.PromptDataBase;
using DI.Reminder.Common.PromptModel;
using DI.Reminder.Data.Searching;
using DI.Reminder.Data.CategoryDataBase;
using DI.Reminder.Common.CategoryModel;

namespace DI.Reminder.BL.PromptStorage
{
    public class Prompts:IPrompts
    {
        private ISearch _search;
        private IPromptRepository _promptRepository;
        private ICategoryRepository _categoryRepository;
        public Prompts(IPromptRepository promptRepository, ICategoryRepository categoryRepository, ISearch search)
        {
            _categoryRepository = categoryRepository;
            _search = search;
            _promptRepository = promptRepository;
            if (_promptRepository == null)
                throw new ArgumentNullException();
        }
        public IList<Prompt> GetCategoryItemsByID(int userID,int? id)
        {
            if (id == null)
                return null;
            else if (id < 0)
                return null;
            IList<Prompt> promptList;
            IList<Category> categoryList;
            try
            {
                promptList = _promptRepository.GetPromptsList(userID,id);
                categoryList = _categoryRepository.GetCategories(id);
            }
            catch
            {
                throw;
            }
            if (promptList == null & categoryList == null)
                return null;
            else if (promptList == null & categoryList!= null)
            {
                promptList = new List<Prompt>();
                return promptList;
            }
            else
                return promptList;
        }
        public Prompt GetPromptDetails(int userID, int? id)
        {
            if (id == null || id<0)
                return null;
            Prompt prompt;
            try
            {
                 prompt = _promptRepository.GetPrompt(userID,id);
            }
            catch
            {
                throw;
            }
            return prompt;
        }
        public void DeletePrompt(int userID, int? id)
        {
            if (id == null)
                return;
            _promptRepository.DeletePrompt(userID,id);
        }
        public void InsertPrompt(int userID, Prompt newprompt)
        {
            if (newprompt == null)
                return;
            _promptRepository.AddPrompt(userID,newprompt);
        }

        public IList<Prompt> GetSearchingPrompts(int userID,int id, string value)
        {

            if (value == null)
                return null;
            return _search.GetSearchItems(userID,id, value);
        }
    
    }
}
