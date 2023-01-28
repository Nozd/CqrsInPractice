namespace Logic.Options
{
    /// <summary>
    /// Конфигурация работы с БД (query)
    /// </summary>
    public sealed class QueryDbOptions
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Количество попыток доступа к БД
        /// </summary>
        public int NumberOfDatabaseRetries { get; set; }
    }
}
