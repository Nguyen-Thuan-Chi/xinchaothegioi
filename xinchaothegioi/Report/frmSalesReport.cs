using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using xinchaothegioi.Models;

namespace xinchaothegioi.Report
{
    public partial class frmSalesReport : Form
    {
        public frmSalesReport()
        {
            InitializeComponent();
        }

        public void LoadData(IEnumerable<InvoiceRow> rows)
        {
            reportViewer1.LocalReport.DataSources.Clear();
            var rds = new ReportDataSource("InvoiceRowDataSet", rows);
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }
    }
}
