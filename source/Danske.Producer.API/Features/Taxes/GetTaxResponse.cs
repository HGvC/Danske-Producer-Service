﻿using Danske.Producer.Enums;
using System;

namespace Danske.Producer.API.Features.Taxes
{
    public class GetTaxResponse
    {
        public string Municipality { get; set; }
        public PeriodType PeriodType { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal Result { get; set; }
    }
}