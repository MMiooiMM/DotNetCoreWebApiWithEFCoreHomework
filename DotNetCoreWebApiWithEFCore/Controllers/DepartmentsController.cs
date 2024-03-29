﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreWebApiWithEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreWebApiWithEFCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public DepartmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            return await _context.Department.Where(x => !x.IsDeleted).ToListAsync();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Department.Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.DepartmentId == id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            var oldDepartment = await _context.Department.FindAsync(id);
            if (oldDepartment == null)
            {
                return NotFound();
            }

            var rowversion = (await _context.Department
                .FromSqlInterpolated(
                    $"EXEC [dbo].[Department_Update] {id}, {department.Name}, {department.Budget}, {department.StartDate}, {department.InstructorId}, {oldDepartment.RowVersion};")
                .Select(x => x.RowVersion)
                .ToListAsync())
                .First();

            if (rowversion == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            var result = (await _context.Department
                .FromSqlInterpolated(
                    $"EXEC [dbo].[Department_Insert] {department.Name}, {department.Budget}, {department.StartDate}, {department.InstructorId};")
                .Select(x => x.DepartmentId)
                .ToListAsync())
                .First();
            await _context.SaveChangesAsync();

            department.DepartmentId = result;

            return CreatedAtAction("GetDepartment", new { id = result }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> DeleteDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.IsDeleted = true;

            await _context.SaveChangesAsync();

            return department;
        }

        // GET: api/Departments/vw/DepartmentCourseCount
        [HttpGet("vw/DepartmentCourseCount")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetvwDepartmentCourseCount()
            => await _context.VwDepartmentCourseCount.FromSqlRaw("SELECT * FROM vwDepartmentCourseCount").ToListAsync();

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }
    }
}