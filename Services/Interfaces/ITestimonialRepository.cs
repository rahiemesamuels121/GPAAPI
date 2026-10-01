using GPACARICOMAPI.Models;

namespace GPACARICOMAPI.Services.Interfaces
{
    public interface ITestimonialRepository
    {
        public Task<List<TestimonialModel>> GetTestimonialsAsync();
        public Task<TestimonialModel?> GetTestimonialbyId(int id);
        public Task<bool> AddNewTestimonial(TestimonialModel testimonial);
        public Task<bool> UpdateTestimonial(TestimonialModel testimonial);
        public Task<bool> DeleteTestimonial(int id);
    }
}
