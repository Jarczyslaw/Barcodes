﻿namespace Barcodes.Core.UI.Views.AdditionalInput
{
    public partial class NmvsProductWindow : BaseWindow
    {
        public NmvsProductWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            tbProductCode.Focus();
        }
    }
}
