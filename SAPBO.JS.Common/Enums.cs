using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class Enums
    {
        public enum OTMType
        {
            Produccion = 1,
            Administracion
        }

        public enum ObjectType
        {
            Only,
            OnlyFull,
            Full,
            FullHeader,
            Custom
        }

        public enum ObjectAction
        {
            Insert,
            Update,
            Delete,
            Init,
            Pause,
            End,
            Approve,
            Reject,
            Override
        }

        public enum StatusType
        {
            Todos = -1,
            Anulado,
            Activo,
            Solicitado,
            Pendiente,
            Completado,
            Vencido,
            PorEntregar,
            Entregado,
            Planificado,
            Iniciado,
            Terminado,
            Pausado,
            Aceptado,
            Rechazado,
            Abierto,
            Cerrado,
            PorDespachar,
            ParcialmenteAtendido,
            Autorizado
        }

        public static IEnumerable<EnumDescriptionAndValue> GetAllEnumsWithChilds()
        {
            var enums = new List<EnumDescriptionAndValue>();
            var order = 0;

            foreach (var type in typeof(Enums).GetNestedTypes())
            {
                var parent = new EnumDescriptionAndValue
                {
                    Name = type.Name,
                    Order = order
                };

                foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                {
                    var i = 0;
                    var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                    parent.Childs.Add(new EnumDescriptionAndValue
                    {
                        Name = field.Name,
                        Value = field.GetRawConstantValue().ToString(),
                        Description = attribute == null ? field.Name : attribute.Description,
                        Order = i
                    });

                    i++;
                }

                enums.Add(parent);
                order++;
            }

            return enums;
        }

        public enum TimeFrequencyType
        {
            None,
            Minute,
            Hour,
            Day,
            Month,
            Year
        }

        public enum OperationType
        {
            Create,
            Update,
            Delete
        }

        public enum ActivityType
        {
            PhoneCall,
            Meeting,
            Task,
            Note,
            Campaign,
            Other
        }

        public enum ActivityPriority
        {
            Low,
            Normal,
            High
        }

        public enum ActivityLocation
        {
            SNAddress = -2,
            Other = -1
        }

        public enum AddressType
        {
            Bill,
            Ship
        }

        public enum ActivityDurationType
        {
            Seconds,
            Minutes,
            Hours,
            Days
        }

        public enum SaleOpportunityType
        {
            Sale,
            Purchase
        }

        public enum SaleOpportunityStatus
        {
            Open,
            Lost,
            Won
        }

        public enum SaleOpportunityInterestLevel
        {
            Low = 1,
            Half = 2,
            High = 3
        }

        public enum OpportunityStages
        {
            FirstContact = 1,
            Offer = 2,
            Negotiation = 3,
            Review = 4,
            Acceptance = 5
        }

        public enum PurchaseOrderAuthorizationStatus
        {
            Pendiente,
            Autorizado,
            Rechazado
        }

        public enum BusinessPartnerType
        {
            Customer,
            Supplier
        }

        public enum SaleOrderType
        {
            Linea,
            Impreso,
            Flexografia,
            Otros
        }

        public enum PriceType
        {
            Local,
            Importado,
            Licitacion
        }

        public enum EmailToType
        {
            Fr,
            To,
            Cc,
            Co
        }

        public enum DeliveryStep
        {
            Received,
            Confirmed,
            Ready,
            Dispatched,
            Delivered
        }

        public enum FileTypeGroup
        {
            Image
        }

        //INTEREST LEVEL
    }

    public class EnumDescriptionAndValue
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public List<EnumDescriptionAndValue> Childs { get; set; }

        public EnumDescriptionAndValue()
        {
            Childs = new List<EnumDescriptionAndValue>();
        }
    }
}
