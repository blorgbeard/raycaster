namespace Raycaster
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.tmrTick = new System.Windows.Forms.Timer(this.components);
            this.pbMap = new System.Windows.Forms.PictureBox();
            this.radMovePlayer = new System.Windows.Forms.RadioButton();
            this.radDrawWalls = new System.Windows.Forms.RadioButton();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.chkTextures = new System.Windows.Forms.CheckBox();
            this.btnClearWalls = new System.Windows.Forms.Button();
            this.dxRender = new SharpDX.Windows.RenderControl();
            this.txtQuadInfo = new System.Windows.Forms.TextBox();
            this.trkQuadSize = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuadSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrTick
            // 
            this.tmrTick.Enabled = true;
            this.tmrTick.Tick += new System.EventHandler(this.tmrTick_Tick);
            // 
            // pbMap
            // 
            this.pbMap.BackColor = System.Drawing.Color.White;
            this.pbMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbMap.Location = new System.Drawing.Point(338, 12);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new System.Drawing.Size(240, 240);
            this.pbMap.TabIndex = 0;
            this.pbMap.TabStop = false;
            this.pbMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMap_Paint);
            this.pbMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseDown);
            this.pbMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseMove);
            this.pbMap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseUp);
            // 
            // radMovePlayer
            // 
            this.radMovePlayer.AutoSize = true;
            this.radMovePlayer.Checked = true;
            this.radMovePlayer.Location = new System.Drawing.Point(338, 259);
            this.radMovePlayer.Name = "radMovePlayer";
            this.radMovePlayer.Size = new System.Drawing.Size(87, 17);
            this.radMovePlayer.TabIndex = 1;
            this.radMovePlayer.TabStop = true;
            this.radMovePlayer.Text = "Move / Look";
            this.radMovePlayer.UseVisualStyleBackColor = true;
            // 
            // radDrawWalls
            // 
            this.radDrawWalls.AutoSize = true;
            this.radDrawWalls.Location = new System.Drawing.Point(483, 259);
            this.radDrawWalls.Name = "radDrawWalls";
            this.radDrawWalls.Size = new System.Drawing.Size(95, 17);
            this.radDrawWalls.TabIndex = 1;
            this.radDrawWalls.Text = "Build / Destroy";
            this.radDrawWalls.UseVisualStyleBackColor = true;
            // 
            // txtInfo
            // 
            this.txtInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInfo.Location = new System.Drawing.Point(12, 259);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(320, 56);
            this.txtInfo.TabIndex = 2;
            // 
            // chkTextures
            // 
            this.chkTextures.AutoSize = true;
            this.chkTextures.Location = new System.Drawing.Point(338, 282);
            this.chkTextures.Name = "chkTextures";
            this.chkTextures.Size = new System.Drawing.Size(58, 17);
            this.chkTextures.TabIndex = 3;
            this.chkTextures.Text = "Bricks!";
            this.chkTextures.UseVisualStyleBackColor = true;
            // 
            // btnClearWalls
            // 
            this.btnClearWalls.Location = new System.Drawing.Point(501, 282);
            this.btnClearWalls.Name = "btnClearWalls";
            this.btnClearWalls.Size = new System.Drawing.Size(75, 23);
            this.btnClearWalls.TabIndex = 4;
            this.btnClearWalls.Text = "Nuke";
            this.btnClearWalls.UseVisualStyleBackColor = true;
            this.btnClearWalls.Click += new System.EventHandler(this.btnClearWalls_Click);
            // 
            // dxRender
            // 
            this.dxRender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dxRender.Location = new System.Drawing.Point(12, 12);
            this.dxRender.Name = "dxRender";
            this.dxRender.Size = new System.Drawing.Size(320, 240);
            this.dxRender.TabIndex = 5;
            this.dxRender.Paint += new System.Windows.Forms.PaintEventHandler(this.dxRender_Paint);
            // 
            // txtQuadInfo
            // 
            this.txtQuadInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuadInfo.Location = new System.Drawing.Point(12, 321);
            this.txtQuadInfo.Multiline = true;
            this.txtQuadInfo.Name = "txtQuadInfo";
            this.txtQuadInfo.ReadOnly = true;
            this.txtQuadInfo.Size = new System.Drawing.Size(320, 40);
            this.txtQuadInfo.TabIndex = 2;
            // 
            // trkQuadSize
            // 
            this.trkQuadSize.LargeChange = 20;
            this.trkQuadSize.Location = new System.Drawing.Point(95, 367);
            this.trkQuadSize.Maximum = 200;
            this.trkQuadSize.Minimum = 10;
            this.trkQuadSize.Name = "trkQuadSize";
            this.trkQuadSize.Size = new System.Drawing.Size(237, 45);
            this.trkQuadSize.TabIndex = 6;
            this.trkQuadSize.TickFrequency = 20;
            this.trkQuadSize.Value = 50;
            this.trkQuadSize.Scroll += new System.EventHandler(this.trkQuadSize_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 367);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Leaf quad size";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 448);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trkQuadSize);
            this.Controls.Add(this.dxRender);
            this.Controls.Add(this.btnClearWalls);
            this.Controls.Add(this.chkTextures);
            this.Controls.Add(this.txtQuadInfo);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.radDrawWalls);
            this.Controls.Add(this.radMovePlayer);
            this.Controls.Add(this.pbMap);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuadSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrTick;
        private System.Windows.Forms.PictureBox pbMap;
        private System.Windows.Forms.RadioButton radMovePlayer;
        private System.Windows.Forms.RadioButton radDrawWalls;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.CheckBox chkTextures;
        private System.Windows.Forms.Button btnClearWalls;
        private SharpDX.Windows.RenderControl dxRender;
        private System.Windows.Forms.TextBox txtQuadInfo;
        private System.Windows.Forms.TrackBar trkQuadSize;
        private System.Windows.Forms.Label label1;
    }
}

