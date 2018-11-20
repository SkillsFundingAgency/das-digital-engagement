﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs.Description;

namespace SFA.DAS.Campaign.Functions.Framework.Attributes
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
    }
}
