using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BusinessPartnerVehicleMapper : ISapB1AutoMapper<BusinessPartnerVehicle>
    {
        public BusinessPartnerVehicle Mapper(IRecordset rs)
        {
            return new BusinessPartnerVehicle
            {
                Id = int.Parse(rs.Fields.Item("Code").Value.ToString()),
                Year = int.Parse(rs.Fields.Item("U_BPP_VEAN").Value.ToString() ?? "0"),
                Color = rs.Fields.Item("U_BPP_VECO").Value.ToString(),
                Marca = rs.Fields.Item("U_BPP_VEMA").Value.ToString(),
                Modelo = rs.Fields.Item("U_BPP_VEMO").Value.ToString(),
                Placa = rs.Fields.Item("U_BPP_VEPL").Value.ToString(),
                PesoMaximo = decimal.Parse(rs.Fields.Item("U_BPP_VEPM").Value.ToString()),
                SerieMotor = rs.Fields.Item("U_BPP_VESE").Value.ToString(),
                ConstanciaInscripcion = rs.Fields.Item("U_CL_CNTINS").Value.ToString(),
                CertificadoInscripcion = rs.Fields.Item("U_TC_CERT_INSC").Value.ToString(),
                Configuracion = rs.Fields.Item("U_TC_CONF_VEHI").Value.ToString(),
                SerieChasis = rs.Fields.Item("U_SM_SERIE_MOTOR").Value.ToString(),
                SerieOpcional = rs.Fields.Item("U_SERIE4").Value.ToString(),
                BusinessPartnerId = rs.Fields.Item("U_CL_CODPRO").Value.ToString(),
                VencimientoSoat = Utilities.DateValueToDateOrNull(rs.Fields.Item("U_VS_VTO_SOAT").Value),
                StatusId = Utilities.GetStatusIdByText(rs.Fields.Item("U_VS_FROZEN").Value.ToString())
            };
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BusinessPartnerVehicle obj)
        {
            table.Name = obj.Id.ToString();
            table.UserFields.Fields.Item("U_BPP_VEAN").Value = obj.Year.ToString();
            table.UserFields.Fields.Item("U_BPP_VECO").Value = obj.Color ?? string.Empty;
            table.UserFields.Fields.Item("U_BPP_VEMA").Value = obj.Marca ?? string.Empty;
            table.UserFields.Fields.Item("U_BPP_VEMO").Value = obj.Modelo ?? string.Empty;
            table.UserFields.Fields.Item("U_BPP_VEPL").Value = obj.Placa ?? string.Empty;
            table.UserFields.Fields.Item("U_BPP_VEPM").Value = (double)obj.PesoMaximo;
            table.UserFields.Fields.Item("U_BPP_VESE").Value = obj.SerieMotor ?? string.Empty;

            if (obj.VencimientoSoat.HasValue)
                table.UserFields.Fields.Item("U_VS_VTO_SOAT").Value = obj.VencimientoSoat.Value.ToString(AppFormats.Date);

            table.UserFields.Fields.Item("U_CL_CNTINS").Value = obj.ConstanciaInscripcion ?? string.Empty;
            table.UserFields.Fields.Item("U_TC_CERT_INSC").Value = obj.CertificadoInscripcion ?? string.Empty;
            table.UserFields.Fields.Item("U_TC_CONF_VEHI").Value = obj.Configuracion ?? string.Empty;
            table.UserFields.Fields.Item("U_SM_SERIE_MOTOR").Value = obj.SerieChasis ?? string.Empty;
            table.UserFields.Fields.Item("U_SERIE4").Value = obj.SerieOpcional ?? string.Empty;
            table.UserFields.Fields.Item("U_CL_CODPRO").Value = obj.BusinessPartnerId ?? string.Empty;
            table.UserFields.Fields.Item("U_VS_FROZEN").Value = Utilities.GetTextByStatusId(obj.StatusId, true);

            return table;
        }
    }
}
