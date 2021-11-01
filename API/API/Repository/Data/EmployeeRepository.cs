using API.Context;
using API.Models;
using API.Repository.Interface;
using API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;

        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public EmployeeRepository(MyContext context) : base(context)
        {
            this.context = context;
        }

        public int Register(RegisterVM registerVM)
        {
            
            Employee employee = new Employee()
            {
                NIK = registerVM.NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Models.Gender)registerVM.Gender
            };
            var cekNik = context.Employees.Find(registerVM.NIK);
            var cekPhone = context.Employees.Where(e => e.Phone == registerVM.Phone).FirstOrDefault();
            var cekEmail = context.Employees.Where(e => e.Email == registerVM.Email).FirstOrDefault();
            if(cekNik != null)
            {
                return 2;
            }
            if (cekPhone != null)
            {
                return 3;
            }
            if (cekEmail != null)
            {
                return 4;
            }
            context.Employees.Add(employee);
            context.SaveChanges();

            Account account = new Account()
            {
                NIK = employee.NIK,
                Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password, GetRandomSalt())
            };
            context.Accounts.Add(account);
            context.SaveChanges();

            Education education = new Education()
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                IdUniversity = registerVM.IdUniversity
            };
            context.Educations.Add(education);
            context.SaveChanges();

            Profiling profiling = new Profiling()
            {
                NIK = employee.NIK,
                IdEducation = education.IdEducation
            };
            context.Profilings.Add(profiling);
            var result = context.SaveChanges();

            int cekRole = registerVM.IdRole;
            int idRole;
            if(cekRole == 0 || cekRole == 1)
            {
                idRole = 1;
            }
            else if(cekRole == 2)
            {
                idRole = 2;
            }
            else
            {
                idRole = 3;
            }

            AccountRole accountRole = new AccountRole()
            {
                NIK = account.NIK,
                IdRole = idRole
            };
            context.AccountRoles.Add(accountRole);
            context.SaveChanges();
            
            return result;
        }

        public IEnumerable<RegisterVM> GetProfile()
        {
            var getProfile = (from e in context.Employees
                              join a in context.Accounts on e.NIK equals a.NIK
                              join p in context.Profilings on a.NIK equals p.NIK
                              join edu in context.Educations on p.IdEducation equals edu.IdEducation
                              join u in context.Universities on edu.IdUniversity equals u.IdUniversity
                              join acr in context.AccountRoles on a.NIK equals acr.NIK
                              join r in context.Roles on acr.IdRole equals r.IdRole
                              select new RegisterVM
                              {
                                  NIK = e.NIK,
                                  FullName = $"{e.FirstName} {e.LastName}",
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  Phone = e.Phone,
                                  BirthDate = e.BirthDate,
                                  Salary = e.Salary,
                                  Email = e.Email,
                                  Gender = (ViewModel.Gender)e.Gender,
                                  Password = a.Password,
                                  Degree = edu.Degree,
                                  GPA = edu.GPA,
                                  IdUniversity = u.IdUniversity,
                                  NamaUniversity = u.NamaUniversity,
                                  IdRole = r.IdRole
                              });

            return getProfile.ToList();
        }

        public RegisterVM GetProfileNIK(string NIK)
        {
            var getProfile = (from e in context.Employees
                              join a in context.Accounts on e.NIK equals a.NIK
                              join p in context.Profilings on a.NIK equals p.NIK
                              join edu in context.Educations on p.IdEducation equals edu.IdEducation
                              join u in context.Universities on edu.IdUniversity equals u.IdUniversity
                              join acr in context.AccountRoles on a.NIK equals acr.NIK
                              join r in context.Roles on acr.IdRole equals r.IdRole
                              orderby e.NIK
                              select new RegisterVM
                              {
                                  NIK = e.NIK,
                                  FullName = $"{e.FirstName} {e.LastName}",
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  Phone = e.Phone,
                                  BirthDate = e.BirthDate,
                                  Salary = e.Salary,
                                  Email = e.Email,
                                  Gender = (ViewModel.Gender)e.Gender,
                                  Password = a.Password,
                                  Degree = edu.Degree,
                                  GPA = edu.GPA,
                                  IdUniversity = u.IdUniversity,
                                  NamaUniversity = u.NamaUniversity,
                                  IdRole = r.IdRole
                              }).Where(emp => emp.NIK == NIK).FirstOrDefault();

            return getProfile;
        }

        public int LoginAutentifikasi(LoginVM loginVM)
        {
            /*if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                return null;*/

            var cekEmail = context.Employees.Where(x => x.Email == loginVM.Email).FirstOrDefault();

            // check if username exists
            if (cekEmail == null)
            {
                return 2;
            }

            var cekPass = context.Accounts.Find(cekEmail.NIK);

            // check if password is correct
            bool validPass = BCrypt.Net.BCrypt.Verify(loginVM.Password, cekPass.Password); 
            if (validPass)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        public int Sign(LoginVM loginVM)
        {
            var cekEmail = context.Employees.Where(e => e.Email == loginVM.Email).FirstOrDefault();
            if(cekEmail != null)
            {
                var getNik = cekEmail.NIK;
                var getPass = context.Accounts.Find(getNik);
                string pass = getPass.Password;
                bool validPass = BCrypt.Net.BCrypt.Verify(loginVM.Password, getPass.Password);
                if (validPass == true)
                {
                    return 1;
                }
                else
                {
                    var result = 0;
                    return result;
                }
            }
            else
            {
                var result = 2;
                return result;
            }
        }

        public string[] GetRole(string email)
        {
            var getData = context.Employees.Where(e => e.Email == email).FirstOrDefault();
            var getRole = (from acr in context.AccountRoles
                           join r in context.Roles
                           on acr.IdRole equals r.IdRole
                           select new
                           {
                               NIK = acr.NIK,
                               RoleName = r.NamaRole
                           }).Where(acr => acr.NIK == getData.NIK).ToList();
            List<string> result = new List<string>();
            foreach (var item in getRole)
            {
                result.Add(item.RoleName);
            }
            return result.ToArray();
        }

        public int AddManager(AccountRole accountRole)
        {
            try
            {
                context.AccountRoles.Add(accountRole);
                var result = context.SaveChanges();
                return result;
            }
            catch
            {

                return 0;
            }
        }
    }
}
