using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationIdentityCoreAuth.Data;
using WebApplicationIdentityCoreAuth.Models;
using WebApplicationIdentityCoreAuth.Models.Repository;
using WebApplicationIdentityCoreAuth.Models.ViewModel;

namespace WebApplicationIdentityCoreAuth.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeerepository;
        private readonly IDepartment _departmentrepository;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IEmployee employeerepository, IDepartment departmentrepository,
                                    ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _employeerepository = employeerepository;
            _departmentrepository = departmentrepository;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Employee
        public IActionResult Index()
        {
            return View( _employeerepository.GetEmps());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee =await _employeerepository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {

            ViewBag.Depts = new SelectList(_departmentrepository.GetDepartments(), "DepartmentId", "DepartmentName");

            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UniqueFileName = null;
                //if (model.Photo != null&&model.Photos.Count > 0)
                //{
                //    foreach (IFormFile Photo in model.Photos)
                //    {
                //        string UploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                //        UniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                //        string filePath = Path.Combine(UploadsFolder, UniqueFileName);
                //        Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                //    }
                //}

                    if (model.Photo != null)
                {
                    string UploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    //UniqueFileName = Path.Combine(Guid.NewGuid().ToString(), "_", model.Photo.FileName);
                    UniqueFileName = Guid.NewGuid().ToString() +  "_" + model.Photo.FileName;
                    string filePath = Path.Combine(UploadsFolder, UniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));

                }

                Employee emp = new Employee
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DepartmentId = model.DepartmentId,
                    Email = model.Email,
                    PhotoPath = UniqueFileName
                };
                //_employeerepository.AddEmployee(employee);
                _employeerepository.AddEmployee(emp);

                return RedirectToAction(nameof(Index));
            }
            ViewBag.Depts = new SelectList(_departmentrepository.GetDepartments(), "DepartmentId", "DepartmentName",model.DepartmentId);

            return View(model);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeerepository.GetEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewBag.Depts = new SelectList(_departmentrepository.GetDepartments(), "DepartmentId", "DepartmentName",employee.DepartmentId);

            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,DateOfBirth,DepartmentId,Gender,PhotoPath")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeerepository.UpdateEmployee(employee);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Depts = new SelectList(_departmentrepository.GetDepartments(), "DepartmentId", "DepartmentName", employee.DepartmentId);

            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeerepository.GetEmployee(id);
              
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

           
            _employeerepository.DeleteEmployee(id);
           
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
