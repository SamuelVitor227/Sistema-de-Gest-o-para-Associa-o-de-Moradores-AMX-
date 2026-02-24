using System;

namespace AssociacaoAMX
{
    /// <summary>
    /// Define um contrato para entidades (associados) que possuem habilidades e podem atender a demandas.
    /// É implementada por classes como <see cref="Prestador"/> para garantir que possuam as funcionalidades necessárias 
    /// para o ecossistema de serviços da associação.
    /// </summary>
    public interface IHabil
    {
        /// <summary>
        /// Atribui uma demanda para a entidade hábil atender.
        /// </summary>
        /// <param name="demanda">A demanda a ser atribuída.</param>
        void AtribuirDemanda(Demanda demanda);

        /// <summary>
        /// Verifica se a entidade possui todas as habilidades necessárias para atender a uma determinada demanda.
        /// </summary>
        /// <param name="demanda">A demanda cujas habilidades necessárias serão verificadas.</param>
        /// <returns><c>true</c> se a entidade possuir todas as habilidades requeridas; caso contrário, <c>false</c>.</returns>
        bool TemHabilidadesNecessarias(Demanda demanda);

        /// <summary>
        /// Atribui uma nova habilidade à entidade.
        /// </summary>
        /// <param name="habilidade">A habilidade a ser adicionada ao conjunto de competências da entidade.</param>
        void AtribuirHabilidade(Habilidade habilidade);

        /// <summary>
        /// Obtém a quantidade total de demandas que já foram atribuídas a esta entidade.
        /// </summary>
        /// <returns>O número de demandas atribuídas como um <see cref="int"/>.</returns>
        int GetQtdDemandasAtribuidas();

        /// <summary>
        /// Obtém o nome da entidade hábil.
        /// </summary>
        /// <returns>O nome da entidade como um <see cref="object"/>.</returns>
        object GetNome();
    }
}