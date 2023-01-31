using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class CRMActivityBusiness : CRMActivityRepository, ICRMActivityBusiness
    {
        private readonly IBusinessPartnerContactBusiness _businessPartnerContactRepository;
        private readonly IBusinessPartnerAddressBusiness _businessPartnerAddressRepository;

        public CRMActivityBusiness(SapB1Context context, ISapB1AutoMapper<CRMActivity> mapper,
            IBusinessPartnerContactBusiness businessPartnerContactRepository,
            IBusinessPartnerAddressBusiness businessPartnerAddressRepository) : base(context, mapper)
        {
            _businessPartnerContactRepository = businessPartnerContactRepository;
            _businessPartnerAddressRepository = businessPartnerAddressRepository;
        }

        public async Task<ICollection<CRMActivity>> GetAllAsync(string businessPartnerId)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_357", new List<dynamic> { businessPartnerId }));
        }

        public async Task<ICollection<CRMActivity>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_358", new List<dynamic> { string.Join(",", ids) }));
        }

        public async Task<ICollection<CRMActivity>> GetAllWithIdsAsync(IEnumerable<string> businessPartnerIds)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_359", new List<dynamic> { string.Join(",", businessPartnerIds) }));
        }

        public async Task<CRMActivity> GetAsync(int id)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_360", new List<dynamic> { id }));
        }

        public Task CreateAsync(CRMActivity obj)
        {
            return Task.Run(() => CreateOrUpdate(obj, Enums.OperationType.Create));
        }

        public async Task UpdateAsync(CRMActivity obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            await Task.Run(() => CreateOrUpdate(obj, Enums.OperationType.Update));
        }

        public async Task DeleteAsync(int id, string deleteBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Check Status
            if (obj.IsClosed)
                throw new Exception(AppMessages.CRMActivityStatus);

            await Task.Run(() => Delete(id, deleteBy));
        }

        private static void CheckRules(CRMActivity obj, Enums.ObjectAction objectAction, CRMActivity currentObj = null)
        {
            if (objectAction == Enums.ObjectAction.Update || objectAction == Enums.ObjectAction.Delete)
            {
                //Check Status
                if (currentObj.IsClosed)
                    throw new Exception(AppMessages.CRMActivityStatus);

                if (objectAction == Enums.ObjectAction.Update)
                {
                    //Check ActivityType
                    if (currentObj.ActivityType != obj.ActivityType)
                        throw new Exception(AppMessages.CRMActivityType);
                }
            }

            if (obj.ActivityType == Enums.ActivityType.PhoneCall || obj.ActivityType == Enums.ActivityType.Meeting)
            {
                //Check EndDate
                if (!obj.EndDate.HasValue)
                    throw new Exception(AppMessages.CRMActivity_EndDate);

                //Check EndDate
                if (obj.EndDate.Value <= obj.StartDate)
                    throw new Exception(AppMessages.CRMActivity_EndDate_Lessthan);

                if (obj.ActivityType == Enums.ActivityType.Meeting)
                {
                    //Check Address
                    if (obj.ActivityLocation == Enums.ActivityLocation.SNAddress && string.IsNullOrEmpty(obj.AddressId))
                        throw new Exception(AppMessages.CRMActivity_AddressId);

                    //Check CountryId
                    if (string.IsNullOrEmpty(obj.CountryId))
                        throw new Exception(AppMessages.CRMActivity_CountryId);

                    //Check StateId
                    if (string.IsNullOrEmpty(obj.StateId))
                        throw new Exception(AppMessages.CRMActivity_StateId);

                    //Check City
                    if (string.IsNullOrEmpty(obj.City))
                        throw new Exception(AppMessages.CRMActivity_City);

                    //Check Street
                    if (string.IsNullOrEmpty(obj.Street))
                        throw new Exception(AppMessages.CRMActivity_Street);

                    //Check Room
                    if (string.IsNullOrEmpty(obj.Room))
                        throw new Exception(AppMessages.CRMActivity_Room);
                }
            }
        }

        public async Task<CRMActivity> SetFullProperties(CRMActivity obj)
        {
            if (obj == null) return obj;

            obj.Contact = await _businessPartnerContactRepository.GetAsync(obj.ContactId);
            if (!string.IsNullOrEmpty(obj.AddressId))
                obj.Address = await _businessPartnerAddressRepository.GetAsync(obj.BusinessPartnerId, obj.AddressId);

            return obj;
        }

        public async Task<ICollection<CRMActivity>> SetFullProperties(ICollection<CRMActivity> objs)
        {
            if (objs == null || !objs.Any()) return objs;

            var contactIds = objs.GroupBy(x => x.ContactId).Select(g => g.Key);
            var contacts = await _businessPartnerContactRepository.GetAllWithIdsAsync(contactIds);

            foreach (var contact in contacts)
                objs.Where(x => x.ContactId.Equals(contact.Id)).ToList().ForEach(x => x.Contact = contact);

            var adIds = objs.GroupBy(x => x.AddressId).Select(g => g.Key);
            var bu = objs.ToList()[0];
            var addresses = await _businessPartnerAddressRepository.GetAllWithIdsAsync(bu.BusinessPartnerId, adIds);

            return objs;
        }
    }
}
