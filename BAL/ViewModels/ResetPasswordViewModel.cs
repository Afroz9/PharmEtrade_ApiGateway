﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

      
        public string Token { get; set; }

        
        public string NewPassword { get; set; }
    }
}
