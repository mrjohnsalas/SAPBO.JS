using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class BusinessPartnerVehicleBusiness : SapB1GenericRepository<BusinessPartnerVehicle>, IBusinessPartnerVehicleBusiness
    {
        private const string _tableName = TableNames.Vehicle;

        public BusinessPartnerVehicleBusiness(SapB1Context context, ISapB1AutoMapper<BusinessPartnerVehicle> mapper) : base(context, mapper, true)
        {
            
        }

        public Task<ICollection<BusinessPartnerVehicle>> GetAllAsync(string businessPartnerId)
        {
            return GetAllAsync("GP_WEB_APP_152", new List<dynamic> { businessPartnerId });
        }

        public Task<ICollection<BusinessPartnerVehicle>> GetAllWithIdsAsync(IEnumerable<int> ids)
        {
            return GetAllAsync("GP_WEB_APP_480", new List<dynamic> { string.Join(",", ids) });
        }

        public Task<BusinessPartnerVehicle> GetAsync(int id)
        {
            return GetAsync("GP_WEB_APP_153", new List<dynamic> { id });
        }

        public Task<BusinessPartnerVehicle> GetByPlacaIdAsync(string placa)
        {
            return GetAsync("GP_WEB_APP_263", new List<dynamic> { placa });
        }

        public async Task CreateAsync(BusinessPartnerVehicle obj)
        {
            CheckRules(obj, Enums.ObjectAction.Insert);

            //Check Placa
            var vehicle = await GetByPlacaIdAsync(obj.Placa);
            if (vehicle != null)
                throw new Exception(AppMessages.Vehicle_Placa);

            obj.StatusId = (int)Enums.StatusType.Activo;
            obj.CreatedAt = DateTime.Now;

            obj.Id = GetNewId();
            await CreateAsync(_tableName, obj, obj.Id.ToString());
        }

        public async Task UpdateAsync(BusinessPartnerVehicle obj)
        {
            //Get obj
            var currentObj = await GetAsync(obj.Id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            CheckRules(obj, Enums.ObjectAction.Update, currentObj);

            //Check Placa
            var vehicle = await GetByPlacaIdAsync(obj.Placa);
            if (vehicle != null && vehicle.Id != obj.Id)
                throw new Exception(AppMessages.Vehicle_Placa);

            //Set obj
            currentObj.UpdatedBy = obj.UpdatedBy;

            currentObj.Year = obj.Year;
            currentObj.Color = obj.Color;
            currentObj.Marca = obj.Marca;
            currentObj.Modelo = obj.Modelo;
            currentObj.Placa = obj.Placa;
            currentObj.PesoMaximo = obj.PesoMaximo;
            currentObj.SerieMotor = obj.SerieMotor;
            currentObj.VencimientoSoat = obj.VencimientoSoat;
            currentObj.ConstanciaInscripcion = obj.ConstanciaInscripcion;
            currentObj.CertificadoInscripcion = obj.CertificadoInscripcion;
            currentObj.Configuracion = obj.Configuracion;
            currentObj.SerieChasis = obj.SerieChasis;
            currentObj.SerieOpcional = obj.SerieOpcional;
            currentObj.BusinessPartnerId = obj.BusinessPartnerId;
            currentObj.UpdatedAt = DateTime.Now;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task DeleteAsync(int id, string deleteBy)
        {
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            obj.DeletedBy = deleteBy;

            CheckRules(obj, Enums.ObjectAction.Delete);

            obj.StatusId = (int)Enums.StatusType.Anulado;
            obj.DeletedAt = DateTime.Now;

            await SoftDeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        private static void CheckRules(BusinessPartnerVehicle obj, Enums.ObjectAction objectAction, BusinessPartnerVehicle currentObj = null)
        {
            switch (objectAction)
            {
                case Enums.ObjectAction.Insert:
                    //Check User - Create
                    if (string.IsNullOrEmpty(obj.CreatedBy))
                        throw new Exception(AppMessages.UserError);

                    break;
                case Enums.ObjectAction.Update:
                    //Check Status
                    if (currentObj?.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Update
                    if (string.IsNullOrEmpty(obj.UpdatedBy))
                        throw new Exception(AppMessages.UserError);

                    break;
                case Enums.ObjectAction.Delete:
                    //Check Status
                    if (obj.StatusType != Enums.StatusType.Activo)
                        throw new Exception(AppMessages.StatusError);

                    //Check User - Delete
                    if (string.IsNullOrEmpty(obj.DeletedBy))
                        throw new Exception(AppMessages.UserError);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(objectAction), objectAction, null);
            }
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_266", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }
    }
}
