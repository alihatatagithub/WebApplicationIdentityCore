using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationIdentityCoreAuth.Data;

namespace WebApplicationIdentityCoreAuth.Models.Repository
{

    public interface IEmployee
    {
        Task<IEnumerable<Employee>> SearchEmployee(string name, Gender? gender);

        IEnumerable<Employee> GetEmps();
        Task<Employee> GetEmployee(int? employeeid);
        Task<Employee> GetEmployeeByEmail(string email);

        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int employeeid);
    }


    public class EmployeeRepository : IEmployee
    {
        private ApplicationDbContext _appDbContext;

        public EmployeeRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public void AddEmployee(Employee employee)
        {
            var result =  _appDbContext.Employees.Add(employee);
             _appDbContext.SaveChanges();
            

        }
        public async Task<Employee> GetEmployeeByEmail(string email)
        {
            return await _appDbContext.Employees.FirstOrDefaultAsync(e => e.Email == email);

        }

        public void DeleteEmployee(int employeeid)
        {
            var result =  _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeid);
            
                _appDbContext.Employees.Remove(result);
                 _appDbContext.SaveChanges();
            
            

        }

        public async Task<Employee> GetEmployee(int? employeeid)
        {
            return await _appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeid);
        }

        public IEnumerable<Employee> GetEmps()
        {
            return _appDbContext.Employees.ToList();
        }

        public void UpdateEmployee(Employee newemployee)
        {
            var result = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == newemployee.EmployeeId);
           
                result.FirstName = newemployee.FirstName;
                result.LastName = newemployee.LastName;
                result.Email = newemployee.Email;
                result.DateOfBirth = newemployee.DateOfBirth;
                result.Gender = newemployee.Gender;
                result.DepartmentId = newemployee.DepartmentId;
                result.PhotoPath = newemployee.PhotoPath;
                 _appDbContext.SaveChanges();

           


        }

        public async Task<IEnumerable<Employee>> SearchEmployee(string name, Gender? gender)
        {

            IQueryable<Employee> query = _appDbContext.Employees;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
            if (gender != null)
            {
                query = query.Where(e => e.Gender == gender);

            }
            return await query.ToListAsync();
        }
    }

}
