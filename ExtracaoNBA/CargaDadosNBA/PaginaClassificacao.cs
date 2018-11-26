using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CargaDadosNBA
{
    public class PaginaClassificacao
    {
        private SeleniumConfigurations _configurations;
        private IWebDriver _driver;

        public PaginaClassificacao(SeleniumConfigurations configurations)
        {
            _configurations = configurations;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            _driver = new ChromeDriver(
                _configurations.CaminhoDriverChrome,
                options);
        }

        public void CarregarPagina()
        {
            _driver.Manage().Timeouts().PageLoad =
                TimeSpan.FromSeconds(_configurations.Timeout);
            _driver.Navigate().GoToUrl(
                _configurations.UrlPaginaClassificacaoNBA);
        }

        public Classificacao ObterClassificacao()
        {
            DateTime dataCarga = DateTime.Now;
            List<Conferencia> conferencias = new List<Conferencia>();

            Classificacao classificacao = new Classificacao();
            classificacao.id = $"NBA {dataCarga.ToString("yyyy-MM-dd HH:mm:ss")}";
            classificacao.NomeCampeonato = "NBA";

            int anoInicial;
            int anoFinal;
            if (dataCarga.Month >= 10)
            {
                anoInicial = dataCarga.Year;
                anoFinal = anoInicial + 1;
            }
            else
            {
                anoFinal = dataCarga.Year;
                anoInicial = anoFinal - 1;
            }

            classificacao.Temporada = anoInicial + " " + anoFinal;
            classificacao.Esporte = "Basquete";
            classificacao.Pais = "Estados Unidos";
            classificacao.DataExtracao = DateTime.Now;
            classificacao.Conferencias = conferencias;

            var dadosConferencias = _driver
                .FindElements(By.ClassName("standings__order-conference"));
            var captions = _driver
                .FindElements(By.ClassName("standings__header"));

            for (int i = 0; i < captions.Count; i++)
            {
                var caption = captions[i];
                Conferencia conferencia = new Conferencia();
                conferencia.Nome =
                    caption.FindElement(By.TagName("span")).Text;
                conferencias.Add(conferencia);

                int posicao = 0;
                var conf = dadosConferencias[i];
                var dadosEquipes = conf.FindElement(By.TagName("tbody"))
                    .FindElements(By.TagName("tr"));
                foreach (var dadosEquipe in dadosEquipes)
                {
                    posicao++;
                    Equipe equipe = new Equipe();
                    equipe.Posicao = posicao;

                    equipe.Nome = dadosEquipe.FindElement(
                        By.TagName("th")).Text;

                    var estatisticasEquipe = dadosEquipe.FindElements(
                        By.TagName("td"));

                    equipe.Vitorias = Convert.ToInt32(
                        ExtrairEstatistica(estatisticasEquipe[0].Text));
                    equipe.Derrotas = Convert.ToInt32(
                        ExtrairEstatistica(estatisticasEquipe[1].Text));
                    equipe.PercentualVitorias =
                        ExtrairEstatistica(estatisticasEquipe[2].Text);

                    conferencia.Equipes.Add(equipe);
                }
            }

            return classificacao;
        }

        private string ExtrairEstatistica(string textoEstatistica)
        {
            return textoEstatistica.Split(
                new string[] { "\r\n" }, StringSplitOptions.None)[1];
        }

        public void Fechar()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}