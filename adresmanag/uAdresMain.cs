using GGPlatform.DBServer;

namespace GGPlatform.AdresManag
{
    public class AdresInfo
    {
        public string Title { get; set; }
        public string House { get; set; }
        public string Korp { get; set; }
        public string Kvart { get; set; }
      //  public int Index { get; set; }
        public int Region { get; set; }
        public int Raion { get; set; }
        public int Gorod { get; set; }
        public int Naspunkt { get; set; }
        public int Street { get; set; }
    }

    public class AdresManager
    {
        private DBSqlServer _dbsql = null;
        private AdresInfo _adres = null;
        public AdresManager(ref AdresInfo inAdres, DBSqlServer inDBSqlServer)
        {
            _adres = inAdres;
            _dbsql = inDBSqlServer;
        }
        public void GetAdresInfo()
        {
            frmAdres _frmAdres = new frmAdres { _dbSql = _dbsql, Adreses = _adres};
            if (_frmAdres.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                _adres = _frmAdres.Adreses;
            _frmAdres.Dispose();
            _frmAdres = null;
            return;
        }

    }
}
