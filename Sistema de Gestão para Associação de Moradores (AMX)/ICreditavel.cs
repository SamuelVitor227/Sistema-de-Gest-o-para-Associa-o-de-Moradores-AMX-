using System;

namespace AssociacaoAMX
{
    /// <summary>
    /// Define um contrato para objetos que possuem um sistema de créditos.
    /// Classes que implementam esta interface devem fornecer uma lógica para calcular seu saldo de créditos.
    /// </summary>
    public interface ICreditavel
    {
        /// <summary>
        /// Calcula e retorna o saldo total de créditos do objeto.
        /// </summary>
        /// <returns>O valor do saldo de créditos como um <see cref="int"/>.</returns>
        int CalcularCreditos();
    }
}