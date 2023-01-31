using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Business
{
    public class SaleQuotationDetailBusiness : SapB1GenericRepository<SaleQuotationDetail>, ISaleQuotationDetailBusiness
    {
        private const string _tableName = TableNames.SaleQuotationDetail;
        private readonly IProductFormulaBusiness _productFormulaRepository;
        private readonly IProductMaterialTypeBusiness _productMaterialTypeRepository;
        private readonly IProductGrammageBusiness _productGrammageRepository;
        private readonly IProductInkLevelBusiness _productInkLevelRepository;
        private readonly IProductFormatBusiness _productFormatRepository;

        public SaleQuotationDetailBusiness(SapB1Context context, ISapB1AutoMapper<SaleQuotationDetail> mapper,
            IProductFormulaBusiness productFormulaRepository,
            IProductMaterialTypeBusiness productMaterialTypeRepository,
            IProductGrammageBusiness productGrammageRepository,
            IProductInkLevelBusiness productInkLevelRepository,
            IProductFormatBusiness productFormatRepository) : base(context, mapper)
        {
            _productFormulaRepository = productFormulaRepository;
            _productMaterialTypeRepository = productMaterialTypeRepository;
            _productGrammageRepository = productGrammageRepository;
            _productInkLevelRepository = productInkLevelRepository;
            _productFormatRepository = productFormatRepository;
        }

        public async Task<ICollection<SaleQuotationDetail>> GetAllAsync(int saleQuotationId, Enums.ObjectType objectType = Enums.ObjectType.FullHeader)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_210", new List<dynamic> { saleQuotationId }), objectType);
        }

        public async Task<ICollection<SaleQuotationDetail>> GetAllWithIdsAsync(IEnumerable<int> saleQuotationIds, Enums.ObjectType objectType = Enums.ObjectType.FullHeader)
        {
            return await SetFullProperties(await GetAllAsync("GP_WEB_APP_443", new List<dynamic> { string.Join(",", saleQuotationIds) }), objectType);
        }

        public async Task<SaleQuotationDetail> GetAsync(int id, Enums.ObjectType objectType = Enums.ObjectType.FullHeader)
        {
            return await SetFullProperties(await GetAsync("GP_WEB_APP_209", new List<dynamic> { id }), objectType);
        }

        public async Task CreateAsync(ICollection<SaleQuotationDetail> objs, int saleQuotationId)
        {
            if (objs != null && objs.Any() && saleQuotationId > 0)
            {
                var id = GetNewId();
                foreach (var obj in objs)
                {
                    obj.Id = id;
                    obj.SaleQuotationId = saleQuotationId;
                    obj.StatusId = (int)Enums.StatusType.Pendiente;
                    if (obj.NroCopias.Equals(0))
                        obj.NroCopias = 1;
                    await CreateAsync(_tableName, obj, obj.Id.ToString());
                    id++;
                }
            }
        }

        public async Task UpdateAsync(ICollection<SaleQuotationDetail> objs, int saleQuotationId)
        {
            if (objs == null || !objs.Any())
            {
                await DeleteBySaleQuotationIdAsync(saleQuotationId);
            }
            else
            {
                //Create
                var createObjs = objs.Where(x => x.Id.Equals(0));
                if (createObjs.Any())
                    await CreateAsync(createObjs.ToList(), saleQuotationId);

                var updateObjs = objs.Where(x => !x.Id.Equals(0));
                var currentObjs = await GetAllAsync(saleQuotationId, Enums.ObjectType.Only);

                //Delete
                var deleteObjs = currentObjs.Where(p => !updateObjs.Any(p2 => p2.Id == p.Id));
                if (deleteObjs.Any())
                    await DeleteAllWithIdsAsync(deleteObjs.Select(x => x.Id));

                //Update
                foreach (var obj in updateObjs)
                {
                    //Get obj
                    //var currentObj = await GetAsync(obj.Id);
                    var currentObj = currentObjs.FirstOrDefault(x => x.Id.Equals(obj.Id));
                    if (currentObj == null)
                        throw new Exception(AppMessages.NotFoundFromOperation);

                    //Check Status
                    if (currentObj.StatusType != Enums.StatusType.Pendiente)
                        throw new Exception(AppMessages.StatusError);

                    //Set obj
                    currentObj.ProductFormulaId = obj.ProductFormulaId;
                    currentObj.ProductMaterialTypeId = obj.ProductMaterialTypeId;
                    currentObj.ProductGrammageId = obj.ProductGrammageId;
                    currentObj.ProductInkLevelId = obj.ProductInkLevelId;
                    currentObj.ProductFormatId = obj.ProductFormatId;
                    currentObj.Quantity = obj.Quantity;
                    currentObj.Ancho = obj.Ancho;
                    currentObj.Alto = obj.Alto;
                    currentObj.Panol = obj.Panol;
                    currentObj.NroCopias = obj.NroCopias;
                    currentObj.TotalWeight = obj.TotalWeight;
                    currentObj.TotalPrice = obj.TotalPrice;
                    currentObj.Remark = obj.Remark;
                    currentObj.PriceTypeId = obj.PriceTypeId;
                    currentObj.ProductId = obj.ProductId;
                    currentObj.RejectReason = obj.RejectReason;

                    await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
                }
            }
        }

        public async Task UpdateMiniAsync(int id, int productInkLevelId, int productFormatId, decimal quantity, decimal ancho, decimal alto, decimal panol, int nroCopias, int priceTypeId)
        {
            //Get obj
            var currentObj = await GetAsync(id);
            if (currentObj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            //Set obj
            currentObj.ProductInkLevelId = productInkLevelId;
            currentObj.ProductFormatId = productFormatId;
            currentObj.Quantity = quantity;
            currentObj.Ancho = ancho;
            currentObj.Alto = alto;
            currentObj.Panol = panol;
            currentObj.NroCopias = nroCopias;
            currentObj.PriceTypeId = priceTypeId;

            await UpdateAsync(_tableName, currentObj, currentObj.Id.ToString());
        }

        public async Task UpdateAsync(ICollection<SaleQuotationDetail> objs)
        {
            if (objs != null && objs.Any())
            {
                foreach (var obj in objs)
                {
                    await UpdateAsync(_tableName, obj, obj.Id.ToString());
                }
            }
        }

        public Task UpdateStatusBySaleQuotationIdAsync(int saleQuotationId, Enums.StatusType statusType)
        {
            return DeleteAllAsync("GP_WEB_APP_484", new List<dynamic> { saleQuotationId, (int)statusType });
        }

        public async Task DeleteAsync(int id)
        {
            //Get obj
            var obj = await GetAsync(id);
            if (obj == null)
                throw new Exception(AppMessages.NotFoundFromOperation);

            await DeleteByIdAsync(_tableName, obj, obj.Id.ToString());
        }

        public Task DeleteAllWithIdsAsync(IEnumerable<int> ids)
        {
            return DeleteAllAsync("GP_WEB_APP_445", new List<dynamic> { string.Join(",", ids) });
        }

        public Task DeleteBySaleQuotationIdAsync(int saleQuotationId)
        {
            return DeleteAllAsync("GP_WEB_APP_444", new List<dynamic> { saleQuotationId });
        }

        private dynamic GetNewId()
        {
            var id = GetValue("GP_WEB_APP_208", "Id", null);
            if (id == null)
                throw new Exception(AppMessages.GenNewId);

            return id;
        }

        public async Task<SaleQuotationDetail> SetFullProperties(SaleQuotationDetail obj, Enums.ObjectType objectType)
        {
            if (obj == null) return null;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                obj.ProductFormula = await _productFormulaRepository.GetAsync(obj.ProductFormulaId);
                obj.ProductMaterialType = await _productMaterialTypeRepository.GetAsync(obj.ProductMaterialTypeId);
                obj.ProductGrammage = await _productGrammageRepository.GetAsync(obj.ProductGrammageId);
                obj.ProductInkLevel = await _productInkLevelRepository.GetAsync(obj.ProductInkLevelId);
                obj.ProductFormat = await _productFormatRepository.GetAsync(obj.ProductFormatId);

                if (objectType == Enums.ObjectType.Full)
                {

                }
            }

            return obj;
        }

        public async Task<ICollection<SaleQuotationDetail>> SetFullProperties(ICollection<SaleQuotationDetail> objs, Enums.ObjectType objectType)
        {
            if (objs == null || !objs.Any()) return objs;

            if (objectType == Enums.ObjectType.Full || objectType == Enums.ObjectType.FullHeader)
            {
                //ProductFormula
                var productFormulaIds = objs.GroupBy(x => x.ProductFormulaId).Select(g => g.Key);
                var productFormulas = await _productFormulaRepository.GetAllWithIdsAsync(productFormulaIds);

                foreach (var productFormula in productFormulas)
                    objs.Where(x => x.ProductFormulaId.Equals(productFormula.Id)).ToList().ForEach(x => x.ProductFormula = productFormula);

                //ProductMaterialType
                var productMaterialTypeIds = objs.GroupBy(x => x.ProductMaterialTypeId).Select(g => g.Key);
                var productMaterialTypes = await _productMaterialTypeRepository.GetAllWithIdsAsync(productMaterialTypeIds);

                foreach (var productMaterialType in productMaterialTypes)
                    objs.Where(x => x.ProductMaterialTypeId.Equals(productMaterialType.Id)).ToList().ForEach(x => x.ProductMaterialType = productMaterialType);

                //ProductGrammage
                var productGrammageIds = objs.GroupBy(x => x.ProductGrammageId).Select(g => g.Key);
                var productGrammages = await _productGrammageRepository.GetAllWithIdsAsync(productGrammageIds);

                foreach (var productGrammage in productGrammages)
                    objs.Where(x => x.ProductGrammageId.Equals(productGrammage.Id)).ToList().ForEach(x => x.ProductGrammage = productGrammage);

                //ProductInkLevel
                var productInkLevelIds = objs.GroupBy(x => x.ProductInkLevelId).Select(g => g.Key);
                var productInkLevels = await _productInkLevelRepository.GetAllWithIdsAsync(productInkLevelIds);

                foreach (var productInkLevel in productInkLevels)
                    objs.Where(x => x.ProductInkLevelId.Equals(productInkLevel.Id)).ToList().ForEach(x => x.ProductInkLevel = productInkLevel);

                //ProductFormat
                var productFormatIds = objs.GroupBy(x => x.ProductFormatId).Select(g => g.Key);
                var productFormats = await _productFormatRepository.GetAllWithIdsAsync(productFormatIds);

                foreach (var productFormat in productFormats)
                    objs.Where(x => x.ProductFormatId.Equals(productFormat.Id)).ToList().ForEach(x => x.ProductFormat = productFormat);

                if (objectType == Enums.ObjectType.Full)
                {

                }
            }

            return objs;
        }
    }
}
