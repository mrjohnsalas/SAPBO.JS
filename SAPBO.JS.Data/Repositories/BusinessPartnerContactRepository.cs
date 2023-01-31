using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Utility;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Data.Repositories
{
    public class BusinessPartnerContactRepository : SapB1GenericRepository<BusinessPartnerContact>
    {
        public BusinessPartnerContactRepository(SapB1Context context, ISapB1AutoMapper<BusinessPartnerContact> mapper) : base(context, mapper)
        {

        }

        public void BusinessPartnerContact(BusinessPartnerContact obj, Enums.OperationType operationType)
        {
            _context.Connect();

            var bp = (SAPbobsCOM.IBusinessPartners)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            if (!bp.GetByKey(obj.BusinessPartnerId))
                throw new Exception(AppMessages.NotFoundFromOperation);

            if (operationType == Enums.OperationType.Update)
                bp.ContactEmployees.SetCurrentLine(obj.LineNum);
            else
            {
                if (bp.ContactEmployees.Count > 1)
                    bp.ContactEmployees.Add();
                else
                    if (bp.ContactEmployees.Name != "")
                    bp.ContactEmployees.Add();
                bp.ContactEmployees.Name = obj.Name;
            }

            bp.ContactEmployees.FirstName = obj.FirstName;
            bp.ContactEmployees.MiddleName = obj.MiddleName;
            bp.ContactEmployees.LastName = obj.LastName;
            bp.ContactEmployees.Title = obj.Title;
            bp.ContactEmployees.Position = obj.Position;
            bp.ContactEmployees.Address = obj.Address;
            bp.ContactEmployees.Phone1 = obj.Phone1;
            bp.ContactEmployees.Phone2 = obj.Phone2;
            bp.ContactEmployees.MobilePhone = obj.MobilePhone;
            bp.ContactEmployees.E_Mail = obj.Email;
            bp.ContactEmployees.Profession = obj.Profession;

            int errorCode = bp.Update();

            if (operationType == Enums.OperationType.Create)
                obj.Id = GetValue("GP_WEB_APP_381", "Id", new List<dynamic> { obj.BusinessPartnerId });

            if (errorCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }
    }
}
