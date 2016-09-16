namespace interopCreator
{
    using System.Windows.Forms;
    using MapWinGIS;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void CheckNewMethods()
        {
            this.axMap1.DeserializeLayer(1, "Test");
            MessageBox.Show(this.axMap1.IsSameProjection("g", "g").ToString());
            var projection = new GeoProjection();
            projection.ImportFromProj4("g");
            var t = tkSavingMode.modeXMLOverwrite;
            axMap1.SaveMapState("r", true, true);

            axMap1.set_LayerSkipOnSaving(1, true);

            var sf = new Shapefile();
            var reprojectedCount = 0;
            sf.Reproject(projection, ref reprojectedCount);
            var geoProjection = sf.GeoProjection;
            var sfSimple = sf.SimplifyLines(10, false);

            var gridHeader = new GridHeader();
            var gridHeaderGeoProjection = gridHeader.GeoProjection;

            var ext = this.axMap1.MaxExtents;
        }
    }
}
