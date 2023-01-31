using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Utility;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Data.Repositories
{
    public class CRMActivityRepository : SapB1GenericRepository<CRMActivity>
    {
        public CRMActivityRepository(SapB1Context context, ISapB1AutoMapper<CRMActivity> mapper) : base(context, mapper)
        {

        }

        private SAPbobsCOM.BoActivities GetActivityType(Enums.ActivityType activityType)
        {
            switch (activityType)
            {
                case Enums.ActivityType.PhoneCall:
                    return SAPbobsCOM.BoActivities.cn_Conversation;
                case Enums.ActivityType.Meeting:
                    return SAPbobsCOM.BoActivities.cn_Meeting;
                case Enums.ActivityType.Task:
                    return SAPbobsCOM.BoActivities.cn_Task;
                case Enums.ActivityType.Note:
                    return SAPbobsCOM.BoActivities.cn_Note;
                case Enums.ActivityType.Campaign:
                    return SAPbobsCOM.BoActivities.cn_Campaign;
                default:
                    return SAPbobsCOM.BoActivities.cn_Other;
            }
        }

        private SAPbobsCOM.BoMsgPriorities GetActivityPriority(Enums.ActivityPriority activityPriority)
        {
            switch (activityPriority)
            {
                case Enums.ActivityPriority.Low:
                    return SAPbobsCOM.BoMsgPriorities.pr_Low;
                case Enums.ActivityPriority.Normal:
                    return SAPbobsCOM.BoMsgPriorities.pr_Normal;
                case Enums.ActivityPriority.High:
                    return SAPbobsCOM.BoMsgPriorities.pr_High;
                default:
                    return SAPbobsCOM.BoMsgPriorities.pr_Normal;
            }
        }

        private SAPbobsCOM.BoAddressType GetAddressType(Enums.AddressType addressType)
        {
            switch (addressType)
            {
                case Enums.AddressType.Bill:
                    return SAPbobsCOM.BoAddressType.bo_BillTo;
                case Enums.AddressType.Ship:
                    return SAPbobsCOM.BoAddressType.bo_ShipTo;
                default:
                    return SAPbobsCOM.BoAddressType.bo_ShipTo;
            }
        }

        public void CreateOrUpdate(CRMActivity obj, Enums.OperationType operationType)
        {
            _context.Connect();

            var activity = (SAPbobsCOM.Contacts)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts);

            if (operationType == Enums.OperationType.Update)
                activity.GetByKey(obj.Id);

            // All
            activity.Activity = GetActivityType(obj.ActivityType);
            activity.ActivityType = obj.ActivityClass;
            activity.Subject = obj.ActivitySubject;
            activity.HandledBy = obj.AssignedByUserId;
            activity.SalesEmployee = obj.AssignedToEmployeeId.Value;
            activity.CardCode = obj.BusinessPartnerId;
            activity.ContactPersonCode = obj.ContactId;
            activity.Phone = obj.ContactPhone;

            // Default values
            activity.DurationType = SAPbobsCOM.BoDurations.du_Minuts;
            activity.ReminderType = SAPbobsCOM.BoDurations.du_Minuts;

            // General - Notes, Meetings and Calls
            activity.Details = obj.Comment;

            if (!obj.StartDate.HasValue)
                obj.StartDate = DateTime.Now;

            activity.StartDate = obj.StartDate.Value;
            activity.StartTime = obj.StartDate.Value;
            activity.Priority = GetActivityPriority(obj.ActivityPriority);

            // General - Meetings and Calls
            if (obj.ActivityType == Enums.ActivityType.PhoneCall || obj.ActivityType == Enums.ActivityType.Meeting)
            {
                activity.EndDuedate = obj.EndDate.Value;
                activity.EndTime = obj.EndDate.Value;

                // General - Meetings
                if (obj.ActivityType == Enums.ActivityType.Meeting)
                {
                    activity.Location = (int)obj.ActivityLocation;
                    activity.Country = obj.CountryId;
                    activity.State = obj.StateId;
                    activity.City = obj.City;
                    activity.Street = obj.Street;

                    activity.Room = obj.Room;
                }
            }

            // Content
            activity.Notes = obj.Notes;

            int errorCode = operationType == Enums.OperationType.Create ? activity.Add() : activity.Update();

            if (operationType == Enums.OperationType.Create)
                obj.Id = GetValue("GP_WEB_APP_362", "Id", new List<dynamic> { obj.AssignedToEmployeeId.Value });

            if (errorCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
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

            var activity = (SAPbobsCOM.Contacts)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts);

            activity.GetByKey(id);

            activity.Inactiveflag = SAPbobsCOM.BoYesNoEnum.tYES;

            int errorCode = activity.Update();

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
