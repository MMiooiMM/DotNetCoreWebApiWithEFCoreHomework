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
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse()
        {
            return await _context.Course.Where(x => !x.IsDeleted).ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Course.Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            course.IsDeleted = true;

            await _context.SaveChangesAsync();

            return course;
        }

        // GET: api/Courses/vw/CourseStudents
        [HttpGet("vw/CourseStudents")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetvwCourseStudents()
            => await _context.VwCourseStudents.ToListAsync();

        // GET: api/Courses/vw/CourseStudentCount
        [HttpGet("vw/CourseStudentCount")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetvwCourseStudentCount()
            => await _context.VwCourseStudentCount.ToListAsync();

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}