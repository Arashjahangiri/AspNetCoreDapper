using AspNetCoreDapper.Model.DTO;
using AspNetCoreDapper.Model.Entities;

namespace AspNetCoreDapper.Model.Repositories
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetCompanies();
        public Task<Company> GetById(int id);
        public Task<Company> GetCompanyEmployeesMultipleResults(int id);
        public Task<List<Company>> GetCompaniesEmployeesMultipleMapping();
        public Task<Company> Sp_GetCompanyByEmployeeId(int id);

        public Task CreateCompany(CompanyForCreationDto  company);
        public Task<Company> CreateCompanyWithResult(CompanyForCreationDto company);
        public Task CreateMultipleCompanies(List<CompanyForCreationDto> companies);


        public Task UpdateCompany(int id, CompanyForUpdateDto company);
        public Task DeleteCompany(int id);




    }
}
