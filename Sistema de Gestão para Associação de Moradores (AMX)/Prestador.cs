// AssociacaoAMX/Prestador.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa um Associado do tipo Prestador, que pode atender demandas com base em suas habilidades.
    /// Herda da classe <see cref="Associado"/> e implementa a interface <see cref="IHabil"/>.
    /// </summary>
    public class Prestador : Associado, IHabil
    {
        /// <summary>
        /// Dicionário privado que armazena as habilidades que o prestador possui. A chave é o nome da habilidade.
        /// </summary>
        private Dictionary<string, Habilidade> _habilidades;

        /// <summary>
        /// Dicionário privado que armazena as demandas que foram atendidas por este prestador. A chave é o hash code da demanda.
        /// </summary>
        private Dictionary<int, Demanda> _demandasAtendidas;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Prestador"/>.
        /// </summary>
        /// <param name="nome">O nome do prestador.</param>
        /// <param name="cpf">O CPF do prestador.</param>
        public Prestador(string nome, string cpf) : base(nome, cpf)
        {
            _habilidades = new Dictionary<string, Habilidade>();
            _demandasAtendidas = new Dictionary<int, Demanda>();
        }

        /// <summary>
        /// Atribui uma demanda para este prestador atender, finalizando-a no processo.
        /// </summary>
        /// <param name="demanda">A demanda a ser atribuída.</param>
        /// <exception cref="ArgumentNullException">Lançada se a demanda fornecida for nula.</exception>
        /// <exception cref="InvalidOperationException">Lançada se o prestador não possuir as habilidades necessárias para a demanda.</exception>
        /// <exception cref="ArgumentException">Lançada se a demanda já tiver sido atribuída a este prestador.</exception>
        public void AtribuirDemanda(Demanda demanda)
        {
            if (demanda == null)
                throw new ArgumentNullException(nameof(demanda), "Demanda não pode ser nula.");

            if (!TemHabilidadesNecessarias(demanda))
                throw new InvalidOperationException("Prestador não possui todas as habilidades necessárias para esta demanda.");

            if (_demandasAtendidas.ContainsKey(demanda.GetHashCode()))
                throw new ArgumentException("Esta demanda já foi atribuída a este prestador.");

            _demandasAtendidas.Add(demanda.GetHashCode(), demanda);
            demanda.FinalizarDemanda(this);
        }

        /// <summary>
        /// Verifica se o prestador possui todas as habilidades requeridas por uma determinada demanda.
        /// </summary>
        /// <param name="demanda">A demanda a ser verificada.</param>
        /// <returns><c>true</c> se o prestador possui todas as habilidades necessárias; caso contrário, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Lançada se a demanda fornecida for nula.</exception>
        public bool TemHabilidadesNecessarias(Demanda demanda)
        {
            ArgumentNullException.ThrowIfNull(demanda);
            return demanda.GetHabilidadesNecessarias().All(h => _habilidades.ContainsKey(h.Key));
        }

        /// <summary>
        /// Adiciona ou atualiza uma habilidade na lista de habilidades do prestador.
        /// </summary>
        /// <param name="habilidade">A habilidade a ser atribuída.</param>
        /// <exception cref="ArgumentNullException">Lançada se a habilidade fornecida for nula.</exception>
        public void AtribuirHabilidade(Habilidade habilidade)
        {
            ArgumentNullException.ThrowIfNull(habilidade);
            _habilidades[habilidade.GetHabilidade()] = habilidade;
        }

        /// <summary>
        /// Calcula o saldo final de créditos do prestador.
        /// O cálculo é baseado nos créditos ganhos por serviços prestados (demandas atendidas)
        /// menos os débitos de demandas criadas pelo próprio prestador.
        /// </summary>
        /// <returns>O total de créditos do prestador.</returns>
        public override int CalcularCreditos()
        {
            int creditosServicos = _demandasAtendidas.Values.Sum(d => d.CalcularCreditos());
            int debitosDemandasCriadas = _demandasCriadas.Values.Sum(d => (d.GetTempoPrevisto() / 2) * 1);

            return creditosServicos - debitosDemandasCriadas;
        }

        /// <summary>
        /// Obtém a quantidade total de demandas atribuídas por este prestador.
        /// </summary>
        /// <returns>O número de demandas atendidas.</returns>
        public int GetQtdDemandasAtribuidas()
        {
            return _demandasAtendidas.Count;
        }

        /// <summary>
        /// Retorna uma representação em string do objeto Prestador.
        /// </summary>
        /// <returns>Uma string que representa o prestador, incluindo informações da classe base, seu tipo e o número de habilidades que possui.</returns>
        public override string ToString()
        {
            return $"{base.ToString()}, Tipo: Prestador, Habilidades: {_habilidades.Count}";
        }

        /// <summary>
        /// Implementação explícita da interface <see cref="IHabil"/>. Retorna o nome do prestador.
        /// </summary>
        /// <returns>O nome do prestador como um <see cref="object"/>.</returns>
        object IHabil.GetNome()
        {
            return this.GetNome();
        }
    }
}