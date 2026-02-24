using System;

namespace AssociacaoAMX
{
    /// <summary>
    /// Define um contrato para entidades (associados) que podem registrar produtos na associação.
    /// É implementada pela classe <see cref="Produtor"/> para garantir a funcionalidade de registro de produtos.
    /// </summary>
    public interface IProdutor
    {
        /// <summary>
        /// Registra um novo produto associado a esta entidade.
        /// </summary>
        /// <param name="produto">O objeto <see cref="Produto"/> a ser registrado.</param>
        void RegistrarProduto(Produto produto);
    }
}