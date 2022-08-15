namespace SQLServer_EF6.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly GeneralDBContext _db;
        public ApplicationService(GeneralDBContext context)
        {
            _db = context;
        }
    }

    public interface IApplicationService
    {
        
    }
}
