using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa uma Demanda de Serviço dentro da associação.
    /// Uma demanda é criada por um associado e pode ser atendida por outro, gerando créditos para o atendente.
    /// </summary>
    public class Demanda
    {
        /// <summary>
        /// A descrição textual da demanda.
        /// </summary>
        private string _descricao;

        /// <summary>
        /// Indica se a demanda já foi finalizada.
        /// </summary>
        private bool _demandaFinalizada;

        /// <summary>
        /// O nível de dificuldade da demanda, que influencia no cálculo de créditos.
        /// </summary>
        private IDemandavel _dificuldade;

        /// <summary>
        /// A data em que a demanda foi criada.
        /// </summary>
        private DateTime _dataCriacao;

        /// <summary>
        /// A data em que a demanda foi atendida. Nulo se ainda não foi atendida.
        /// </summary>
        private DateTime? _dataAtendimento;

        /// <summary>
        /// O tempo previsto para a conclusão da demanda, em frações de 30 minutos.
        /// </summary>
        private int _tempoPrevisto;

        /// <summary>
        /// O prazo em dias para a conclusão da demanda.
        /// </summary>
        private int _prazo;

        /// <summary>
        /// Dicionário com as habilidades necessárias para atender a esta demanda.
        /// </summary>
        private Dictionary<string, Habilidade> _habilidadesNecessarias;

        /// <summary>
        /// O associado que criou a demanda.
        /// </summary>
        private Associado _autor;

        /// <summary>
        /// O associado que atendeu e finalizou a demanda.
        /// </summary>
        private Associado _atendente;

        /// <summary>
        /// O identificador único da demanda.
        /// </summary>
        private int _idDemanda;

        /// <summary>
        /// Contador estático para gerar IDs únicos para cada nova demanda.
        /// </summary>
        private static int s_id = 1;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Demanda"/>.
        /// </summary>
        /// <param name="autor">O associado que está criando a demanda.</param>
        /// <param name="descricao">A descrição da tarefa a ser realizada.</param>
        /// <param name="tempoPrevisto">O tempo estimado para conclusão, em frações de 30 minutos.</param>
        /// <param name="prazo">O prazo em dias para a conclusão.</param>
        /// <exception cref="ArgumentNullException">Lançada se o autor for nulo.</exception>
        /// <exception cref="ArgumentException">Lançada se a descrição for nula ou vazia.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lançada se o tempo previsto ou o prazo não forem positivos.</exception>
        public Demanda(Associado autor, string descricao, int tempoPrevisto, int prazo)
        {
            if (autor == null) throw new ArgumentNullException(nameof(autor));
            if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descrição da demanda não pode ser vazia.");
            if (tempoPrevisto <= 0) throw new ArgumentOutOfRangeException(nameof(tempoPrevisto), "Tempo previsto deve ser maior que zero.");
            if (prazo <= 0) throw new ArgumentOutOfRangeException(nameof(prazo), "Prazo deve ser maior que zero.");

            _autor = autor;
            _descricao = descricao;
            _tempoPrevisto = tempoPrevisto;
            _prazo = prazo;
            _dataCriacao = DateTime.Now.Date;
            _dataAtendimento = null;
            _demandaFinalizada = false;
            _habilidadesNecessarias = new Dictionary<string, Habilidade>();
            _idDemanda = s_id++;
            _dificuldade = new DemandaSimples();
        }

        /// <summary>
        /// Adiciona uma habilidade necessária para a conclusão desta demanda.
        /// </summary>
        /// <param name="habilidade">A habilidade a ser adicionada.</param>
        /// <exception cref="ArgumentNullException">Lançada se a habilidade for nula.</exception>
        /// <exception cref="InvalidOperationException">Lançada se for tentado adicionar uma habilidade a uma demanda já finalizada.</exception>
        public void AtribuirHabilidadesNecessarias(Habilidade habilidade)
        {
            if (habilidade == null)
                throw new ArgumentNullException(nameof(habilidade));

            if (_demandaFinalizada)
                throw new InvalidOperationException("Não é possível adicionar habilidades a uma demanda finalizada.");

            if (!_habilidadesNecessarias.ContainsKey(habilidade.GetHabilidade()))
            {
                _habilidadesNecessarias.Add(habilidade.GetHabilidade(), habilidade);
                _dificuldade = DemandaHelper.IdentificarDificuldade(_habilidadesNecessarias.Values.ToList());
            }
        }

        /// <summary>
        /// Marca a demanda como finalizada, registrando quem a atendeu e a data do atendimento.
        /// </summary>
        /// <param name="atendente">O associado que atendeu a demanda.</param>
        /// <exception cref="InvalidOperationException">Lançada se a demanda já estiver finalizada.</exception>
        /// <exception cref="ArgumentNullException">Lançada se o atendente for nulo.</exception>
        public void FinalizarDemanda(Associado atendente)
        {
            if (_demandaFinalizada)
                throw new InvalidOperationException("Demanda já finalizada.");
            if (atendente == null)
                throw new ArgumentNullException(nameof(atendente), "Atendente não pode ser nulo ao finalizar a demanda.");

            _atendente = atendente;
            _dataAtendimento = DateTime.Now;
            _demandaFinalizada = true;
            _dificuldade = DemandaHelper.IdentificarDificuldade(_habilidadesNecessarias.Values.ToList());
        }

        /// <summary>
        /// Calcula o tempo decorrido em dias desde a criação da demanda até seu atendimento.
        /// Se a demanda não foi finalizada, calcula os dias até a data atual.
        /// </summary>
        /// <returns>O número de dias de atendimento.</returns>
        public int CalcularTempoAtendimento()
        {
            if (!_dataAtendimento.HasValue)
                return (DateTime.Now.Date - _dataCriacao).Days;
            return (_dataAtendimento.Value.Date - _dataCriacao.Date).Days;
        }

        /// <summary>
        /// Calcula a quantidade de créditos que esta demanda gera para o associado que a atende.
        /// O cálculo baseia-se no tempo previsto e na dificuldade da demanda.
        /// </summary>
        /// <returns>A quantidade de créditos gerada.</returns>
        public int CalcularCreditos()
        {
            return (_tempoPrevisto / 6) * 2 * _dificuldade.MultiplicadorCreditos();
        }

        /// <summary>
        /// Calcula a diferença em dias entre o tempo real de atendimento e o prazo estipulado.
        /// </summary>
        /// <returns>A diferença de dias. Um valor negativo indica adiantamento, positivo indica atraso.</returns>
        /// <exception cref="InvalidOperationException">Lançada se a demanda ainda não foi finalizada.</exception>
        public int CalcularDiferencaTempoPrazo()
        {
            if (!_demandaFinalizada || !_dataAtendimento.HasValue)
                throw new InvalidOperationException("Demanda não finalizada ou data de atendimento não definida.");
            return CalcularTempoAtendimento() - _prazo;
        }

        /// <summary>
        /// Verifica se a demanda foi finalizada.
        /// </summary>
        /// <returns><c>true</c> se a demanda foi finalizada; caso contrário, <c>false</c>.</returns>
        public bool FoiFinalizada() => _demandaFinalizada;

        /// <summary>
        /// Obtém a descrição da demanda.
        /// </summary>
        /// <returns>A descrição da demanda.</returns>
        public string GetDescricao() => _descricao;

        /// <summary>
        /// Obtém a lista de habilidades necessárias para a demanda.
        /// </summary>
        /// <returns>Um dicionário contendo as habilidades.</returns>
        public Dictionary<string, Habilidade> GetHabilidadesNecessarias() => _habilidadesNecessarias;

        /// <summary>
        /// Obtém o tempo previsto para a demanda em frações de 30 minutos.
        /// </summary>
        /// <returns>O tempo previsto.</returns>
        public int GetTempoPrevisto() => _tempoPrevisto;

        /// <summary>
        /// Obtém o associado que atendeu a demanda.
        /// </summary>
        /// <returns>O associado atendente, ou nulo se não foi atendida.</returns>
        public Associado GetAtendente() => _atendente;

        /// <summary>
        /// Obtém o associado que criou a demanda.
        /// </summary>
        /// <returns>O associado autor da demanda.</returns>
        public Associado GetAutor() => _autor;

        /// <summary>
        /// Retorna o código de hash para esta demanda, que é seu ID único.
        /// </summary>
        /// <returns>Um inteiro que representa o ID único da demanda.</returns>
        public override int GetHashCode() => _idDemanda;

        /// <summary>
        /// Determina se o objeto especificado é igual à demanda atual.
        /// A comparação é baseada no ID único da demanda.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado com a demanda atual.</param>
        /// <returns><c>true</c> se o objeto especificado for uma <see cref="Demanda"/> com o mesmo ID; caso contrário, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Demanda outraDemanda && outraDemanda.GetHashCode() == _idDemanda;
        }

        /// <summary>
        /// Retorna uma representação em string detalhada do objeto Demanda.
        /// </summary>
        /// <returns>Uma string que representa a demanda, incluindo ID, descrição, créditos, status, autor, atendente e dificuldade.</returns>
        public override string ToString()
        {
            string atendenteInfo = _atendente != null ? $"Atendente: {_atendente.GetNome()}" : "Não atendida";
            return $"ID: {_idDemanda}, Descrição: {_descricao}, Creditos: {CalcularCreditos()}, Tempo Previsto: {_tempoPrevisto} (30min fracs.), Criador: {_autor.GetNome()}, Status: {(_demandaFinalizada ? "Finalizada" : "Aberta")}, {atendenteInfo}, Dificuldade: {_dificuldade.GetType().Name}";
        }
    }
}