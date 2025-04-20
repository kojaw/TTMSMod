using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TTMS
{
    // TTMS ��һ���̳��� Mod ���࣬���ڶ��� Terraria ���Զ���ģ��
    public class TTMS : Mod
    {
        // ��ģ�����ʱ����
        public override void Load()
        {
            // Ϊ NetMessage.greetPlayer �������һ�����ӣ���������Ҽ���ʱִ���Զ����߼�
            On_NetMessage.greetPlayer += On_NetMessage_greetPlayer;
        }

        // �Զ���� greetPlayer ��������
        // orig ��ԭʼ�� greetPlayer ������plr ����ҵ�����
        private void On_NetMessage_greetPlayer(On_NetMessage.orig_greetPlayer orig, int plr)
        {
            // �����Ҷ����Ƿ����
            if (Main.player[plr] != null)
            {
                var cplr = Main.player[plr]; // ��ȡ��ǰ��Ҷ���

                // ���������ֳ��ȳ��� 18 ���ַ����߳����
                if (cplr.name.Length > 18)
                {
                    NetMessage.SendData(MessageID.Kick, plr);
                }

                // ���������Ϊ�ж�״̬
                cplr.hostile = true;
            }
        }
    }
}
