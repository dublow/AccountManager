namespace AccountManager.Providers.Persistences.Models
{
    public class KeyWordCategoryDb : IDatabasable
    {
        public readonly string CategoryId;
        public readonly string KeyWord;

        public KeyWordCategoryDb(string categoryId, string keyWord)
        {
            CategoryId = categoryId;
            KeyWord = keyWord;
        }
    }
}
