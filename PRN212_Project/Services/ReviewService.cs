using PRN212_Project.Models;
using PRN212_Project.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Project.Services
{
    internal class ReviewService
    {
        private ReviewRepository _reviewRepository = new ReviewRepository();

        public List<Review> GetAllReviewByCourse(int courseId) => _reviewRepository.GetAllReviewByCourse(courseId);

        public Review? GetReviewByCourseAndStudent(int courseId, int studentId)
            => _reviewRepository.GetReviewByCourseAndStudent(courseId, studentId);

        public List<Review> FilterReviews(int courseId, string? keyword, int? star)
            => _reviewRepository.FilterReviews(courseId, keyword, star);

        public (bool OK, string? Error) AddReview(int courseId, int studentId, int rating, string? comment)
            => _reviewRepository.Insert(courseId, studentId, rating, comment);

        public (bool OK, string? Error) UpdateReview(int courseId, int studentId, int rating, string? comment)
            => _reviewRepository.Update(courseId, studentId, rating, comment);
    }
}
