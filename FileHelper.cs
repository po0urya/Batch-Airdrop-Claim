using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BatchAirdropClaim
{
    public class WalletInfo
    {
        public int No { get; set; }
        public string Address { get; set; }
        public string PrivateKey { get; set; }
        public decimal Amount { get; set; }
    }

    public static class FileHelper
    {
        public static List<WalletInfo> ImportCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)))
            {
                return csv.GetRecords<WalletInfo>().ToList();
            }
        }

        public static List<WalletInfo> ImportExcel(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var dataTable = result.Tables[0];

                    var wallets = new List<WalletInfo>();

                    for (var i = 1; i < dataTable.Rows.Count; i++)
                    {
                        var row = dataTable.Rows[i];
                        wallets.Add(new WalletInfo
                        {
                            No = int.Parse(row[0].ToString()),
                            Address = row[1].ToString(),
                            PrivateKey = row[2].ToString(),
                            Amount = decimal.Parse(row[3].ToString())
                        });
                    }

                    return wallets;
                }
            }
        }
    }
}