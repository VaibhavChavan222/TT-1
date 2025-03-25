using System.Collections.Generic;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public void Update(User user, string name, string email, UserTypes type, decimal? annualSalary, IEnumerable<string> tags)
        {
            user.SetEmail(email);
            user.SetName(name);
            user.SetType(type);
            if (annualSalary.HasValue)
            {
                user.SetMonthlySalary(annualSalary.Value / 12);
            }
            else
            {
                //Task 1 : Handle the case where annualSalary is null then set a default value.
                user.SetMonthlySalary(1000); // Example default value
            }
            user.SetTags(tags);
        }
    }
}