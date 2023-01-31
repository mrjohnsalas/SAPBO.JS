using SAPBO.JS.Common;
using SAPBO.JS.Model.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SAPBO.JS.Model.Domain
{
    public class ProductionMachine : AuditEntity
    {
        [Key]
        [Display(Name = "Maquina Id")]
        public int Id { get; set; }

        [Display(Name = "Maquina")]
        [Required(ErrorMessage = AppMessages.RequiredFieldErrorMessage)]
        [StringLength(30, ErrorMessage = AppMessages.StringLengthFieldErrorMessage, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Estado Id")]
        public int StatusId { get; set; }

        [Display(Name = "Estado")]
        public Enums.StatusType StatusType => (Enums.StatusType)StatusId;

        public ICollection<MachineFailure> MachineFailures { get; set; }

        public ICollection<MaintenanceProgram> MaintenancePrograms { get; set; }

        public ICollection<MaintenanceWorkOrder> MaintenanceWorkOrders { get; set; }

        public ICollection<ProductionMachineZone> Zones { get; set; }
    }
}
