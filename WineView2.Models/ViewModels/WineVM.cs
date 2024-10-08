﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineView2.Models.ViewModels
{
    public class WineVM
    {
        public Wine Wine { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ColorList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> WineryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> StyleList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GrapeList { get; set; }
        [ValidateNever]
        public double Sweetness { get; set; }
        [ValidateNever]
        public double Acidity { get; set; }
        [ValidateNever]
        public double Tannin { get; set; }
        [ValidateNever]
        public Body Body { get; set; }
    }
}
