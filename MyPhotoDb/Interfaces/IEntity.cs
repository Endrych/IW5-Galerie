﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoDb.Interfaces
{
    interface IEntity<T>
    {
        [Key]
        Guid Id { get; set; }
    }
}
