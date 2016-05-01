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
    public class TexteAffichable : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string NomFont {get; set;}
        string Message { get; set; }
        Vector2 FontPos {get; set;}
        SpriteFont Écriture { get; set; }
        Color Couleur { get; set; }
        float Rotation { get; set; }
        Vector2 Origine { get; set; }
        float Scale { get; set; }
        float Profondeur { get; set; }
        
        
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        SpriteBatch GestionSprites {get; set;} 
        
        

        public TexteAffichable(Game game, string nomFont, Vector2 fontPos, string message, Color couleur, float rotation, float scale, float profondeur)
            : base(game)
        {
            NomFont = nomFont;
            FontPos = fontPos;
            Message = message;
            Couleur = couleur;
            Rotation = rotation;
            Scale = scale;
            Profondeur = profondeur;
            
        }

        public TexteAffichable(Game game, string nomFont, string message, Color couleur, float rotation, Vector2 origine, float scale, float profondeur)
            : base(game)
        {
            NomFont = nomFont;
            Message = message;
            Couleur = couleur;
            Rotation = rotation;
            Origine = origine;
            Scale = scale;
            Profondeur = profondeur;
            FontPos = new Vector2(Game.GraphicsDevice.Viewport.Width / 2,Game.GraphicsDevice.Viewport.Height / 2);
            
        }
        public TexteAffichable(Game game, string nomFont, string message, Color couleur, float rotation, float scale, float profondeur)
            : base(game)
        {
            NomFont = nomFont;
            Message = message;
            Couleur = couleur;
            Rotation = rotation;
            Scale = scale;
            Profondeur = profondeur;
            FontPos = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            
        }
        public TexteAffichable(Game game, string nomFont, string message, Color couleur, float rotation, float scale)
            : base(game)
        {
            NomFont = nomFont;
            Message = message;
            Couleur = couleur;
            Rotation = rotation;
            Scale = scale;
            FontPos = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            Profondeur = 0.5f;
            
        }
        public TexteAffichable(Game game, string nomFont, string message, Vector2 position, Color couleur, float scale)
        : base(game)
    {
        NomFont = nomFont;
        Message = message;
        FontPos = position;
        Couleur = couleur;
        Rotation = 0;
        Scale = scale;
        Profondeur = 0f;
    }

        public override void Initialize()
        {
            GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
            GestionSprites = Game.Services.GetService(typeof(SpriteBatch))as SpriteBatch;
            Écriture = GestionnaireDeFonts.Find(NomFont);
            Origine = Écriture.MeasureString(Message) / 2;
            base.Initialize();
            if (this.Message.Contains(':'))
            {
                FontPos = new Vector2(FontPos.X + Écriture.MeasureString(Message).X, FontPos.Y - Écriture.MeasureString(Message).Y);
            }
        }

 	

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            GestionSprites.Begin();


           
            GestionSprites.DrawString(Écriture, Message, FontPos, Couleur,
                Rotation, Origine, Scale, SpriteEffects.None,Profondeur);

            GestionSprites.End();
            base.Draw(gameTime);
        }
        public void ModifierTexte(string nvMessage)
        {
            this.Message = nvMessage;
        }
    }
}
