using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class SaleOpportunityBusiness : SaleOpportunityRepository, ISaleOpportunityBusiness
    {
        private readonly IOpportunityStageBusiness _opportunityStageRepository;
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly ISaleOpportunityStageBusiness _saleOpportunityStageRepository;
        private readonly ISaleOpportunityLossReasonBusiness _saleOpportunityLossReasonRepository;

        public SaleOpportunityBusiness(SapB1Context context, ISapB1AutoMapper<SaleOpportunity> mapper,
            IOpportunityStageBusiness opportunityStageRepository,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            ISaleOpportunityStageBusiness saleOpportunityStageRepository,
            ISaleOpportunityLossReasonBusiness saleOpportunityLossReasonRepository) : base(context, mapper)
        {
            _opportunityStageRepository = opportunityStageRepository;
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _saleOpportunityStageRepository = saleOpportunityStageRepository;
            _saleOpportunityLossReasonRepository = saleOpportunityLossReasonRepository;
        }

        public async Task<ICollection<SaleOpportunity>> GetAllAsync(string businessPartnerId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_375", new List<dynamic> { businessPartnerId }));
        }

        public async Task<ICollection<SaleOpportunity>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_376", new List<dynamic> { string.Join(",", ids) }));
        }

        public async Task<ICollection<SaleOpportunity>> GetAllWithIdsAsync(IEnumerable<string> businessPartnerIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_377", new List<dynamic> { string.Join(",", businessPartnerIds) }));
        }

        public async Task<SaleOpportunity> GetAsync(int id)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_378", new List<dynamic> { id }));
        }

        public Task<int> GetCountBySaleEmployeeIdAsync(int saleEmployeeId)
        {
            return Task.FromResult(GetValue("GP_WEB_APP_490", "NRO_OP", new List<dynamic> { saleEmployeeId }));
        }

        public async Task CreateAsync(SaleOpportunity obj)
        {
            obj.StartDate = DateTime.Now;
            var firstContactStage = await _opportunityStageRepository.GetByStageAsync(Enums.OpportunityStages.FirstContact);

            obj.Stages = new List<SaleOpportunityStage>
            {
                new SaleOpportunityStage
                {
                    StartDate = obj.StartDate.Value,
                    CloseDate = obj.StartDate.Value,
                    SaleEmployeeId = obj.SaleEmployeeId,
                    OpportunityStageId = firstContactStage.Id,
                    ClosePercentage = firstContactStage.ClosePercentage,
                    PotentialAmount = obj.PotentialAmount,
                    WeightedAmount = obj.WeightedAmount,
                    EmployeeId = obj.EmployeeId
                }
            };

            await Task.Run(() => Create(obj));
        }

        public async Task UpdateAsync(SaleOpportunity obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            obj.Stages = currentObj.Stages;

            await Task.Run(() => Update(obj));
        }

        public async Task WonAsync(int id, string updateBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check Status
            if (obj.SaleOpportunityStatus != Enums.SaleOpportunityStatus.Open)
                throw new Exception(AppMessages.SaleOpportunityStatus);

            obj.CloseDate = DateTime.Now;
            obj.SaleOpportunityStatus = Enums.SaleOpportunityStatus.Won;
            var acceptanceStage = obj.Stages.FirstOrDefault(x => x.OpportunityStageId == (int)Enums.OpportunityStages.Acceptance);
            if (acceptanceStage == null)
            {
                var stage = await _opportunityStageRepository.GetByStageAsync(Enums.OpportunityStages.Acceptance);

                obj.Stages.Add(new SaleOpportunityStage
                {
                    Id = -1,
                    StartDate = obj.CloseDate.Value,
                    CloseDate = obj.CloseDate.Value,
                    SaleEmployeeId = obj.SaleEmployeeId,
                    OpportunityStageId = stage.Id,
                    ClosePercentage = stage.ClosePercentage,
                    PotentialAmount = obj.PotentialAmount,
                    WeightedAmount = obj.WeightedAmount,
                    EmployeeId = obj.EmployeeId
                }); ;
            }

            await Task.Run(() => Won(obj));
        }

        public async Task LostAsync(int id, List<SaleOpportunityLossReason> lossReasons, string updateBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check Status
            if (obj.SaleOpportunityStatus != Enums.SaleOpportunityStatus.Open)
                throw new Exception(AppMessages.SaleOpportunityStatus);

            //Check Lost reasons
            if (lossReasons == null || !lossReasons.Any())
                throw new Exception(AppMessages.SaleOpportunityLostReasons);

            obj.CloseDate = DateTime.Now;
            obj.SaleOpportunityStatus = Enums.SaleOpportunityStatus.Lost;
            obj.LossReasons = lossReasons;

            await Task.Run(() => Lost(obj));
        }

        public async Task DeleteAsync(int id, string deleteBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check Status
            if (obj.SaleOpportunityStatus != Enums.SaleOpportunityStatus.Open)
                throw new Exception(AppMessages.SaleOpportunityStatus);

            await Task.Run(() => Delete(id, deleteBy));
        }

        private static void CheckRules(SaleOpportunity obj, Enums.ObjectAction objectAction, SaleOpportunity currentObj = null)
        {
            if (objectAction == Enums.ObjectAction.Update || objectAction == Enums.ObjectAction.Delete)
            {
                //Check Status
                if (obj.SaleOpportunityStatus != Enums.SaleOpportunityStatus.Open)
                    throw new Exception(AppMessages.SaleOpportunityStatus);
            }
        }

        public async Task<SaleOpportunity> SetFullProperties(SaleOpportunity obj)
        {
            if (obj == null) return obj;

            obj.Contact = await _businessPartnerContactRepository.GetAsync(obj.ContactId);
            obj.LossReasons = await _saleOpportunityLossReasonRepository.GetAllAsync(obj.Id);
            obj.Stages = await _saleOpportunityStageRepository.GetAllAsync(obj.Id);

            return obj;
        }

        public async Task<ICollection<SaleOpportunity>> SetFullProperties(ICollection<SaleOpportunity> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            var contactIds = objs.GroupBy(x => x.ContactId).Select(g => g.Key);
            var contacts = await _businessPartnerContactRepository.GetAllWithIdsAsync(contactIds);

            foreach (var contact in contacts)
                objs.Where(x => x.ContactId.Equals(contact.Id)).ToList().ForEach(x => x.Contact = contact);

            foreach (var opportunity in objs)
            {
                opportunity.LossReasons = await _saleOpportunityLossReasonRepository.GetAllAsync(opportunity.Id);
                opportunity.Stages = await _saleOpportunityStageRepository.GetAllAsync(opportunity.Id);
            }

            return objs;
        }
    }
}
