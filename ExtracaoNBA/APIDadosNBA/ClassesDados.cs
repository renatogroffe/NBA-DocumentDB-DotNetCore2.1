using System;
using System.Collections.Generic;

namespace APIDadosNBA
{
    public class Classificacao
    {
        public string id { get; set; }
        public string NomeCampeonato { get; set; }
        public string Temporada { get; set; }
        public string Esporte { get; set; }
        public string Pais { get; set; }
        public DateTime DataExtracao { get; set; }
        public List<Conferencia> Conferencias { get; set; } = new List<Conferencia>();
    }

    public class Conferencia
    {
        public string Nome { get; set; }
        public List<Equipe> Equipes { get; set; } = new List<Equipe>();
    }

    public class Equipe
    {
        public int Posicao { get; set; }
        public string Nome { get; set; }
        public int Vitorias { get; set; }
        public int Derrotas { get; set; }
        public string PercentualVitorias { get; set; }
    }
}