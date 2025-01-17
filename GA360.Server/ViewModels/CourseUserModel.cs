namespace GA360.Server.ViewModels
{
    public class CourseUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime CertificateDate { get; set; }
        public string CertificateNumber { get; set; }
        public double Price { get; set; }
    }
}
