using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface ISaleGoalDataBySaleEmployeeBusiness
    {
        Task<ICollection<SaleGoalDataBySaleEmployee>> GetSaleGoalDataBySaleEmployeeIdAsync(int saleEmployeeId);
    }
}
