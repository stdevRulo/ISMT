﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BizData.Entities
{
    public enum Estado
    {
        Creado,
        PendienteAprobacionJefeArea,
        AprobadoJefeArea,
        PendienteAprobacionDecano,
        AprobadoDecano,
        PendienteAprobacionRector,
        AprobadoRector,
        PendientePasaporte,
        AprobadoPasaporte,
        PendienteVisas,
        AprobadasVisas,
        PendienteRealizacion,
        Realizado,
        Cancelado,
        Pendiente
    }

    public class Itinerario
    { 
        public int ItinerarioID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public Estado Estado { get; set; }
        public int Update { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Viaje> Viajes { get; set; }
        public virtual ICollection<Historial> Historial { get; set; }

        [NotMapped]
        public IEnumerable<Pais> GetPaises
        {
            get
            {
                if (Viajes != null)
                    foreach (var viaje in Viajes)
                        yield return viaje.Pais;
            }
            set { }
        }

        [NotMapped]
        public IEnumerable<Institucion> GetInstituciones
        {
            get
            {
                if (Viajes != null)
                    foreach (var viaje in Viajes)
                        yield return viaje.Institucion;
            }
            set { }
        }
    }
}
