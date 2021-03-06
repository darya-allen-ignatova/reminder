﻿using DI.Reminder.Common.LoginModels;
using System.Collections.Generic;

namespace DI.Reminder.Data.AccountDatabase
{
    public interface IAccountRepository
    {
        void InsertAccount(Account account);
        Account GetAccount(string login);
        void DeleteAccount(int id);
        void UpdateAccount(Account account);
        List<Account> GetAccountList();
        Account GetAccount(int id);
        void AdminUpdateAccount(Account account);
    }
}
