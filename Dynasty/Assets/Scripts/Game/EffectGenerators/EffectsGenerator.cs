using System;
using CardEffect = System.Func<bool>;
public interface IEffectsGenerator {
    public CardEffect GetEffect(Player player, Card card);
}