using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Utility;
using System.Runtime.InteropServices;

namespace SAPBO.JS.Data.Repositories
{
    public class SapB1GenericRepository<T> where T : class
    {
        public readonly SapB1Context _context;
        private readonly ISapB1AutoMapper<T> _mapper;
        private readonly bool _auditEntityMapper;

        public SapB1GenericRepository(SapB1Context context, ISapB1AutoMapper<T> mapper, bool auditEntityMapper = false)
        {
            _context = context;
            _mapper = mapper;
            _auditEntityMapper = auditEntityMapper;
        }

        #region BaseMethods

        private ICollection<T> GetObjects(string query)
        {
            var objects = new List<T>();
            SAPbobsCOM.IRecordset rs = null;
            try
            {
                _context.Connect();
                rs = (SAPbobsCOM.IRecordset)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(query);

                if (_auditEntityMapper)
                {
                    while (!rs.EoF)
                    {
                        objects.Add(GetAuditEntityValues(rs, _mapper.Mapper(rs)));
                        rs.MoveNext();
                    }
                }
                else
                {
                    while (!rs.EoF)
                    {
                        objects.Add(_mapper.Mapper(rs));
                        rs.MoveNext();
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (rs != null)
                    Marshal.ReleaseComObject(rs);
                GC.Collect();
            }

            return objects;
        }

        private void Run(string query)
        {
            SAPbobsCOM.IRecordset rs = null;
            try
            {
                _context.Connect();
                rs = (SAPbobsCOM.IRecordset)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(query);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (rs != null)
                    Marshal.ReleaseComObject(rs);
                GC.Collect();
            }
        }

        private dynamic GetValueDb(string query, string fieldName)
        {
            dynamic value = null;
            SAPbobsCOM.IRecordset rs = null;
            try
            {
                _context.Connect();
                rs = (SAPbobsCOM.IRecordset)_context.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                rs.DoQuery(query);

                if (!rs.EoF)
                    value = rs.Fields.Item(fieldName).Value;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (rs != null)
                    Marshal.ReleaseComObject(rs);
                GC.Collect();
            }

            return value;
        }

        private void UTableObject(string tableName, Enums.ObjectAction objectAction, string id, T obj)
        {
            _context.Connect();

            var uTable = _context.Company.UserTables.Item(tableName);
            var errorCode = 0;

            if (objectAction == Enums.ObjectAction.Update || objectAction == Enums.ObjectAction.Delete)
                uTable.GetByKey(id);

            if (objectAction == Enums.ObjectAction.Insert || objectAction == Enums.ObjectAction.Update)
            {
                _mapper.SetValuesToUserTable(uTable, obj);
                if (_auditEntityMapper)
                    SetAuditEntityValues(uTable, obj);
            }

            switch (objectAction)
            {
                case Enums.ObjectAction.Insert:
                    uTable.Code = id;
                    errorCode = uTable.Add();
                    break;
                case Enums.ObjectAction.Update:
                    errorCode = uTable.Update();
                    break;
                case Enums.ObjectAction.Delete:
                    errorCode = uTable.Remove();
                    break;
            }

            if (errorCode.Equals(0)) return;

            var ex = SapB1ExceptionBuilder.BuildException(errorCode, _context.Company.GetLastErrorDescription());
            if (!string.IsNullOrEmpty(ex.Message))
            {
                GC.Collect();
                // TODO task.run exception - user-unmanaged
                throw ex;
            }
        }

        public void SetAuditEntityValues(SAPbobsCOM.IUserTable ut, dynamic obj)
        {
            if (obj.CreatedAt != null)
                ut.UserFields.Fields.Item("U_CL_CREDAT").Value = obj.CreatedAt.ToString(AppFormats.Date);
            ut.UserFields.Fields.Item("U_CL_CREABY").Value = obj.CreatedBy ?? string.Empty;

            if (obj.UpdatedAt != null)
                ut.UserFields.Fields.Item("U_CL_UPDDAT").Value = obj.UpdatedAt.ToString(AppFormats.Date);
            ut.UserFields.Fields.Item("U_CL_UPDABY").Value = obj.UpdatedBy ?? string.Empty;

            if (obj.DeletedAt != null)
                ut.UserFields.Fields.Item("U_CL_DELDAT").Value = obj.DeletedAt.ToString(AppFormats.Date);
            ut.UserFields.Fields.Item("U_CL_DELEBY").Value = obj.DeletedBy ?? string.Empty;
        }

        public dynamic GetAuditEntityValues(SAPbobsCOM.IRecordset rs, dynamic obj)
        {
            var date = DateTime.Parse(rs.Fields.Item("U_CL_CREDAT").Value.ToString()).ToString(AppFormats.Date);
            if (!date.Equals(AppMessages.SapDateMinValue))
                obj.CreatedAt = DateTime.Parse(date);

            obj.CreatedBy = rs.Fields.Item("U_CL_CREABY").Value?.ToString();

            date = DateTime.Parse(rs.Fields.Item("U_CL_UPDDAT").Value.ToString()).ToString(AppFormats.Date);
            if (!date.Equals(AppMessages.SapDateMinValue))
                obj.UpdatedAt = DateTime.Parse(date);

            obj.UpdatedBy = rs.Fields.Item("U_CL_UPDABY").Value?.ToString();

            date = DateTime.Parse(rs.Fields.Item("U_CL_DELDAT").Value.ToString()).ToString(AppFormats.Date);
            if (!date.Equals(AppMessages.SapDateMinValue))
                obj.DeletedAt = DateTime.Parse(date);

            obj.DeletedBy = rs.Fields.Item("U_CL_DELEBY").Value?.ToString();

            return obj;
        }

        #endregion

        protected ICollection<T> GetAll(string spName, List<dynamic> parameters = null)
        {
            return GetObjects(SapB1QueryBuilder.BuildQuery(spName, parameters));
        }

        protected Task<ICollection<T>> GetAllAsync(string spName, List<dynamic> parameters = null)
        {
            return Task.Run(() => GetAll(spName, parameters));
        }

        protected T Get(string spName, List<dynamic> parameters = null)
        {
            return GetObjects(SapB1QueryBuilder.BuildQuery(spName, parameters)).FirstOrDefault();
        }

        protected Task<T> GetAsync(string spName, List<dynamic> parameters = null)
        {
            return Task.Run(() => Get(spName, parameters));
        }

        protected dynamic GetValue(string spName, string fieldName, List<dynamic> parameters)
        {
            return GetValueDb(SapB1QueryBuilder.BuildQuery(spName, parameters), fieldName);
        }

        protected Task<dynamic> GetValueAsync(string spName, string fieldName, List<dynamic> parameters)
        {
            return Task.Run(() => GetValue(spName, fieldName, parameters));
        }

        protected void Create(string tableName, T obj, string id)
        {
            UTableObject(tableName, Enums.ObjectAction.Insert, id, obj);
        }

        protected Task CreateAsync(string tableName, T obj, string id)
        {
            return Task.Run(() => Create(tableName, obj, id));
        }

        protected void Update(string tableName, T obj, string id)
        {
            UTableObject(tableName, Enums.ObjectAction.Update, id, obj);
        }

        protected Task UpdateAsync(string tableName, T obj, string id)
        {
            return Task.Run(() => Update(tableName, obj, id));
        }

        protected void SoftDeleteById(string tableName, T obj, string id)
        {
            UTableObject(tableName, Enums.ObjectAction.Update, id, obj);
        }

        protected Task SoftDeleteByIdAsync(string tableName, T obj, string id)
        {
            return Task.Run(() => SoftDeleteById(tableName, obj, id));
        }

        protected void DeleteById(string tableName, T obj, string id)
        {
            UTableObject(tableName, Enums.ObjectAction.Delete, id, obj);
        }

        protected Task DeleteByIdAsync(string tableName, T obj, string id)
        {
            return Task.Run(() => DeleteById(tableName, obj, id));
        }

        protected void DeleteAll(string spName, List<dynamic> parameters = null)
        {
            Run(SapB1QueryBuilder.BuildQuery(spName, parameters));
        }

        protected Task DeleteAllAsync(string spName, List<dynamic> parameters = null)
        {
            return Task.Run(() => DeleteAll(spName, parameters));
        }

        protected void UpdateAll(string spName, List<dynamic> parameters = null)
        {
            Run(SapB1QueryBuilder.BuildQuery(spName, parameters));
        }

        protected Task UpdateAllAsync(string spName, List<dynamic> parameters = null)
        {
            return Task.Run(() => DeleteAll(spName, parameters));
        }
    }
}
