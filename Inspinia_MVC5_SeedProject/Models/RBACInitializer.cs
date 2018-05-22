namespace phoebe.DatabaseInitializer
{
    using Microsoft.AspNet.Identity;
    using phoebe.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    public class RBACDatabaseInitializer : CreateDatabaseIfNotExists<RBACDbContext>
    {
        private readonly string c_SysAdmin = "System Administrator";
        private readonly string c_DefaultUser = "Default User";

        protected override void Seed(RBACDbContext context)
        {
            //Create Default Roles...
            IList<ApplicationRole> defaultRoles = new List<ApplicationRole>();
            defaultRoles.Add(new ApplicationRole { Name = c_SysAdmin, RoleDescription = "Allows system administration of Users/Roles/Permissions", LastModified = DateTime.Now, IsSysAdmin = true });
            defaultRoles.Add(new ApplicationRole { Name = c_DefaultUser, RoleDescription = "Default role with limited permissions", LastModified = DateTime.Now, IsSysAdmin = false });

            ApplicationRoleManager RoleManager = new ApplicationRoleManager(new ApplicationRoleStore(context));
            foreach (ApplicationRole role in defaultRoles)
            {                
                RoleManager.Create(role);
            }              
            
            //Create User...
            var user = new ApplicationUser { UserName = "Admin", Email = "ancgate@aim.com", Firstname = "System", Lastname = "Administrator", LastModified = DateTime.Now, Inactive = false, EmailConfirmed = true };

            ApplicationUserManager UserManager = new ApplicationUserManager(new ApplicationUserStore(context));
            var result = UserManager.Create(user, "root");

            if (result.Succeeded)
            {
                //Add User to Admin Role...
                UserManager.AddToRole(user.Id, c_SysAdmin);                
            }


            //Create Default User...
            user = new ApplicationUser { UserName = "DefaultUser", Email = "ancgate@yahoo.fr", Firstname = "Default", Lastname = "User", LastModified = DateTime.Now, Inactive = false, EmailConfirmed = true };
            result = UserManager.Create(user, "root");            

            if (result.Succeeded)
            {
                //Add User to Admin Role...
                UserManager.AddToRole(user.Id, c_DefaultUser);
            }

            //Create User with NO Roles...
            user = new ApplicationUser { UserName = "Guest", Email = "guest@somedomain.com", Firstname = "Guest", Lastname = "User", LastModified = DateTime.Now, Inactive = false, EmailConfirmed=true };
            result = UserManager.Create(user, "root");

          
            base.Seed(context);

            //Create a permission...
            PERMISSION _permission = new PERMISSION();
            _permission.PermissionDescription = "Home-Reports";
            ApplicationRoleManager.AddPermission(_permission);

            //Add Permission to DefaultUser Role...
            ApplicationRoleManager.AddPermission2Role(context.Roles.Where(p=>p.Name == c_DefaultUser).First().Id, context.PERMISSIONS.First().PermissionId);
        }        
    }
}
