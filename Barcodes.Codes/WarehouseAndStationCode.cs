using System;

namespace Barcodes.Codes
{
    public class WarehouseAndStationCode : BaseCode
    {
        public WarehouseAndStationCode(string code)
        {
            Parse(code);
        }

        public WarehouseAndStationCode(int warehouse, string station)
        {
            CheckWarehouse(warehouse);
            CheckStation(station);

            Warehouse = warehouse;
            Station = station;
        }

        public WarehouseAndStationCode(WarehouseAndStationCode code)
        {
            Warehouse = code.Warehouse;
            Station = code.Station;
        }

        public int Warehouse { get; private set; } = 1;
        public string Station { get; private set; } = string.Empty;

        public override string Code => Prefix + Warehouse.ToString().PadLeft(2, '0') + Station.PadLeft(2, '0');

        public override BarcodeType Type => BarcodeType.Code128;
        public override int Length => 6;
        public override string Prefix => "ST";

        private void CheckStation(string station)
        {
            if (string.IsNullOrEmpty(station) || station.Length != 2)
            {
                throw new ArgumentException("Invalid station number");
            }
        }

        private void CheckWarehouse(int warehouse)
        {
            if (warehouse < 1 || warehouse > 99)
            {
                throw new ArgumentException("Invalid warehouse number");
            }
        }

        public void Parse(string code)
        {
            CheckCode(code);
            var body = GetCodeBody(code);
            var warehouse = body.Substring(0, 2);
            CheckCodeOnlyDigits(warehouse);
            var station = body.Substring(2, 2);

            Warehouse = int.Parse(warehouse);
            Station = station;
        }

        public static bool TryParse(string code, out WarehouseAndStationCode warehouseAndStationCode)
        {
            warehouseAndStationCode = null;
            try
            {
                warehouseAndStationCode = new WarehouseAndStationCode(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}