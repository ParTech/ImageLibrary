using ParTech.ImageLibrary.Core.Models;
using System;

namespace ParTech.ImageLibrary.Core.ViewModels.General
{
    public class DisplayOrderLine
    {
        public int OrderlineID { get; set; }
        public int ImageID { get; set; }
        public int BuyerID { get; set; }
        public int SellerID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string created { get; set; }
        public string updated { get; set; }
        public string Price { get; set; }
        public Nullable<int> InvoiceID { get; set; }
        public int UserID { get; set; }

        //public Image Image { get; set; }
        //public Invoice Invoice { get; set; }
        //public Product Product { get; set; }
        //public UserProfile UserProfile { get; set; }

        public DisplayOrderLine(OrderLine orderLine)
        {
            OrderlineID = orderLine.OrderlineID;
            ImageID = orderLine.ImageID;
            BuyerID = orderLine.BuyerID;
            SellerID = orderLine.SellerID;
            ProductID = orderLine.ProductID;
            ProductName = orderLine.ProductName;
            created = orderLine.created.ToString("g");
            updated = orderLine.updated.ToString("g");
            Price = orderLine.Price.ToString("C");
            InvoiceID = orderLine.InvoiceID;
        }
    }
}
