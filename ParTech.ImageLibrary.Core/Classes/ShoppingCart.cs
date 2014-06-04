using System.Collections.Generic;

namespace ParTech.ImageLibrary.Core.Classes
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            CartImages = new List<ShoppingCartImage>();
        }

        public string AdditionalInformation { get; set; }
        
        public int Count { get; set; }

        public string Status { get; set; }

        public List<ShoppingCartImage> CartImages { get; set; }
    }
}
