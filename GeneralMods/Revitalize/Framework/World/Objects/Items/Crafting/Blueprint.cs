using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.Crafting;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.World.Objects.InformationFiles;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.StardustCore.Animations;
using StardewValley;
using StardewValley.Network;

namespace Omegasis.Revitalize.Framework.World.Objects.Crafting
{
    [XmlType("Mods_Revitalize.Framework.World.Objects.Crafting.Blueprint")]
    public class Blueprint : CustomItem
    {
        /// <summary>
        /// A mapping from the name of the crafting book to the name of the crafting recipe to unlock.
        /// </summary>
        public readonly NetStringDictionary<string, NetString> craftingRecipesToUnlock = new();
        public NetRef<Drawable> itemToDraw = new NetRef<Drawable>();


        public Blueprint()
        {

        }

        public Blueprint(BasicItemInformation Info, string CraftingRecipeBookName, string CraftingRecipe, Drawable itemToDraw =null ) : base(Info)
        {
            this.addCraftingRecipe(CraftingRecipeBookName, CraftingRecipe);
            this.itemToDraw.Value = itemToDraw;
        }

        public Blueprint(BasicItemInformation Info, Dictionary<string,string> CraftingRecipesToUnlock, Drawable itemToDraw = null) : base(Info)
        {
            this.addCraftingRecipe(CraftingRecipesToUnlock);
            this.itemToDraw.Value = itemToDraw;
        }

        public Blueprint(BasicItemInformation Info, NetStringDictionary<string, NetString> CraftingRecipesToUnlock, Drawable itemToDraw = null) : base(Info)
        {
            foreach (var craftingBookNameToCraftingRecipeName in CraftingRecipesToUnlock)
            {
                this.addCraftingRecipe(craftingBookNameToCraftingRecipeName);
            }
            this.itemToDraw.Value = itemToDraw;
        }

        protected virtual void addCraftingRecipe(Dictionary<string,string> CraftingRecipes)
        {
            foreach (KeyValuePair<string, string> craftingBookNameToCraftingRecipeName in CraftingRecipes)
            {
                this.addCraftingRecipe(craftingBookNameToCraftingRecipeName.Key, craftingBookNameToCraftingRecipeName.Value);
            }
        }

        /// <summary>
        /// Adds a single crafting recipe to this blueprint when used.
        /// </summary>
        /// <param name="CraftingBookName"></param>
        /// <param name="CraftingRecipeName"></param>
        protected virtual void addCraftingRecipe(string CraftingBookName, string CraftingRecipeName)
        {
            this.craftingRecipesToUnlock.Add(CraftingBookName, CraftingRecipeName);
        }

        protected override void initNetFieldsPostConstructor()
        {
            base.initNetFieldsPostConstructor();
            this.NetFields.AddFields(this.craftingRecipesToUnlock,this.itemToDraw);
        }

        public override bool performUseAction(GameLocation location)
        {
            return this.learnRecipes();
        }


        protected virtual bool learnRecipes()
        {
            bool anyUnlocked = false;
            Dictionary<KeyValuePair<string, string>, bool> recipiesLearned = RevitalizeModCore.CraftingManager.learnCraftingRecipes(this.craftingRecipesToUnlock);

            foreach(var bookRecipePairToLearnedValues in recipiesLearned) {
                if (bookRecipePairToLearnedValues.Value == true)
                {
                    anyUnlocked = true;

                    Game1.drawObjectDialogue(string.Format("You learned how to make {0}! You can make it on a {1}. ", bookRecipePairToLearnedValues.Key.Value, Constants.ItemIds.Objects.CraftingStations.GetCraftingStationNameFromRecipeBookId(bookRecipePairToLearnedValues.Key.Key)));

                }
            }
            return anyUnlocked;
        }



        public override Item getOne()
        {
            Blueprint component = new Blueprint(this.basicItemInformation.Copy(), this.craftingRecipesToUnlock, this.itemToDraw.Value!=null? this.itemToDraw.Value.Copy(): new Drawable());
            return component;
        }

        public override void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            if (this.itemToDraw.Value != null)
            {
                this.itemToDraw.Value.drawInMenu(spriteBatch, location, scaleSize, .5f, layerDepth, drawStackNumber, color, drawShadow);
                base.drawInMenu(spriteBatch, location + new Vector2(16, 8), scaleSize * .5f, 1f, layerDepth, drawStackNumber, color, drawShadow);
                return;
            }
            base.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);

        }

        public override void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            base.drawWhenHeld(spriteBatch, objectPosition, f);
        }

        protected virtual bool blueprintOutputIsCustomObject()
        {
            if(this.itemToDraw.Value!=null && this.itemToDraw.Value is ICustomModObject)
            {
                return true;
            }
            return false;
        }
    }
}
