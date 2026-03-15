using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using WeaponRevamp.Projectiles.Sentries;

namespace WeaponRevamp.Items.Sentries
{
    public class StaffOfTheFrostHydra : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstatiation)
        {

            return item.type == ItemID.StaffoftheFrostHydra;

        }
        
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            entity.damage = 30;
            entity.knockBack = 5f;
            entity.shoot = ModContent.ProjectileType<FrostHydraSentry>();

        }
        

    }
}