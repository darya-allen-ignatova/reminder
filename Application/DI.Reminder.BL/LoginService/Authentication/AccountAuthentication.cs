﻿using DI.Reminder.BL.LoginService.UserProv;
using DI.Reminder.Common.Logger;
using DI.Reminder.Common.LoginModels;
using DI.Reminder.Data.AccountDatabase;
using DI.Reminder.Data.RoleDatabase;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace DI.Reminder.BL.LoginService.Authentication
{
    public class AccountAuthentication:IAuthentication
    {
        private string cookiename = "authCookie";
        private IRoleRepository _roleRepository;
        private IAccountRepository _accountRepository;
        private ILogger _logger;
        public AccountAuthentication(IRoleRepository roleRepository, IAccountRepository accountRepository, ILogger logger)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public HttpContext httpContext { get; set; }

        public void Registration(Account account)
        {
            if (account != null)
            {
                CreateCookie(account);
            }
        }
        public bool Authentication(Account newaccount, bool isPersistent=false)
        {
            if (newaccount == null || newaccount.Login == null || newaccount.Password == null)
                return false;
            Account account = _accountRepository.GetAccount(newaccount.Login);
            if (account != null && account.Password.Replace(" ", string.Empty)==newaccount.Password)
            {
                CreateCookie(account, isPersistent);
                return true;
            }
            return false;
        }
        private Account GetAccount(string login)
        {
            return _accountRepository.GetAccount(login);
        }

        private void CreateCookie(Account account, bool isPersistent = false)
        {
            var SerializeObj = JsonConvert.SerializeObject(account);
            var ticket = new FormsAuthenticationTicket(1,                
                  account.Login,
                  DateTime.Now,
                  DateTime.Now.Add(FormsAuthentication.Timeout),
                  isPersistent,
                  SerializeObj,
                  FormsAuthentication.FormsCookiePath);

            var encTicket = FormsAuthentication.Encrypt(ticket);

            var AuthCookie = new HttpCookie(cookiename)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            httpContext.Response.Cookies.Set(AuthCookie);
        }


        private IPrincipal _currentUser;

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null || !_currentUser.Identity.IsAuthenticated)
                {
                    try
                    {
                        HttpCookie authCookie = httpContext.Request.Cookies.Get(cookiename);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            _currentUser = new UserProvider(_roleRepository, _accountRepository, ticket.Name);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Failed authentication: " + ex.Message);
                        _currentUser = new UserProvider(null, null, null);
                    }
                }
                
                else if (_currentUser != null)
                {
                    try
                    {
                        HttpCookie authCookie = httpContext.Request.Cookies.Get(cookiename);
                        if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
                        {
                            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                            if (ticket.Name != _currentUser.Identity.Name)
                                _currentUser = new UserProvider(null, null, null);
                        }
                        else
                        {
                            _currentUser = new UserProvider(null, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Failed authentication: " + ex.Message);
                        _currentUser = new UserProvider(null, null, null);
                    }
                }
                return _currentUser;
            }
        }
    }
}
