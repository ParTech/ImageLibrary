using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Castle.Core.Logging;
using Ionic.Zip;
using ParTech.ImageLibrary.Core.Classes;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;

namespace ParTech.ImageLibrary.Core.Repositories
{
    public interface IOrderRepository: IRepository
    {
        #region Invoices

        Invoice GetInvoice(int invoiceId);

        Invoice GetLastInvoice();

        bool SaveInvoice(Invoice invoice);

        #endregion

        #region OrderLine

        bool CreateOrderLinesForCartItems(UserProfile byerUserProfile, List<ShoppingCartItem> cartItems);

        OrderLine GetOrderLine(int? orderLineid);

        IEnumerable<OrderLine> GetOrderLines();

        IEnumerable<OrderLine> GetOrderLinesForByer(int? byerId);

        IEnumerable<OrderLine> GetOrderLinesForByerInvoices(DateTime date);

        IEnumerable<OrderLine> GetOrderLinesForSeller(int? sellerId);

        bool SaveOrderLine(OrderLine orderLine);

        #endregion

        #region ShoppingCartItem

        MemoryStream CreateZipFileForCartItems(List<ShoppingCartItem> cartItems);

        bool DeleteCart(int userid);

        bool DeleteCartItem(int userid, int imageid);

        ShoppingCart GetCartForUser(int userId);

        ShoppingCartItem GetCartItem(int userid, int imageid);

        List<ShoppingCartItem> GetCartItemsForUser(int userid);

        bool SaveCartItem(ShoppingCartItem cartItem);

        #endregion

    }

    public class OrderRepository : IOrderRepository
    {
        public ILogger Logger { get; set; }

        private readonly IUserRepository _userRepository;

        public OrderRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region Invoice

