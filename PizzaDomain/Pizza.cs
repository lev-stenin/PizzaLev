using System;
using System.Collections.Generic;

namespace PizzaDomain
{
    [Serializable]
    public class Pizza
    {
        public List<Ingredients> Ingredients = new List<Ingredients>();
        private int size = Constraints.MinPizzaSize;

        public int Size
        {
            get => size;
            set
            {
                if (value < Constraints.MinPizzaSize || value > Constraints.MaxPizzaSize)
                    return;
                size = value;
            }
        }

        public void AddIngredient(Ingredients ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public void RemoveIngredient(Ingredients ingredient)
        {
            Ingredients.Remove(ingredient);
        }
    }
}