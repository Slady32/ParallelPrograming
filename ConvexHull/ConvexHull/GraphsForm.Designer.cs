namespace ConvexHull
{
    partial class GraphsForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GenGraph = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(607, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(165, 537);
            this.textBox1.TabIndex = 0;
            // 
            // GenGraph
            // 
            this.GenGraph.Location = new System.Drawing.Point(12, 10);
            this.GenGraph.Name = "GenGraph";
            this.GenGraph.Size = new System.Drawing.Size(169, 23);
            this.GenGraph.TabIndex = 1;
            this.GenGraph.Text = "Generate Graph";
            this.GenGraph.UseVisualStyleBackColor = true;
            this.GenGraph.Click += new System.EventHandler(this.GenGraph_Click);
            // 
            // GraphsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.GenGraph);
            this.Controls.Add(this.textBox1);
            this.Name = "GraphsForm";
            this.Text = "ConvexHull";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button GenGraph;
    }
}

