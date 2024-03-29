﻿using System;
using System.Collections.Generic;
using System.Globalization;
using ParTech.ImageLibrary.Core.Models;
using ParTech.ImageLibrary.Core.ViewModels.General;

namespace ParTech.ImageLibrary.Core.Utils
{
    public class InvoiceUtilities
    {
        public static string GetInvoiceNumberForDisplay(DateTime invoiceDate, int invoiceNumber)
        {
            var monthAsString = invoiceDate.Month.ToString(CultureInfo.InvariantCulture);
            if (invoiceDate.Month < 10)
            {
                monthAsString = string.Concat("0", monthAsString);
            }

            var invoiceNumberAsString = invoiceNumber.ToString(CultureInfo.InvariantCulture).PadLeft(6).Replace(' ', '0');

            return string.Format("{0}{1}/{2}", invoiceDate.Year, monthAsString, invoiceNumberAsString);
        }
        public static List<DisplayOrderLine> GetOrderLinesForDisplay(ICollection<OrderLine> orderLines)
        {
            List<DisplayOrderLine> displayOrderLines = new List<DisplayOrderLine>();

            foreach (OrderLine orderLine in orderLines)
            {
                displayOrderLines.Add(new DisplayOrderLine(orderLine));
            }

            return displayOrderLines;
        }
    }
}
