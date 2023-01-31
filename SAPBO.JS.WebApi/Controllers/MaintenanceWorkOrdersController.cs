using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using Microsoft.ReportingServices.Interfaces;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Helper;
using SAPBO.JS.WebApi.Utilities;
using System.Data;
using System.Text;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin + ", " + RoleNames.MaintenanceEmployees)]
    public class MaintenanceWorkOrdersController : ControllerBase
    {
        private readonly IFileStorage _fileStorage;
        private readonly IMaintenanceWorkOrderBusiness repository;
        private readonly ILogger<MaintenanceWorkOrdersController> logger;

        public MaintenanceWorkOrdersController(IMaintenanceWorkOrderBusiness repository, IFileStorage fileStorage, ILogger<MaintenanceWorkOrdersController> logger)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _fileStorage = fileStorage;
            this.repository = repository;
            this.logger = logger;
        }

        // GET api/values
        [HttpGet(Name = "GetMaintenanceWorkOrders")]
        public async Task<ICollection<MaintenanceWorkOrder>> Get(int year, int month, Enums.ObjectType objectType = Enums.ObjectType.Only)
        {
            return await repository.GetAllAsync(year, month, objectType);
        }

        // GET api/values/5
        [HttpGet("{id}", Name = "GetMaintenanceWorkOrder")]
        public async Task<ActionResult<MaintenanceWorkOrder>> Get(int id, Enums.ObjectType objectType = Enums.ObjectType.Full)
        {
            try
            {
                var maintenanceWorkOrder = await repository.GetAsync(id, objectType);

                if (maintenanceWorkOrder == null)
                    return NotFound();

                return maintenanceWorkOrder;
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MaintenanceWorkOrder maintenanceWorkOrder)
        {
            try
            {
                await repository.CreateAsync(maintenanceWorkOrder);

                return new CreatedAtRouteResult("GetMaintenanceWorkOrder", new { id = maintenanceWorkOrder.Id }, maintenanceWorkOrder);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenanceWorkOrder.CreatedBy
                });
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MaintenanceWorkOrder maintenanceWorkOrder)
        {
            try
            {
                if (!id.Equals(maintenanceWorkOrder.Id))
                    return BadRequest(new ServiceException
                    {
                        Message = $"{AppMessages.ErrorMessage} {AppMessages.ParameterIdAndObjectIdNotMatch}",
                        UserId = maintenanceWorkOrder.UpdatedBy
                    });

                await repository.UpdateAsync(maintenanceWorkOrder);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = maintenanceWorkOrder.UpdatedBy
                });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deleteBy)
        {
            try
            {
                await repository.DeleteAsync(id, deleteBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = deleteBy
                });
            }
        }

        // POST api/values/init/5
        [HttpPost("Init/{id}")]
        public async Task<ActionResult> Init(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.InitAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }

        // POST api/values/end/5
        [HttpPost("End/{id}")]
        public async Task<ActionResult> End(int id, [FromQuery] string updatedBy)
        {
            try
            {
                await repository.EndAsync(id, updatedBy);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException
                {
                    Message = $"{AppMessages.ErrorMessage} {e.Message}",
                    UserId = updatedBy
                });
            }
        }

        //GET api/values/5
        [HttpGet("Print/{id}")]
        public async Task<ActionResult> Print(int id)
        {
            try
            {
                var maintenanceWorkOrder = await repository.GetAsync(id, Enums.ObjectType.Full);

                if (maintenanceWorkOrder == null)
                    return NotFound();

                var fullPath = _fileStorage.GetWebPath("Reports", "MaintenanceWorkOrderReport.rdlc");

                Stream reportDefinition;
                using var fileStream = new FileStream(fullPath, FileMode.Open);
                reportDefinition = fileStream;

                var localReport = new LocalReport();
                localReport.LoadReportDefinition(reportDefinition);

                //Set Parameters
                localReport.SetParameters(new[] { new ReportParameter("rp1", "Hello") });

                //Set Datasources
                var otmDT = new ReportDataSources.MaintenanceWorkOrderReportDS.MaintenanceWorkOrderDTDataTable();
                var otmRow = otmDT.NewRow();
                otmRow["Id"] = maintenanceWorkOrder.Id;
                otmRow["Description"] = maintenanceWorkOrder.Description;
                otmRow["MaintenancePriorityId"] = maintenanceWorkOrder.MaintenancePriority.Id;
                otmRow["MaintenancePriorityName"] = maintenanceWorkOrder.MaintenancePriority.Name;
                otmRow["MaintenanceTypeId"] = maintenanceWorkOrder.MaintenanceType.Id;
                otmRow["MaintenanceTypeName"] = maintenanceWorkOrder.MaintenanceType.Name;

                if (maintenanceWorkOrder.OTMType == Enums.OTMType.Produccion)
                {
                    otmRow["ProductionMachineId"] = maintenanceWorkOrder.ProductionMachine.Id;
                    otmRow["ProductionMachineName"] = maintenanceWorkOrder.ProductionMachine.Name;
                    otmRow["NameMaqCen"] = maintenanceWorkOrder.ProductionMachine.Name;
                    if (maintenanceWorkOrder.ProductionMachineZoneId.HasValue)
                    {
                        otmRow["ProductionMachineZoneId"] = maintenanceWorkOrder.ProductionMachineZone.Id;
                        otmRow["ProductionMachineZoneName"] = maintenanceWorkOrder.ProductionMachineZone.Name;
                        otmRow["NameZone"] = maintenanceWorkOrder.ProductionMachineZone.Name;
                    }
                    otmRow["TxtNameMaqCen"] = "Maquina";
                    otmRow["TxtZone"] = "Zona";
                }
                else
                {
                    otmRow["CostCenterId"] = maintenanceWorkOrder.CostCenter.Id;
                    otmRow["CostCenterName"] = maintenanceWorkOrder.CostCenter.Name;
                    otmRow["NameMaqCen"] = maintenanceWorkOrder.CostCenter.Name;
                    otmRow["TxtNameMaqCen"] = "Centro costos";
                }

                otmRow["StartDate"] = maintenanceWorkOrder.StartDate.ToString(AppFormats.FullDate);
                otmRow["FinalDate"] = maintenanceWorkOrder.FinalDate.HasValue ? maintenanceWorkOrder.FinalDate.Value.ToString(AppFormats.FullDate) : "";
                otmRow["EffectiveHours"] = maintenanceWorkOrder.EffectiveHours;
                otmRow["EmployeeId"] = maintenanceWorkOrder.Employee.Id;
                otmRow["EmployeeName"] = maintenanceWorkOrder.Employee.FullName;
                otmRow["Remark"] = maintenanceWorkOrder.Remark;
                otmRow["StopPlant"] = maintenanceWorkOrder.StopPlant ? "Si" : "No";
                otmRow["StopMachine"] = maintenanceWorkOrder.StopMachine ? "Si" : "No";
                otmRow["StatusId"] = maintenanceWorkOrder.StatusId;
                otmRow["Status"] = maintenanceWorkOrder.StatusType;

                if (maintenanceWorkOrder.MaintenanceProgramId.HasValue)
                    otmRow["MaintenanceProgramId"] = maintenanceWorkOrder.MaintenanceProgramId.Value;

                otmRow["OtmTypeId"] = maintenanceWorkOrder.OtmTypeId;
                otmRow["OtmType"] = maintenanceWorkOrder.OTMType;

                otmDT.Rows.Add(otmRow);
                localReport.DataSources.Add(new ReportDataSource("MaintenanceWorkOrderDS", (DataTable)otmDT));

                var employeesDT = new ReportDataSources.MaintenanceWorkOrderReportDS.MaintenanceWorkOrderEmployeeDTDataTable();
                if (maintenanceWorkOrder.Employees != null && maintenanceWorkOrder.Employees.Count > 0)
                    foreach (var employee in maintenanceWorkOrder.Employees)
                    {
                        var employeeRow = employeesDT.NewRow();
                        employeeRow["Id"] = employee.Id;
                        employeeRow["MaintenanceWorkOrderId"] = employee.MaintenanceWorkOrderId;
                        employeeRow["EmployeeId"] = employee.Employee.Id;
                        employeeRow["EmployeeName"] = employee.Employee.FullName;
                        employeeRow["Task"] = employee.Task;
                        employeeRow["EstimatedTime"] = employee.EstimatedTime;
                        employeesDT.Rows.Add(employeeRow);
                    }
                localReport.DataSources.Add(new ReportDataSource("MaintenanceWorkOrderEmployeeDS", (DataTable)employeesDT));

                var toolDT = new ReportDataSources.MaintenanceWorkOrderReportDS.MaintenanceWorkOrderToolDTDataTable();
                if (maintenanceWorkOrder.Tools != null && maintenanceWorkOrder.Tools.Count > 0)
                    foreach (var tool in maintenanceWorkOrder.Tools)
                    {
                        var toolRow = toolDT.NewRow();
                        toolRow["Id"] = tool.Id;
                        toolRow["MaintenanceWorkOrderId"] = tool.MaintenanceWorkOrderId;
                        toolRow["MaintenanceToolId"] = tool.MaintenanceTool.Id;
                        toolRow["MaintenanceToolName"] = tool.MaintenanceTool.Name;
                        toolRow["MaintenanceToolMedidaId"] = "";
                        toolRow["Quantity"] = tool.Quantity;
                        toolDT.Rows.Add(toolRow);
                    }
                localReport.DataSources.Add(new ReportDataSource("MaintenanceWorkOrderToolDS", (DataTable)toolDT));

                var replacementDT = new ReportDataSources.MaintenanceWorkOrderReportDS.MaintenanceWorkOrderReplacementDTDataTable();
                if (maintenanceWorkOrder.Replacements != null && maintenanceWorkOrder.Replacements.Count > 0)
                    foreach (var replacement in maintenanceWorkOrder.Replacements)
                    {
                        var replacementRow = replacementDT.NewRow();
                        replacementRow["Id"] = replacement.Id;
                        replacementRow["MaintenanceWorkOrderId"] = replacement.MaintenanceWorkOrderId;
                        replacementRow["ReplacementId"] = replacement.Replacement.Id;
                        replacementRow["ReplacementName"] = replacement.Replacement.Name;
                        replacementRow["ReplacementMedidaId"] = replacement.Replacement.MedidaId;
                        replacementRow["PlannedQuantity"] = replacement.PlannedQuantity;
                        replacementRow["ConsumedQuantity"] = replacement.ConsumedQuantity;
                        replacementRow["TimeFrequencyId"] = replacement.TimeFrequency.Id;
                        replacementRow["TimeFrequencyName"] = replacement.TimeFrequency.Name;
                        replacementRow["TimeFrequencyValue"] = replacement.TimeFrequencyValue;
                        replacementDT.Rows.Add(replacementRow);
                    }
                localReport.DataSources.Add(new ReportDataSource("MaintenanceWorkOrderReplacementDS", (DataTable)replacementDT));

                var failureDT = new ReportDataSources.MaintenanceWorkOrderReportDS.MaintenanceWorkOrderMachineFailureDTDataTable();
                if (maintenanceWorkOrder.Failures != null && maintenanceWorkOrder.Failures.Count > 0)
                    foreach (var failure in maintenanceWorkOrder.Failures)
                    {
                        var failureRow = failureDT.NewRow();

                        failureRow["Id"] = failure.Id;
                        failureRow["ProductionMachineId"] = failure.ProductionMachine.Id;
                        failureRow["ProductionMachineName"] = failure.ProductionMachine.Name;
                        failureRow["ProductionMachineZoneId"] = failure.ProductionMachineZone.Id;
                        failureRow["ProductionMachineZoneName"] = failure.ProductionMachineZone.Name;
                        failureRow["StartDate"] = failure.StartDate.ToString(AppFormats.FullDate);
                        failureRow["FinalDate"] = failure.FinalDate.ToString(AppFormats.FullDate);
                        failureRow["FailureTypeId"] = failure.FailureType.Id;
                        failureRow["FailureTypeName"] = failure.FailureType.Name;
                        failureRow["FailureSeverityId"] = failure.FailureSeverity.Id;
                        failureRow["FailureSeverityName"] = failure.FailureSeverity.Name;
                        failureRow["FailureCauseId"] = failure.FailureCause.Id;
                        failureRow["FailureCauseName"] = failure.FailureCause.Name;
                        failureRow["FailureMechanismId"] = failure.FailureMechanism.Id;
                        failureRow["FailureMechanismName"] = failure.FailureMechanism.Name;
                        failureRow["FailureImpactId"] = failure.FailureImpact.Id;
                        failureRow["FailureImpactName"] = failure.FailureImpact.Name;
                        failureRow["Remark"] = failure.Remark;
                        failureRow["StopMachine"] = failure.StopMachine ? "Si" : "No";

                        if (failure.StopMachine)
                        {
                            failureRow["StopStartDate"] = failure.StopStartDate.Value.ToString(AppFormats.FullDate);
                            failureRow["StopFinalDate"] = failure.StopFinalDate.Value.ToString(AppFormats.FullDate);
                        }

                        failureRow["StatusId"] = failure.StatusId;
                        failureRow["Status"] = failure.StatusType;

                        if (failure.MaintenanceWorkOrderId.HasValue)
                            failureRow["MaintenanceWorkOrderId"] = failure.MaintenanceWorkOrderId.Value;

                        failureDT.Rows.Add(failureRow);
                    }
                localReport.DataSources.Add(new ReportDataSource("MaintenanceWorkOrderMachineFailureDS", (DataTable)failureDT));

                var pdfReport = localReport.Render("PDF");

                fileStream.Dispose();

                var reportName = $"OT-{id}.pdf";
                return File(pdfReport, AppDefaultValues.PDFApplication, reportName);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = $"{AppMessages.ErrorMessage} {e.Message}" });
            }
        }

    }
}