        public Invoice GetInvoice(int invoiceId)
        {
            Invoice invoice = null;

            try
            {
                if (invoiceId != null)
                {
                    using (var db = new Entities())
                    {
                        invoice = db.Invoices.SingleOrDefault(i => i.InvoiceID == invoiceId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetInvoice - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return invoice;
        }

        public Invoice GetLastInvoice()
        {
            Invoice invoice = null;

            try
            {
                using (var db = new Entities())
                {
                    invoice = db.Invoices.Where(i => i.Date.Year == DateTime.Now.Year)
                                         .OrderByDescending(i => i.InvoiceNumber)
                                         .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetInvoice - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return invoice;
        }
        public bool SaveInvoice(Invoice invoice)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (invoice.InvoiceID > 0)
                    {
                        var tmpInvoice = db.Invoices.SingleOrDefault(i => i.InvoiceID == invoice.InvoiceID);
                        if (tmpInvoice != null)
                        {
                            tmpInvoice.InvoiceID = invoice.InvoiceID;
                            tmpInvoice.InvoiceNumber = invoice.InvoiceNumber;
                            tmpInvoice.Date = invoice.Date;
                            tmpInvoice.ProfileID = invoice.ProfileID;
                            tmpInvoice.InvoiceTotal = invoice.InvoiceTotal;
                        }
                    }
                    else
                    {
                        invoice.Date = DateTime.Now;

                        db.Invoices.Add(invoice);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveInvoice - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region OrderLine

        public bool CreateOrderLinesForCartItems(UserProfile byerUserProfile, List<ShoppingCartItem> cartItems)
        {
            var saveSucceeded = false;

            if (byerUserProfile != null && byerUserProfile.ProfileID != null)
            {
                try
                {
                    using (var db = new Entities())
                    {
                        foreach (var cartItem in cartItems)
                        {
                            var ownerProfile = _userRepository.GetUserProfileById(cartItem.Image.Product.UserID);
                            if (ownerProfile.ProfileID != null)
                            {
                                var orderLine = new OrderLine
                                {
                                    BuyerID = (int)byerUserProfile.ProfileID,
                                    UserID = byerUserProfile.Id,
                                    SellerID = (int)ownerProfile.ProfileID,
                                    ImageID = cartItem.ImageID,
                                    ProductID = cartItem.Image.ProductID,
                                    ProductName = cartItem.Image.Product.Name,
                                    updated = DateTime.Now,
                                    created = DateTime.Now
                                };

                                db.OrderLines.Add(orderLine);
                            }
                        }

                        db.SaveChanges();
                    }

                    saveSucceeded = true;
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("CreateOrderLinesForCartItems - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
                }
            }

            return saveSucceeded;
        }

        public OrderLine GetOrderLine(int? orderLineid)
        {
            OrderLine orderLine = null;

            try
            {
                if (orderLineid != null)
                {
                    using (var db = new Entities())
                    {
                        orderLine = db.OrderLines.SingleOrDefault(i => i.OrderlineID == orderLineid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetOrderLine - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return orderLine;
        }

        public IEnumerable<OrderLine> GetOrderLines()
        {
            var orderLines = new List<OrderLine>();

            try
            {
                using (var db = new Entities())
                {
                    orderLines = db.OrderLines.OrderBy(i => i.created)
                                              .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetOrderLines - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return orderLines;
        }

        public IEnumerable<OrderLine> GetOrderLinesForByer(int? byerId)
        {
            var orderLines = new List<OrderLine>();

            if (byerId != null)
            {
                try
                {
                    using (var db = new Entities())
                    {
                        orderLines = db.OrderLines.Where(i => i.BuyerID == byerId)
                                                  .Include("Image")
                                                  .Include("Invoice")
                                                  .Include("Product")
                                                  .OrderByDescending(i => i.created)
                                                  .ToList();
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("GetOrderLinesForByer - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
                }
            }

            return orderLines;
        }

        public IEnumerable<OrderLine> GetOrderLinesForByerInvoices(DateTime date)
        {
            var orderLines = new List<OrderLine>();

            try
            {
                using (var db = new Entities())
                {
                    orderLines = db.OrderLines.Where(i => i.InvoiceID == null
                                                            && i.created.Year == date.Year
                                                            && i.created.Month == date.Month)
                                                .Include("Image")
                                                .Include("Invoice")
                                                .Include("Product")
                                                .OrderByDescending(i => i.created)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetOrderLinesForByerInvoice - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return orderLines;
        }

        public IEnumerable<OrderLine> GetOrderLinesForSeller(int? sellerId)
        {
            var orderLines = new List<OrderLine>();

            if (sellerId != null)
            {
                try
                {
                    using (var db = new Entities())
                    {
                        orderLines = db.OrderLines.Where(i => i.SellerID == sellerId)
                                                  .Include("Image")
                                                  .Include("Invoice")
                                                  .Include("Product")
                                                  .OrderByDescending(i => i.created)
                                                  .ToList();
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("GetOrderLinesForSeller - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
                }
            }

            return orderLines;
        }

        public bool SaveOrderLine(OrderLine orderLine)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (orderLine.OrderlineID > 0)
                    {
                        var tmpOrderLine = db.OrderLines.SingleOrDefault(i => i.OrderlineID == orderLine.OrderlineID);
                        if (tmpOrderLine != null)
                        {
                            tmpOrderLine.BuyerID = orderLine.BuyerID;
                            tmpOrderLine.SellerID = orderLine.SellerID;
                            tmpOrderLine.ImageID = orderLine.ImageID;
                            tmpOrderLine.InvoiceID = orderLine.InvoiceID;
                            tmpOrderLine.ProductID = orderLine.ProductID;
                            tmpOrderLine.ProductName = orderLine.ProductName;
                            tmpOrderLine.Price = orderLine.Price;
                            tmpOrderLine.updated = DateTime.Now;
                        }
                    }
                    else
                    {
                        orderLine.updated = DateTime.Now;
                        orderLine.created = DateTime.Now;

                        db.OrderLines.Add(orderLine);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveOrderLine - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

        #region ShoppingCartItem

        public MemoryStream CreateZipFileForCartItems(List<ShoppingCartItem> cartItems)
        {
            var outputStream = new MemoryStream();

            using (var zip = new ZipFile())
            {
                foreach (var cartItem in cartItems)
                {
                    zip.AddFile(cartItem.Image.Filepath, string.Empty);
                }

                zip.Save(outputStream);
            }

            // ensure that pointer of the output stream is set to
            // the beginning of the stream
            outputStream.Position = 0;

            return outputStream;
        }

        public bool DeleteCart(int userid)
        {
            var cartDeleted = false;

            try
            {
                using (var db = new Entities())
                {
                    var items = db.ShoppingCartItems.Where(i => i.UserID == userid)
                                                    .ToList();

                    db.ShoppingCartItems.RemoveRange(items);

                    db.SaveChanges();
                }

                cartDeleted = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("DeleteCart - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return cartDeleted;
        }

        public bool DeleteCartItem(int userid, int imageid)
        {
            var cartItemDeleted = false;

            try
            {
                using (var db = new Entities())
                {
                    var items = db.ShoppingCartItems.Where(i => i.UserID == userid
                                                                && i.ImageID == imageid)
                                                    .ToList();

                    db.ShoppingCartItems.RemoveRange(items);

                    db.SaveChanges();
                }

                cartItemDeleted = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("DeleteCartItem - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return cartItemDeleted;
        }

        public ShoppingCart GetCartForUser(int userId)
        {
            var cart = new ShoppingCart
            {
                Status = "error",
                Count = 0
            };

            var cartItems = GetCartItemsForUser(userId);
            if (cartItems.Any())
            {
                cart.Status = "success";
                cart.Count = cartItems.Count;

                foreach (var cItem in cartItems)
                {
                    var cartImage = new ShoppingCartImage
                    {
                        CartId = cItem.CartItemID,
                        ImageId = cItem.ImageID,
                        ProductName = cItem.Image.Product.Name,
                        Resolution = cItem.Image.Resolution,
                        Type = cItem.Image.ImageFormat.Replace("image/", string.Empty)
                    };

                    cart.CartImages.Add(cartImage);
                }
            }

            return cart;
        }

        public ShoppingCartItem GetCartItem(int userid, int imageid)
        {
            ShoppingCartItem cartItem = null;

            try
            {
                using (var db = new Entities())
                {
                    cartItem = db.ShoppingCartItems.SingleOrDefault(i => i.UserID == userid 
                                                                         && i.ImageID == imageid);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCartItem - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return cartItem;
        }

        public List<ShoppingCartItem> GetCartItemsForUser(int userid)
        {
            var cartItems = new List<ShoppingCartItem>();

            try
            {
                using (var db = new Entities())
                {
                    cartItems = db.ShoppingCartItems.Where(i => i.UserID == userid)
                                                    .Include("Image")
                                                    .Include("Image.Product")
                                                    .ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GetCartItemsForUser - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return cartItems;
        }

        public bool SaveCartItem(ShoppingCartItem cartItem)
        {
            var saveSucceeded = false;

            try
            {
                using (var db = new Entities())
                {
                    if (cartItem.CartItemID > 0)
                    {
                        var tmpCartItem = db.ShoppingCartItems.SingleOrDefault(i => i.CartItemID == cartItem.CartItemID);
                        if (tmpCartItem != null)
                        {
                            tmpCartItem.ImageID = cartItem.ImageID;
                            tmpCartItem.UserID = cartItem.UserID;
                        }
                    }
                    else
                    {
                        cartItem.created = DateTime.Now;

                        db.ShoppingCartItems.Add(cartItem);
                    }

                    db.SaveChanges();
                }

                saveSucceeded = true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SaveCartItem - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return saveSucceeded;
        }

        #endregion

    }
}
