using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Omegasis.Revitalize.Framework.World.Objects;
using Omegasis.Revitalize.Framework.World.Objects.Interfaces;
using Omegasis.StardustCore.Networking;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Utilities
{
    [XmlType("Mods_Revitalize.Framework.Utilities.Drawable")]
    public class Drawable: NetObject
    {
        public readonly NetRef<StardewValley.Item> item = new NetRef<StardewValley.Item>();

        public Drawable()
        {

        }

        public Drawable(StardewValley.Item item)
        {
            this.item.Value = item;
        }

        protected override void initializeNetFields()
        {
            base.initializeNetFields();
            this.NetFields.AddFields(this.item);
        }

        public virtual void drawInMenu(SpriteBatch spriteBatch, Vector2 location, float scaleSize, float transparency, float layerDepth, StackDrawType drawStackNumber, Color color, bool drawShadow)
        {
            if (this.item.Value != null)
            {
                this.item.Value.drawInMenu(spriteBatch, location, scaleSize, transparency, layerDepth, drawStackNumber, color, drawShadow);
            }
        }

        public virtual void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f)
        {
            if (this.item.Value != null)
            {
                if(this.item.Value is StardewValley.Object)
                {
                    StardewValley.Object obj =(StardewValley.Object) this.item.Value;
                    obj.drawWhenHeld(spriteBatch, objectPosition, f);
                }
            }
        }

        public virtual void drawWhenHeld(SpriteBatch spriteBatch, Vector2 objectPosition, Farmer f, float transparency, float Scale)
        {
            if (this.item.Value != null)
            {
                if (this.item.Value is StardewValley.Object && !(this.item.Value is ICustomModObject))
                {
                    StardewValley.Object obj = (StardewValley.Object)this.item.Value;
                    obj.drawWhenHeld(spriteBatch, objectPosition, f);
                }
                if (this.item.Value is StardewValley.Object && this.item.Value is ICustomModObject)
                {
                    ICustomModObject obj = (ICustomModObject)this.item.Value;
                    obj.drawWhenHeld(spriteBatch, objectPosition, f,transparency,Scale);
                }
            }
        }

        public virtual Drawable Copy()
        {
            if (this.item.Value != null)
            {
                return new Drawable(this.item.Value.getOne());
            }
            return new Drawable();
        }
    }
}
