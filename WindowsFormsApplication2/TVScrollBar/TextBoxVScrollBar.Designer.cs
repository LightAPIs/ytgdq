namespace WindowsFormsApplication2.TVScrollBar
{
    partial class TextBoxVScrollBar
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomMouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomMouseMove);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.CustomMouseWhell);
            this.MouseLeave += new System.EventHandler(this.CustomMouseLeave);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
