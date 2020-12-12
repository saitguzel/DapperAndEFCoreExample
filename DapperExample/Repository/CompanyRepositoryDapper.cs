using Dapper;
using DapperExample.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DapperExample.Repository
{
    public class CompanyRepositoryDapper : ICompanyRepository
    {
        private IDbConnection db;
        public CompanyRepositoryDapper(IConfiguration configuration)
        {
            db = new SqlConnection(configuration.GetConnectionString("DefaultCS"));
        }

        public Company Add(Company company)
        {
            string sql = @"INSERT INTO [dbo].[Companies] ([Name] ,[Address] ,[City] ,[State] ,[PostalCode]) VALUES (@Name, @Address , @City, @State, @PostalCode)" +
                "select CAST( @@IDENTITY as int)";

            int id = db.Query<int>(sql, new
            {
                company.Name,
                company.Address,
                company.City,
                company.State,
                company.PostalCode
            }).Single();
            company.CompanyId = id;
            return company;
        }

        public Company Find(int id)
        {
            string sql = @"select * from Companies where CompanyId=@id";
            return db.Query<Company>(sql, new { id }).FirstOrDefault();
        }

        public List<Company> GetAll()
        {
            string sql = @"select * from Companies";
            return db.Query<Company>(sql).ToList();
        }

        public void Remove(int id)
        {
            string sql = @"delete from Companies where CompanyId=@id";
            db.Execute(sql, new { id });
        }

        public Company Update(Company company)
        {
            string sql = @"UPDATE [dbo].[Companies] SET [Name] = @Name ,[Address] = @Address ,[City] = @City ,[State] = @State ,[PostalCode] = @PostalCode WHERE CompanyId = @CompanyId";
            db.Execute(sql, company);
            return company;
        }

    }
}
