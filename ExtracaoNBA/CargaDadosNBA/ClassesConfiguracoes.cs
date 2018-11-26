namespace CargaDadosNBA
{
    public class SeleniumConfigurations
    {
        public string CaminhoDriverChrome { get; set; }
        public string UrlPaginaClassificacaoNBA { get; set; }
        public int Timeout { get; set; }
    }

    public class DocumentDBConfigurations
    {
        public string EndpointUri { get; set; }
        public string PrimaryKey { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}