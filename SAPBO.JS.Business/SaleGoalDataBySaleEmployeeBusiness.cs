using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public class SaleGoalDataBySaleEmployeeBusiness : SapB1GenericRepository<SaleGoalDataBySaleEmployee>, ISaleGoalDataBySaleEmployeeBusiness
    {
        public SaleGoalDataBySaleEmployeeBusiness(SapB1Context context, ISapB1AutoMapper<SaleGoalDataBySaleEmployee> mapper) : base(context, mapper)
        {
        }

        public Task<ICollection<SaleGoalDataBySaleEmployee>> GetSaleGoalDataBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            return GetAllAsync("GP_WEB_APP_492", new List<dynamic> { saleEmployeeId });
        }
    }
}
