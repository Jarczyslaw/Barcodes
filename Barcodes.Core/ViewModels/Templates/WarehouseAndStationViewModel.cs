using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels.Templates
{
    public class WarehouseAndStationViewModel : BaseTemplateViewModel
    {
        private int warehouse = 1;
        private string station = string.Empty;

        public WarehouseAndStationViewModel(IAppDialogsService dialogsService)
            : base(dialogsService)
        {
        }

        public int Warehouse
        {
            get => warehouse;
            set => SetProperty(ref warehouse, value);
        }

        public string Station
        {
            get => station;
            set => SetProperty(ref station, value);
        }

        public override void LoadData(string data)
        {
            if (WarehouseAndStationCode.TryParse(data, out WarehouseAndStationCode warehouseAndStation))
            {
                Warehouse = warehouseAndStation.Warehouse;
                Station = warehouseAndStation.Station;
            }
        }

        protected override TemplateResult GetResultData()
        {
            var code = new WarehouseAndStationCode(Warehouse, Station.Trim());
            return new TemplateResult(code);
        }

        protected override bool Validate()
        {
            try
            {
                new WarehouseAndStationCode(Warehouse, Station.Trim());
                return true;
            }
            catch (Exception exc)
            {
                dialogsService.ShowError(exc.Message);
                return false;
            }
        }
    }
}