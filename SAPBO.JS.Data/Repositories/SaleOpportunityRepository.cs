using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Utility;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Data.Repositories
{
    public class SaleOpportunityRepository : SapB1GenericRepository<SaleOpportunity>
    {
        public SaleOpportunityRepository(SapB1Context context, ISapB1AutoMapper<SaleOpportunity> mapper) : base(context, mapper)
        {

        }

        private SAPbobsCOM.OpportunityTypeEnum GetSaleOpportunityType(Enums.SaleOpportunityType saleOpportunityType)
        {
            switch (saleOpportunityType)
            {
                case Enums.SaleOpportunityType.Sale:
                    return SAPbobsCOM.OpportunityTypeEnum.boOpSales;
                case Enums.SaleOpportunityType.Purchase:
                    return SAPbobsCOM.OpportunityTypeEnum.boOpPurchasing;
                default:
                    return SAPbobsCOM.OpportunityTypeEnum.boOpSales;
            }
        }

        private SAPbobsCOM.BoSoOsStatus GetSaleOpportunityStatus(Enums.SaleOpportunityStatus saleOpportunityStatus)
        {
            switch (saleOpportunityStatus)
            {
                case Enums.SaleOpportunityStatus.Open:
                    return SAPbobsCOM.BoSoOsStatus.sos_Open;
                case Enums.SaleOpportunityStatus.Lost:
                    return SAPbobsCOM.BoSoOsStatus.sos_Missed;
                case Enums.SaleOpportunityStatus.Won:
                    return SAPbobsCOM.BoSoOsStatus.sos_Sold;
                default:
                    return SAPbobsCOM.BoSoOsStatus.sos_Open;
            }
        }

        public void Create(SaleOpportunity obj)
        {
            _context.Connect();

            var oppportunity = (SAPbobsCOM.SalesOpportunities)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesOpportunities);

            oppportunity.OpportunityType = GetSaleOpportunityType(obj.SaleOpportunityType);
            oppportunity.CardCode = obj.BusinessPartnerId;
            oppportunity.ContactPerson = obj.ContactId;
            oppportunity.SalesPerson = obj.SaleEmployeeId;
            oppportunity.DataOwnershipfield = obj.EmployeeId;
            oppportunity.OpportunityName = obj.Subject;
            oppportunity.Remarks = obj.Notes;
            oppportunity.StartDate = obj.StartDate.Value;
            oppportunity.PredictedClosingDate = obj.ExpectedCloseDate;
            oppportunity.InterestLevel = (int)obj.SaleOpportunityInterestLevel;
            oppportunity.Status = GetSaleOpportunityStatus(obj.SaleOpportunityStatus);

            foreach (var stage in obj.Stages)
            {
                oppportunity.Lines.StartDate = stage.StartDate;
                oppportunity.Lines.ClosingDate = stage.CloseDate;
                oppportunity.Lines.SalesPerson = stage.SaleEmployeeId;
                oppportunity.Lines.StageKey = stage.OpportunityStageId;
                oppportunity.Lines.PercentageRate = (double)stage.ClosePercentage;
                oppportunity.Lines.MaxLocalTotal = (double)stage.PotentialAmount;
                oppportunity.Lines.DataOwnershipfield = stage.EmployeeId;

                oppportunity.Lines.Add();
            }

            // oppportunity.ClosingDate = obj.CloseDate.Value // Only last stage
            // oppportunity.ClosingPercentage = obj.ClosePercentage //AutoCalcule
            // oppportunity.MaxLocalTotal = obj.PotentialAmount; //AutoCalcule
            // oppportunity.WeightedSumLC = obj.WeightedAmount //AutoCalcule

            int errorCode = oppportunity.Add();

            obj.Id = GetValue("GP_WEB_APP_380", "Id", new List<dynamic> { obj.SaleEmployeeId });

            if (errorCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void Update(SaleOpportunity obj)
        {
            _context.Connect();

            var oppportunity = (SAPbobsCOM.SalesOpportunities)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesOpportunities);
            oppportunity.GetByKey(obj.Id);

            oppportunity.OpportunityType = GetSaleOpportunityType(obj.SaleOpportunityType);
            oppportunity.CardCode = obj.BusinessPartnerId;
            oppportunity.ContactPerson = obj.ContactId;
            oppportunity.SalesPerson = obj.SaleEmployeeId;
            oppportunity.DataOwnershipfield = obj.EmployeeId;
            oppportunity.OpportunityName = obj.Subject;
            oppportunity.Remarks = obj.Notes;
            oppportunity.StartDate = obj.StartDate.Value;
            oppportunity.PredictedClosingDate = obj.ExpectedCloseDate;
            oppportunity.InterestLevel = (int)obj.SaleOpportunityInterestLevel;
            oppportunity.Status = GetSaleOpportunityStatus(obj.SaleOpportunityStatus);

            foreach (var stage in obj.Stages)
            {
                stage.PotentialAmount = obj.PotentialAmount;
                if (!stage.Id.Equals(-1))
                    oppportunity.Lines.SetCurrentLine(stage.Id);
                oppportunity.Lines.StartDate = stage.StartDate;
                oppportunity.Lines.ClosingDate = stage.CloseDate;
                oppportunity.Lines.SalesPerson = stage.SaleEmployeeId;
                oppportunity.Lines.StageKey = stage.OpportunityStageId;
                oppportunity.Lines.PercentageRate = (double)stage.ClosePercentage;
                oppportunity.Lines.MaxLocalTotal = (double)stage.PotentialAmount;
                oppportunity.Lines.DataOwnershipfield = stage.EmployeeId;
                oppportunity.Lines.Add();
            }

            int errorCode = oppportunity.Update();

            if (errorCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void Won(SaleOpportunity obj)
        {
            _context.Connect();

            var oppportunity = (SAPbobsCOM.SalesOpportunities)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesOpportunities);
            oppportunity.GetByKey(obj.Id);

            oppportunity.ClosingDate = obj.CloseDate.Value;
            oppportunity.Status = GetSaleOpportunityStatus(obj.SaleOpportunityStatus);

            foreach (var stage in obj.Stages)
            {
                if (stage.Id.Equals(-1))
                {
                    // create stage
                    oppportunity.Lines.StartDate = stage.StartDate;
                    oppportunity.Lines.ClosingDate = stage.CloseDate;
                    oppportunity.Lines.SalesPerson = stage.SaleEmployeeId;
                    oppportunity.Lines.StageKey = stage.OpportunityStageId;
                    oppportunity.Lines.PercentageRate = (double)stage.ClosePercentage;
                    oppportunity.Lines.MaxLocalTotal = (double)stage.PotentialAmount;
                    oppportunity.Lines.DataOwnershipfield = stage.EmployeeId;
                }
                else
                {
                    oppportunity.Lines.SetCurrentLine(stage.Id);
                }
                oppportunity.Lines.Add();
            }

            int errorCode = oppportunity.Update();

            if (errorCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void Lost(SaleOpportunity obj)
        {
            _context.Connect();

            var oppportunity = (SAPbobsCOM.SalesOpportunities)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesOpportunities);
            oppportunity.GetByKey(obj.Id);

            oppportunity.ClosingDate = obj.CloseDate.Value;
            oppportunity.Status = GetSaleOpportunityStatus(obj.SaleOpportunityStatus);

            foreach (var stage in obj.Stages)
            {
                oppportunity.Lines.SetCurrentLine(stage.Id);
                oppportunity.Lines.Add();
            }

            int errorCode = oppportunity.Update();

            Exception ex = null;
            if (!errorCode.Equals(0))
            {
                ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    GC.Collect();
                    // TODO task.run exception - user-unmanaged
                    throw ex;
                }
            }

            foreach (var reason in obj.LossReasons)
            {
                oppportunity.Reasons.Reason = reason.LossReasonId;
                oppportunity.Reasons.UserFields.Fields.Item("U_CL_DETMOT").Value = reason.Notes;
                oppportunity.Reasons.Add();
            }

            errorCode = oppportunity.Update();

            if (errorCode.Equals(0)) return;

            ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void Delete(int id, string deleteBy)
        {
            _context.Connect();

            var oppportunity = (SAPbobsCOM.SalesOpportunities)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesOpportunities);
            oppportunity.GetByKey(id);

            int errorCode = oppportunity.Remove();

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
