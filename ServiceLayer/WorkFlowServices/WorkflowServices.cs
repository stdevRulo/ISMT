﻿using BizData.Entities;
using BizDbAccess.Authentication;
using BizDbAccess.GenericInterfaces;
using BizDbAccess.Repositories;
using BizDbAccess.Utils;
using BizLogic.Workflow;
using BizLogic.Workflow.Concrete;
using BizLogic.WorkflowManager;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.BizRunners;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.WorkFlowServices
{
    public class WorkflowServices
    {
        private readonly IUnitOfWork _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly GetterUtils _getterUtils;

        private readonly RunnerWriteDb<ItinerarioCommand, Itinerario> _runnerItinerario;
        private readonly RunnerWriteDb<ViajeCommand, Viaje> _runnerViaje;
        private readonly RunnerWriteDb<ViajeInvitado, ViajeInvitado> _runnerViajeInvitado;

        private readonly ItinerarioDbAccess _itinerarioDbAccess;
        private readonly ViajeDbAccess _viajeDbAccess;
        private readonly PaisDbAccess _paisDbAccess;
        private readonly InstitucionDbAccess _institucionDbAccess;
        private readonly UserDbAccess _userDbAccess;
        private readonly VisaDbAccess _visaDbAccess;
        private readonly ViajeInvitadoDbAccess _viajeInvitadoDbAccess;
        private readonly HistorialDbAccess _historialDbAccess;

        private readonly WorkflowManagerLocal _workflowManagerLocal;
        private readonly WorkflowManagerGuest _workflowManagerGuest;

        public WorkflowServices(IUnitOfWork context, UserManager<Usuario> userManager, GetterUtils getterUtils, SignInManager<Usuario> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _getterUtils = getterUtils;

            _runnerItinerario = new RunnerWriteDb<ItinerarioCommand, Itinerario>(
                new RegisterItinerarioAction(new ItinerarioDbAccess(_context)), _context);
            _runnerViaje = new RunnerWriteDb<ViajeCommand, Viaje>(
                new RegisterViajeAction(new ViajeDbAccess(_context)), _context);
            _runnerViajeInvitado = new RunnerWriteDb<ViajeInvitado, ViajeInvitado>(
                new RegisterViajeInvitadoAction(new ViajeInvitadoDbAccess(_context)), _context);

            _itinerarioDbAccess = new ItinerarioDbAccess(_context);
            _viajeDbAccess = new ViajeDbAccess(_context);
            _paisDbAccess = new PaisDbAccess(_context);
            _institucionDbAccess = new InstitucionDbAccess(_context);
            _userDbAccess = new UserDbAccess(_context, signInManager, userManager);
            _visaDbAccess = new VisaDbAccess(context);
            _viajeInvitadoDbAccess = new ViajeInvitadoDbAccess(context);
            _historialDbAccess = new HistorialDbAccess(context);

            _workflowManagerLocal = new WorkflowManagerLocal(context);
            _workflowManagerGuest = new WorkflowManagerGuest(context);
        }

        public IEnumerable<Historial> GetHistorial()
        {
            return _historialDbAccess.GetAll();
        }

        public int RegisterItinerarioAsync(ItinerarioCommand cmd)
        {
            var iters = _userDbAccess.GetAllItinerarios();
            cmd.Usuario = _userDbAccess.GetUsuario(cmd.UsuarioID); //await _userManager.FindByIdAsync(cmd.UsuarioID);

            var itinerario = _runnerItinerario.RunAction(cmd);
            return itinerario.ItinerarioID;
        }

        public void CreateItinerarioWorkflow(int itinerarioId, string claimTipoInstitucion)
        {
            var itinerario = _itinerarioDbAccess.GetItinerario(itinerarioId);
            _workflowManagerLocal.CrearViaje(itinerario, claimTipoInstitucion);
        }

        public Itinerario UpdateItinerario(Itinerario entity, Itinerario toUpd)
        {
            var itinerario = _itinerarioDbAccess.Update(entity, toUpd);
            _context.Commit();
            return itinerario;
        }

        public void Removeitinerario(Itinerario entity)
        {
            _itinerarioDbAccess.Delete(entity);
            _context.Commit();
        }

        public Itinerario GetItinerario(int id)
        {
            return _itinerarioDbAccess.GetItinerario(id);
        }

        public void CalculateDates(Itinerario iter)
        {
            var initials = iter.Viajes.Select(v => v.FechaInicio).ToList();
            initials.Sort();
            iter.FechaInicio = initials.First();

            var finals = iter.Viajes.Select(v => v.FechaFin).ToList();
            finals.Sort();
            iter.FechaFin = finals.First();

            _context.Commit();
        }

        public long RegisterViajeAsync(ViajeCommand cmd)
        {
            //cmd.Institucion = _institucionDbAccess.GetInstitucion(cmd.InstitucionName);
            cmd.Pais = _paisDbAccess.GetPais(cmd.PaisName);

            try
            {
                cmd.Itinerario = _userDbAccess.GetItinerario(cmd.UsuarioId, cmd.ItinerarioID);
            }
            catch (InvalidOperationException)
            {
                return -1;
            }

            var viaje = _runnerViaje.RunAction(cmd);

            return viaje.ViajeID;
        }

        public IEnumerable<Itinerario> GetItinerarioNotFinished(Usuario usuario)
        {
            return _userDbAccess.GetItinerariosNotFinished(usuario);
        }

        public IEnumerable<Itinerario> GetItinerarioDone(Usuario usuario)
        {
            return _userDbAccess.GetItinerariosDone(usuario);
        }

        public IEnumerable<Itinerario> GetItinerarioCanceled(Usuario usuario)
        {
            return _userDbAccess.GetItinerariosCanceled(usuario);
        }

        public IEnumerable<Itinerario> GetItinerariosEstado(Estado estado, Usuario user)
        {
            return _itinerarioDbAccess.GetItinerariosEstado(estado, user);
        }

        public void ManageActionAprobar(int itinerarioId, string usuarioId, string comentario)
        {
            var itinerario = _itinerarioDbAccess.GetItinerario(itinerarioId);
            var usuario = _userDbAccess.GetUsuario(usuarioId);

            if(itinerario.Estado == Estado.PendienteAprobacionJefeArea)
            {
                _workflowManagerLocal.ManageActionJefeArea(itinerario, BizLogic.WorkflowManager.Action.Aprobar, usuario, comentario);
                return;
            }

            if (itinerario.Estado == Estado.PendienteAprobacionDecano)
            {
                _workflowManagerLocal.ManageActionDecano(itinerario, BizLogic.WorkflowManager.Action.Aprobar, usuario, comentario);
                return;
            }

            if (itinerario.Estado == Estado.PendienteAprobacionRector)
            {
                _workflowManagerLocal.ManageActionRector(itinerario, BizLogic.WorkflowManager.Action.Aprobar, usuario, comentario);
                return;
            }
        }
        
        public void ManageActionRechazar(int itinerarioId, string usuarioId, string comentario)
        {
            var itinerario = _itinerarioDbAccess.GetItinerario(itinerarioId);
            var usuario = _userDbAccess.GetUsuario(usuarioId);

            if (itinerario.Estado == Estado.PendienteAprobacionJefeArea)
            {
                _workflowManagerLocal.ManageActionJefeArea(itinerario, BizLogic.WorkflowManager.Action.Rechazar, usuario, comentario);
                return;
            }

            if (itinerario.Estado == Estado.PendienteAprobacionDecano)
            {
                _workflowManagerLocal.ManageActionDecano(itinerario, BizLogic.WorkflowManager.Action.Rechazar, usuario, comentario);
                return;
            }

            if (itinerario.Estado == Estado.PendienteAprobacionRector)
            {
                _workflowManagerLocal.ManageActionRector(itinerario, BizLogic.WorkflowManager.Action.Rechazar, usuario, comentario);
                return;
            }
        }
        
        public void RealizarItinerario(int itinerarioId)
        {
            var itinerario = _itinerarioDbAccess.GetItinerario(itinerarioId);
            _workflowManagerLocal.RealizarItinerario(itinerario);
        }

        public void CancelItinerario(int itinerarioId, string usuarioId, string comentario)
        {
            var trip = _itinerarioDbAccess.GetItinerario(itinerarioId);
            var usuario = _userDbAccess.GetUsuario(usuarioId);
            _workflowManagerLocal.CancelarItinerario(trip, usuario, comentario);
        }

        public void SetPassportToUser(string usuarioID)
        {
            var usuario = _userDbAccess.GetUsuario(usuarioID);
            
            usuario.HasPassport = true;
            //await _userManager.UpdateAsync(usuario);
            _context.Commit();
        }

        public void ManageActionPasaporte(string usuarioId, string updatorId, BizLogic.WorkflowManager.Action action, string comentario)
        {
            var usuario = _userDbAccess.GetUsuario(usuarioId);
            var updator = _userDbAccess.GetUsuario(updatorId);

            _workflowManagerLocal.ManageActionPasaporte(usuario, action, updator, comentario);
        }

        public IEnumerable<Usuario> GetUsuariosPendientePasaporte(Usuario usuario)
        {
            var itinerarios = GetItinerariosEstado(Estado.PendientePasaporte, usuario);
            HashSet<Usuario> data = new HashSet<Usuario>();

            foreach (var itinerario in itinerarios)
                data.Add(itinerario.Usuario);

            return data;
        }

        public void SetVisaToUser(string usuarioId, int visaID, string updatorID)
        {
            var visa = _visaDbAccess.GetVisa(visaID);
            var userToUpd = _userDbAccess.GetUsuario(usuarioId);
            var updator = _userDbAccess.GetUsuario(updatorID);

            var user_visa = new Usuario_Visa()
            {
                Usuario = userToUpd,
                Visa = visa
            };

            if (userToUpd.Visas == null)
                userToUpd.Visas = new List<Usuario_Visa>();
            if (visa.Usuarios == null)
                visa.Usuarios = new List<Usuario_Visa>();

            userToUpd.Visas.Add(user_visa);
            visa.Usuarios.Add(user_visa);

            //await _userManager.UpdateAsync(userToUpd);
            visa = _visaDbAccess.Update(visa, _visaDbAccess.GetVisa(visaID));
            _context.Commit();
        }

        public void ManageActionVisa(string usuarioId, string updaterId, int visaId, BizLogic.WorkflowManager.Action action, string comentario)
        {
            var visa = new Visa();
            if(action != BizLogic.WorkflowManager.Action.Rechazar)
                visa = _visaDbAccess.GetVisa(visaId);
            var usuario = _userDbAccess.GetUsuario(usuarioId);
            var updator = _userDbAccess.GetUsuario(updaterId);

            _workflowManagerLocal.ManageActionVisas(usuario, action, updator, visa.Name, comentario);
        }

        public IEnumerable<(Usuario, IEnumerable<Visa>)> GetVisasUsuarioVisasPendiente(Usuario usuario)
        {
            var itinerarios = GetItinerariosEstado(Estado.PendienteVisas, usuario);
            HashSet<Usuario> usuarios = new HashSet<Usuario>();

            foreach (var itinerario in itinerarios)
                usuarios.Add(itinerario.Usuario);

            var data = new HashSet<(Usuario, IEnumerable<Visa>)>();

            foreach (var user in usuarios)
            {
                var visas = _workflowManagerLocal.ManageVisas(user);
                if (visas.Count() != 0)
                    data.Add((user, visas));
            }
                

            return data;
        }

        public void ContinuarItinerario(int itinerarioId)
        {
            var itinerario = _itinerarioDbAccess.GetItinerario(itinerarioId);
            _workflowManagerLocal.ManageItinerarioPendiente(itinerario);
        }

        public int RegisterViajeInvitado(string userId, string name, string procedencia, string motivo, DateTime fecha)
        {
            var user = _userDbAccess.GetUsuario(userId);
            var vi = new ViajeInvitado()
            {
                FechaLLegada = new DateTime?(fecha),
                Usuario = user,
                Nombre = name,
                Procedencia = procedencia,
                Motivo = motivo
            };

            var viaje = _runnerViajeInvitado.RunAction(vi);

            if (_runnerViajeInvitado.HasErrors)
            {
                return -1;
            }
            
            return viaje.ViajeInvitadoID;
        }
        
        public ViajeInvitado UpdateViajeInvitado(ViajeInvitado entity, ViajeInvitado toUpd)
        {
            var viaje = _viajeInvitadoDbAccess.Update(entity, toUpd);
            _context.Commit();
            return viaje;
        }

        public void CreateViajeInvitadoWorkflow(int viajeInvidtadoId, string claimTipoInstitucion)
        {
            var viajeInvitado = _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvidtadoId);
            _workflowManagerGuest.CrearViaje(viajeInvitado, claimTipoInstitucion);
        }

        public void ManageActionAprobarViajeInvitado(int viajeInvitadoId, string usuarioId, string comentario)
        {
            var viajeInvitado = _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvitadoId);
            var usuario = _userDbAccess.GetUsuario(usuarioId);

            if (viajeInvitado.Estado == Estado.PendienteAprobacionJefeArea)
            {
                _workflowManagerGuest.ManageActionJefeArea(viajeInvitado, BizLogic.WorkflowManager.Action.Aprobar, usuario, comentario);
                return;
            }

            if (viajeInvitado.Estado == Estado.PendienteAprobacionDecano)
            {
                _workflowManagerGuest.ManageActionDecano(viajeInvitado, BizLogic.WorkflowManager.Action.Aprobar, usuario, comentario);
                return;
            }

            if (viajeInvitado.Estado == Estado.PendienteAprobacionRector)
            {
                _workflowManagerGuest.ManageActionRector(viajeInvitado, BizLogic.WorkflowManager.Action.Aprobar, usuario, comentario);
                return;
            }
        }

        public void ManageActionRechazarViajeInvitado(int viajeInvitadoId, string usuarioId, string comentario)
        {
            var viajeInvitado = _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvitadoId);
            var usuario = _userDbAccess.GetUsuario(usuarioId);

            if (viajeInvitado.Estado == Estado.PendienteAprobacionJefeArea)
            {
                _workflowManagerGuest.ManageActionJefeArea(viajeInvitado, BizLogic.WorkflowManager.Action.Rechazar, usuario, comentario);
                return;
            }

            if (viajeInvitado.Estado == Estado.PendienteAprobacionDecano)
            {
                _workflowManagerGuest.ManageActionDecano(viajeInvitado, BizLogic.WorkflowManager.Action.Rechazar, usuario, comentario);
                return;
            }

            if (viajeInvitado.Estado == Estado.PendienteAprobacionRector)
            {
                _workflowManagerGuest.ManageActionRector(viajeInvitado, BizLogic.WorkflowManager.Action.Rechazar, usuario, comentario);
                return;
            }
        }

        public IEnumerable<ViajeInvitado> GetViajesInvitadosNotFinished(Usuario usuario)
        {
            return _userDbAccess.GetViajesInvitadosNotFinished(usuario);
        }

        public IEnumerable<ViajeInvitado> GetViajesInvitadosEstado(Estado estado, Usuario user)
        {
            return _viajeInvitadoDbAccess.GetViajesInvitadoEstado(estado, user);
        }

        public void CancelViajeInvitado(int viajeInvitadoId, string usuarioId, string comentario)
        {
            var trip = _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvitadoId) ;
            var usuario = _userDbAccess.GetUsuario(usuarioId);
            _workflowManagerGuest.CancelarViajeInvitado(trip, usuario, comentario);
        }

        public void ContinuarViajeInvitado(int viajeInvitadoId)
        {
            var viajeInvitado = _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvitadoId);
            _workflowManagerGuest.ManageViajeInvitadoPendiente(viajeInvitado);
        }

        public void RealizarViajeInvitado(int viajeInvitadoId)
        {
            var viajeInvitado = _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvitadoId);
            _workflowManagerGuest.RealizarViajeInvitado(viajeInvitado);
        }

        public ViajeInvitado GetViajeInvitado(int viajeInvitadoId)
        {
            return _viajeInvitadoDbAccess.GetViajeInvitado(viajeInvitadoId);
        }
    }
}
