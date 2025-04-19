using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TTMS.Content.Projectiles
{
    // 自定义弹幕类，继承自 ModProjectile
    public class bccProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4; // 设置弹幕的帧数
        }
        // 设置弹幕的默认属性
        public override void SetDefaults()
        {
            Projectile.width = 14; // 弹幕的宽度（像素）
            Projectile.height = 14; // 弹幕的高度（像素）
            Projectile.aiStyle = 0; // 自定义 AI（设置为 0 表示不使用默认 AI）
            Projectile.friendly = true; // 是否对敌人友好（即是否会伤害敌人）
            Projectile.hostile = false; // 是否对玩家敌对
            Projectile.DamageType = DamageClass.Melee; // 弹幕的伤害类型（此处为近战）
            Projectile.penetrate = -1; // 弹幕可以穿透的敌人数量（-1 表示无限穿透）
            Projectile.timeLeft = 600; // 弹幕的存活时间（单位为帧，60 帧 = 1 秒）
            Projectile.light = 0.5f; // 弹幕发出的光亮度
            Projectile.ignoreWater = true; // 弹幕是否忽略水的影响
            Projectile.tileCollide = false; // 弹幕是否会与方块碰撞
            Projectile.stepSpeed = 0.5f; // 弹幕的移动速度
        }

        // 自定义弹幕的行为
        public override void AI()
        {
            Player oplr = Main.player[Projectile.owner]; // 获取发射弹幕的玩家
            Projectile.ai[0] += 1f; // 增加 AI 数组的第一个元素

            // 寻找最近的敌怪
            float maxDetectRadius = 500f; // 最大检测范围（像素）
            NPC target = FindClosestNPC(maxDetectRadius);

            if (target != null)
            {
                // 计算弹幕指向目标的方向
                Vector2 direction = target.Center - Projectile.Center;
                direction.Normalize(); // 将方向向量归一化

                // 让弹幕围绕目标旋转
                float rotationSpeed = 0.1f; // 旋转速度
                Projectile.velocity = Projectile.velocity.RotatedBy(rotationSpeed);

                // 调整速度以逐渐靠近目标
                float speed = 10f; // 弹幕的速度
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction * speed, 0.05f); // 平滑调整速度
            }

            // 根据速度动态切换帧
            float velocityMagnitude = Projectile.velocity.Length(); // 获取弹幕的速度大小
            int maxFrames = Main.projFrames[Type]; // 获取弹幕的总帧数
            Projectile.frame = (int)(velocityMagnitude / 10f * maxFrames) % maxFrames; // 根据速度映射到帧数范围

            // 防止帧计数器超出范围
            if (Projectile.frame >= maxFrames)
            {
                Projectile.frame = 0;
            }
        }

        // 寻找最近的敌怪
        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float closestDistance = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy()) // 检查敌怪是否可以被追踪
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
            Rectangle rectangle = new Rectangle(//因为手动绘制需要自己填写帧图框,所以要先算出来
               0,//这个框的左上角的水平坐标(填0就好)
               texture.Height / Main.projFrames[Type] * Projectile.frame,//框的左上角的纵向坐标 
               texture.Width, //框的宽度(材质宽度即可)
               texture.Height / 4//框的高度（用材质高度除以帧数得到单帧高度）
               );
            Main.EntitySpriteDraw(  //entityspritedraw是弹幕，NPC等常用的绘制方法
               texture,//第一个参数是材质
               Projectile.Center - Main.screenPosition,//注意，绘制时的位置是以屏幕左上角为0点
                                                       //因此要用弹幕世界坐标减去屏幕左上角的坐标
               rectangle,//第三个参数就是帧图选框了
               Main.DiscoColor,//第四个参数是颜色，这里我们用自带的lightcolor，可以受到自然光照影响
               Projectile.rotation,//第五个参数是贴图旋转方向
               new Vector2(texture.Width / 2, texture.Height / 2 / Main.projFrames[Type]),
               //第六个参数是贴图参照原点的坐标，这里写为贴图单帧的中心坐标，这样旋转和缩放都是围绕中心
               new Vector2(1, 1),//第七个参数是缩放，X是水平倍率，Y是竖直倍率
               Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
               //第八个参数是设置图片翻转效果，需要手动判定并设置spriteeffects
               0//第九个参数是绘制层级，但填0就行了，不太好使
               );

            return false;//return false阻止自动绘制
        }
    }
}
