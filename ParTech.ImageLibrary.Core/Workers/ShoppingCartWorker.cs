using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Classes;
using ParTech.ImageLibrary.Core.Enums;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using WebMatrix.WebData;

namespace ParTech.ImageLibrary.Core.Workers
{
    public interface IShoppingCartWorker : IWorker
    {
        MessageIdEnum AddBrandImagesToShoppingCart(int brandid);

        MessageIdEnum AddCollectionImagesToShoppingCart(int collectionid);

        ShoppingCart AddImageToCart(int imageid);

        MessageIdEnum CreateDowload();

        MessageIdEnum DeleteCart();

        MessageIdEnum DeleteCartItem(int imageid);

        ShoppingCart GetCart();
    }

    public class ShoppingCartWorker : IShoppingCartWorker
    {
        private readonly IObjectRepository _objectRepository;

        private readonly IOrderRepository _orderRepository;

        private readonly IUserRepository _userRepository;

        public ILogger Logger { get; set; }

        public ShoppingCartWorker(IObjectRepository objectRepository, 
            IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _objectRepository = objectRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public MessageIdEnum AddBrandImagesToShoppingCart(int brandid)
        {
            var returnMessage = MessageIdEnum.AddImagesToCartFailure;

            if (HttpContext.Current.User != null)
            {
                var userId = WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name);
                var brand = _objectRepository.GetBrandAndContext(brandid);

                var images = new List<Image>();
                foreach (var product in brand.Products)
                {
                    if (product.Images.Any())
                    {
                        images = images.Union(product.Images.ToList()).ToList();
                    }
                }

                foreach (var image in images)
                {
                    var cartItem = new ShoppingCartItem
                    {
                        UserID = userId,
                        ImageID = image.ImageID
                    };

                    var cartItemExist = (_orderRepository.GetCartItem(userId, image.ImageID) != null);
                    if (!cartItemExist)
                    {
                        _orderRepository.SaveCartItem(cartItem);
                    }
                }

                returnMessage = MessageIdEnum.AddImagesToCartSuccess;
            }

            return returnMessage;
        }

        public MessageIdEnum AddCollectionImagesToShoppingCart(int collectionid)
        {
            var returnMessage = MessageIdEnum.AddImagesToCartFailure;

            if (HttpContext.Current.User != null)
            {
                var userId = WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name);
                var collection = _objectRepository.GetCollectionAndContext(collectionid);

                var images = new List<Image>();
                foreach (var product in collection.Products)
                {
                    if (product.Images.Any())
                    {
                        images = images.Union(product.Images.ToList()).ToList();
                    }
                }

                foreach (var image in images)
                {
                    var cartItem = new ShoppingCartItem
                    {
                        UserID = userId,
                        ImageID = image.ImageID
                    };

                    var cartItemExist = (_orderRepository.GetCartItem(userId, image.ImageID) != null);
                    if (!cartItemExist)
                    {
                        _orderRepository.SaveCartItem(cartItem);
                    }
                }

                returnMessage = MessageIdEnum.AddImagesToCartSuccess;
            }

            return returnMessage;
        }
        public ShoppingCart AddImageToCart(int imageid)
        {
            var cart = new ShoppingCart
            {
                Status = "success",
                AdditionalInformation = string.Empty
            };

            if (HttpContext.Current.User != null)
            {
                var userId = WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name);

                var cartItem = new ShoppingCartItem
                {
                    UserID = userId,
                    ImageID = imageid
                };

                var cartItemExist = (_orderRepository.GetCartItem(userId, imageid) != null);
                var saveCartItemSucceeded = false;
                if (!cartItemExist)
                {
                    saveCartItemSucceeded = _orderRepository.SaveCartItem(cartItem);
                }

                if (cartItemExist || saveCartItemSucceeded)
                {
                    cart = _orderRepository.GetCartForUser(userId);
                    if (cart.Count == 0)
                    {
                        cart.Status = "error";
                        cart.AdditionalInformation = "No cart items to display";
                    }
                }
                else
                {
                    cart.Status = "error";
                    cart.AdditionalInformation = "Cart item could not be saved";
                }
            }
            else
            {
                cart.Status = "error";
                cart.AdditionalInformation = "User not specified.";
            }

            return cart;
        }

        public MessageIdEnum CreateDowload()
        {
            var returnMessage = MessageIdEnum.CreateDownloadCartFailed;

            if (HttpContext.Current.User != null)
            {
                try
                {
                    var userProfile = _userRepository.GetUserProfileAndContextByName(HttpContext.Current.User.Identity.Name);
                    var cartItems = _orderRepository.GetCartItemsForUser(userProfile.Id);
                    if (cartItems.Any() && _orderRepository.CreateOrderLinesForCartItems(userProfile, cartItems))
                    {
                        var zipFileStream = _orderRepository.CreateZipFileForCartItems(cartItems);
                        if (zipFileStream.Length > 0 && _orderRepository.DeleteCart(userProfile.Id))
                        {
                            HttpContext.Current.Session["ZipFileStream"] = zipFileStream;
                            returnMessage = MessageIdEnum.CreateDownloadCartSuccess;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("CreateDowload - error [{0}] - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
                }
            }

            return returnMessage;
        }

        public MessageIdEnum DeleteCart()
        {
            var returnMessage = MessageIdEnum.DeleteCartFailure;
            
            if (HttpContext.Current.User != null)
            {
                if (_orderRepository.DeleteCart(WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name)))
                {
                    returnMessage = MessageIdEnum.DeleteCartSuccess;
                }
            }

            return returnMessage;
        }

        public MessageIdEnum DeleteCartItem(int imageid)
        {
            var returnMessage = MessageIdEnum.DeleteCartItemFailure;

            if (HttpContext.Current.User != null
                && imageid > 0)
            {
                if (_orderRepository.DeleteCartItem(WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name), imageid))
                {
                    returnMessage = MessageIdEnum.DeleteCartItemSuccess;
                }
            }

            return returnMessage;
        }

        public ShoppingCart GetCart()
        {
            var cart = new ShoppingCart
            {
                Status = "success",
                AdditionalInformation = string.Empty
            };

            if (HttpContext.Current.User != null)
            {
                var userId = WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name);

                cart = _orderRepository.GetCartForUser(userId);

                // in this case an empty cart is possible and NOT an error
                // so set the cart status accordingly and pass the cart item
                // back

                cart.Status = "success";
            }
            else
            {
                cart.Status = "error";
                cart.AdditionalInformation = "User not specified.";
            }

            return cart;
        }
    }
}
