﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SampleASPEF.Data;
using SampleASPEF.Models;

namespace SampleASPEF.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            //var models = await _context.Students.OrderBy(s=>s.FirstMidName).ToListAsync();
            var models = await (from s in _context.Students
                                orderby s.FirstMidName descending
                                select s).ToListAsync();
            
            return View(models);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Students
            //    .FirstOrDefaultAsync(m => m.ID == id);
            var student = await (from s in _context.Students.Include(s=>s.Enrollments)
                                 .ThenInclude(e=>e.Course)
                                 where s.ID==id
                                 select s).AsNoTracking().FirstOrDefaultAsync();
            
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> InsertMultiple()
        {
            try
            {
                var newStudent = new Student
                {
                    FirstMidName = "Budi",
                    LastName = "Kurniawan",
                    Address = "Jl Mangga",
                    EnrollmentDate = DateTime.Now
                };

                _context.Students.Add(newStudent);


                var newCourse = new Course
                {
                    CourseID = 1045,
                    Credits = 4,
                    Title = "Data Science"
                };

                _context.Courses.Add(newCourse);

                var newEnrollment = new Enrollment
                {
                    CourseID = 1050,
                    StudentID = 2,
                    Grade = Grade.A
                };

                _context.Enrollments.Add(newEnrollment);

               

                await _context.SaveChangesAsync();

                return Content("Data berhasil ditambahkan !");

            }
            catch (DbUpdateException dbEx)
            {
                return Content(dbEx.Message);
            }
        }
        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstMidName,EnrollmentDate,Address")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("","Gagal menambahkan Student");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Students.FindAsync(id);
            var student = await (from s in _context.Students
                                where s.ID == id
                                select s).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,EnrollmentDate,Address")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
