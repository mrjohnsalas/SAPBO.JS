using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class CRMActivityMapper : ISapB1AutoMapper<CRMActivity>
    {
        public CRMActivity Mapper(IRecordset rs)
        {
            var activity = new CRMActivity();
            //{
            activity.Id = int.Parse(rs.Fields.Item("ClgCode").Value.ToString());
            activity.ActivityType = Utilities.StringToActivityType(rs.Fields.Item("Action").Value);

            activity.AssignedToEmployeeId = Utilities.IntValueToIntOrNull(rs.Fields.Item("AttendEmpl").Value);
            activity.AssignedToEmployee = rs.Fields.Item("ASSIGNED_TO_EMPLOYEE").Value.ToString();

            activity.AssignedToUserId = Utilities.IntValueToIntOrNull(rs.Fields.Item("AttendUser").Value);
            activity.AssignedToUser = rs.Fields.Item("ASSIGNED_TO_USER").Value.ToString();

            activity.AssignedByUserId = int.Parse(rs.Fields.Item("AssignedBy").Value.ToString());
            activity.AssignedByUser = rs.Fields.Item("ASSIGNED_BY").Value.ToString();

            activity.BusinessPartnerId = rs.Fields.Item("CardCode").Value.ToString();
            activity.ContactId = int.Parse(rs.Fields.Item("CntctCode").Value.ToString());
            activity.ContactPhone = rs.Fields.Item("Tel").Value.ToString();

            activity.Comment = rs.Fields.Item("Details").Value.ToString();
            activity.StartDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("Recontact").Value, rs.Fields.Item("BeginTime").Value);

            activity.Notes = rs.Fields.Item("Notes").Value.ToString();

            activity.ActivityPriority = (Enums.ActivityPriority)int.Parse(rs.Fields.Item("Priority").Value.ToString());
            activity.ActivityDurationType = Utilities.StringToActivityDurationType(rs.Fields.Item("DurType").Value);
            activity.ActivityDuration = decimal.Parse(rs.Fields.Item("Duration").Value.ToString());
            activity.EndDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("endDate").Value, rs.Fields.Item("ENDTime").Value);
            activity.ActivityLocation = (Enums.ActivityLocation)int.Parse(rs.Fields.Item("Location").Value.ToString());

            activity.AddressId = rs.Fields.Item("AddrName").Value.ToString();
            activity.CountryId = rs.Fields.Item("country").Value.ToString();
            activity.StateId = rs.Fields.Item("state").Value.ToString();
            activity.City = rs.Fields.Item("city").Value.ToString();
            activity.Street = rs.Fields.Item("street").Value.ToString();
            activity.Room = rs.Fields.Item("room").Value.ToString();
            activity.IsClosed = rs.Fields.Item("Closed").Value.ToString().Equals('Y');
            activity.ClosedDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("CloseDate").Value, 0);
            //};
            return activity;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, CRMActivity obj)
        {
            return table;
        }
    }
}
