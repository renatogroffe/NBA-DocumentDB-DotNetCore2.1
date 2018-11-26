using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;

namespace APIDadosNBA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NBAController : ControllerBase
    {
        private DocumentDBConfigurations _configurations;

        public NBAController(
            DocumentDBConfigurations configurations)
        {
            _configurations = configurations;
        }

        [HttpGet]
        public IEnumerable<Classificacao> Get()
        {
            using (var client = new DocumentClient(
                new Uri(_configurations.EndpointUri),
                        _configurations.PrimaryKey))
            {
                FeedOptions queryOptions =
                    new FeedOptions { MaxItemCount = -1 };

                return client.CreateDocumentQuery<Classificacao>(
                    UriFactory.CreateDocumentCollectionUri(
                        _configurations.Database,
                        _configurations.Collection), queryOptions)
                    .Where(c => c.NomeCampeonato == "NBA").ToList();
            }
        }
    }
}