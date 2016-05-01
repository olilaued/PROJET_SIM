using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ZoneDéroulante : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch GestionSprites { get; set; }
        string NomImage { get; set; }
        Texture2D ImageDeFond { get; set; }
        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        Rectangle ZoneDeJeu { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
        const float RYTHME = 0.001f;
        float Échelle { get; set; }
        Vector2 PositionÉcran { get; set; }
        Vector2 PositionOrigine { get; set; }
        Vector2 TailleImage { get; set; }
        Rectangle RectangleSource { get; set; }
        
        
       
        



        public ZoneDéroulante(Game jeu, string nomImage, Rectangle zoneAffichage, float intervalleMAJ)
            : base(jeu)
        {
            IntervalleMAJ = intervalleMAJ;
            NomImage = nomImage;
            ZoneDeJeu = zoneAffichage;
        }
        public override void Initialize()
        {
            //this.Enabled = false;
            base.Initialize();
            FigerAnimer();
        }
        public override void Update(GameTime gameTime)
        {
            float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;

            TempsÉcouléDepuisMAJ += tempsÉcoulé;
            if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
            {

                PositionÉcran = new Vector2((PositionÉcran.X + (RYTHME * ZoneDeJeu.Width)) % ((ImageDeFond.Width * Échelle)), PositionÉcran.Y);

                RectangleSource = new Rectangle(0, 0, (int)(ZoneDeJeu.Width / Échelle) - (int)((PositionÉcran.X / Échelle)), (int)(ZoneDeJeu.Height / Échelle));

                TempsÉcouléDepuisMAJ = 0;



            }





        }




        protected override void LoadContent()
        {

           
                GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
                GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
                ImageDeFond = GestionnaireDeTextures.Find(NomImage);
                PositionOrigine = new Vector2(0, 0);
                PositionÉcran = new Vector2(0, 0);
                Échelle = MathHelper.Max(ZoneDeJeu.Width / (float)ImageDeFond.Width,
                                         ZoneDeJeu.Height / (float)ImageDeFond.Height);

                TailleImage = new Vector2(((ImageDeFond.Width * Échelle)), 0);
                RectangleSource = new Rectangle(0, 0, (int)(ZoneDeJeu.Width / Échelle) , (int)(ZoneDeJeu.Height / Échelle));
                
               
               
            
        
        
         
            
        }
        public override void Draw(GameTime gameTime)
        {
            GestionSprites.Begin();
            if (PositionÉcran.X< ZoneDeJeu.Width)
            {
              GestionSprites.Draw(ImageDeFond, PositionÉcran,RectangleSource, Color.White, 0, PositionOrigine, Échelle, SpriteEffects.None, 1f);
            }
          
    
          GestionSprites.Draw(ImageDeFond, PositionÉcran - TailleImage, null, Color.White, 0, PositionOrigine, Échelle, SpriteEffects.None, 1f);
          GestionSprites.End();
        }
        public void ModifierActivation()
        {
            //this.Enabled = !this.Enabled;
            this.Visible = !this.Visible;
        }
        public void FigerAnimer()
        {
            this.Enabled = !Enabled;
        }

    }
}