namespace Logic.Options
{
    /// <summary>
    /// Конфигурация работы с БД
    /// </summary>
    public sealed class DbOptions
    {
        /// <summary>
        /// Количество попыток доступа к БД
        /// </summary>
        public int NumberOfDatabaseRetries { get; set; }
    }
}
