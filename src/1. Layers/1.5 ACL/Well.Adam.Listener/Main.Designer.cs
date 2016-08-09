namespace PH.Well.Adam.Listener
{
    using System;
    using System.Windows.Forms;

    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

        private ContextMenu trayMenu;

        private NotifyIcon trayIcon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();


            this.trayMenu = new ContextMenu();
            this.trayMenu.MenuItems.Add("Exit", OnExit);
            // 
            // notifyIcon1
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.trayIcon.Text = "Order Adapter";
            this.trayIcon.Visible = true;
            this.trayIcon.ContextMenu = this.trayMenu;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "OrderWell File Monitor";
            this.Text = "OrderWell File Monitor";
            this.ResumeLayout(false);

        }

        #endregion

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

