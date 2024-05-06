﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Controllers
{
    public interface InterfacePaymentFormController
    {
        Task SendPaymentConfirmationMailAsync();
        Product GetProduct();
    }
}
