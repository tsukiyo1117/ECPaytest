namespace ECPayTest.Models;

public class EcpayOrders
{
    public string MerchantTradeNo { get; set; }
    public string MemberID { get; set; }
    public Nullable<int> RtnCode { get; set; }
    public string RtnMsg { get; set; }
    public string TradeNo { get; set; }
    public Nullable<int> TradeAmt { get; set; }
    public Nullable<System.DateTime> PaymentDate { get; set; }
    public string PaymentType { get; set; }
    public string PaymentTypeChargeFee { get; set; }
    public string TradeDate { get; set; }
    public Nullable<int> SimulatePaid { get; set; }
}