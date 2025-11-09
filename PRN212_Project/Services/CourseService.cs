using PRN212_Project.Models;
using PRN212_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project.Services
{
    public class CourseService
    {
        private CourseRepository _courseRepository = new CourseRepository();
        private PrnProjectContext _context = new PrnProjectContext();

        public List<Enrollment> GetEnrolledCourses(int studentId, string? keyword, int? categoryId, string? level, string? language, string? sortTag)
        {
           return _courseRepository.GetEnrolledCourses(studentId, keyword, categoryId, level, language, sortTag);
        }

        public List<Category> GetAllCategories() => _courseRepository.GetAllCategories();

        public List<string> GetAllLevel() => _courseRepository.GetAllLevel();
    
        public List<String> GetAllLanguages() => _courseRepository.GetAllLanguages();

        public Course GetCourseById(int courseId) => _courseRepository.GetCourseById(courseId);

        public bool ExistEnroll(int studentId, int courseId) => _courseRepository.ExistEnroll(studentId, courseId);

    }
}
