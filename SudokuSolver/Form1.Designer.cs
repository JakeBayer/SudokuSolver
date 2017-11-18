using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver
{
    partial class Form1
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
            this.pnlSolution = new System.Windows.Forms.Panel();
            this.txtStep = new System.Windows.Forms.TextBox();
            this.trkStepSlider = new System.Windows.Forms.TrackBar();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.lblStepType = new System.Windows.Forms.Label();
            this.pnlSolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkStepSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSolution
            // 
            this.pnlSolution.Controls.Add(this.lblStepType);
            this.pnlSolution.Controls.Add(this.txtStep);
            this.pnlSolution.Controls.Add(this.trkStepSlider);
            this.pnlSolution.Controls.Add(this.btnNext);
            this.pnlSolution.Controls.Add(this.btnPrev);
            this.pnlSolution.Location = new System.Drawing.Point(375, 12);
            this.pnlSolution.Name = "pnlSolution";
            this.pnlSolution.Size = new System.Drawing.Size(563, 387);
            this.pnlSolution.TabIndex = 0;
            this.pnlSolution.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSolution_Paint);
            // 
            // txtStep
            // 
            this.txtStep.Location = new System.Drawing.Point(165, 51);
            this.txtStep.Name = "txtStep";
            this.txtStep.Size = new System.Drawing.Size(30, 20);
            this.txtStep.TabIndex = 3;
            this.txtStep.TextChanged += new System.EventHandler(this.txtStep_TextChanged);
            // 
            // trkStepSlider
            // 
            this.trkStepSlider.Location = new System.Drawing.Point(20, 0);
            this.trkStepSlider.Name = "trkStepSlider";
            this.trkStepSlider.Size = new System.Drawing.Size(332, 45);
            this.trkStepSlider.TabIndex = 2;
            this.trkStepSlider.Scroll += new System.EventHandler(this.trkStepSlider_Scroll);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(201, 49);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(151, 24);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(20, 49);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(139, 24);
            this.btnPrev.TabIndex = 0;
            this.btnPrev.Text = "Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblStepType
            // 
            this.lblStepType.AutoSize = true;
            this.lblStepType.Location = new System.Drawing.Point(377, 20);
            this.lblStepType.Name = "lblStepType";
            this.lblStepType.Size = new System.Drawing.Size(0, 13);
            this.lblStepType.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 411);
            this.Controls.Add(this.pnlSolution);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.pnlSolution.ResumeLayout(false);
            this.pnlSolution.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkStepSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox[,] txtGrid;
        private System.Windows.Forms.Label[,] lblGrid;
        private Panel pnlSolution;
        private Button btnNext;
        private Button btnPrev;
        private TextBox txtStep;
        private TrackBar trkStepSlider;
        private Label lblStepType;
    }
}

