using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TTMS
{
    // TTMS 是一个继承自 Mod 的类，用于定义 Terraria 的自定义模组
    public class TTMS : Mod
    {
        // 在模组加载时调用
        public override void Load()
        {
            // 为 NetMessage.greetPlayer 方法添加一个钩子，用于在玩家加入时执行自定义逻辑
            On_NetMessage.greetPlayer += On_NetMessage_greetPlayer;
        }

        // 自定义的 greetPlayer 方法钩子
        // orig 是原始的 greetPlayer 方法，plr 是玩家的索引
        private void On_NetMessage_greetPlayer(On_NetMessage.orig_greetPlayer orig, int plr)
        {
            // 检查玩家对象是否存在
            if (Main.player[plr] != null)
            {
                var cplr = Main.player[plr]; // 获取当前玩家对象

                // 如果玩家名字长度超过 18 个字符，踢出玩家
                if (cplr.name.Length > 18)
                {
                    NetMessage.SendData(MessageID.Kick, plr);
                }

                // 将玩家设置为敌对状态
                cplr.hostile = true;
            }
        }
    }
}
