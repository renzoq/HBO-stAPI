﻿using AutoMapper;
using PoohAPI.Common;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.Common.Repositories;
using PoohAPI.Infrastructure.UserDB.Models;
using System.Collections.Generic;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public class UserRepository : MySQLBaseRepository, IUserRepository
    {
        private IMapper _mapper;

        public UserRepository(IMapper mapper, IMySQLClient client) : base(mapper, client)
        {
            _mapper = mapper;
        }

        public IEnumerable<WPUser> GetAllUsers(string query)
        {
            return GetAll<WPUser>(query);
        }

        public WPUser GetUser(string query)
        {
            return GetSingle<WPUser>(query);
        }

        public DBUser GetUser(string query, Dictionary<string, object> parameters)
        {
            return GetSingle<DBUser>(query, parameters);
        }

        public void UpdateUser(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }

        public void DeleteUser(string query, Dictionary<string, object> parameters)
        {
            NonQuery(query, parameters);
        }
    }
}
