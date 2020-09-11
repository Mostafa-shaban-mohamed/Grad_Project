using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Grad_Project.Models
{
    public class UsersRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        public override string[] GetRolesForUser(string username)
        {
            using (LMSDBEntities db = new LMSDBEntities())
            {
                var userRoles = new List<string>();
                if (db.Student_tbl.Any(user => user.Email == username))
                {
                    userRoles.Add("Student");
                }
                else if (db.Lecturer_tbl.Any(user => user.Email == username))
                {
                    userRoles.Add("Lecturer");
                }
                else if (db.Admin_tbl.Any(user => user.Email == username))
                {
                    userRoles.Add("Admin");
                }

                return userRoles.ToArray();
            }
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}