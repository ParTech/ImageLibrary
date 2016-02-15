using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Logging;
using ParTech.ImageLibrary.Core.Interfaces;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.Repositories;
using ParTech.ImageLibrary.Core.Utils;
using Postal;
using Westwind.Globalization;

namespace ParTech.ImageLibrary.Core.Workers
{
    public interface IOrderWorker : IWorker
    {
        Invoice GenerateInvoiceForByer(int byerId, List<OrderLine> orderLines);

        bool SendInvoiceEmail(Invoice newInvoice, ControllerContext controllerContext);
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

        public Invoice GenerateInvoiceForByer(int byerId, List<OrderLine> orderLines)
        {
            Invoice newInvoice = null;

            try
            {
                var byerProfile = _userRepository.GetProfile(byerId);
                if (byerProfile != null)
                {
                    var invoiceToSave = new Invoice
                    {
                        InvoiceNumber = GenerateInvoiceNumber(),
                        ProfileID = byerProfile.ProfileID,
                        CompanyName = byerProfile.CompanyName,
                        SalutationID = byerProfile.SalutationID,
                        FirstName = byerProfile.FirstName,
                        LastName = byerProfile.LastName,
                        Address = byerProfile.Address,
                        PostalCode = byerProfile.PostalCode,
                        City = byerProfile.City,
                        CountryID = byerProfile.CountryID,
                        InvoiceTotal = orderLines.Sum(ol => ol.Price)
                    };

                    // save invoice to database and update the associated orderlines
                    invoiceToSave = _orderRepository.SaveInvoice(invoiceToSave);
                    if (invoiceToSave != null
                        && _orderRepository.AddInvoiceIdToOrderLines(invoiceToSave.InvoiceID, orderLines))
                    {
                        // reload invoice to retrieve all data of the created invoice
                        newInvoice = _orderRepository.GetInvoiceAndContext(invoiceToSave.InvoiceID);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("GenerateInvoiceForByer - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return newInvoice;
        }

        public bool SendInvoiceEmail(Invoice newInvoice, ControllerContext controllerContext)
        {
            var emailSent = false;

            try
            {
                dynamic email = new Email("ByerInvoice");
                email.To = newInvoice.Profile.Email;
                email.Subject = string.Format(DbRes.T("Emails.ByerInvoice.Subject", "Resources"), DateTime.Now.ToString("MMMM yyyy"));
                email.InvoiceNumber = @InvoiceUtilities.GetInvoiceNumberForDisplay(newInvoice.Date, newInvoice.InvoiceNumber);
                email.Date = newInvoice.Date.ToString("dd MMMM yyyy");
                email.CompanyName = newInvoice.CompanyName;
                email.FirstName= newInvoice.FirstName;
                email.LastName= newInvoice.LastName;
                email.Address= newInvoice.Address;
                email.PostalCode= newInvoice.PostalCode;
                email.City= newInvoice.City;
                email.InvoiceTotal= newInvoice.InvoiceTotal.ToString("C");

                email.Country = newInvoice.Country;
                email.Salutation = newInvoice.Salutation;
                email.OrderLines = @InvoiceUtilities.GetOrderLinesForDisplay(newInvoice.OrderLines);

                email.Send();

                emailSent = true;

                _orderRepository.UpdateInvoiceSent(newInvoice.InvoiceID, true);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SendInvoiceEmail - error [{0}] - - \r\n {1} \r\n\r\n", ex.Message, ex.StackTrace);
            }

            return emailSent;
        }

        private int GenerateInvoiceNumber()
        {
            var newInvoiceNumber = 1;

            var lastInvoice = _orderRepository.GetLastInvoice();
            if (lastInvoice != null)
            {
                newInvoiceNumber = lastInvoice.InvoiceNumber + 1;
            }

            return newInvoiceNumber;
        }
    }
}
