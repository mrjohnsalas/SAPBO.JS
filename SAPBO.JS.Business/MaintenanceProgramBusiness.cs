using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    /// <summary>
    /// Info:
    /// Status: Pausado, Iniciado, Anulado
    ///     New => Pausado
    ///     Init => Iniciado
    ///     Pause => Pausado
    ///     Delete => Anulado
    /// </summary>
    public class MaintenanceProgramBusiness : SapB1GenericRepository<MaintenanceProgram>, IMaintenanceProgramBusiness
    {
        private readonly IMaintenancePriorityBusiness _maintenancePriorityRepository;
        private readonly IMaintenanceTypeBusiness _maintenanceTypeRepository;
        private readonly IProductionMachineBusiness _productionMachineRepository;
        private readonly ITimeFrequencyBusiness _timeFrequencyRepository;
        private readonly IMaintenanceProgramToolBusiness _maintenanceProgramToolRepository;
        private readonly IMaintenanceProgramReplacementBusiness _maintenanceProgramReplacementRepository;
        private readonly IMaintenanceProgramJobBusiness _maintenanceProgramJobRepository;

        private const string _tableName = TableNames.MaintenanceProgram;

        public MaintenanceProgramBusiness(SapB1Context context, ISapB1AutoMapper<MaintenanceProgram> mapper, IMaintenancePriorityBusiness maintenancePriorityRepository, IMaintenanceTypeBusiness maintenanceTypeRepository, IProductionMachineBusiness productionMachineRepository, ITimeFrequencyBusiness timeFrequencyRepository, IMaintenanceProgramToolBusiness maintenanceProgramToolRepository, IMaintenanceProgramReplacementBusiness maintenanceProgramReplacementRepository, IMaintenanceProgramJobBusiness maintenanceProgramJobRepository) : base(context, mapper, true)
        {
            _maintenancePriorityRepository = maintenancePriorityRepository;
            _maintenanceTypeRepository = maintenanceTypeRepository;
            _productionMachineRepository = productionMachineRepository;
            _timeFrequencyRepository = timeFrequencyRepository;
            _maintenanceProgramToolRepository = maintenanceProgramToolRepository;
            _maintenanceProgramReplacementRepository = maintenanceProgramReplacementRepository;
            _maintenanceProgramJobRepository = maintenanceProgramJobRepository;
        }

        public async Task<ICollection<MaintenanceProgram>> GetAllAsync(Enums.StatusType statusType = Enums.StatusType.Todos, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await SetFullProperties(statusType == Enums.StatusType.Todos
                ? await GetAllAsync("GP_WEB_APP_127")
                : await GetAllAsync("GP_WEB_APP_299", new List<dynamic> { (int)statusType }), objectType);
        }

        public async Task<MaintenanceProgram> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_126", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(MaintenanceProgram obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            obj.StatusId = (int)Enums.StatusType.Pausado;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());

            //Tools
            if (obj.Tools != null && obj.Tools.Any())
                foreach (var tool in obj.Tools)
                {
                    tool.MaintenanceProgramId = obj.Id;
                    await _maintenanceProgramToolRepository.CreateAsync(tool);
                }
            //Replacements
            if (obj.Replacements != null && obj.Replacements.Any())
                foreach (var replacement in obj.Replacements)
                {
                    replacement.MaintenanceProgramId = obj.Id;
                    await _maintenanceProgramReplacementRepository.CreateAsync(replacement);
                }
            //Tools
            if (obj.Jobs != null && obj.Jobs.Any())
                foreach (var job in obj.Jobs)
                {
                    job.MaintenanceProgramId = obj.Id;
                    await _maintenanceProgramJobRepository.CreateAsync(job);
                }
        }

        public async Task UpdateAsync(MaintenanceProgram obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Description = obj.Description;
            currentObj.Remark = obj.Remark;

            currentObj.ProductionMachineId = obj.ProductionMachineId;
            currentObj.MaintenanceTypeId = obj.MaintenanceTypeId;

            currentObj.MaintenancePriorityId = obj.MaintenancePriorityId;
            currentObj.TimeFrequencyId = obj.TimeFrequencyId;

            currentObj.TimeFrequencyValue = obj.TimeFrequencyValue;
            currentObj.EstimatedTime = obj.EstimatedTime;


            currentObj.LastDate = obj.LastDate;

            currentObj.StopPlant = obj.StopPlant;
            currentObj.StopMachine = obj.StopMachine;

            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());

            await _maintenanceProgramToolRepository.DeleteByMaintenanceProgramIdAsync(obj.Id);
            await _maintenanceProgramReplacementRepository.DeleteByMaintenanceProgramIdAsync(obj.Id);
            await _maintenanceProgramJobRepository.DeleteByMaintenanceProgramIdAsync(obj.Id);

            //Tools
            if (obj.Tools != null && obj.Tools.Any())
                foreach (var tool in obj.Tools)
                {
                    tool.MaintenanceProgramId = obj.Id;
                    await _maintenanceProgramToolRepository.CreateAsync(tool);
                }
            //Replacements
            if (obj.Replacements != null && obj.Replacements.Any())
                foreach (var replacement in obj.Replacements)
                {
                    replacement.MaintenanceProgramId = obj.Id;
                    await _maintenanceProgramReplacementRepository.CreateAsync(replacement);
                }
            //Tools
            if (obj.Jobs != null && obj.Jobs.Any())
                foreach (var job in obj.Jobs)
                {
                    job.MaintenanceProgramId = obj.Id;
                    await _maintenanceProgramJobRepository.CreateAsync(job);
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

        public async Task InitAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);
            currentObj.UpdatedBy = updatedBy;

            CheckRules(currentObj, Enums.ObjectAction.Init);

            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Iniciado;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task PauseAsync(int id, string updatedBy)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);
            currentObj.UpdatedBy = updatedBy;

            CheckRules(currentObj, Enums.ObjectAction.Pause);

            //Set obj
            currentObj.StatusId = (int)Enums.StatusType.Pausado;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        private static void CheckRules(MaintenanceProgram obj, Enums.ObjectAction objectAction, MaintenanceProgram currentObj = null)
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
                    if (currentObj?.StatusType == Enums.StatusType.Anulado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Delete:
                    //Check Status
                    if (obj.StatusType == Enums.StatusType.Anulado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Delete
                    if (string.IsNullOrEmpty(obj.DeletedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Init:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Pausado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                case Enums.ObjectAction.Pause:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Iniciado)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_125", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<MaintenanceProgram> SetFullProperties(MaintenanceProgram obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            obj.MaintenancePriority = await _maintenancePriorityRepository.GetAsync(obj.MaintenancePriorityId);
            obj.MaintenanceType = await _maintenanceTypeRepository.GetAsync(obj.MaintenanceTypeId);
            obj.ProductionMachine = await _productionMachineRepository.GetAsync(obj.ProductionMachineId);
            obj.TimeFrequency = await _timeFrequencyRepository.GetAsync(obj.TimeFrequencyId);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                if (objectType == Enums.ObjectType.Full)
                {
                    obj.Tools = await _maintenanceProgramToolRepository.GetAllAsync(obj.Id, objectType);
                    obj.Replacements = await _maintenanceProgramReplacementRepository.GetAllAsync(obj.Id, objectType);
                    obj.Jobs = await _maintenanceProgramJobRepository.GetAllAsync(obj.Id, objectType);
                }
            }

            return obj;
        }

        public async Task<ICollection<MaintenanceProgram>> SetFullProperties(ICollection<MaintenanceProgram> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            //MaintenancePriority
            var maintenancePriorityIds = objs.GroupBy(x => x.MaintenancePriorityId).Select(g => g.Key);
            var maintenancePriorities = await _maintenancePriorityRepository.GetAllWithIdsAsync(maintenancePriorityIds);

            foreach (var maintenancePriority in maintenancePriorities)
                objs.Where(x => x.MaintenancePriorityId.Equals(maintenancePriority.Id)).ToList().ForEach(x => x.MaintenancePriority = maintenancePriority);

            //MaintenanceType
            var maintenanceTypeIds = objs.GroupBy(x => x.MaintenanceTypeId).Select(g => g.Key);
            var maintenanceTypes = await _maintenanceTypeRepository.GetAllWithIdsAsync(maintenanceTypeIds);

            foreach (var maintenanceType in maintenanceTypes)
                objs.Where(x => x.MaintenanceTypeId.Equals(maintenanceType.Id)).ToList().ForEach(x => x.MaintenanceType = maintenanceType);

            //ProductionMachine
            var productionMachineIds = objs.GroupBy(x => x.ProductionMachineId).Select(g => g.Key);
            var productionMachines = await _productionMachineRepository.GetAllWithIdsAsync(productionMachineIds);

            foreach (var productionMachine in productionMachines)
                objs.Where(x => x.ProductionMachineId.Equals(productionMachine.Id)).ToList().ForEach(x => x.ProductionMachine = productionMachine);

            //TimeFrequency
            var timeFrequencyIds = objs.GroupBy(x => x.TimeFrequencyId).Select(g => g.Key);
            var timeFrequencies = await _timeFrequencyRepository.GetAllWithIdsAsync(timeFrequencyIds);

            foreach (var timeFrequency in timeFrequencies)
                objs.Where(x => x.TimeFrequencyId.Equals(timeFrequency.Id)).ToList().ForEach(x => x.TimeFrequency = timeFrequency);

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                if (objectType == Enums.ObjectType.Full)
                {
                    //Details
                    var maintenanceProgramIds = objs.Select(x => x.Id);

                    var tools = await _maintenanceProgramToolRepository.GetAllWithIdsAsync(maintenanceProgramIds, objectType);
                    var replacements = await _maintenanceProgramReplacementRepository.GetAllWithIdsAsync(maintenanceProgramIds, objectType);
                    var jobs = await _maintenanceProgramJobRepository.GetAllWithIdsAsync(maintenanceProgramIds, objectType);

                    foreach (var maintenanceProgram in objs)
                    {
                        maintenanceProgram.Tools = tools.Where(x => x.MaintenanceProgramId.Equals(maintenanceProgram.Id)).ToList();
                        maintenanceProgram.Replacements = replacements.Where(x => x.MaintenanceProgramId.Equals(maintenanceProgram.Id)).ToList();
                        maintenanceProgram.Jobs = jobs.Where(x => x.MaintenanceProgramId.Equals(maintenanceProgram.Id)).ToList();
                    }
                }
            }

            return objs;
        }
    }
}
