using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CargaDadosNBA
{
    public class ClassificacaoRepository
    {
        private DocumentDBConfigurations _configurations;
        private DocumentClient _client;

        public ClassificacaoRepository(
            DocumentDBConfigurations configurations)
        {
            _configurations = configurations;
            _client = new DocumentClient(
                new Uri(configurations.EndpointUri), configurations.PrimaryKey);

            // Cria o banco de dados caso o mesmo não exista
            _client.CreateDatabaseIfNotExistsAsync(
                new Database { Id = configurations.Database }).Wait();

            // Cria a coleção caso a mesma não exista
            DocumentCollection collectionInfo = new DocumentCollection();
            collectionInfo.Id = _configurations.Collection;

            collectionInfo.IndexingPolicy =
                new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

            _client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(_configurations.Database),
                collectionInfo,
                new RequestOptions { OfferThroughput = 400 }).Wait();
        }

        public void Incluir(Classificacao classificacao)
        {
            _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(
                    _configurations.Database,
                    _configurations.Collection), classificacao).Wait();
        }
    }
}