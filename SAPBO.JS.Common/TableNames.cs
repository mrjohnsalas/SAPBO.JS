using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class TableNames
    {
        public const string MachineFailure = "GP_REGFAL";
        public const string FailureCause = "GP_CAUFAL";
        public const string FailureImpact = "GP_IMPFAL";
        public const string FailureMechanism = "GP_MECFAL";
        public const string FailureSeverity = "GP_SEVFAL";
        public const string FailureType = "GP_TIPFAL";
        public const string ProductionMachine = "CL_MAQUINA";
        public const string ProductionMachineZone = "GP_ZONMAQ";
        public const string Employee = "CL_OPEMAQ";
        public const string Job = "GP_PUETRA";
        public const string MaintenanceType = "GP_TIPMAN";
        public const string TimeFrequency = "GP_FREMAN";
        public const string MaintenancePriority = "GP_PRIORI";
        public const string MaintenanceTool = "GP_HERMAN";
        public const string WebAppEmail = "GP_WAPEMA";

        public const string Driver = "BPP_CONDUC";
        public const string Vehicle = "BPP_VEHICU";

        public const string MaintenanceWorkOrder = "GP_OTMANT";
        public const string MaintenanceWorkOrderEmployee = "GP_OTMEMP";
        public const string MaintenanceWorkOrderReplacement = "GP_OTMREP";
        public const string MaintenanceWorkOrderTool = "GP_OTMHER";

        public const string MaintenanceProgram = "GP_PROMAN";
        public const string MaintenanceProgramJob = "GP_PMTEMP";
        public const string MaintenanceProgramReplacement = "GP_PMTREP";
        public const string MaintenanceProgramTool = "GP_PMTHER";
        public const string MaintenanceProgramEpp = "GP_PMTEPP";

        public const string Flete = "GP_FLECAB";
        public const string FleteDetalle = "GP_FLEDET";

        public const string BusinessPartnerTemp = "GP_CLITEM";

        public const string ShoppingCart = "GP_CARITM";

        public const string PurchaseOrderAuthorization = "GP_AUPEOC";
        public const string SaleOrderAuthorization = "GP_AUTPED";

        public const string QuotationAccessory = "GP_ACCESO";
        public const string ProductFormat = "GP_FORMAT";
        public const string ProductInkLevel = "GP_NIVTIN";
        public const string QuotationIndirectSpending = "GP_GASIND";
        public const string ProductGrammage = "GP_GRAMAJ";
        public const string ProductMaterialType = "GP_CATMAT";
        public const string ProductMaterial = "GP_MAFOPR";
        public const string ProductionProcess = "GP_PROCES";
        public const string ProductionProcessTypeCost = "GP_TIPCOS";
        public const string ProductFormula = "GP_FORPRO";
        public const string ProductFormulaConsumptionFactor = "GP_FORFCO";
        public const string ProductFormulaProductionProcess = "GP_FORPPR";

        public const string SaleQuotation = "GP_COTCAB";
        public const string SaleQuotationDetail = "GP_COTDET";
        public const string SaleQuotationDetailProcess = "GP_COTPRO";
        public const string SaleQuotationDetailMaterial = "GP_COTMAT";
        public const string SaleQuotationDetailAccessory = "GP_COTACC";
    }
}
