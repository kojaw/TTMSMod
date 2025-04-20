using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TTMS.Content.Projectiles
{
    // �Զ��嵯Ļ�࣬�̳��� ModProjectile
    public class bccProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4; // ���õ�Ļ��֡��
        }
        // ���õ�Ļ��Ĭ������
        public override void SetDefaults()
        {
            Projectile.width = 14; // ��Ļ�Ŀ�ȣ����أ�
            Projectile.height = 14; // ��Ļ�ĸ߶ȣ����أ�
            Projectile.aiStyle = 0; // �Զ��� AI������Ϊ 0 ��ʾ��ʹ��Ĭ�� AI��
            Projectile.friendly = true; // �Ƿ�Ե����Ѻã����Ƿ���˺����ˣ�
            Projectile.hostile = false; // �Ƿ����ҵж�
            Projectile.DamageType = DamageClass.Melee; // ��Ļ���˺����ͣ��˴�Ϊ��ս��
            Projectile.penetrate = -1; // ��Ļ���Դ�͸�ĵ���������-1 ��ʾ���޴�͸��
            Projectile.timeLeft = 600; // ��Ļ�Ĵ��ʱ�䣨��λΪ֡��60 ֡ = 1 �룩
            Projectile.light = 0.5f; // ��Ļ�����Ĺ�����
            Projectile.ignoreWater = true; // ��Ļ�Ƿ����ˮ��Ӱ��
            Projectile.tileCollide = false; // ��Ļ�Ƿ���뷽����ײ
            Projectile.stepSpeed = 0.5f; // ��Ļ���ƶ��ٶ�
        }

        // �Զ��嵯Ļ����Ϊ
        public override void AI()
        {
            Player oplr = Main.player[Projectile.owner]; // ��ȡ���䵯Ļ�����
            Projectile.ai[0] += 1f; // ���� AI ����ĵ�һ��Ԫ��

            // Ѱ������ĵй�
            float maxDetectRadius = 500f; // ����ⷶΧ�����أ�
            NPC target = FindClosestNPC(maxDetectRadius);

            if (target != null)
            {
                // ���㵯Ļָ��Ŀ��ķ���
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize(); // ������������һ��

                // �õ�ĻΧ��Ŀ����ת
                float rotationSpeed = 0.1f; // ��ת�ٶ�
                Projectile.velocity = Projectile.velocity.RotatedBy(rotationSpeed);

                // �����ٶ����𽥿���Ŀ��
                float speed = 10f; // ��Ļ���ٶ�
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, 0.05f); // ƽ�������ٶ�
            }

            // �����ٶȶ�̬�л�֡
            float velocityMagnitude = Projectile.velocity.Length(); // ��ȡ��Ļ���ٶȴ�С
            int maxFrames = Main.projFrames[Type]; // ��ȡ��Ļ����֡��
            Projectile.frame = (int)(velocityMagnitude / 10f * maxFrames) % maxFrames; // �����ٶ�ӳ�䵽֡����Χ

            // ��ֹ֡������������Χ
            if (Projectile.frame >= maxFrames)
            {
                Projectile.frame = 0;
            }
        }

        // Ѱ������ĵй�
        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float closestDistance = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy()) // ���й��Ƿ���Ա�׷��
                {
                    float distance = Vector2.Distance(Projectile.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }
            return closestNPC;
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rectangle = new Rectangle(//��Ϊ�ֶ�������Ҫ�Լ���д֡ͼ��,����Ҫ�������
               0,//���������Ͻǵ�ˮƽ����(��0�ͺ�)
               texture.Height / Main.projFrames[Type] * Projectile.frame,//������Ͻǵ��������� 
               texture.Width, //��Ŀ��(���ʿ�ȼ���)
               texture.Height / 4//��ĸ߶ȣ��ò��ʸ߶ȳ���֡���õ���֡�߶ȣ�
               );
            Main.EntitySpriteDraw(  //entityspritedraw�ǵ�Ļ��NPC�ȳ��õĻ��Ʒ���
               texture,//��һ�������ǲ���
               Projectile.Center - Main.screenPosition,//ע�⣬����ʱ��λ��������Ļ���Ͻ�Ϊ0��
                                                       //���Ҫ�õ�Ļ���������ȥ��Ļ���Ͻǵ�����
               rectangle,//��������������֡ͼѡ����
               Main.DiscoColor,//���ĸ���������ɫ�������������Դ���lightcolor�������ܵ���Ȼ����Ӱ��
               Projectile.rotation,//�������������ͼ��ת����
               new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
               //��������������ͼ����ԭ������꣬����дΪ��ͼ��֡���������꣬������ת�����Ŷ���Χ������
               new Vector2(1, 1),//���߸����������ţ�X��ˮƽ���ʣ�Y����ֱ����
               Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
               //�ڰ˸�����������ͼƬ��תЧ������Ҫ�ֶ��ж�������spriteeffects
               0//�ھŸ������ǻ��Ʋ㼶������0�����ˣ���̫��ʹ
               );

            return false;//return false��ֹ�Զ�����
        }
    }
}
