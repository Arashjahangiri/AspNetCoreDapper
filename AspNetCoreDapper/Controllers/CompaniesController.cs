using AspNetCoreDapper.Model.DTO;
using AspNetCoreDapper.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;

        public CompaniesController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var companies = await companyRepository.GetCompanies();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //Log Error
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var comapny = await companyRepository.GetById(id);
                if (comapny==null)
                {
                    return NotFound();
                }
                return Ok(comapny);

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }

        }

        [HttpGet("Sp_GetCompanyByEmployeeId/{id}")]
        public async Task<IActionResult> GetCompanyByEmployeeId(int id)
        {
            try
            {
                var company = await companyRepository.Sp_GetCompanyByEmployeeId(id);
                if (company == null)
                    return NotFound();
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }

        }


        [HttpGet("GetCompanyEmployeesMultipleResults/{id}")]
        public async Task<IActionResult> GetCompanyEmployeesMultipleResults(int id)
        {
            try
            {
                var company = await companyRepository.GetCompanyEmployeesMultipleResults(id);
                if (company == null)
                {
                    return NotFound();
                }
                return Ok(company);


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet("MultipleMapping")]
        public async Task<IActionResult> GetCompaniesEmployeesMultipleMapping()
        {
            try
            {
                var company = await companyRepository.GetCompaniesEmployeesMultipleMapping();
                return Ok(company);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("CreateCompany")]
        public async Task<IActionResult> PostCreateCompany(CompanyForCreationDto company)
        {
            try
            {
                await companyRepository.CreateCompany(company);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }
        }

        [HttpPost("CreateCompanyWithResult")]
        public async Task<IActionResult> PostCreateCompanyWithResult(CompanyForCreationDto company)
        {
            try
            {
                var createCompany = await companyRepository.CreateCompanyWithResult(company);
                return CreatedAtAction("GetById",new {id=createCompany.Id },createCompany);
            }
            catch (Exception ex)
            {

                return StatusCode(500,ex.ToString());
            }

        }

        [HttpPost("CreateMultipleCompanies")]
        public async Task<IActionResult> CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            try
            {
                await companyRepository.CreateMultipleCompanies(companies);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComapny(int id ,CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany=await companyRepository.GetById(id);
                if (dbCompany==null)
                {
                    return NotFound();
                }
                await companyRepository.UpdateCompany(id, company);
                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.ToString());
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var dbCompany= await companyRepository.GetById(id);
                if (dbCompany==null)
                {
                    return NotFound();
                }
                await companyRepository.DeleteCompany(id);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500,ex.ToString());
            }
        }


     
    }
}
