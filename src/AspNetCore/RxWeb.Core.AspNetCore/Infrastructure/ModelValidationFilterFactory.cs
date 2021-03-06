﻿using Microsoft.AspNetCore.Mvc.Filters;
using RxWeb.Core.Annotations.Filters;
using System;

namespace RxWeb.Core.AspNetCore
{
    class ModelValidationFilterFactory : IFilterFactory, IOrderedFilter
    {
        public int Order => ModelValidationFilter.ValidationFilterOrder;

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ModelValidationFilter();
        }
    }
}
