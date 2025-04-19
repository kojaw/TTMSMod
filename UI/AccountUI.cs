using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
namespace TTMS.UI
{
    public class AccountUI : UIState
    {
        private UIText text;
        public override void OnInitialize()
        {
            text = new UIText("Account UI");
            text.Left.Set(100, 0f);
            text.Top.Set(100, 0f);
            Append(text);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // ¸üÐÂÂß¼­
        }
    }
}