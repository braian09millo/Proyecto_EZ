﻿using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocios.BusinessControllers
{
    public class ClienteCtrl
    {
        #region SINGLETON

        private static ClienteCtrl _ClienteCtrl;

        public static ClienteCtrl GetInstancia()
        {
            if (_ClienteCtrl == null)
                _ClienteCtrl = new ClienteCtrl();

            return _ClienteCtrl;
        }

        #endregion

        #region COMPORTAMIENTO

        public void GuardarCliente(cliente xoCliente, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    var cliente = xoDB.cliente.Find(xoCliente.cli_id);

                    if (cliente != null)
                        xoDB.Entry(cliente).CurrentValues.SetValues(xoCliente);
                    else
                        xoDB.cliente.Add(xoCliente);

                    xoDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        public List<spGetClientes> ObtenerClientes(out string xsError)
        {
            xsError = "";
            List<spGetClientes> xoResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetClientes>("exec spGetClientes").ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return xoResultado;
        }

        public void EliminarCliente(int xiId, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    var xoCliente = xoDB.cliente.Find(xiId);

                    if (xoCliente != null)
                    {
                        xoCliente.cli_delet = "S";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El usuario no existe";
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        public void HabilitarCliente(int xiId, out string xsError)
        {
            xsError = "";

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    var xoCliente = xoDB.cliente.Find(xiId);

                    if (xoCliente != null)
                    {
                        xoCliente.cli_delet = "N";
                        xoDB.SaveChanges();
                    }
                    else
                        xsError = "El usuario no existe";
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }
        }

        public List<provincia> ObtenerProvincias(out string xsError)
        {
            xsError = "";
            List<provincia> loResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loResultado = xoDB.provincia.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loResultado;
        }

        public List<localidad> ObtenerLocalidades(out string xsError)
        {
            xsError = "";
            List<localidad> loResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loResultado = xoDB.localidad.ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loResultado;
        }

        public List<localidad> FiltrarLocalidades(int xiProvincia, out string xsError)
        {
            xsError = "";
            List<localidad> loResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    loResultado = xoDB.localidad.Where(x => x.loc_provincia == xiProvincia)
                                                .OrderBy(x => x.loc_descr)
                                                .ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return loResultado;
        }

        public List<spGetClientesConMasVentas> ObtenerClientesMayoresVentas(out string xsError)
        {
            xsError = "";
            List<spGetClientesConMasVentas> xoResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetClientesConMasVentas>("exec spRptFacturacion").ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return xoResultado;
        }
        public List<spGetClientesMorosos> ObtenerClientesMorosos(out string xsError)
        {
            xsError = "";
            List<spGetClientesMorosos> xoResultado = null;

            try
            {
                using (BD_Entities xoDB = new BD_Entities())
                {
                    xoResultado = xoDB.Database.SqlQuery<spGetClientesMorosos>("exec spRptClientesMorosos").ToList();
                }
            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return xoResultado;
        }

        #endregion
    }
}
