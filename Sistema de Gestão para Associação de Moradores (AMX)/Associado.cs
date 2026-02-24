using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace AssociacaoAMX
{
    /// <summary>
    /// Representa a base para todos os tipos de Associados da Associação.
    /// Esta classe é abstrata e implementa a interface <see cref="ICreditavel"/>.
    /// </summary>
    public abstract class Associado : ICreditavel
    {
        /// <summary>
        /// O nome do associado.
        /// </summary>
        private string _nome;

        /// <summary>
        /// O CPF do associado.
        /// </summary>
        private string _cpf;

        /// <summary>
        /// Dicionário protegido que armazena as demandas criadas por este associado.
        /// Acessível pelas classes derivadas para o cálculo de débitos.
        /// </summary>
        protected Dictionary<string, Demanda> _demandasCriadas;

        /// <summary>
        /// O identificador único do associado.
        /// </summary>
        private int _idAssociado;

        /// <summary>
        /// Contador estático para gerar IDs únicos para cada novo associado.
        /// </summary>
        private static int s_id = 1;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="Associado"/>.
        /// </summary>
        /// <param name="nome">O nome do associado. Não pode ser nulo ou vazio.</param>
        /// <param name="cpf">O CPF do associado. Não pode ser nulo ou vazio.</param>
        /// <exception cref="ArgumentException">Lançada se o nome ou CPF forem nulos ou vazios.</exception>
        public Associado(string nome, string cpf)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome do associado não pode ser vazio.");

            if (string.IsNullOrWhiteSpace(cpf))
                throw new ArgumentException("CPF do associado não pode ser vazio.");

            _nome = nome;
            _cpf = cpf;
            _demandasCriadas = new Dictionary<string, Demanda>();
            _idAssociado = s_id++;
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, calcula o saldo de créditos do associado.
        /// O método de cálculo varia conforme o tipo de associado (Produtor, Prestador, etc.).
        /// </summary>
        /// <returns>O saldo de créditos atual do associado.</returns>
        public abstract int CalcularCreditos();

        /// <summary>
        /// Registra uma nova demanda criada por este associado, se ele tiver créditos suficientes.
        /// </summary>
        /// <param name="demanda">A demanda a ser registrada.</param>
        /// <exception cref="ArgumentNullException">Lançada se a demanda for nula.</exception>
        /// <exception cref="CreditosInsuficientesException">Lançada se o associado tiver um saldo devedor maior que o limite permitido (-10 créditos).</exception>
        public void RegistrarDemanda(Demanda demanda)
        {
            if (demanda == null)
                throw new ArgumentNullException(nameof(demanda));

            if (CalcularCreditos() < -10)
                throw new CreditosInsuficientesException();

            _demandasCriadas.Add(demanda.GetHashCode().ToString(), demanda);
        }

        /// <summary>
        /// Exceção lançada quando um associado tenta criar uma demanda sem ter créditos suficientes.
        /// </summary>
        public class CreditosInsuficientesException : Exception
        {
            /// <summary>
            /// Inicializa uma nova instância da classe <see cref="CreditosInsuficientesException"/> com uma mensagem de erro padrão.
            /// </summary>
            public CreditosInsuficientesException()
                : base("Não possui créditos suficientes para criar a demanda. Saldo devedor acima do limite de -10.") { }
        }

        /// <summary>
        /// Obtém o nome do associado.
        /// </summary>
        /// <returns>O nome do associado.</returns>
        public string GetNome() => _nome;

        /// <summary>
        /// Obtém o CPF do associado.
        /// </summary>
        /// <returns>O CPF do associado.</returns>
        public string GetCPF() => _cpf;

        /// <summary>
        /// Retorna o código de hash para este associado, que é seu ID único.
        /// </summary>
        /// <returns>Um inteiro que representa o ID único do associado.</returns>
        public override int GetHashCode()
        { return _idAssociado; }

        /// <summary>
        /// Determina se o objeto especificado é igual ao associado atual.
        /// A comparação é baseada no ID único do associado.
        /// </summary>
        /// <param name="obj">O objeto a ser comparado com o associado atual.</param>
        /// <returns><c>true</c> se o objeto especificado for um <see cref="Associado"/> com o mesmo ID; caso contrário, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        { return obj is Associado && obj.GetHashCode() == _idAssociado; }

        /// <summary>
        /// Retorna uma representação em string do objeto Associado.
        /// </summary>
        /// <returns>Uma string que contém o ID, nome, CPF e saldo de créditos do associado.</returns>
        public override string ToString()
        {
            return $"ID: {_idAssociado}, Nome: {_nome}, CPF: {_cpf}, Saldo: {CalcularCreditos()}";
        }
    }
}