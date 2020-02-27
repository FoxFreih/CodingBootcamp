﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SupportTroubleshootingTool.Core.Contract;
using SupportTroubleshootingTool.Core.Model;

namespace SupportTroubleshootingTool.UI
{
    public partial class ExistingSessionFormUi : Form
    {
        private const bool V = true;
        private SessionProvider _sessionProvider;
        private SessionInfo _currentSession;

        public ExistingSessionFormUi(SessionProvider sessionProvider, NewSessionFormUi backForm)
         { //TODO: Check sessionProvider and _currentSession are not null.
            //If null log error and show error dialog
            _sessionProvider = sessionProvider;
            _currentSession = _sessionProvider.CurrentSession;

            InitializeComponent();
           }
          private void ExistingSessionFormUi_Load(object sender, EventArgs e)
        {
            this.Text =this.Text + _currentSession.SessionID;
            this.loadData.Items.Add("workflow:  " + _currentSession.WorkflowName);

            string EVL = "";
            EVL = "Event View Logs:";
            int evilog = 1;
            foreach (EVLogInfo EVlog in _currentSession.SelectedEVLogs)
            {
                if (evilog == 1) {
                    EVL = EVL + " " + EVlog.LogName;
                    evilog = 0;
                }
                else
                    EVL = EVL + ", " + EVlog.LogName;
              }
               this.loadData.Items.Add(EVL);


            string fileloge = "";
            fileloge = "File Logs:";
            int log = 1;

            foreach (FileLogInfo fileLog in _currentSession.SelectedFileLogs)
            {
                if (log == 1)
                {
                 fileloge = fileloge + " " + fileLog.LogFileName;
                    log = 0;
                }
                else
                    fileloge = fileloge + ", " + fileLog.LogFileName;
               }
                 this.loadData.Items.Add(fileloge);

            string trce = "";
            trce = "Traces:";
            int Trce = 1;
            foreach (TraceInfo trace in _currentSession.SelectedTraces)
            {
                if (Trce == 1)
                {
                    trce = trce + " " + trace.Description;
                    Trce= 0;
                }
                else
                    trce = trce + " ," + trace.Description;
             }
               this.loadData.Items.Add(trce);

            loadData.Items.Add($"loglevel:{_currentSession.LogLevel}");

            dateTimeFrom.Value = _sessionProvider.CurrentSession.From;
            //dateTimeTo.Value = _sessionProvider.CurrentSession.To;
            //dateTimeTo.Value= DateTime.Today;


        }
        private void butCollectDataClick(object sender, EventArgs e)
        {
            _sessionProvider.CurrentSession.From = dateTimeFrom.Value;
            _sessionProvider.CurrentSession.To = dateTimeTo.Value;

            //if (dateTimeTo.Value < dateTimeFrom.Value)
            //{
            //    MessageBox.Show("It cannot be 'DateTo' a less from 'DateFrom'", "Error");
            //}
            //else
            //{
                bool s = _sessionProvider.CollectData();

                if (!s)
                {
                  DialogResult dialogResult = MessageBox.Show("Data for the same data and time already was collect ! ," +
                 "Do you want to override it?," +
                 "\nClick Ok,to override." +
                 "\nClick cancel,to change date and time.", "Collect data",MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                      _sessionProvider.CollectData(true);
                    }
                    else if (dialogResult == DialogResult.Cancel)
                    {
                        this.Show();
                    }
                }
            //}
    }
        private void butCloseSessionClick(object sender, EventArgs e)
        {
            _sessionProvider.CurrentSession.From = dateTimeFrom.Value;
            _sessionProvider.CurrentSession.To = dateTimeTo.Value;
            //_sessionProvider.CollectData();
            _sessionProvider.StopSession();
            this.Close();
        }

         private void butOpenSeesion_Click(object sender, EventArgs e)
        {
          System.Diagnostics.Process.Start(_currentSession.SessionOtputFolderPath);
        }
        private void dateTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            dateTimeTo.MinDate = dateTimeFrom.Value.AddDays(1);
            //dateTimeTo.MinDate = dateTimeFrom.Value.AddHours(1);
        }
       private void dateTimeTo_ValueChanged_1(object sender, EventArgs e)
        {
            dateTimeTo.MaxDate = DateTime.Now;
            dateTimeFrom.MaxDate = dateTimeTo.Value.AddDays(-1);
            //dateTimeFrom.MaxDate = dateTimeTo.Value.AddHours(-1);
            //dateTimeTo.Value = DateTime.Today;
        }
    }
}
