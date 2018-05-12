using System;
using System.ComponentModel;

namespace PizzaDomain
{
    [Serializable]
    public enum Ingredients
    {
        [Description("Помидор")] Tomato,
        [Description("Сыр")] Cheeze,
        [Description("Бекон")] Bacon,
        [Description("Лук")] Onion,
        [Description("Ананас")] Pineapple
    }
}