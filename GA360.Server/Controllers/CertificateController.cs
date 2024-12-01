using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICertificateService _certificateService;
        private readonly IPermissionService _permissionService;
        private readonly IMemoryCache _cache;

        private const string CertificatesCacheKey = "certificatesCache";

        public CertificateController(ICertificateService certificateService, IPermissionService permissionService, IMemoryCache cache)
        {
            _certificateService = certificateService;
            _permissionService = permissionService;
            _cache = cache;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCertificates()
        {
            var emailClaim = User?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

            if (!_cache.TryGetValue(CertificatesCacheKey, out List<Certificate> certificates))
            {
                certificates = await _certificateService.GetAllCertificates();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(CertificatesCacheKey, certificates, cacheEntryOptions);
            }

            var certificatesPermissions = await _permissionService.FilterPermissions(emailClaim, certificates);

            return Ok(certificatesPermissions);
        }

    
    }
}
