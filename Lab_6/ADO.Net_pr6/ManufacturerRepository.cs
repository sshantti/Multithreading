using ClassLibrary;
using Microsoft.Data.SqlClient;

namespace ADO.Net_pr6
{
    public class ManufacturerRepository
    {
        private readonly DatabaseContext _context;

        public ManufacturerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Manufacturer manufacturer)
        {
            const string sql = @"
                INSERT INTO Manufacturers (Name, Address, IsAChildCompany) 
                VALUES (@Name, @Address, @IsAChildCompany);
                SELECT SCOPE_IDENTITY();";

            using var command = new SqlCommand(sql, _context.Connection);
            command.Parameters.AddWithValue("@Name", manufacturer.Name);
            command.Parameters.AddWithValue("@Address", manufacturer.Address);
            command.Parameters.AddWithValue("@IsAChildCompany", manufacturer.IsAChildCompany);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }
    }
}