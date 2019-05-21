using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace App.Models
{
    public static class Reporting
    {
        public static byte[] GenerarInforme<T>(this List<T> lista, string sPath, string sNombreInforme, string sNombreDS, string sFormato)
        {
            string xsError = "";
            byte[] bytes = null;
            var _rv = new ReportViewer();

            try
            {
                string sBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                var _assembly = Assembly.Load(File.ReadAllBytes(sBinPath + "\\App.dll"));
                var _informe = "App.Reportes." + sNombreInforme;

                using (Stream stream = _assembly.GetManifestResourceStream(_informe))
                {
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    _rv.LocalReport.EnableExternalImages = true;
                    _rv.LocalReport.LoadReportDefinition(stream);

                    _rv.LocalReport.DataSources.Clear();
                    _rv.LocalReport.DataSources.Add(new ReportDataSource(sNombreDS, lista));
                    _rv.LocalReport.Refresh();

                    bytes = _rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }

            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return bytes;
        }

        public static byte[] GenerarInforme<T>(this List<T> lista, string sPath, string sNombreInforme, List<string> sNombresDS, string sFormato)
        {
            string xsError = "";
            byte[] bytes = null;
            var _rv = new ReportViewer();

            try
            {
                string sBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                var _assembly = Assembly.Load(File.ReadAllBytes(sBinPath + "\\App.dll"));
                var _informe = "App.Reportes." + sNombreInforme;

                using (Stream stream = _assembly.GetManifestResourceStream(_informe))
                {
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    _rv.LocalReport.EnableExternalImages = true;
                    _rv.LocalReport.LoadReportDefinition(stream);
                    _rv.LocalReport.DataSources.Clear();

                    for (int i = 0; i < lista.Count; i++)
                    {
                        _rv.LocalReport.DataSources.Add(new ReportDataSource(sNombresDS[i], lista[i]));
                    }

                    _rv.LocalReport.Refresh();

                    bytes = _rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                }

            }
            catch (Exception ex)
            {
                xsError = ex.Message;
            }

            return bytes;
        }
    }
}