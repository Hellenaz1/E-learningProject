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

        public List<Course> GetAllCourses()
        {
            return _courseRepository.GetAllCourses();
        }
        public (bool OK, string? Error) AddCourse(Course course)
        {
            try
            {
               
                _context.Courses.Add(course);
                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        public (bool OK, string? Error) DeleteCourse(int courseId)
        {
            bool deleted = _courseRepository.DeleteCourse(courseId);
            return deleted ? (true, null) : (false, "Không tìm thấy khóa học để xóa!");
        }
        public (bool OK, string? Error) UpdateCourse(Course updatedCourse)
        {
            try
            {
                var existing = _context.Courses.FirstOrDefault(c => c.CourseId == updatedCourse.CourseId);
                if (existing == null)
                    return (false, "Không tìm thấy khóa học cần cập nhật.");

                // Cập nhật các trường cần thiết
                existing.Title = updatedCourse.Title;
                existing.Description = updatedCourse.Description;
                existing.Language = updatedCourse.Language;
                existing.Level = updatedCourse.Level;
                existing.CategoryId = updatedCourse.CategoryId;

                _context.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


    }
}
