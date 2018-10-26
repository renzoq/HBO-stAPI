﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Models.OptionModels
{
    /// <summary>
    /// Model that holds the information about an education level
    /// </summary>
    public class EducationLevel
    {
        // Deze informatie vind je in de options tabel met de naam cs_job_cus_fields. Het zit in een PHP serialized array.
        public string Id { get; set; }
        public string Name { get; set; }
    }
}