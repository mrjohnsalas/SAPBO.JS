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
    /// Validations: No se puede Anular o Editar si tiene: MaintenanceWorkOrderId
    /// </summary>
    public class MachineFailureBusiness : SapB1GenericRepository<MachineFailure>, IMachineFailureBusiness
    {
        private readonly IProductionMachineBusiness _productionMachineRepository;
        private readonly IProductionMachineZoneBusiness _productionMachineZoneRepository;
        private readonly IFailureTypeBusiness _failureTypeRepository;
        private readonly IFailureSeverityBusiness _failureSeverityRepository;
        private readonly IFailureCauseBusiness _failureCauseRepository;
        private readonly IFailureMechanismBusiness _failureMechanismRepository;
        private readonly IFailureImpactBusiness _failureImpactRepository;

        private const string _tableName = TableNames.MachineFailure;

        public MachineFailureBusiness(SapB1Context context, ISapB1AutoMapper<MachineFailure> mapper, IProductionMachineBusiness productionMachineRepository, IProductionMachineZoneBusiness productionMachineZoneRepository, IFailureTypeBusiness failureTypeRepository, IFailureSeverityBusiness failureSeverityRepository, IFailureCauseBusiness failureCauseRepository, IFailureMechanismBusiness failureMechanismRepository, IFailureImpactBusiness failureImpactRepository) : base(context, mapper, true)
        {
            _productionMachineRepository = productionMachineRepository;
            _productionMachineZoneRepository = productionMachineZoneRepository;
            _failureTypeRepository = failureTypeRepository;
            _failureSeverityRepository = failureSeverityRepository;
            _failureCauseRepository = failureCauseRepository;
            _failureMechanismRepository = failureMechanismRepository;
            _failureImpactRepository = failureImpactRepository;
        }

        public async Task<ICollection<MachineFailure>> GetAllAsync(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_242", new List<dynamic> { year, month }), objectType);
        }

        public async Task<ICollection<MachineFailure>> GetAllByProductionMachineIdAsync(int productionMachineId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_382", new List<dynamic> { productionMachineId }), objectType);
        }

        public async Task<ICollection<MachineFailure>> GetAllWithIdsAsync(IEnumerable<int> ids, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_319", new List<dynamic> { string.Join(",", ids) }), objectType);
        }

        public async Task<ICollection<MachineFailure>> GetAllByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_251", new List<dynamic> { maintenanceWorkOrderId }), objectType);
        }

        public async Task<MachineFailure> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_114", new List<dynamic> { id }), objectType);
        }

        public Task CreateAsync(MachineFailure obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            return CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(MachineFailure obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.ProductionMachineId = obj.ProductionMachineId;

            currentObj.StartDate = obj.StartDate;
            currentObj.FinalDate = obj.FinalDate;

            currentObj.FailureTypeId = obj.FailureTypeId;
            currentObj.FailureSeverityId = obj.FailureSeverityId;
            currentObj.FailureCauseId = obj.FailureCauseId;
            currentObj.FailureMechanismId = obj.FailureMechanismId;
            currentObj.FailureImpactId = obj.FailureImpactId;
            currentObj.Remark = obj.Remark;

            currentObj.StopMachine = obj.StopMachine;
            currentObj.StopStartDate = obj.StopStartDate;
            currentObj.StopFinalDate = obj.StopFinalDate;

            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task UpdateByMaintenanceWorkOrderIdAsync(List<int> objs, string userId, int parentId)
        {
            //Check User - Update
            if (string.IsNullOrEmpty(userId))
                throw new Exception(AppMessages.UserError);

            if (objs != null && !objs.Count.Equals(0))
            {
                var failures = await GetAllWithIdsAsync(objs);

                foreach (var failure in failures)
                {
                    //Check Status
                    if (failure.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check OTM
                    if (failure.MaintenanceWorkOrderId.HasValue &&
                        !failure.MaintenanceWorkOrderId.Equals(0) &&
                        !failure.MaintenanceWorkOrderId.Equals(parentId))
                        throw new Exception($"{AppMessages.MachineFailure_MaintenanceWorkOrderId}: {failure.MaintenanceWorkOrderId}.");

                    //Update failure
                    failure.UpdatedAt = DateTime.Now;
                    failure.UpdatedBy = userId;
                    failure.MaintenanceWorkOrderId = parentId;

                    await UpdateAsync(_tableName, failure, failure.Id.ToString());
                }
            }
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

        public async Task DeleteByMaintenanceWorkOrderIdAsync(int maintenanceWorkOrderId, string userId)
        {
            //Check User - Update
            if (string.IsNullOrEmpty(userId))
                throw new Exception(AppMessages.UserError);

            var objs = await GetAllByMaintenanceWorkOrderIdAsync(maintenanceWorkOrderId);

            if (objs != null && !objs.Count.Equals(0))
            {
                foreach (var obj in objs)
                {
                    obj.UpdatedAt = DateTime.Now;
                    obj.UpdatedBy = userId;
                    obj.MaintenanceWorkOrderId = null;

                    await UpdateAsync(_tableName, obj, obj.Id.ToString());
                }
            }
        }

        private static void CheckRules(MachineFailure obj, Enums.ObjectAction objectAction, MachineFailure currentObj = null)
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

                    //Check OTM
                    if (currentObj.MaintenanceWorkOrderId.HasValue && currentObj.MaintenanceWorkOrderId.Value > 0)
                        throw new Exception($"{AppMessages.MachineFailure_MaintenanceWorkOrderId}: {obj.MaintenanceWorkOrderId}.");

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Delete:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check OTM
                    if (obj.MaintenanceWorkOrderId.HasValue && obj.MaintenanceWorkOrderId.Value > 0)
                        throw new Exception($"{AppMessages.MachineFailure_MaintenanceWorkOrderId}: {obj.MaintenanceWorkOrderId}.");

                    //Check User - Delete
                    if (string.IsNullOrEmpty(obj.DeletedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }

            if (objectAction == Enums.ObjectAction.Insert || objectAction == Enums.ObjectAction.Update)
            {
                //Check FinalDate < StartDate
                if (obj.FinalDate < obj.StartDate)
                    throw new Exception(AppMessages.MachineFailure_FinalDate_Lessthan);

                //Check StopStartDate and StopFinalDate
                if (obj.StopMachine)
                {
                    if (!obj.StopStartDate.HasValue)
                        throw new Exception(AppMessages.MachineFailure_StopStartDate);

                    if (!obj.StopFinalDate.HasValue)
                        throw new Exception(AppMessages.MachineFailure_StopFinalDate);

                    //Check StopStartDate < StartDate
                    if (obj.StopStartDate.Value < obj.StartDate)
                        throw new Exception(AppMessages.MachineFailure_StopStartDate_Lessthan_StartDate);

                    //Check StopStartDate > FinalDate
                    if (obj.StopStartDate.Value > obj.FinalDate)
                        throw new Exception(AppMessages.MachineFailure_StopStartDate_GreaterThan_FinalDate);

                    //Check StopFinalDate < StartDate
                    if (obj.StopFinalDate.Value < obj.StartDate)
                        throw new Exception(AppMessages.MachineFailure_StopFinalDate_Lessthan_StartDate);

                    //Check StopFinalDate > FinalDate
                    if (obj.StopFinalDate.Value > obj.FinalDate)
                        throw new Exception(AppMessages.MachineFailure_StopFinalDate_GreaterThan_FinalDate);

                    //Check StopFinalDate < StopStartDate
                    if (obj.StopFinalDate.Value < obj.StopStartDate.Value)
                        throw new Exception(AppMessages.MachineFailure_FinalDate_Lessthan);
                }
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_113", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MachineFailure> SetFullProperties(MachineFailure obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            obj.ProductionMachine = await _productionMachineRepository.GetAsync(obj.ProductionMachineId);

            if (obj.ProductionMachineZoneId.HasValue)
                obj.ProductionMachineZone = await _productionMachineZoneRepository.GetAsync(obj.ProductionMachineZoneId.Value);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.FailureType = await _failureTypeRepository.GetAsync(obj.FailureTypeId);
                obj.FailureSeverity = await _failureSeverityRepository.GetAsync(obj.FailureSeverityId);
                obj.FailureCause = await _failureCauseRepository.GetAsync(obj.FailureCauseId);
                obj.FailureMechanism = await _failureMechanismRepository.GetAsync(obj.FailureMechanismId);
                obj.FailureImpact = await _failureImpactRepository.GetAsync(obj.FailureImpactId);
            }

            return obj;
        }

        public async Task<ICollection<MachineFailure>> SetFullProperties(ICollection<MachineFailure> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            //ProductionMachine
            var productionMachineIds = objs.GroupBy(x => x.ProductionMachineId).Select(g => g.Key);
            var productionMachines = await _productionMachineRepository.GetAllWithIdsAsync(productionMachineIds);

            foreach (var productionMachine in productionMachines)
                objs.Where(x => x.ProductionMachineId.Equals(productionMachine.Id)).ToList().ForEach(x => x.ProductionMachine = productionMachine);

            //ProductionMachineZone
            var productionMachineZoneIds = objs.Where(y => y.ProductionMachineZoneId.HasValue).GroupBy(x => x.ProductionMachineZoneId.Value).Select(g => g.Key);
            var productionMachineZones = await _productionMachineZoneRepository.GetAllWithIdsAsync(productionMachineZoneIds);

            foreach (var productionMachineZone in productionMachineZones)
                objs.Where(x => x.ProductionMachineZoneId.Equals(productionMachineZone.Id)).ToList().ForEach(x => x.ProductionMachineZone = productionMachineZone);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                //FailureType
                var failureTypeIds = objs.GroupBy(x => x.FailureTypeId).Select(g => g.Key);
                var failureTypes = await _failureTypeRepository.GetAllWithIdsAsync(failureTypeIds);

                foreach (var failureType in failureTypes)
                    objs.Where(x => x.FailureTypeId.Equals(failureType.Id)).ToList().ForEach(x => x.FailureType = failureType);

                //FailureSeverity
                var failureSeverityIds = objs.GroupBy(x => x.FailureSeverityId).Select(g => g.Key);
                var failureSeverities = await _failureSeverityRepository.GetAllWithIdsAsync(failureSeverityIds);

                foreach (var failureSeverity in failureSeverities)
                    objs.Where(x => x.FailureSeverityId.Equals(failureSeverity.Id)).ToList().ForEach(x => x.FailureSeverity = failureSeverity);

                //FailureCause
                var failureCauseIds = objs.GroupBy(x => x.FailureCauseId).Select(g => g.Key);
                var failureCauses = await _failureCauseRepository.GetAllWithIdsAsync(failureCauseIds);

                foreach (var failureCause in failureCauses)
                    objs.Where(x => x.FailureCauseId.Equals(failureCause.Id)).ToList().ForEach(x => x.FailureCause = failureCause);

                //FailureMechanism
                var failureMechanismIds = objs.GroupBy(x => x.FailureMechanismId).Select(g => g.Key);
                var failureMechanisms = await _failureMechanismRepository.GetAllWithIdsAsync(failureMechanismIds);

                foreach (var failureMechanism in failureMechanisms)
                    objs.Where(x => x.FailureMechanismId.Equals(failureMechanism.Id)).ToList().ForEach(x => x.FailureMechanism = failureMechanism);

                //FailureImpact
                var failureImpactIds = objs.GroupBy(x => x.FailureImpactId).Select(g => g.Key);
                var failureImpacts = await _failureImpactRepository.GetAllWithIdsAsync(failureImpactIds);

                foreach (var failureImpact in failureImpacts)
                    objs.Where(x => x.FailureImpactId.Equals(failureImpact.Id)).ToList().ForEach(x => x.FailureImpact = failureImpact);
            }

            return objs;
        }
    }
}
