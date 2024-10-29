using AspNetCoreDapper.Model.Context;
using AspNetCoreDapper.Model.DTO;
using AspNetCoreDapper.Model.Entities;
using AspNetCoreDapper.Model.Repositories;
using Dapper;
using System.Data;
using System.Data.Common;

namespace AspNetCoreDapper.Model.Services
{
    public class CompanyRepository:ICompanyRepository
    {
        private readonly DapperContext dapperContext;

        public CompanyRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }
        public async Task<IEnumerable<Company>> GetCompanies()
        {
            var query = "select * from Company";
            using (var connection = dapperContext.CreateConnection())
            {
                var companies = await connection.QueryAsync<Company>(query);
                return companies.ToList();
            }
        }
        public async Task<Company> GetById(int id)
        {
            var query = "select * from company where Id=@Id";
            using (var connection=dapperContext.CreateConnection())
            {
                var company=await connection.QuerySingleOrDefaultAsync<Company>(query,new { Id=id});
                return company;

            }
        }

        public async Task<List<Company>> GetCompaniesEmployeesMultipleMapping()
        {
            var query = "Select * from Company c Join Employee e ON  c.id=e.CompanyId";
            using (var connection = dapperContext.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();
                var companies = await connection.QueryAsync<Company, Employee, Company>(
                    query, (company, employee) =>
                    {
                        if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                        {
                            currentCompany = company;
                            companyDict.Add(currentCompany.Id, currentCompany);
                        }
                        currentCompany.Employees.Add(employee);
                        return currentCompany;
                    }
                );
                return companies.Distinct().ToList();
            }
        }

        public async Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            var query = "Select * from Company where id=@Id;" +
                       "Select * from Employee where CompanyId=@Id";
 
            using (var connection=dapperContext.CreateConnection())
            {
                using (var multi= await connection.QueryMultipleAsync(query,new { Id=id}))
                {
                    var comapny = await multi.ReadSingleOrDefaultAsync<Company>();
                    if (comapny!=null)
                    {

                        comapny.Employees = (await multi.ReadAsync<Employee>()).ToList();   
                    }
                    return comapny;


                }

            }

        }

        public async Task<Company> Sp_GetCompanyByEmployeeId(int id)
        {
            var procedureName = "Sp_GetCompanyByEmployeeId";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

            using (var connection=dapperContext.CreateConnection())
            {
                var company = await connection.QueryFirstOrDefaultAsync<Company>(procedureName, parameters,commandType:CommandType.StoredProcedure);
                return company;
            }
        }

        public async Task CreateCompany(CompanyForCreationDto company)
        {
            var query = "Insert Into Company(Name,Address,Country) Values(@Name,@Address,@Country)";
            var parameters = new DynamicParameters();
            parameters.Add("@Name", company.Name, DbType.String);
            parameters.Add("@Address", company.Address, DbType.String);
            parameters.Add("@Country", company.Country, DbType.String);

            using (var connection = dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Company> CreateCompanyWithResult(CompanyForCreationDto company)
        {
            var query = "Insert into Company (Name,Address,Country) Values(@Name,@Address,@Country) " +
                "Select Cast(SCOPE_IDENTITY() As int)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", company.Name, DbType.String);
            parameters.Add("Address", company.Address, DbType.String);
            parameters.Add("Country", company.Country, DbType.String);

            using (var connection = dapperContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var companyCreation = new Company
                {
                    Id = id,
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country
                };
                return companyCreation;

            }

        }

        public async Task CreateMultipleCompanies(List<CompanyForCreationDto> companies)
        {
            var query = "INSERT INTO Company (Name, Address, Country) VALUES (@Name, @Address, @Country)";
            using (var connection = dapperContext.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var company in companies)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("Name", company.Name, DbType.String);
                        parameters.Add("Address", company.Address, DbType.String);
                        parameters.Add("Country", company.Country, DbType.String);
                        await connection.ExecuteAsync(query, parameters, transaction: transaction);
                    }
                    transaction.Commit();
                }
            }
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company)
        {
            var query = "Update Company set Name=@Name,Address=@Address,Country=@Country where Id=@Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Name",company.Name);
            parameters.Add("@Address", company.Address);
            parameters.Add("@Country", company.Country);
            parameters.Add("@Id", id);

            using (var connection=dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(query,parameters);
            }
        }

        public async Task DeleteCompany(int id)
        {
            var query = "Delete From Company where Id=@Id";
            using (var connection = dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
