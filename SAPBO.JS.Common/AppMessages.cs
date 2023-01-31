using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class AppMessages
    {
        public const string AppTitle = "JS+";
        public const string GoodNotification = "La operación se realizó satisfactoriamente.";
        public const string BadNotification = "La operación no pudo completarse satisfactoriamente.";
        public const string NotFound = "Su búsqueda no produjo ningún resultado.";
        public const string NotFoundFromOperation = "No se encontró el objeto, para realizar la operación.";
        public const string CarrierNotFoundFromOperation = "No se encontró el transportista, para realizar la operación.";
        public const string AddressNotFoundFromOperation = "No se encontró la dirección, para realizar la operación.";
        public const string NotFoundContentFromOperation = "No se encontró el contenido para el objeto, para realizar la operación.";
        public const string ParameterIdAndObjectIdNotMatch = "El Id del parámetro no coincide con el Id del objeto.";
        public const string StatusError = "No se pudo actualizar el registro. Verifique el status.";
        public const string UserError = "Debe especificar el Id de usuario responsable de la operación.";
        public const string CheckUserError = "Los usuarios no coinciden.";
        public const string TitleError = "Error";
        public const string PleaseWait = "Espere por favor...";
        public const string LoadingData = "Cargando datos...";
        public const string GenNewId = "No se pudo generar el nuevo id.";
        public const string NameAlreadyExists = "El nombre ya existe.";
        public const string MaxFileWeightErrorMessage = "El peso del archivo no debe ser mayor a {0}.";
        public const string ValidFileTypeErrorMessage = "El tipo de archivo debe ser uno de los siguientes: {0}.";

        public const string SapDateMinValue = "1899-12-30";

        public const string InvalidLogin = "Usuario y/o contraseña invalidos.";
        public const string CreateUserError = "No se pudo crear el usuario.";
        public const string UserNotFound = "Usuario no encontrado.";
        public const string RoleNotFound = "Rol no encontrado.";
        public const string UserAlreadyExists = "El usuario ya existe.";
        public const string RoleAlreadyExists = "El rol ya existe.";
        public const string RoleCouldNotBeAssignedToUser = "El rol no se pudo asignar al usuario.";
        public const string RoleCouldNotBeRemovedFromUser = "El rol no se pudo eliminar del usuario.";
        public const string RoleAlreadyAssignedToUser = "El rol ya está asignado al usuario.";
        public const string RoleIsNotAssignedToUser = "El rol no está asignado al usuario.";

        public const string ErrorMessage = "Error Message:";
        public const string RequiredFieldErrorMessage = "Debes ingresar el campo {0}.";
        public const string StringLengthFieldErrorMessage = "El campo {0} debe estar entre {2} y {1} caracteres.";
        public const string StringMaxFieldErrorMessage = "El campo {0} debe tener máximo {1} caracteres.";
        public const string ValueGreaterThanFieldErrorMessage = "El campo {0} debe ser igual o mayor a {1}.";
        public const string ValueLessFieldErrorMessage = "El campo desde: {0} debe ser menor que el hasta: {1}.";
        public const string ValueGreaterZeroFieldErrorMessage = "El campo {0} debe ser mayor a 0.";
        public const string PageTitleEdit = "Editar";
        public const string PageTitleDetail = "Detalle";
        public const string PageTitleCreate = "Crear";
        public const string PageTitleDelete = "Eliminar";
        public const string PageTitleIndex = "Lista";
        public const string DeleteQuestion = "¿Estás seguro que quieres eliminar esto?";
        public const string TableHeadActions = "Acciones";

        public const string ActionEdit = "Editar";
        public const string ActionInit = "Iniciar";
        public const string ActionEnd = "Terminar";
        public const string ActionPause = "Pausar";
        public const string ActionPrint = "Imprimir";
        public const string ActionNew = "Crear nuevo";
        public const string ActionCreate = "Crear";
        public const string ActionSave = "Guardar";
        public const string ActionDetail = "Detalle";
        public const string ActionDelete = "Anular";
        public const string ActionSearch = "Buscar";
        public const string ActionBackToIndex = "Volver a la lista";
        public const string ActionCancel = "Cancelar";
        public const string ActionAdd = "Agregar";
        public const string ActionSelect = "Seleccionar";
        public const string ActionCotizar = "Cotizar";
        public const string ActionRefresh = "Refrescar";
        public const string ActionAccept = "Aceptar";
        public const string ActionDeny = "Rechazar";
        public const string ActionBackToPending = "Volver a pendiente";

        public const string DateLessToday = "La fecha no puede ser menor a la fecha actual.";

        public const string MachineFailure_MaintenanceWorkOrderId = "No se pudieron actualizar los datos, porque la falla de maquina ya se encuentra asignada a una orden de mantenimiento";
        public const string MachineFailure_FinalDate_Lessthan = "La fecha final no puede ser menor que la fecha de inicio.";
        public const string MachineFailure_StopFinalDate_Lessthan = "La fecha final de parada de maquina no puede ser menor que la fecha de inicio de la parada de maquina.";

        public const string MachineFailure_StopStartDate_Lessthan_StartDate = "La fecha de inicio de parada no puede ser menor que la fecha de inicio.";
        public const string MachineFailure_StopStartDate_GreaterThan_FinalDate = "La fecha de inicio de parada no puede ser mayor que la fecha de fin.";

        public const string MachineFailure_StopFinalDate_Lessthan_StartDate = "La fecha de fin de parada no puede ser menor que la fecha de inicio.";
        public const string MachineFailure_StopFinalDate_GreaterThan_FinalDate = "La fecha de fin de parada no puede ser mayor que la fecha de fin.";

        public const string MachineFailure_StopStartDate = "Debe indicar la fecha y hora de inicio de la parada de máquina.";
        public const string MachineFailure_StopFinalDate = "Debe indicar la fecha y hora de fin de la parada de máquina.";

        public const string MaintenanceWorkOrder_FinalDate = "Debe indicar la fecha y hora de termino de la orden de trabajo de mantenimiento.";
        public const string MaintenanceWorkOrder_FinalDate_Lessthan = "La fecha de termino de la orden de trabajo de mantenimiento no puede ser menor que la fecha de inicio de la orden de trabajo de mantenimiento.";
        public const string MaintenanceWorkOrder_MaintenanceType = "Debe seleccionar las fallas de la orden de trabajo de mantenimiento.";
        public const string MaintenanceWorkOrder_Employees = "Debe seleccionar al menos un empleado para la orden de trabajo de mantenimiento.";
        public const string MaintenanceWorkOrder_Create_Subject = "WA-Alerta: Nueva OTM: {0}.";
        public const string MaintenanceProgram_Alert_Subject = "WA-Alerta: Programas de mantenimiento próximos a iniciar.";
        public const string Maintenance_LogAlert_Subject = "WA-Alerta: Log de Programa de mantenimiento: {0}.";

        public const string MaintenanceWorkOrder_Ancho = "El Ancho debe ser mayor a 0.";
        public const string MaintenanceWorkOrder_Alto = "El Alto debe ser mayor a 0.";
        public const string MaintenanceWorkOrder_Panol = "El Panol debe ser mayor a 0.";
        public const string MaintenanceWorkOrder_NroCopias = "El Nro. Copias debe ser mayor a 0.";

        public const string Driver_LicenseId = "Ya existe un conductor con el mismo nro. De licencia";

        public const string Vehicle_Placa = "Ya existe un vehículo con el mismo nro. De placa";

        public const string Flete_SerieDocumentAndNroDocument = "La factura: {0} ya fue ingresada en el flete: {1}.";
        public const string Flete_TotalFactura = "El total de la factura: {0} no es igual al total de las guías: {1}, la diferencia es: {2}";
        public const string Flete_Detalle = "Necesita seleccionar guías para el registro del flete.";
        public const string FleteDetalle_SerieDocumentAndNroDocument = "La guía: {0} ya fue ingresada en el flete: {1}.";

        public const string SaleOrder_DeliveryDate = "La fecha de entrega no puede ser menor a la fecha del sistema.";
        public const string SaleOrder_Products = "Necesita seleccionar productos para la orden de venta.";
        public const string SaleOrder_PaymentId = "Necesita seleccionar una condición de pago para la orden de venta.";
        public const string SaleOrder_ContactId = "Necesita seleccionar un contacto para la orden de venta.";
        public const string SaleOrder_CurrencyId = "Necesita seleccionar una moneda para la orden de venta.";
        public const string RateNotFound = "No hay tipo de cambio ingresado para la moneda seleccionada.";
        public const string SaleOrder_BpReferenceNumberIsNotUnique = "La O/C del Cliente ya existe en otra orden de compra.";
        public const string BusinessPartnerContactNotFound = "No se encontró el contacto.";
        public const string BusinessPartnerPaymentNotFound = "No se encontró la condición de pago.";
        public const string BusinessPartnerAddressNotFound = "No se encontró la dirección.";
        
        public const string CurrencyIdNotValid = "La moneda seleccionada no es válida para la orden de venta.";
        public const string BusinessPartnerNotFound = "No se encontró el cliente.";
        public const string SaleOrder_BusinessPartnerCreditLineError = "El cliente no tiene suficiente línea de crédito para crear el pedido.";

        public const string SaleQuotation_Products = "Necesita seleccionar productos para la cotización.";
        public const string SaleQuotation_CurrencyId = "Necesita seleccionar una moneda para la cotización.";
        public const string SaleQuotation_ContactId = "Necesita seleccionar un contacto para la cotización.";
        public const string SaleQuotation_PaymentId = "Necesita seleccionar una condición de pago para la cotización.";
        public const string SaleQuotation_DeliveryDate = "La fecha de entrega no puede ser menor a la fecha del sistema.";
        public const string SaleQuotation_End_Products = "No se puede terminar la cotización porque todos los artículos no tienen un precio definido.";
        public const string SaleQuotation_Accept_Products = "Necesitas seleccionar artículos aceptados para poder marcar la cotización como aceptada.";

        public const string Cotizacion_UpdatedBy = "Usted no puede actualizar la cotización porque usted no fue quien la creo.";

        public const string Cotizacion_End_Products = "El producto {0} aún no tiene un precio definido.";

        public const string Cotizacion_Create_Subject = "WA-Alerta: Nueva solicitud de cotización: {0}.";
        public const string Cotizacion_Create_Message = "El usuario: {0} creo una solicitud de cotización: {1} el: {2}.";

        public const string Cotizacion_End_Subject = "WA-Alerta: Cotización: {0} terminada.";
        public const string Cotizacion_End_Message = "El usuario: {0} termino la cotización: {1} el: {2}.";

        public const string Cotizacion_Accept_Subject = "WA-Alerta: Cotización: {0} aceptada por el cliente.";
        public const string Cotizacion_Accept_Message = "El cliente: {0} acepto la cotización: {1} el: {2}.";

        public const string Cotizacion_Deny_Subject = "WA-Alerta: Cotización: {0} rechazada por el cliente.";
        public const string Cotizacion_Deny_Message = "El cliente: {0} rechazo la cotización: {1} el: {2} por el siguiente motivo: {3}.";

        public const string Cotizacion_Pending_Subject = "WA-Alerta: Cotización: {0} pendiente.";
        public const string Cotizacion_Pending_Message = "El usuario: {0} puso en status pendiente la cotización: {1} el: {2}.";

        public const string SaleEmployeeNotFound = "No se encontró los datos del vendedor.";
        public const string SaleDataNotFound = "No se encontró los datos de ventas del vendedor.";

        public const string CRMActivityType = "No puede cambiar el tipo de actividad.";
        public const string CRMActivityStatus = "No se puede actualizar la actividad, por que ya esta cerrada.";
        public const string CRMActivity_EndDate = "Debe indicar la fecha y hora de termino de la actividad.";
        public const string CRMActivity_EndDate_Lessthan = "La fecha de termino de la actividad no puede ser menor que la fecha de inicio de la actividad.";
        public const string CRMActivity_AddressId = "Necesita seleccionar una dirección para la actividad.";
        public const string CRMActivity_CountryId = "Necesita seleccionar un país para la actividad.";
        public const string CRMActivity_StateId = "Necesita seleccionar una provincia para la actividad.";
        public const string CRMActivity_City = "Necesita ingresar una ciudad para la actividad.";
        public const string CRMActivity_Street = "Necesita ingresar una calle para la actividad.";
        public const string CRMActivity_Room = "Necesita ingresar una sala para la actividad.";

        public const string SaleOpportunityStatus = "No se puede actualizar / eliminar la oportunidad, por que ya no esta abierta.";
        public const string SaleOpportunityLostReasons = "No se puede actualizar / eliminar la oportunidad, por que no tiene ninguna razón de perdida ingresada.";

        public const string BusinessPartnerTempCreatedSubject = "WA-Alerta: Se ha creado un cliente temporal para cotizaciones: {0}";
        public const string BusinessPartnerTempCreatedBody = "El usuario: {0} ha creado el cliente temporal: {1} – {2} – {3} para cotizaciones, el {4}.";
        public const string BusinessPartnerTempExist = "El cliente ya existe y está asignado al vendedor: {0}.";
        public const string BusinessPartnerTempRuc = "Necesita ingresar un ruc.";
        public const string BusinessPartnerTempRucError = "El ruc ingresado no es correcto.";
        public const string BusinessPartnerTempName = "Necesita ingresar una razón social.";
        public const string BusinessPartnerTempSaleEmployee = "Necesita seleccionar un vendedor.";

        public const string QuantityGreaterZero = "La cantidad tiene que ser mayor a 0.";
        public const string QuantityGreaterMinQuantity = "La cantidad tiene que ser mayor o igual a la cantidad mínima: {0}";
        public const string QuantityNotEqualMultipleQuantity = "La cantidad tiene que ser multiplo de: {0}";
        public const string ProductMaxQuantityErrorMessage = "La cantidad máxima que se puede comprar de este producto es: {0}";

        public const string PurchaseOrderAuthorizationRejectReason = "Se necesita ingresar un motivo de rechazo.";

        public const string PurchaseOrderAuthorization_Response_Subject = "WA-Alerta: Respuesta de solicitud de autorización de orden de compra: {0}";
        public const string PurchaseOrderAuthorization_Approve_Message = "Su solicitud de autorización de la orden de compra: {0} fue: <strong>[APROBADA]</strong> por: {1} el: {2}.";
        public const string PurchaseOrderAuthorization_Reject_Message = "Su solicitud de autorización de la orden de compra: {0} fue: <strong>[RECHAZADA]</strong> por: {1} el: {2}. El motivo es: {3}.";

        public const string PurchaseOrderAuthorization_Override_Subject = "WA-Alerta: Respuesta de solicitud de desautorización de orden de compra: {0}";
        public const string PurchaseOrderAuthorization_Override_Message = "Su orden de compra: {0} fue: <strong>[DESAUTORIZADA]</strong> por: {1} el: {2}.";

        public const string SaleOrderAuthorizationRejectReason = "Se necesita ingresar un motivo de rechazo.";

        public const string SaleOrderAuthorization_Response_Subject = "WA-Alerta: Respuesta de solicitud de autorización de orden de venta: {0}";
        public const string SaleOrderAuthorization_Approve_Message = "Su solicitud de autorización de la orden de venta: {0} fue: <strong>[APROBADA]</strong> por: {1} el: {2}.";
        public const string SaleOrderAuthorization_Reject_Message = "Su solicitud de autorización de la orden de venta: {0} fue: <strong>[RECHAZADA]</strong> por: {1} el: {2}. El motivo es: {3}.";

        public const string SaleOrderAuthorization_Override_Subject = "WA-Alerta: Respuesta de solicitud de desautorización de orden de venta: {0}";
        public const string SaleOrderAuthorization_Override_Message = "Su orden de venta: {0} fue: <strong>[DESAUTORIZADA]</strong> por: {1} el: {2}.";

        public const string lcastillo = "lcastillo@grafipapel.com.pe";
        public const string jcolqui = "jcolqui@grafipapel.com.pe";
        public const string fvaler = "fvaler@grafipapel.com.pe";
        public const string jsalas = "jsalas@grafipapel.com.pe";
        public const string bhuamani = "bhuamani@grafipapel.com.pe";

        public const string Step0_SaleOrderReceivedSubject = "WA-Alerta: Tu pedido Nro.: {0} fue recibido.";
        public const string Step0_SaleOrderReceivedImageLink = "https://www.grafipapel.com.pe/mailing_files/step_0.png";
        public const string Step0_SaleOrderReceivedTitle = "Tu pedido Nro.: {0} fue recibido.";
        public const string Step0_SaleOrderReceivedText = "En breve un asesor comercial se comunicará contigo para confirmar el pedido.";

        public const string Step1_SaleOrderConfirmedSubject = "WA-Alerta: Tu pedido Nro.: {0} está confirmado.";
        public const string Step1_SaleOrderConfirmedImageLink = "https://www.grafipapel.com.pe/mailing_files/step_1.png";
        public const string Step1_SaleOrderConfirmedTitle = "Tu pedido Nro.: {0} está confirmado.";
        public const string Step1_SaleOrderConfirmedText = "Gracias por comprar con nosotros. Te avisaremos cuando tu pedido esté listo para ser enviado.";

        public const string Step2_DeliveryReadySubject = "WA-Alerta: Tu entrega Nro.: {0} está lista para ser enviada.";
        public const string Step2_DeliveryReadyImageLink = "https://www.grafipapel.com.pe/mailing_files/step_2.png";
        public const string Step2_DeliveryReadyTitle = "Tu entrega Nro.: {0} está lista para ser enviada.";
        public const string Step2_DeliveryReadyText = "Estamos coordinando el envío de tu entrega. Te avisaremos cuando tu entrega esté en camino.";

        public const string Step3_DeliveryDispatchedSubject = "WA-Alerta: Tu entrega Nro.: {0} está en camino.";
        public const string Step3_DeliveryDispatchedImageLink = "https://www.grafipapel.com.pe/mailing_files/step_3.png";
        public const string Step3_DeliveryDispatchedTitle = "Tu entrega Nro.: {0} está en camino.";
        public const string Step3_DeliveryDispatchedText = "En breve recibirás tu entrega. Te avisaremos cuando la hayamos entregado.";

        public const string Step4_DeliveryDeliveredSubject = "WA-Alerta: Tu entrega Nro.: {0} fue entregada.";
        public const string Step4_DeliveryDeliveredImageLink = "https://www.grafipapel.com.pe/mailing_files/step_4.png";
        public const string Step4_DeliveryDeliveredTitle = "Tu entrega Nro.: {0} fue entregada.";
        public const string Step4_DeliveryDeliveredText = "Gracias por comprar con nosotros.";
    }
}
