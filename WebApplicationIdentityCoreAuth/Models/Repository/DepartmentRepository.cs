using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationIdentityCoreAuth.Data;

namespace WebApplicationIdentityCoreAuth.Models.Repository
{

    public interface IDepartment
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartment(int id);
    }
    public class DepartmentRepository:IDepartment
    {
        private readonly ApplicationDbContext _appDbContext;

        public DepartmentRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Department GetDepartment(int id)
        {
            return _appDbContext.Departments.FirstOrDefault(d => d.DepartmentId == id);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _appDbContext.Departments;
        }

    }
}
