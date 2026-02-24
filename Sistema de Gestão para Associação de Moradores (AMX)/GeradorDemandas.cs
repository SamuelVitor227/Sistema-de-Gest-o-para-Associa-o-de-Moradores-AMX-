using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociacaoAMX
{
    /// <summary>
    /// Classe estática utilitária para gerar uma carga de dados de teste de demandas.
    /// Popula a associação com um grande número de demandas, atribuindo autores e tentando alocá-las a prestadores.
    /// </summary>
    public static class GeradorDemandas
    {
        /// <summary>
        /// O número total de demandas a serem geradas.
        /// </summary>
        private static int _quantidadeDemandas = 2500;

        /// <summary>
        /// Instância estática de Random para operações de aleatoriedade.
        /// </summary>
        private static Random _rnd = new Random();

        /// <summary>
        /// Ponto de entrada principal para executar a geração de demandas.
        /// </summary>
        /// <param name="associacao">A instância da <see cref="Associacao"/> a ser populada com demandas.</param>
        /// <exception cref="ArgumentNullException">Lançada se a associação for nula.</exception>
        /// <exception cref="InvalidOperationException">Lançada se a associação não contiver associados ou habilidades, que são pré-requisitos.</exception>
        public static void Executar(Associacao associacao)
        {
            if (associacao == null)
                throw new ArgumentNullException(nameof(associacao), "Associação não pode ser nula.");
            if (associacao.GetAssociados() == null || associacao.GetAssociados().Count == 0 ||
                associacao.GetHabilidades() == null || associacao.GetHabilidades().Count == 0)
                throw new InvalidOperationException("Associação deve ter associados e habilidades cadastradas para gerar demandas.");

            List<Associado> todosAssociados = associacao.GetAssociados().Values.ToList();
            List<Habilidade> todasHabilidades = associacao.GetHabilidades().Values.ToList();
            List<Associado> autoresDeDemanda = todosAssociados;
            List<Prestador> prestadoresDisponiveis = todosAssociados.OfType<Prestador>().ToList();

            DefinirQuantidadesDemandas();
            GerarEAtribuirDemandas(associacao, todasHabilidades, autoresDeDemanda, prestadoresDisponiveis);
        }

        /// <summary>
        /// Campos para armazenar a quantidade de demandas a serem geradas com 1, 2, 3, 4 e 5 habilidades, respectivamente.
        /// </summary>
        private static int _com1, _com2, _com3, _com4, _com5;

        /// <summary>
        /// Define a distribuição de quantas demandas serão geradas com base no número de habilidades necessárias.
        /// </summary>
        private static void DefinirQuantidadesDemandas()
        {
            _com5 = (int)(_quantidadeDemandas * 0.10);
            _com4 = (int)(_quantidadeDemandas * 0.20);
            _com3 = (int)(_quantidadeDemandas * 0.20);
            _com2 = (int)(_quantidadeDemandas * 0.20);
            _com1 = (int)(_quantidadeDemandas * 0.30);
        }

        /// <summary>
        /// Orquestra a geração, o cadastro e a atribuição de demandas na associação.
        /// </summary>
        /// <param name="associacao">A instância da associação.</param>
        /// <param name="habilidadesDisponiveis">Lista de todas as habilidades disponíveis.</param>
        /// <param name="autoresDeDemanda">Lista de todos os associados que podem criar demandas.</param>
        /// <param name="prestadoresDisponiveis">Lista de todos os prestadores que podem atender demandas.</param>
        private static void GerarEAtribuirDemandas(Associacao associacao, List<Habilidade> habilidadesDisponiveis, List<Associado> autoresDeDemanda, List<Prestador> prestadoresDisponiveis)
        {
            Dictionary<int, int> demandasPorQtdHabilidades = new Dictionary<int, int>(5)
            {
                {5, _com5}, {4, _com4}, {3, _com3}, {2, _com2}, {1, _com1}
            };

            int demandaContador = 1;

            foreach (var item in demandasPorQtdHabilidades)
            {
                int quantidadeHabilidades = item.Key;
                int numeroDemandasGerar = item.Value;

                for (int i = 0; i < numeroDemandasGerar; i++)
                {
                    Associado autor = autoresDeDemanda[_rnd.Next(0, autoresDeDemanda.Count)];
                    string descricao = $"Demanda {demandaContador}";
                    int tempoPrevisto = CalcularTempoDemanda(quantidadeHabilidades);
                    int prazo = _rnd.Next(5, 60);

                    Demanda novaDemanda = new Demanda(autor, descricao, tempoPrevisto, prazo);

                    List<Habilidade> habilidadesParaDemanda = SelecionarHabilidadesAleatorias(habilidadesDisponiveis, quantidadeHabilidades);
                    foreach (Habilidade habilidade in habilidadesParaDemanda)
                    {
                        novaDemanda.AtribuirHabilidadesNecessarias(habilidade);
                    }

                    try
                    {
                        associacao.CadastrarDemanda(novaDemanda);
                        autor.RegistrarDemanda(novaDemanda);
                        Prestador melhorPrestador = associacao.DefinirMelhorPrestadorParaDemanda(novaDemanda);
                        if (melhorPrestador != null)
                        {
                            try
                            {
                                melhorPrestador.AtribuirDemanda(novaDemanda);
                                Console.WriteLine($"Demanda {novaDemanda.GetHashCode()} ('{novaDemanda.GetDescricao()}') atribuída a {melhorPrestador.GetNome()}.");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Erro ao atribuir demanda {novaDemanda.GetHashCode()} a {melhorPrestador.GetNome()}: {ex.Message}");
                            }
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Erro ao cadastrar demanda: {ex.Message}");
                    }
                    catch (Associado.CreditosInsuficientesException ex)
                    {
                        Console.WriteLine($"Associado {autor.GetNome()} não pôde criar demanda {novaDemanda.GetHashCode()}: {ex.Message}");
                        associacao.ProcurarAssociadoParaDemanda(autor);
                        demandaContador++;
                    }
                }
            }
        }

        /// <summary>
        /// Calcula um tempo previsto aleatório para uma demanda, baseado na sua complexidade (quantidade de habilidades).
        /// </summary>
        /// <param name="quantidadeHabilidades">O número de habilidades da demanda.</param>
        /// <returns>O tempo previsto em frações de 30 minutos.</returns>
        private static int CalcularTempoDemanda(int quantidadeHabilidades)
        {
            switch (quantidadeHabilidades)
            {
                case 1:
                case 2:
                    return _rnd.Next(30, 180) / 30;
                case 3:
                case 4:
                    return _rnd.Next(180, 480) / 30;
                case 5:
                    return _rnd.Next(480, 960) / 30;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Seleciona uma quantidade específica de habilidades aleatórias de uma lista.
        /// </summary>
        /// <param name="habilidadesDisponiveis">A lista de todas as habilidades disponíveis para seleção.</param>
        /// <param name="quantidade">O número de habilidades a serem selecionadas.</param>
        /// <returns>Uma nova lista contendo as habilidades selecionadas aleatoriamente.</returns>
        /// <exception cref="InvalidOperationException">Lançada se o número de habilidades a selecionar for maior que o número de habilidades disponíveis.</exception>
        private static List<Habilidade> SelecionarHabilidadesAleatorias(List<Habilidade> habilidadesDisponiveis, int quantidade)
        {
            if (habilidadesDisponiveis.Count < quantidade)
                throw new InvalidOperationException($"Não há habilidades suficientes disponíveis para selecionar {quantidade} habilidades.");

            return habilidadesDisponiveis.OrderBy(h => _rnd.Next()).Take(quantidade).ToList();
        }
    }
}