using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ECPayTest.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECPayTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
        //需填入你的網址
        var website = $"http://localhost:5154";
        var order = new Dictionary<string, string>
        {
            //綠界需要的參數
            { "MerchantTradeNo", orderId },
            { "MerchantTradeDate", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") },
            { "TotalAmount", "100" },
            { "TradeDesc", "無" },
            { "ItemName", "測試商品" },
            { "ExpireDate", "3" },
            { "CustomField1", "1" },
            { "CustomField2", "" },
            { "CustomField3", "" },
            { "CustomField4", "" },
            { "ReturnURL", $"{website}/api/AddPayInfo" },
            { "OrderResultURL", $"{website}/Home/PayInfo/{orderId}" },
            { "PaymentInfoURL", $"{website}/api/AddAccountInfo" },
            { "ClientRedirectURL", $"https://localhost:7036/" },
            //{ "ClientRedirectURL", $"{website}/Home/AccountInfo/{orderId}" },
            { "MerchantID", "2000132" },
            { "IgnorePayment", "GooglePay#WebATM#CVS#BARCODE" },
            { "PaymentType", "aio" },
            { "ChoosePayment", "ALL" },
            { "EncryptType", "1" },
        };
        //檢查碼
        order["CheckMacValue"] = GetCheckMacValue(order);
        return View(order);
    }
    [HttpPost]
    public IActionResult PayInfo(IFormCollection id)
    {
        var data = new Dictionary<string, string>();
        foreach (string key in id.Keys) {
            data.Add(key, id[key]);
        }
        // Database1Entities db = new Database1Entities();
        // var Orders = db.EcpayOrders.ToList().Where(m => m.MerchantTradeNo == id["MerchantTradeNo"]).FirstOrDefault();
        // Orders.RtnCode = int.Parse(id["RtnCode"]);
        // if (id["RtnMsg"] == "Succeeded") Orders.RtnMsg = "已付款";
        // Orders.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
        // Orders.SimulatePaid = int.Parse(id["SimulatePaid"]);
        // db.SaveChanges();
        
        //return View("EcpayView",data);
        return Json("data");
    }
    // [HttpPost]
    // public IActionResult AccountInfo(IFormCollection id)
    // {
    //     var data = new Dictionary<string, string>();
    //     foreach (string key in id.Keys)
    //     {
    //         data.Add(key, id[key]);
    //     }
    //     //Database1Entities db = new Database1Entities();
    //     // var Orders = db.EcpayOrders.ToList().Where(m => m.MerchantTradeNo == id["MerchantTradeNo"]).FirstOrDefault();
    //     //Orders.RtnCode = int.Parse(id["RtnCode"]);
    //     //if (id["RtnMsg"] == "Succeeded") Orders.RtnMsg = "已付款";
    //     //Orders.PaymentDate = Convert.ToDateTime(id["PaymentDate"]);
    //     //Orders.SimulatePaid = int.Parse(id["SimulatePaid"]);
    //     //db.SaveChanges();
    //     return View("EcpayView", data);
    // }

    private string GetCheckMacValue(Dictionary<string, string> order)
    {
        var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();
        var checkValue = string.Join("&", param);
        //測試用的 HashKey
        var hashKey = "5294y06JbISpM5x9";
        //測試用的 HashIV
        var HashIV = "v77hoKGq4kWxNNIS";
        checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";
        checkValue = HttpUtility.UrlEncode(checkValue).ToLower();
        checkValue = GetSHA256(checkValue);
        return checkValue.ToUpper();
    }

    private string GetSHA256(string value)
    {
        var result = new StringBuilder();
        var sha256 = SHA256.Create();
        var bts = Encoding.UTF8.GetBytes(value);
        var hash = sha256.ComputeHash(bts);
        for (int i = 0; i < hash.Length; i++)
        {
            result.Append(hash[i].ToString("X2"));
        }

        return result.ToString();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}