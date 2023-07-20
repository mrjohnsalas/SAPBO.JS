using SAPBO.JS.Common;
using SAPBO.JS.Model.Domain;
using SAPbobsCOM;

namespace SAPBO.JS.Data.Mappers
{
    public class BillFileMapper : ISapB1AutoMapper<BillFile>
    {
        public BillFile Mapper(IRecordset rs)
        {
            var file = new BillFile
            {
                Id = int.Parse(rs.Fields.Item("Line").Value.ToString()),

                BillId = int.Parse(rs.Fields.Item("AbsEntry").Value.ToString()),

                FilePath = rs.Fields.Item("trgtPath").Value.ToString(),
                FileName = rs.Fields.Item("FULLFILENAME").Value.ToString(),
                FileDate = Utilities.DateValueToDateOrNull(rs.Fields.Item("Date").Value).Value
            };

            var fileExtension = file.FileName.Substring(file.FileName.Length - 4, 4).ToUpper();
            switch (fileExtension)
            {
                case ".XML":
                    file.BillFileType = Enums.BillFileType.XML;
                    break;
                case ".ZIP":
                    file.BillFileType = Enums.BillFileType.Zip;
                    break;
                case ".PDF":
                default:
                    file.BillFileType = Enums.BillFileType.PDF;
                    break;
            }

            return file;
        }

        public IUserTable SetValuesToUserTable(IUserTable table, BillFile obj) => table;
    }
}
