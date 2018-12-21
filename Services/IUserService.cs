using BibleStudyColabApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleStudyColabApi.Services
{
   public interface IUserService
   {
      User Authenticate(string username, string password);
      IEnumerable<User> GetAll();
   }
}
