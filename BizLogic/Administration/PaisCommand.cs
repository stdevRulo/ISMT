﻿using BizData.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BizLogic.Administration
{
    public class PaisCommand : PaisViewModel
    {
        public Region Region { get; set; }

        public Pais ToPais()
        {
            return new Pais
            {
                PaisID = Id,
                Nombre = Name,
                Region = Region == null ? null : Region
            };
        }
    }
}
