using AssociacaoAMX;
using System;

namespace AssociacaoAMX
{
    /// <summary>
    /// Define um contrato para os diferentes níveis de dificuldade de uma demanda.
    /// Classes que implementam esta interface representam uma estratégia específica (ex: Simples, Difícil)
    /// e fornecem um multiplicador para o cálculo de créditos.
    /// </summary>
    internal interface IDemandavel
    {
        /// <summary>
        /// Obtém o fator multiplicador de créditos com base na dificuldade da demanda.
        /// </summary>
        /// <returns>O valor do multiplicador de créditos como um <see cref="int"/>.</returns>
        int MultiplicadorCreditos();
    }
}