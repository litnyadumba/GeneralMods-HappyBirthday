using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Revitalize.Framework;
using Revitalize.Framework.Configs;
using Revitalize.Framework.Environment;
using Revitalize.Framework.Objects;
using Revitalize.Framework.World.Objects;
using Revitalize.Framework.World.Objects.Machines;
using Revitalize.Framework;
using Revitalize.Framework.Crafting;
using Revitalize.Framework.Hacks;
using Revitalize.Framework.Menus;
using Revitalize.Framework.Menus.MenuComponents;
using Revitalize.Framework.Objects;
using Revitalize.Framework.Player;
using Revitalize.Framework.Utilities;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Tools;
using StardustCore.Animations;
using StardustCore.UIUtilities;
using StardustCore.UIUtilities.MenuComponents.ComponentsV2.Buttons;
using xTile.Dimensions;
using Animation = StardustCore.Animations.Animation;
using SpaceShared.APIs;
using Revitalize.Framework.Constants.ItemIds.Resources.EarthenResources;
using Revitalize.Framework.Constants.ItemIds.Objects;
using Revitalize.Framework.SaveData;
using Omegasis.Revitalize.Framework.Utilities;
using Revitalize.Framework.World.WorldUtilities;
using Revitalize.Framework.World;

namespace Revitalize
{

    // TODO:
    /*
    // -Make this mod able to load content packs for easier future modding
    //
    //  -Multiple Lights On Object
    //  -Illumination Colors
    //  Furniture:
    //      -rugs 
    //      -tables
    //      -lamps
    //      -dressers/other storage containers 
    //      -fun interactables
    //          -Arcade machines
    //      -More crafting tables 
    //      -Baths (see chairs but swimming)
    //
    //  -Machines
    //      !=Energy
    //            Generators:
                  -solar
                  -burnable
                  -watermill
                  -windmill
                  -crank (costs stamina)
                  Storage:
                  -Batery Pack
             -Mini-greenhouse
                   -takes fertilizer which can do things like help crops grow or increase prodcuction yield/quality.
                   -takes crop/extended crop seeds
                   -takes sprinklers
                   -has grid (1x1, 2x2, 3x3, 4x4, 5x5) system for growing crops/placing sprinkers
                   -sprinkers auto water crops
                   -can auto harvest
                   -hover over crop to see it's info
                   -can be upgraded to grow crops from specific seasons with season stones (spring,summer, fall winter) (configurable if they are required)
                   -Add in season stone recipe

    //      -Furnace
    //      -Seed Maker
    //      -Stone Quarry
    //      -Mayo Maker
    //      -Cheese Maker
            -Yogurt Maker
                   -Fruit yogurts (artisan good)
    //      -Auto fisher
    //      -Auto Preserves
    //      -Auto Keg
    //      -Auto Cask
    //      -Calcinator (oil+stone: produces titanum?)
    //  -Materials
    //      -Tin/Bronze/Alluminum/Silver?Platinum/Etc (all but platinum: may add in at a later date)
            -titanium (d0ne)
            -Alloys!
                -Brass (done)
                -Electrum (done)
                -Steel (done)
                -Bronze (done)
            -Mythrill
            
            -Star Metal
            -Star Steel
            -Cobalt
        -Liquids
            -oil
            -water
            -coal
            -juice???
            -lava?

        -Dyes!
            -Dye custom objects certain colors!
            -Rainbow Dye -(set a custom object to any color)
            -red, green, blue, yellow, pink, etc
            -Make dye from flowers/coal/algee/minerals/gems (black), etc
                -soapstone (washes off dye)
                -Lunarite (white)
        Dye Machine
            -takes custom object and dye
            -dyes the object
            -can use water to wash off dye.
            -maybe dye stardew valley items???
            -Dyed Wool (Artisan good)

        Menus:
    //  -Crafting Menu
    //  -Item Grab Menu (Extendable) (Done!)
    //   -Yes/No Dialogue Box
    //   -Multi Choice dialogue box


    //  -Gift Boxes

    //  Magic!
    //      -Alchemy Bags
    //      -Transmutation
    //      -Effect Crystals
    //      -Spell books
    //      -Potions!
    //      -Magic Meter
    //      -Connected chests (3 digit color code) much like Project EE2 from MC
    //
    //
    //  -Food
            -multi flavored sodas

    //  -Bigger chests
    //
    //  Festivals
    //      -Firework festival
    //      -Horse Racing Festival
            -Valentines day (Maybe make this just one holiday)
                -Spring. Male to female gifts.
                -Winter. Female to male gifts. 
    //  Stargazing???
    //      -Moon Phases+DarkerNight
    //  Bigger/Better Museum?
    // 
    //  Equippables!
    //      -accessories that provide buffs/regen/friendship
    //      -braclets/rings/broaches....more crafting for these???
    //      
    //  Music???
    //      -IDK maybe add in instruments???
    //      
    //  More buildings????
    //  
    //  More Animals???
    //  
    //  Readable Books?
    //  
    //  Custom NPCs for shops???
    //
    //  Minigames:
    //      Frisbee Minigame?
    //      HorseRace Minigame/Betting?
    //  
    //  Locations:
            -Make extra bus stop sign that travels between new towns/locations.
    //      -Small Island Home?
    //      -New town inspired by FOMT;Mineral Town/The Valley HM DS
    //
    //  More crops
    //      -RF Crops
    //      -HM Crops
    //
    //  More monsters
    //  -boss fights
    //
    //  More dungeons??

    //  More NPCS?

        Accessories
        (recover hp/stamina,max hp,more friendship ,run faster, take less damage, etc)
            -Neckalces
            -Broaches
            -Earings
            -Pendants
    */

