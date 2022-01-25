using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dictionary.Models
{
    // Models returned by QueryController actions.
    public class GetViewModel
    {
        public string Hometown { get; set; }
    }
}