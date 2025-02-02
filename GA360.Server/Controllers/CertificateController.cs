using GA360.DAL.Entities.Entities;
using GA360.Domain.Core.Interfaces;
using GA360.Domain.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static GA360.Domain.Core.Interfaces.IAuditTrailService;

namespace GA360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificateController : ControllerBase
    {
        private readonly ILogger<CertificateController> _logger;
        private readonly ICertificateService _certificateService;
        private readonly IPermissionService _permissionService;
        private readonly IMemoryCache _cache;
        private readonly IAuditTrailService _auditTrailService;

        private const string CertificatesCacheKey = "certificatesCache";

        public CertificateController(ICertificateService certificateService, IPermissionService permissionService, IMemoryCache cache, IAuditTrailService auditTrailService)
        {
            _certificateService = certificateService;
            _permissionService = permissionService;
            _cache = cache;
            _auditTrailService = auditTrailService;
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

            await _auditTrailService.InsertAudit(AuditTrailArea.Certificates, AuditTrailType.Information, "Retrieved all certificates.", emailClaim);

            return Ok(certificates);
        }
    }
}
