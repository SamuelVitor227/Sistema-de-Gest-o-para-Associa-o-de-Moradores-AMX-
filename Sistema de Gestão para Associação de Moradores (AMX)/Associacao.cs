using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssociacaoAMX
{
    /// <summary>
    /// Gerencia o funcionamento da Associação, controlando o cadastro e a interação entre 
    /// Associados, Habilidades, Produtos e Demandas.
    /// </summary>
    public class Associacao
    {
        /// <summary>
        /// Dicionário que armazena todos os associados, utilizando o CPF como chave.
        /// </summary>
        private Dictionary<string, Associado> _associados;

        /// <summary>
        /// Dicionário que armazena todas as habilidades cadastradas, utilizando um ID numérico como chave.
        /// </summary>
        private Dictionary<int, Habilidade> _habilidades;

        /// <summary>
        /// Dicionário que armazena todas as demandas, utilizando um ID numérico como chave.
        /// </summary>
        private Dictionary<int, Demanda> _demandas;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Associacao"/>,
        /// criando os dicionários para armazenamento de dados.
        /// </summary>
        public Associacao()
        {
            _associados = new Dictionary<string, Associado>();
            _habilidades = new Dictionary<int, Habilidade>();
            _demandas = new Dictionary<int, Demanda>();
        }

        /// <summary>
        /// Analisa os prestadores de serviço disponíveis e define qual é o mais adequado para atender a uma demanda específica.
        /// O critério de desempate é o prestador com o menor número de demandas já atribuídas.
        /// </summary>
        /// <param name="demanda">A demanda para a qual se busca um prestador.</param>
        /// <returns>O <see cref="Prestador"/> mais adequado para a demanda, ou <c>null</c> se nenhum candidato for encontrado.</returns>
        /// <exception cref="ArgumentNullException">Lançada se a demanda fornecida for nula.</exception>
        public Prestador DefinirMelhorPrestadorParaDemanda(Demanda demanda)
        {
            if (demanda == null)
                throw new ArgumentNullException(nameof(demanda));

            List<IHabil> candidatos = _associados.Values
                .OfType<IHabil>() // Filtra apenas quem implementa a interface
                .Where(p => p.TemHabilidadesNecessarias(demanda))
                .ToList();

            if (!candidatos.Any())
                return null;

            // Seleciona o prestador com menor quantidade de demandas atribuídas
            // O cast é seguro porque filtramos por IHabil, que é implementado por Prestador
            Prestador escolhido = (Prestador)candidatos.OrderBy(p => p.GetQtdDemandasAtribuidas()).FirstOrDefault();
            return escolhido;
        }

        /// <summary>
        /// Lista todos os associados que não possuem créditos suficientes para criar novas demandas (saldo inferior a -10).
        /// </summary>
        /// <returns>Uma lista de <see cref="Associado"/> com saldo devedor acima do limite.</returns>
        public List<Associado> ListarAssociadosSemCreditoParaDemanda()
        {
            return _associados.Values
                .Where(a => a.CalcularCreditos() < -10)
                .ToList();
        }

        /// <summary>
        /// Lista todos os prestadores de serviço que possuem as habilidades necessárias para atender a uma demanda específica.
        /// </summary>
        /// <param name="demandaId">O ID da demanda a ser verificada.</param>
        /// <returns>Uma lista de <see cref="Prestador"/> qualificados, ordenada por saldo de créditos.</returns>
        /// <exception cref="KeyNotFoundException">Lançada se a demanda com o ID fornecido não for encontrada.</exception>
        public List<Prestador> ListarPrestadoresHabeisParaDemanda(int demandaId)
        {
            Demanda demanda = LocalizarDemanda(demandaId);
            if (demanda == null)
                throw new KeyNotFoundException("Demanda não encontrada.");

            return _associados.Values
                .OfType<Prestador>() // Aqui podemos usar Prestador diretamente para listar
                .Where(p => p.TemHabilidadesNecessarias(demanda))
                .OrderBy(p => p.CalcularCreditos())
                .ToList();
        }

        /// <summary>
        /// Gera uma string formatada listando todos os associados hábeis para atender uma determinada demanda.
        /// </summary>
        /// <param name="demanda">A demanda a ser verificada.</param>
        /// <returns>Uma string com a lista de associados hábeis ou uma mensagem informativa se nenhum for encontrado.</returns>
        /// <exception cref="ArgumentNullException">Lançada se a demanda for nula.</exception>
        public string AssociadosHabeis(Demanda demanda)
        {
            if (demanda == null) throw new ArgumentNullException(nameof(demanda));

            var habeis = _associados.Values
                .OfType<IHabil>() // Filtrar pela interface
                .Where(a => a.TemHabilidadesNecessarias(demanda))
                .OrderBy(a => ((Associado)a).CalcularCreditos()) // Cast para Associado para acessar CalcularCreditos
                .Select(a => ((Associado)a).ToString()) // Cast para Associado para o ToString
                .ToList();

            return habeis.Any() ? string.Join("\n", habeis) : "Nenhum associado hábil encontrado.";
        }

        /// <summary>
        /// Cadastra um novo associado na associação.
        /// </summary>
        /// <param name="associado">O objeto do associado a ser cadastrado.</param>
        /// <returns>O ID (HashCode) do associado cadastrado.</returns>
        /// <exception cref="ArgumentNullException">Lançada se o associado for nulo.</exception>
        /// <exception cref="ArgumentException">Lançada se já existir um associado com o mesmo CPF.</exception>
        public int CadastrarAssociado(Associado associado)
        {
            if (associado == null)
                throw new ArgumentNullException(nameof(associado), "Associado não pode ser nulo.");

            if (_associados.ContainsKey(associado.GetCPF()))
                throw new ArgumentException("Um associado com este CPF já está cadastrado.");

            _associados.Add(associado.GetCPF(), associado);
            return associado.GetHashCode();
        }

        /// <summary>
        /// Cadastra uma nova habilidade na associação.
        /// </summary>
        /// <param name="habilidade">O objeto da habilidade a ser cadastrada.</param>
        /// <returns>O ID (HashCode) da habilidade cadastrada.</returns>
        /// <exception cref="ArgumentNullException">Lançada se a habilidade for nula.</exception>
        /// <exception cref="ArgumentException">Lançada se a habilidade já estiver cadastrada.</exception>
        public int CadastrarHabilidade(Habilidade habilidade)
        {
            if (habilidade == null)
                throw new ArgumentNullException(nameof(habilidade), "Habilidade não pode ser nula.");

            if (_habilidades.ContainsKey(habilidade.GetHashCode()))
                throw new ArgumentException("Habilidade já cadastrada.");

            _habilidades.Add(habilidade.GetHashCode(), habilidade);
            return habilidade.GetHashCode();
        }

        /// <summary>
        /// Cadastra uma nova demanda na associação.
        /// </summary>
        /// <param name="demanda">O objeto da demanda a ser cadastrada.</param>
        /// <returns>O ID (HashCode) da demanda cadastrada.</returns>
        /// <exception cref="ArgumentNullException">Lançada se a demanda for nula.</exception>
        /// <exception cref="ArgumentException">Lançada se a demanda já estiver cadastrada.</exception>
        public int CadastrarDemanda(Demanda demanda)
        {
            if (demanda == null)
                throw new ArgumentNullException(nameof(demanda), "Demanda não pode ser nula.");

            int demandaId = demanda.GetHashCode();
            if (_demandas.ContainsKey(demandaId))
                throw new ArgumentException("Demanda já cadastrada.");

            _demandas.Add(demandaId, demanda);
            return demandaId;
        }

        /// <summary>
        /// Localiza um associado pelo seu CPF.
        /// </summary>
        /// <param name="cpf">O CPF do associado a ser localizado.</param>
        /// <returns>O objeto <see cref="Associado"/> correspondente, ou <c>null</c> se não for encontrado.</returns>
        public Associado LocalizarAssociado(string cpf)
        {
            _associados.TryGetValue(cpf, out Associado associado);
            return associado; // Retorna o associado ou null se não for encontrado.
        }

        /// <summary>
        /// Localiza uma habilidade pelo seu ID.
        /// </summary>
        /// <param name="idHabilidade">O ID da habilidade a ser localizada.</param>
        /// <returns>O objeto <see cref="Habilidade"/> correspondente, ou <c>null</c> se não for encontrada.</returns>
        public Habilidade LocalizarHabilidade(int idHabilidade)
        {
            _habilidades.TryGetValue(idHabilidade, out Habilidade habilidade);
            return habilidade;
        }

        /// <summary>
        /// Localiza uma demanda pelo seu ID.
        /// </summary>
        /// <param name="idDemanda">O ID da demanda a ser localizada.</param>
        /// <returns>O objeto <see cref="Demanda"/> correspondente, ou <c>null</c> se não for encontrada.</returns>
        public Demanda LocalizarDemanda(int idDemanda)
        {
            _demandas.TryGetValue(idDemanda, out Demanda demanda);
            return demanda;
        }

        /// <summary>
        /// Obtém uma lista consolidada de todos os produtos registrados por todos os produtores na associação.
        /// </summary>
        /// <returns>Uma lista contendo todos os <see cref="Produto"/>.</returns>
        public List<Produto> GetAllProdutos()
        {
            return _associados.Values
                .OfType<Produtor>()               // Filtra apenas instâncias do tipo Produtor
                .SelectMany(p => p.GetProdutos()) // Agora é seguro acessar GetProdutos()
                .ToList();
        }

        /// <summary>
        /// Tenta encontrar e atribuir automaticamente até duas demandas para um associado com baixo saldo de créditos.
        /// Este método é interativo e imprime resultados no console.
        /// </summary>
        /// <param name="associado">O associado para o qual procurar demandas.</param>
        /// <exception cref="ArgumentNullException">Lançada se o associado for nulo.</exception>
        public void ProcurarAssociadoParaDemanda(Associado associado)
        {
            if (associado == null)
                throw new ArgumentNullException(nameof(associado));

            if (associado.CalcularCreditos() < -10)
            {
                var demandasDisponiveis = _demandas.Values
                    .Where(d => d.GetAtendente() == null)
                    .ToList();

                if (associado is IHabil prestador)
                {
                    Random rnd = new Random();
                    for (int i = 0; i < 2; i++)
                    {
                        if (demandasDisponiveis.Any())
                        {
                            int randomIndex = rnd.Next(0, demandasDisponiveis.Count);
                            Demanda demandaParaAssociar = demandasDisponiveis[randomIndex];

                            if (prestador.TemHabilidadesNecessarias(demandaParaAssociar))
                            {
                                try
                                {
                                    prestador.AtribuirDemanda(demandaParaAssociar);
                                    demandasDisponiveis.RemoveAt(randomIndex);
                                }
                                catch (InvalidOperationException ex)
                                {
                                    Console.WriteLine($"Não foi possivel atribuir a demanda {demandaParaAssociar.GetHashCode()} to {associado.GetNome()}: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Prestador {associado.GetNome()} não cumpre as habilidades necessária para atribuir a demanda {demandaParaAssociar.GetHashCode()}. Próxima.");
                                demandasDisponiveis.RemoveAt(randomIndex);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não há demandas disponíveis para atribuir.");
                            break;
                        }
                    }
                }
                else if (associado is IProdutor)
                {
                    Console.WriteLine($"Produtor {associado.GetNome()} precisa aumentar sua produção para aumentar os créditos.");
                }
            }
        }

        /// <summary>
        /// Obtém o dicionário de todos os associados.
        /// </summary>
        /// <returns>O dicionário de associados.</returns>
        public Dictionary<string, Associado> GetAssociados() => _associados;

        /// <summary>
        /// Obtém o dicionário de todas as habilidades.
        /// </summary>
        /// <returns>O dicionário de habilidades.</returns>
        public Dictionary<int, Habilidade> GetHabilidades() => _habilidades;

        /// <summary>
        /// Obtém o dicionário de todas as demandas.
        /// </summary>
        /// <returns>O dicionário de demandas.</returns>
        public Dictionary<int, Demanda> GetDemandas() => _demandas;

        /// <summary>
        /// Retorna uma representação em string do estado atual da associação, listando todos os seus membros.
        /// </summary>
        /// <returns>Uma string detalhada com o resumo da associação.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Associação com {_associados.Count} associados, {_habilidades.Count} habilidades, e {_demandas.Count} demandas.");
            sb.AppendLine("==================================================");

            foreach (Associado associado in _associados.Values)
            {
                sb.AppendLine(associado.ToString());
                sb.AppendLine("-------------------------");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Lista todas as demandas que ainda não foram atribuídas a um atendente.
        /// </summary>
        /// <returns>Uma lista de <see cref="Demanda"/> não alocadas.</returns>
        public List<Demanda> ListarDemandasNaoAlocadas()
        {
            return _demandas.Values
                .Where(d => d.GetAtendente() == null)
                .ToList();
        }

        /// <summary>
        /// Calcula a média de dias de diferença entre a data de atendimento e o prazo para todas as demandas finalizadas.
        /// Um valor negativo indica que, em média, as demandas são entregues antes do prazo.
        /// </summary>
        /// <returns>A média de dias para atendimento em relação ao prazo.</returns>
        public int MediaAtendimentoPrazos()
        {
            var demandasFinalizadas = _demandas.Values.Where(d => d.FoiFinalizada()).ToList();
            if (!demandasFinalizadas.Any())
                return 0;

            return (int)demandasFinalizadas.Average(d => d.CalcularDiferencaTempoPrazo());
        }

        /// <summary>
        /// Lista os 10 associados com o maior saldo de créditos.
        /// </summary>
        /// <returns>Uma lista com os top 10 associados por saldo de créditos.</returns>
        internal List<Associado> ListarTop10AssociadosMaiorSaldo()
        {
            return _associados.Values
                .OrderByDescending(a => a.CalcularCreditos())
                .Take(10)
                .ToList();
        }
    }
}