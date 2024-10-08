﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineView2.Models.ViewModels
{
    public class ReviewVM
    {
        public Review Review { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BodyList { get; set; }
    }
}
