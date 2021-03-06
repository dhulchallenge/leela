﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    public static class Conventions
    {
        public const string Prefix = "carrentals";

        public static readonly string StorageConfigName = Prefix + "-storage";
        public static readonly string SmtpConfigName = Prefix + "-smtp";



        public static readonly string DefaultRouterQueue = Prefix + "-route-cmd";
        public static readonly string FunctionalEventRecorderQueue = Prefix + "-route-events";
        public static readonly string DefaultErrorsFolder = Prefix + "-errors";

        public static readonly string ViewsFolder = Prefix + "-view";
        public static readonly string DocsFolder = Prefix + "-doc";
    }
}
