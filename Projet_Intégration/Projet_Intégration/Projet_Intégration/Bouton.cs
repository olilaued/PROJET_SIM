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
    public class Bouton : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int Index { get; set; }
        Texture2D Image {get; set;}
        SpriteFont Font {get; set;}
        Rectangle Location { get; set; }
        string Texte { get; set; }
        string NomTexture { get; set; }
        string NomFont { get; set; }
        Vector2 TexteLocation { get; set; }
        SpriteBatch GestionSprites { get; set; }
        InputManager GestionInput { get; set; }
        RessourcesManager<Texture2D> GestionnaireDeTextures{ get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        Vector2 Origine { get; set; }
        Vector2 Dimensions { get; set; }
        public bool Clicked { get; set; }
        string clickText = "Button was Clicked!";

        public Bouton(Game game,string nomTexture, string nomFont, string texte, Vector2 origine, Vector2 dimensions)
            :base(game)
        {
            NomTexture = nomTexture;
            Texte = texte; 
            NomFont = nomFont;
            Origine = origine;
            Dimensions = dimensions;
            
             
        }
        public override void Initialize()
        {
            Clicked = false;
            
          //  GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
            
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
 	        base.Initialize();
            Index = Jeu.ListeDesBoutons.FindIndex(x=> x.Equals(this));
        }

        protected override void LoadContent()
        {
            GestionSprites = new SpriteBatch(GraphicsDevice);
            Image = GestionnaireDeTextures.Find(NomTexture);
            Font = GestionnaireDeFonts.Find(NomFont);
            Vector2 size = Font.MeasureString(Texte);
            //int longueur = GraphicsDevice.Viewport.Width / 5;
           // int hauteur = GraphicsDevice.Viewport.Width / 8;
            Location = new Rectangle((int)Origine.X - (int)Dimensions.X/2, (int)Origine.Y + (int)(Dimensions.Y/2), (int)Dimensions.X , (int)Dimensions.Y);
            TexteLocation = new Vector2(Location.X + ((Location.Width / 2) - (size.X / 2)),Location.Y + ((Location.Height / 2) - (size.Y / 2)));
           
            base.LoadContent();
        }

        
        

        public void ChangerLocation(int x, int y)
        {
            Location = new Rectangle(x, y, Image.Width, Image.Height);
        }

        public override void Update(GameTime gametime)
        {

            if ( Index < 7  && this.Clicked == true)
            {

               this.Clicked = false;
            }
                

            if (GestionInput.EstNouveauClicGauche())
            {
                
                if (SourisOnBouton())
                {
                    Clicked = true;
                }
                
            }

            base.Update(gametime);
            
        }

        public override void Draw(GameTime gametime)
        {
            GestionSprites.Begin();

            if (SourisOnBouton())
            {
                GestionSprites.Draw(Image,Location,Color.Silver);
            }
            else
            {
                GestionSprites.Draw(Image,Location,Color.White);
            }

            GestionSprites.DrawString(Font,Texte,TexteLocation,Color.Black);

            if (Clicked)
            {
                Vector2 position = new Vector2(10, 75);
                GestionSprites.DrawString(Font,clickText,position,Color.White);
            }

            GestionSprites.End();
            
            
        }

        bool SourisOnBouton()
        {
            return Location.Contains(new Point(GestionInput.GetPositionSouris().X, GestionInput.GetPositionSouris().Y));
        }
    }
    
}

    

