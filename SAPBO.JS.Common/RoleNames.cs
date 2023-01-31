using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class RoleNames
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Supplier = "Supplier";
        public const string Manager = "Manager";
        public const string Carrier = "Carrier";

        public const string All =
            SalesEmployees + ", " +
            CreditEmployees + ", " +
            LogisticsEmployees + ", " +
            MaintenanceEmployees + ", " +
            ProductionEmployees + ", " +
            PurchaseEmployees + ", " +
            Admin + ", " +
            Customer + ", " +
            Supplier + ", " +
            Carrier + ", " +
            Manager;

        public const string SalesAdmin = "SalesAdmin";
        public const string SalesManager = "SalesManager";
        public const string SalesEmployee = "SalesEmployee";
        public const string SalesEmployees = SalesAdmin + ", " + SalesManager + ", " + SalesEmployee;

        public const string CreditAdmin = "CreditAdmin";
        public const string CreditManager = "CreditManager";
        public const string CreditEmployee = "CreditEmployee";
        public const string CreditEmployees = CreditAdmin + ", " + CreditManager + ", " + CreditEmployee;

        public const string LogisticsAdmin = "LogisticsAdmin";
        public const string LogisticsManager = "LogisticsManager";
        public const string LogisticsEmployee = "LogisticsEmployee";
        public const string LogisticsGrocer = "LogisticsGrocer";
        public const string LogisticsEmployees = LogisticsAdmin + ", " + LogisticsManager + ", " + LogisticsEmployee + ", " + LogisticsGrocer;

        public const string MaintenanceAdmin = "MaintenanceAdmin";
        public const string MaintenanceManager = "MaintenanceManager";
        public const string MaintenanceEmployee = "MaintenanceEmployee";
        public const string MaintenanceGrocer = "MaintenanceGrocer";
        public const string MaintenanceEmployees = MaintenanceAdmin + ", " + MaintenanceManager + ", " + MaintenanceEmployee + ", " + MaintenanceGrocer;

        public const string ProductionAdmin = "ProductionAdmin";
        public const string ProductionManager = "ProductionManager";
        public const string ProductionEmployee = "ProductionEmployee";
        public const string ProductionEmployees = ProductionAdmin + ", " + ProductionManager + ", " + ProductionEmployee;

        public const string PurchaseAdmin = "PurchaseAdmin";
        public const string PurchaseManager = "PurchaseManager";
        public const string PurchaseEmployee = "PurchaseEmployee";
        public const string PurchaseEmployees = PurchaseAdmin + ", " + PurchaseManager + ", " + PurchaseEmployee;
    }
}
