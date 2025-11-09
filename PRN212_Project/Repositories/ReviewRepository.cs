using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project.Repositories
{
    internal class ReviewRepository
    {
        private PrnProjectContext _context = new PrnProjectContext();
        public List<Review> GetAllReviewByCourse(int courseId) =>
            _context.Reviews.Include(r => r.Student).Where(c => c.CourseId == courseId).
            OrderByDescending(r => r.CreatedAt).ToList();

        public Review? GetReviewByCourseAndStudent(int courseId, int studentId) =>
            _context.Reviews.FirstOrDefault(r => r.CourseId == courseId && r.StudentId == studentId);

        public List<Review> FilterReviews(int courseId, string? keyword, int? star)
        {
            var q = _context.Reviews.Include(r => r.Student).Where(r => r.CourseId == courseId);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var k = keyword.ToLower().Trim();
                q = q.Where(r => r.Comment.ToLower().Contains(k));
            }

            if (star.HasValue) {
                q = q.Where(q => q.Rating == star);
            }

            return q.OrderByDescending(r => r.CreatedAt).ToList();
        }

        public (bool OK, string? Error) Insert(int courseId, int studentId, int rating, string? comment)
        {
            if (rating < 1 || rating > 5)
                return (false, "Rating phải từ 1 đến 5");

            if (comment != null && comment.Length > 500)
                return (false, "Bình luận quá dài");

            var r = new Review
            {
                CourseId = courseId,
                StudentId = studentId,
                Rating = rating,
                Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Reviews.Add(r);
            _context.SaveChanges();
            return (true, null);
        }

        public (bool OK, string? Error) Update(int courseId, int studentId, int rating, string? comment)
        {
            if (rating < 1 || rating > 5)
                return (false, "Rating phải từ 1 đến 5");

            if (comment != null && comment.Length > 500)
                return (false, "Bình luận quá dài"); ;

            var exist = GetReviewByCourseAndStudent(courseId, studentId);
            if (exist == null)
                return (false, "Không tìm thấy review để cập nhật");

            exist.Rating = rating;
            exist.Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

            _context.SaveChanges();
            return (true, null);

        }

    }
}
