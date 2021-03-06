﻿using DI.Reminder.Common.PromptModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI.Reminder.Data.SearchingDatabase
{
    public interface ISearchService
    {
        IList<Prompt> GetSearchResult(string promptval, string categoryval, string dateval, int UserID);
    }
}
