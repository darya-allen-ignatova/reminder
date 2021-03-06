﻿using System;
using System.Linq;
using System.Security.Principal;
using DI.Reminder.BL.LoginService.UserIndent;
using DI.Reminder.Data.AccountDatabase;
using DI.Reminder.Data.RoleDatabase;

namespace DI.Reminder.BL.LoginService.UserProv
{
    public class UserProvider : IPrincipal
    {
        private IRoleRepository _roleRepository;
        private IAccountRepository _accountRepository;
        public UserProvider(IRoleRepository roleRepository, IAccountRepository accountRepository, string name )
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;


            userIndentity = new UserIndentity(_accountRepository);
            userIndentity.Init(name);
        }

        private UserIndentity userIndentity { get; set; }

        public IIdentity Identity
        {
            get
            {
                return userIndentity;
            }
        }
        public bool IsInRole(string role)
        {
            if (!userIndentity.IsAuthenticated || string.IsNullOrWhiteSpace(role))
            {
                return false;
            }
            var UserRole = _roleRepository.GetRole(userIndentity.account.ID);
            var hasRole = string.Compare(UserRole.Name.Replace(" ", string.Empty), role, true) == 0;
            if (hasRole)
                {
                    return true;
                }
            
            return false;
        }
    }
}
