﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Datos;

namespace Negocios.BusinessControllers
{
    public class HomeCtrl
    {
        #region SINGLETON

        private static HomeCtrl _HomeCtrl;

        public static HomeCtrl GetInstancia()
        {
            if (_HomeCtrl == null)
                _HomeCtrl = new HomeCtrl();

            return _HomeCtrl;
        }

        #endregion

        #region COMPORTAMIENTO

        public List<spGetMayoresVentasClientes> ObtenerClientesConMayoresVentas()
        {
            List<spGetMayoresVentasClientes> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                loResultado = xoDB.Database.SqlQuery<spGetMayoresVentasClientes>("exec spDashboardMayoresVentasClientes").ToList();
            }

            return loResultado;
        }

        public List<spGetProductosMasVendidos> ObtenerProductosMasVendidos()
        {
            List<spGetProductosMasVendidos> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                loResultado = xoDB.Database.SqlQuery<spGetProductosMasVendidos>("exec spDashboardProductosMasVendidos").ToList();
            }

            return loResultado;
        }

        public List<spGetDatosGrafico> ObtenerDatosGrafico(out string xsError)
        {
            xsError = "";
            List<spGetDatosGrafico> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    loResultado = xoDB.Database.SqlQuery<spGetDatosGrafico>("exec spDashboardDatosGrafico").ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return loResultado;
        }

        public List<spGetDatosGrafico> ObtenerDatosGananciaMensualGrafico(out string xsError)
        {
            xsError = "";
            List<spGetDatosGrafico> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                try
                {
                    loResultado = xoDB.Database.SqlQuery<spGetDatosGrafico>("exec spDashboardGananciaNetaGrafico").ToList();
                }
                catch (Exception ex)
                {
                    xsError = ex.Message;
                }
            }

            return loResultado;
        }

        public List<spGetClientesMorosos> ObtenerClientesMorosos()
        {
            List<spGetClientesMorosos> loResultado = null;

            using (BD_Entities xoDB = new BD_Entities())
            {
                loResultado = xoDB.Database.SqlQuery<spGetClientesMorosos>("exec spDashboardClientesMorosos").ToList();
            }

            return loResultado;
        }

        #endregion
    }
}
