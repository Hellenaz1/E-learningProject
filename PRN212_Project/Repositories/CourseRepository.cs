using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project.Repositories
{
    public class CourseRepository
    {
        private PrnProjectContext _context;

        public CourseRepository()
        {
            _context = new PrnProjectContext();
        }


        public List<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public List<string> GetAllLevel()
        {
            return _context.Courses.Select(c => c.Level!).Distinct().OrderBy(s => s).ToList();
        }

        public List<String> GetAllLanguages()
        {
            return _context.Courses.Select(c => c.Language!).Distinct().OrderBy(s => s).ToList();
        }

        public List<Enrollment> GetEnrolledCourses(
            int studentId, string? keyword, int? categoryId, string? level, string? language, string? sortTag
            )
        {
            var q = _context.Enrollments.
                Include(e => e.Course).
                ThenInclude(c => c.Category).
                Where(e => e.StudentId == studentId);

            //filter
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim();
                q = q.Where(e => e.Course.Title.Contains(kw));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
                q = q.Where(e => e.Course.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(level) && level != "Tất cả")
                q = q.Where(e => e.Course.Level == level);

            if (!string.IsNullOrWhiteSpace(language) && language != "Tất cả")
                q = q.Where(e => e.Course.Language == language);

            //sort
                q = sortTag switch
                {
                    "enrolledAsc" => q.OrderBy(e => e.EnrolledAt),
                    "enrolledDesc" => q.OrderByDescending(e => e.EnrolledAt),
                    "titleAsc" => q.OrderBy(e => e.Course.Title),
                    "titleDesc" => q.OrderByDescending(e => e.Course.Title),
                    _ => q.OrderBy(e => e.EnrolledAt)
                };

            return q.ToList();
        }
    }
}
