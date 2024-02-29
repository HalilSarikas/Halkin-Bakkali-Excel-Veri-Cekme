using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using System.IO;

string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
string downloadsFolder = Path.Combine(userProfile, "Downloads");


string excelUrl = "https://acikveri.bizizmir.com/dataset/e55d2b2d-1798-45df-befb-47d8e508f55b/resource/44855909-9060-4e8b-8de2-a878697760a2/download/halkin-bakkali-fiyat.xlsx";
 
string tempFilePath = Path.Combine(downloadsFolder, "_" + Guid.NewGuid().ToString()+".xlsx"); 

using (HttpClient client = new HttpClient())
{
    using (HttpResponseMessage response = await client.GetAsync(excelUrl))
    {
        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
        {
            using (FileStream fileStream = File.Create(tempFilePath))
            {
                await contentStream.CopyToAsync(fileStream);
                fileStream.Close();
            }
        }
    }
}

var rows = MiniExcel.Query<ExcelDataList>(tempFilePath);

foreach (var item in rows)
{
    Console.WriteLine("Ürün : "+item.Adi+" - 1 "+item.Birim + " - "+item.Fiyat+" - "+item.Kategori);
    Console.WriteLine("------------------------------------------------------------------------------");
}

Console.WriteLine("İşlem bitti");
Console.ReadKey();
public class ExcelDataList
{
    [ExcelColumnName("SİPARİŞ ÜRÜN GRUBU")]
    public string? Kategori { get; set; }
    [ExcelColumnName("ÜRÜN KODU")]
    public string? Kodu { get; set; }
    [ExcelColumnName("ÜRÜN TANIMI")]
    public string? Adi { get; set; }
    [ExcelColumnName("BİRİM")]
    public string? Birim { get; set; }
    [ExcelColumnName("FİYAT ")]
    public string? Fiyat { get; set; }//String vermemin nedeni excel tablosu içerisinde fiyat "#N/A" olarak değer olması
}