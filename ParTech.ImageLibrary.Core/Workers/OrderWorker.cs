using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;

namespace ParTech.ImageLibrary.Core.Workers
{
    public interface IOrderWorker : IWorker
    {
        bool GenerateInvoiceForByer(int byerId, List<OrderLine> orderLines);
    }

    public class OrderWorker : IOrderWorker
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IUserRepository _userRepository;

        public ILogger Logger { get; set; }

        public OrderWorker(IOrderRepository orderRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public bool GenerateInvoiceForByer(int byerId, List<OrderLine> orderLines)
        {
            var generationSucceeded = false;

            try
            {
                var byerProfile = _userRepository.GetProfile(byerId);
                if (byerProfile != null)
                {
                    var newInvoice = new Invoice
                    {
                        InvoiceNumber = GenerateInvoiceNumber(),
                        Date = DateTime.Now,
                        ProfileID = byerProfile.ProfileID,
                        SalutationID = byerProfile.SalutationID,
                        FirstName = byerProfile.FirstName,
                        LastName = byerProfile.LastName,
                        Address = byerProfile.Address,
                        PostalCode = byerProfile.PostalCode,
                        City = byerProfile.City,
                        CountryID = byerProfile.CountryID,
                        InvoiceTotal = orderLines.Sum(ol => ol.Price)
                    };

                    generationSucceeded = _orderRepository.SaveInvoice(newInvoice);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GenerateInvoiceForByer - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return generationSucceeded;
        }

        private int GenerateInvoiceNumber()
        {
            var newInvoiceNumber = 0;

            var lastInvoice = _orderRepository.GetLastInvoice();
            if (lastInvoice != null)
            {
                newInvoiceNumber = lastInvoice.InvoiceNumber + 1;
            }

            return newInvoiceNumber;
        }
    }
}