    public class ModCore : Mod, IAssetEditor
    {
        public static IModHelper ModHelper;
        public static IMonitor ModMonitor;
        public static IManifest Manifest;

        /// <summary>
        /// Keeps track of custom objects.
        /// </summary>
        public static ObjectManager ObjectManager;

        public static PlayerInfo playerInfo;

        public static Serializer Serializer;

        public static CraftingManager CraftingManager;

        public static ConfigManager Configs;

        public static SaveDataManager SaveDataManager;

        public static MailManager MailManager;

        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;
            Manifest = this.ModManifest;
            Configs = new ConfigManager();
            SaveDataManager = new SaveDataManager();
            MailManager = new MailManager();

            this.createDirectories();
            this.initailizeComponents();
            Serializer = new Serializer();
            playerInfo = new PlayerInfo();

            //Loads in textures to be used by the mod.
            this.loadInTextures();

            //Loads in objects to be use by the mod.
            ObjectManager = new ObjectManager(Manifest);

            //Adds in event handling for the mod.
            ModHelper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;

            ModHelper.Events.GameLoop.TimeChanged += this.GameLoop_TimeChanged;
            ModHelper.Events.GameLoop.UpdateTicked += this.GameLoop_UpdateTicked;
            ModHelper.Events.GameLoop.ReturnedToTitle += this.GameLoop_ReturnedToTitle;

            ModHelper.Events.Player.Warped += ObjectManager.resources.OnPlayerLocationChanged;
            ModHelper.Events.GameLoop.DayStarted += this.GameLoop_DayStarted;
            ModHelper.Events.GameLoop.DayEnding += this.GameLoop_DayEnding;

            ModHelper.Events.Input.ButtonPressed += ObjectInteractionHacks.Input_CheckForObjectInteraction;

            ModHelper.Events.Display.RenderedWorld += ObjectInteractionHacks.Render_RenderCustomObjectsHeldInMachines;
            //ModHelper.Events.Display.Rendered += MenuHacks.EndOfDay_OnMenuChanged;
            ModHelper.Events.Display.MenuChanged += ShopHacks.OnNewMenuOpened;
            ModHelper.Events.Display.MenuChanged += MailManager.onNewMenuOpened ;
            //ModHelper.Events.GameLoop.Saved += MenuHacks.EndOfDay_CleanupForNewDay;
            ModHelper.Events.Input.ButtonPressed += ObjectInteractionHacks.ResetNormalToolsColorOnLeftClick;

            ModHelper.Events.Input.ButtonPressed += this.Input_ButtonPressed1;

            ModHelper.Events.GameLoop.GameLaunched += this.GameLoop_GameLaunched;



        }


        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            ObjectManager.loadInItems();
            //Adds in recipes to the mod.
            CraftingManager = new CraftingManager();
            CraftingManager.initializeRecipeBooks();

