// ReportForm.cs
using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Windows.Forms;

namespace Diploma.Forms
{
    public partial class ReportForm : Form
    {
        private readonly DataTable _data;
        private readonly string _dsName;
        private readonly string _group;
        private readonly string _curator;
        private readonly string _period;

        public ReportForm(DataTable data, string dsName, string group, string curator, string period)
        {
            _data = data;
            _dsName = dsName;
            _group = group;
            _curator = curator;
            _period = period;
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            var rp = reportViewer1.LocalReport;
            rp.DataSources.Clear();
            rp.ReportEmbeddedResource = "Diploma.Report1.rdlc";
            rp.DataSources.Add(new ReportDataSource(_dsName, _data));
            rp.SetParameters(new[]
            {
                new ReportParameter("pGroup",   _group   ?? string.Empty),
                new ReportParameter("pCurator", _curator ?? string.Empty),
                new ReportParameter("pPeriod",  _period  ?? string.Empty)
            });
            reportViewer1.ZoomMode = ZoomMode.PageWidth;
            reportViewer1.RefreshReport();
        }
    }
}
