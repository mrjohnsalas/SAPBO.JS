using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    /// <summary>
    /// Info:
    /// Status: Activo, Anulado
    ///     New => Activo
    ///     Delete => Anulado
    /// </summary>
    public class EmployeeBusiness : SapB1GenericRepository<Employee>, IEmployeeBusiness
    {
        private readonly IJobBusiness _jobRepository;
        private readonly IBusinessUnitBusiness _businessUnitRepository;

        private const string _tableName = TableNames.Employee;

        public EmployeeBusiness(SapB1Context context, ISapB1AutoMapper<Employee> mapper, IJobBusiness jobRepository, IBusinessUnitBusiness businessUnitRepository) : base(context, mapper, true)
        {
            _jobRepository = jobRepository;
            _businessUnitRepository = businessUnitRepository;
        }

        public async Task<ICollection<Employee>> GetAllAsync(string businessUnitId = "", Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_343", new List<dynamic> { businessUnitId })
                : await GetAllAsync("GP_WEB_APP_344", new List<dynamic> { (int)statusType, businessUnitId }), objectType);
        }

        public async Task<ICollection<Employee>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_349", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<Employee>> GetAllSupersAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var supers = await GetAllAsync("MM", statusType, objectType);
            return await SetFullProperties(supers.Where(x => x.IsSuper).ToList(), objectType);
        }

        public async Task<ICollection<Employee>> GetAllSaleEmployeesAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            var saleEmployees = await GetAllAsync("VE", statusType, objectType);
            return await SetFullProperties(saleEmployees.Where(x => x.SaleEmployeeId.HasValue).ToList(), objectType);
        }

        public async Task<ICollection<Employee>> GetAllWithSaleEmployeeIdsAsync(IEnumerable<int> saleEmployeeIds, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_336", new List<dynamic> { string.Join(",", saleEmployeeIds) }), objectType);
        }

        public async Task<ICollection<Employee>> GetAllWithPurchaseEmployeeIdsAsync(IEnumerable<int> purchaseEmployeeIds, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_405", new List<dynamic> { string.Join(",", purchaseEmployeeIds) }), objectType);
        }

        public async Task<Employee> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_342", new List<dynamic> { id }), objectType);
        }

        public async Task<Employee> GetByEmailAsync(string email, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_334", new List<dynamic> { email }), objectType);
        }

        public async Task<Employee> GetBySaleEmployeeIdAsync(int saleEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_335", new List<dynamic> { saleEmployeeId }), objectType);
        }

        public async Task<Employee> GetByPurchaseEmployeeIdAsync(int purchaseEmployeeId, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_404", new List<dynamic> { purchaseEmployeeId }), objectType);
        }

        public Task CreateAsync(Employee obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(Employee obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.FirstName = obj.FirstName;
            currentObj.LastName = obj.LastName;
            currentObj.GrafipapelId = obj.GrafipapelId;
            currentObj.DNI = obj.DNI;
            currentObj.CostHour = obj.CostHour;
            currentObj.JobId = obj.JobId;
            currentObj.IsSuper = obj.IsSuper;
            currentObj.WebApp = obj.WebApp;
            currentObj.Phone = obj.Phone;
            currentObj.SaleEmployeeId = obj.SaleEmployeeId;
            currentObj.BusinessUnitId = obj.BusinessUnitId;
            currentObj.Email = obj.Email;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task DeleteAsync(int id, string deleteBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            obj.DeletedBy = deleteBy;

            CheckRules(obj, Enums.ObjectAction.Delete);

            obj.StatusId = (int)Enums.StatusType.Anulado;
            obj.DeletedAt = DateTime.Now;

            await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        private static void CheckRules(Employee obj, Enums.ObjectAction objectAction, Employee currentObj = null)
        {
            switch (objectAction)
            {
                case Enums.ObjectAction.Insert:
                    //Check User - Create
                    if (string.IsNullOrEmpty(obj.CreatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Update:
                    //Check Status
                    if (currentObj?.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Delete:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Delete
                    if (string.IsNullOrEmpty(obj.DeletedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_093", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<Employee> SetFullProperties(Employee obj, Enums.ObjectType objectType)
        {
            if (obj == null) return obj;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.Job = await _jobRepository.GetAsync(obj.JobId);
                obj.BusinessUnit = await _businessUnitRepository.GetAsync(obj.BusinessUnitId);
            }

            return obj;
        }

        public async Task<ICollection<Employee>> SetFullProperties(ICollection<Employee> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                var jobIds = objs.GroupBy(x => x.JobId).Select(g => g.Key);
                var jobs = await _jobRepository.GetAllWithIdsAsync(jobIds);

                var buIds = objs.GroupBy(x => x.BusinessUnitId).Select(g => g.Key);
                var bus = await _businessUnitRepository.GetAllWithIdsAsync(buIds);

                foreach (var job in jobs)
                    objs.Where(x => x.JobId.Equals(job.Id)).ToList().ForEach(x => x.Job = job);

                foreach (var bu in bus)
                    objs.Where(x => x.BusinessUnitId.Equals(bu.Id)).ToList().ForEach(x => x.BusinessUnit = bu);
            }

            return objs;
        }
    }
}