            Revitalize.Framework.Utilities.Serializer.SerializeTypesForXMLUsingSpaceCore();
        }


        /// <summary>
        /// What happens when a new day starts.
        /// </summary>
        /// <param name="senderm"></param>
        /// <param name="e"></param>
        private void GameLoop_DayStarted(object senderm, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            ObjectManager.resources.DailyResourceSpawn(senderm, e);
            ShopHacks.OnNewDay(senderm, e);
            MailManager.tryToAddMailToMailbox();
        }

        /// <summary>
        /// Called when the day is ending. At this point the save data should all be saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoop_DayEnding(object sender, StardewModdingAPI.Events.DayEndingEventArgs e)
        {
            SaveDataManager.save();
        }

        private void Input_ButtonPressed1(object sender, StardewModdingAPI.Events.ButtonPressedEventArgs e)
        {


            if (e.Button == SButton.MouseLeft)
            {
                if (Game1.player != null)
                {
                    if (Game1.activeClickableMenu != null || Game1.eventUp || Game1.currentMinigame != null) return;
                    pressUseToolButtonCheckForCustomObjects();
                }
            }

        }

        public static bool pressUseToolButtonCheckForCustomObjects()
        {
            Game1.player.toolPower = 0;
            Game1.player.toolHold = 0;

            //ModCore.log("Press the tool button!");
            Vector2 c = Game1.player.GetToolLocation() / 64f;
            c.X = (int)c.X;
            c.Y = (int)c.Y;
            Point p = new Point((int)(c.X * 64f), (int)(c.Y * 64f));

            if (Game1.player.currentLocation.objects.ContainsKey(c))
            {
                //ModCore.log("Spot is taken: " + p.ToString());
                //Only want to check spots that might not be covered by the game.
                return false;
            }

            foreach (Furniture f in Game1.player.currentLocation.furniture)
            {
                //ModCore.log("I see a furniture: " + f.DisplayName);
                if (f is CustomObject)
                {
                    //ModCore.log("I see a custom furniture piece: " + f.DisplayName);
                    if (f.boundingBox.Value.Contains(p))
                    {
                        //ModCore.log("Found an object at a non object spot position: " + p.ToString());
                        // ModCore.log("The name is: " + f.DisplayName);
                        f.performToolAction(Game1.player.CurrentTool, Game1.player.currentLocation);
                        return true;
                    }
                    else
                    {
                        //ModCore.log("BB is: " + f.boundingBox.Value.ToString());
                        //ModCore.log("Point is: " + p.ToString());
                    }
                }
            }
            return false;
        }



        /// <summary>
        /// Loads in textures to be used by the mod.
        /// </summary>
        private void loadInTextures()
        {
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Furniture");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Furniture").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Objects", "Furniture"));
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Machines");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Machines").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Objects", "Machines"));
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.InventoryMenu");
            TextureManager.GetTextureManager(Manifest, "Revitalize.InventoryMenu").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Menus", "InventoryMenu"));
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Resources.Ore");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Resources.Ore").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Objects", "Resources", "Ore"));
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Items.Resources.Misc");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Items.Resources.Misc").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Items", "Resources", "Misc"));
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Items.Resources.Ore");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Items.Resources.Ore").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Items", "Resources", "Ore"));
            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Tools");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Tools").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Items", "Tools"));

            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Menus");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Menus").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Menus", "Misc"));

            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Menus.EnergyMenu");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Menus.EnergyMenu").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Menus", "EnergyMenu"));

            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.CraftingMenu");
            TextureManager.GetTextureManager(Manifest, "Revitalize.CraftingMenu").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Menus", "CraftingMenu"));

            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.HUD");
            TextureManager.GetTextureManager(Manifest, "Revitalize.HUD").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "HUD"));

            TextureManager.AddTextureManager(this.Helper.DirectoryPath, Manifest, "Revitalize.Objects.Crafting");
            TextureManager.GetTextureManager(Manifest, "Revitalize.Objects.Crafting").searchForTextures(ModHelper, this.ModManifest, Path.Combine("Content", "Graphics", "Objects", "Crafting"));
        }

        private void GameLoop_ReturnedToTitle(object sender, StardewModdingAPI.Events.ReturnedToTitleEventArgs e)
        {
        }

        private void createDirectories()
        {
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Configs"));

            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics"));
            //Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics","Furniture"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics", "Furniture", "Chairs"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics", "Furniture", "Lamps"));
            Directory.CreateDirectory(Path.Combine(this.Helper.DirectoryPath, "Content", "Graphics", "Furniture", "Tables"));
        }

        /// <summary>
        /// Initialize all modular components for this mod.
        /// </summary>
        private void initailizeComponents()
        {
            DarkerNight.InitializeConfig();
        }

        private void GameLoop_UpdateTicked(object sender, StardewModdingAPI.Events.UpdateTickedEventArgs e)
        {
            if (playerInfo.justPlacedACustomObject == true) playerInfo.justPlacedACustomObject = false;
            DarkerNight.SetDarkerColor();
            playerInfo.update();
        }

        private void GameLoop_TimeChanged(object sender, StardewModdingAPI.Events.TimeChangedEventArgs e)
        {
            DarkerNight.CalculateDarkerNightColor();
        }

        private void GameLoop_SaveLoaded(object sender, StardewModdingAPI.Events.SaveLoadedEventArgs e)
        {
            SaveDataManager.loadOrCreateSaveData();
            MailManager.tryToAddMailToMailbox();


            //HACKS
            Game1.player.Money = 100_000;
            Game1.player.addItemToInventoryBool(ObjectManager.GetItem(CraftingStations.WorkStation));
            Game1.player.addItemsByMenuIfNecessary(new List<Item>()
            {
                //new StardewValley.Object((int)Enums.SDVObject.Coal,100),
                //ObjectManager.GetItem(Ingots.SteelIngot, 20),
                //ObjectManager.GetItem(CraftingStations.Anvil,1),
                //ObjectManager.GetItem(Machines.AdvancedSolarPanelV1,1),
                //ObjectManager.GetItem(Machines.SolarArrayV1,1),
                //new StardewValley.Object(Vector2.Zero,(int)Enums.SDVBigCraftable.Furnace,false),
                //new StardewValley.Object((int)Enums.SDVObject.CopperOre,10),
                //ObjectManager.GetItem(Machines.MiningDrillV1),
                //new StardewValley.Object((int)Enums.SDVObject.IronBar,100),
                //ObjectManager.GetItem(Machines.WindmillV1),
                ObjectManager.GetItem(Machines.HayMaker),
                new StardewValley.Object((int)Enums.SDVObject.Corn,10),
                ObjectManager.GetItem(Enums.SDVObject.Stone,999),
                                ObjectManager.GetItem(Enums.SDVObject.Wood,999),
                ObjectManager.GetItem(Enums.SDVObject.Clay,100),
                ObjectManager.GetItem(Enums.SDVObject.CopperBar,100)
            });


            this.addModdedMachinesToGameWorld();

        }

        /// <summary>
        /// Adds various machines and stuff to the game world.
        /// </summary>
        private void addModdedMachinesToGameWorld()
        {
            GameLocation cinderSapForestLocation = Game1.getLocationFromName("Forest");
            HayMaker hayMaker = (ObjectManager.GetObject<HayMaker>(Machines.HayMaker, 1).getOne(true) as HayMaker);
            if (Configs.shopsConfigManager.hayMakerShopConfig.IsHayMakerShopSetUpOutsideOfMarniesRanch &&
                cinderSapForestLocation.isObjectAtTile((int)Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y) == false)
            {
                hayMaker.placementActionAtTile(cinderSapForestLocation, (int)Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.X, (int)Configs.shopsConfigManager.hayMakerShopConfig.HayMakerTileLocation.Y);
            }

        }

        /// <summary>
        ///Logs information to the console.
        /// </summary>
        /// <param name="message"></param>
        public static void log(object message, bool StackTrace = true)
        {
            if (StackTrace)
                ModMonitor.Log(message.ToString() + " " + getFileDebugInfo());
            else
                ModMonitor.Log(message.ToString());
        }

        public static string getFileDebugInfo()
        {
            string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(2).GetFileName();
            int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(2).GetFileLineNumber();
            return currentFile + " line:" + currentLine;
        }

        public static bool IsNullOrDefault<T>(T argument)
        {
            // deal with normal scenarios
            if (argument == null) return true;
            if (Equals(argument, default(T))) return true;

            // deal with non-null nullables
            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;

            // deal with boxed value types
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }

        public bool CanEdit<T>(IAssetInfo asset)
        {
            return MailManager.canEditAsset(asset);
        }

        public void Edit<T>(IAssetData asset)
        {
            MailManager.editMailAsset(asset);
        }
    }
}
