using GA360.DAL.Entities.Entities;

namespace GA360.Server.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int Duration { get; set; }
        public DateTime CertificateDate { get; set; }
        public string CertificateNumber { get; set; }
        public int Status { get; set; }
    }

    public static class CourseMapper
    {
        public static CourseViewModel ToViewModel(this Course course)
        {
            return new CourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                RegistrationDate = course.RegistrationDate,
                ExpectedDate = course.ExpectedDate,
                Duration = course.Duration,
                CertificateDate = course.CertificateDate,
                CertificateNumber = course.CertificateNumber,
                Status = course.Status
            };
        }

        public static Course ToEntity(this CourseViewModel courseViewModel)
        {
            return new Course
            {
                Id = courseViewModel.Id,
                Name = courseViewModel.Name,
                Description = courseViewModel.Description,
                RegistrationDate = courseViewModel.RegistrationDate,
                ExpectedDate = courseViewModel.ExpectedDate,
                Duration = courseViewModel.Duration,
                CertificateDate = courseViewModel.CertificateDate,
                CertificateNumber = courseViewModel.CertificateNumber,
                Status = courseViewModel.Status,
                QualificationCustomerCourseCertificates = new List<QualificationCustomerCourseCertificate>() // Initialize with an empty list or map accordingly
            };
        }
    }
}
