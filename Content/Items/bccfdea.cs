using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TTMS.Content.Projectiles;

namespace TTMS.Content.Items
{
    // bccfdea 是一个自定义物品类，继承自 ModItem
    // 用于定义一个新的物品及其属性和行为
    public class bccfdea : ModItem
    {
        // 设置物品的默认属性
        public override void SetDefaults()
        {
            Item.damage = 1000; // 物品的基础伤害值
            Item.DamageType = DamageClass.Melee; // 物品的伤害类型为近战
            Item.width = 50; // 物品的宽度（以像素为单位）
            Item.height = 50; // 物品的高度（以像素为单位）
            Item.useTime = 20; // 使用物品所需的时间（越低越快，单位为帧）
            Item.useAnimation = 20; // 使用物品的动画时间（单位为帧）
            Item.useStyle = ItemUseStyleID.Swing; // 使用物品的方式（此处为举起物品）
            Item.value = Item.buyPrice(platinum: 5, gold: 14); // 物品的价值（购买价格为 5 铂金和 14 金币）
            Item.rare = ItemRarityID.Expert; // 物品的稀有度（专家模式专属）
            Item.UseSound = SoundID.Item1; // 使用物品时播放的声音
            Item.autoReuse = true; // 是否可以自动重复使用（按住使用键时自动连续使用）
            Item.useTurn = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<bccProjectile>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile projectile = new();
            Vector2 plrToMouse = Main.MouseWorld - player.Center; // 计算玩家到鼠标位置的向量
            float r = (float)Math.Atan2(plrToMouse.Y, plrToMouse.X); // 计算角度
            for(int i =-1; i<=1;i++)
            {
                float angle = r +i*MathHelper.Pi/36;
                Vector2 shootVel = angle.ToRotationVector2() * 10;
                Projectile.NewProjectile(projectile.GetSource_FromAI(), player.Center, shootVel, ModContent.ProjectileType<bccProjectile>(), Item.damage, Item.knockBack, player.whoAmI); // 创建新的弹幕  // 计算新的角度
            } 
                    
            return false; // 返回 false 表示不执行默认的射击行为
        }
        // 添加物品的合成配方
        public override void AddRecipes()
        {
            // 创建一个新的配方实例
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 12); // 添加 12 个月亮锭作为合成材料
            recipe.AddTile(TileID.LunarCraftingStation); // 指定需要在月球制作站上进行合成
            recipe.Register(); // 注册配方，使其生效
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }
    }
}
