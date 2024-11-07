using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationDemo.Controllers {
    // [AllowAnonymous] can be accessed by anyone
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase {

        [Authorize(Roles ="User")]
        [HttpGet]       
        public async Task<string> GetSampleData() {
            return "Sample data from sample controller";
        }
    
    }
}

// postman->authorization->bearer token->copy token from swagger after login and paste
// authorization